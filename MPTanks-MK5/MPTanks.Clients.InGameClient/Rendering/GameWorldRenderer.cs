using MPTanks.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MPTanks.Engine.Core;

namespace MPTanks.Clients.GameClient.Rendering
{
    partial class GameWorldRenderer : Renderer, IDisposable
    {
        private MPTanks.Engine.GameCore _game;

        private BasicEffect _effect;
        private AssetCache _cache;
        private RectangleF _viewRect;

        public GameWorldRenderer(Screens.Screen screen, MPTanks.Engine.GameCore game)
            : base(screen)
        {
            if (game == null) throw new ArgumentNullException("game");

            _game = game;
            _cache = new AssetCache(screen.Game);
            _effect = new BasicEffect(screen.Game.GraphicsDevice);
            _effect.View = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            _effect.TextureEnabled = true;
        }

        public void SetViewport(RectangleF view)
        {
            _viewRect = view;
        }

        public override void Render(SpriteBatch sb, GameTime gameTime)
        {
            _game.Diagnostics.BeginMeasurement("Compute view matrix", "World rendering", "Rendering");
            const float overdraw = 0.15f;

            var _boundsRect = new RectangleF( //Compute the bounds for visibility checks which have overdraw for simplicity
                _viewRect.X - (_viewRect.Width * overdraw),
                _viewRect.Y - (_viewRect.Height * overdraw),
                _viewRect.Width * (1 + 2 * overdraw),
                _viewRect.Height * (1 + 2 * overdraw));

            _viewRect = new RectangleF( //Scale the view rectangle to world space (for now, 100x)
                ScaleF(_viewRect.X),
                ScaleF(_viewRect.Y),
                ScaleF(_viewRect.Width),
                ScaleF(_viewRect.Height));

            //compute the projection matrix to offset the screen view
            var projectionMatrix = Matrix.Identity *
                Matrix.CreateOrthographicOffCenter(_viewRect.Left, _viewRect.Right,
                _viewRect.Bottom, _viewRect.Top, -1, 1);

            //upload the projection matrix 
            _effect.Projection = projectionMatrix;
            _game.Diagnostics.EndMeasurement("Compute view matrix", "World rendering", "Rendering");

            //Draw the below objects
            _game.Diagnostics.BeginMeasurement("Draw Particles (Below Objects)", "World rendering", "Rendering");
            DrawParticles(_game.ParticleEngine.Particles, _boundsRect, gameTime, sb, true);
            _game.Diagnostics.EndMeasurement("Draw Particles (Below Objects)", "World rendering", "Rendering");

            //Draw game objects
            _game.Diagnostics.BeginMeasurement("Draw Objects", "World rendering", "Rendering");
            // DrawObjects(_boundsRect, gameTime, sb);
            _game.Diagnostics.EndMeasurement("Draw Objects", "World rendering", "Rendering");

            //And animations
            _game.Diagnostics.BeginMeasurement("Draw Animations", "World rendering", "Rendering");
            DrawAnimations(_boundsRect, gameTime, sb);
            _game.Diagnostics.EndMeasurement("Draw Animations", "World rendering", "Rendering");

            //And particles
            _game.Diagnostics.BeginMeasurement("Draw Particles (Above Objects)", "World rendering", "Rendering");
            DrawParticles(_game.ParticleEngine.Particles, _boundsRect, gameTime, sb, false);
            _game.Diagnostics.EndMeasurement("Draw Particles (Above Objects)", "World rendering", "Rendering");
        }
        #region Object Rendering
        private SortedDictionary<int, SortListItem> _renderLayers =
            new SortedDictionary<int, SortListItem>();
        private void DrawObjects(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            if (_game.GameObjects != null)
                foreach (var obj in _game.GameObjects)
                {
                    //Cache position and size for perf reasons
                    var objPos = Scale(obj.Position);
                    var objSize = Scale(obj.Size);

                    //View culling
                    if (!IsVisible(obj.Position, obj.Size, boundsRect))
                        continue;

                    //Compute the global model matrix - the object's position in the world
                    var modelMatrix = Matrix.CreateScale(new Vector3(obj.Scale, 0)) *
                        Matrix.CreateTranslation(
                            new Vector3(-objSize / 2, 0)) *
                        Matrix.CreateRotationZ(obj.Rotation) *
                        Matrix.CreateTranslation(
                            new Vector3(objPos, 0));

                    foreach (var component in obj.Components.Values)
                    {
                        //Blend the object color mask with the component mask
                        var mask = new Color(component.Mask.ToVector4() * obj.ColorMask.ToVector4());

                        //Compute the matrix (space relative to object) and multiply by world
                        var cmpMatrix = Matrix.CreateScale(new Vector3(component.Scale, 0)) *
                            Matrix.CreateTranslation(new Vector3(-Scale(component.RotationOrigin), 0)) *
                            Matrix.CreateRotationZ(component.Rotation) *
                            Matrix.CreateTranslation(
                            new Vector3(Scale(component.RotationOrigin + component.Offset), 0)) *
                            modelMatrix;

                        var cmp = new RComponentInternal()
                        {
                            Component = component,
                            ComputedTransforms = cmpMatrix,
                            ComputedColor = mask
                        };
                        AddComponentToInternalList(cmp);
                    }
                }

            //And actually draw them
            var list = SortRenderList();
            foreach (var item in list)
                DrawComponent(sb, item, gameTime);

            ClearLists();
        }

        private void AddComponentToInternalList(RComponentInternal component)
        {
            if (!_renderLayers.ContainsKey(component.Component.DrawLayer))
                _renderLayers.Add(component.Component.DrawLayer, new SortListItem());

            _renderLayers[component.Component.DrawLayer].Id = component.Component.DrawLayer;
            _renderLayers[component.Component.DrawLayer].LastAccessed = DateTime.Now;
            _renderLayers[component.Component.DrawLayer].List.Add(component);
        }

        private List<RComponentInternal> _sortedLayers = new List<RComponentInternal>();
        private IEnumerable<RComponentInternal> SortRenderList()
        {
            foreach (var value in _renderLayers.Values)
                _sortedLayers.AddRange(value.List);

            return _sortedLayers;
        }

        private List<int> _layerRemoveList = new List<int>();
        private void ClearLists()
        {
            _sortedLayers.Clear();
            foreach (var layer in _renderLayers)
            {
                if ((DateTime.Now - layer.Value.LastAccessed).TotalSeconds > 5)
                    _layerRemoveList.Add(layer.Key);
                else //Just clear it
                    layer.Value.List.Clear();
            }

            foreach (var layerId in _layerRemoveList)
                _renderLayers.Remove(layerId);

            _layerRemoveList.Clear();
        }

        private void DrawComponent(SpriteBatch sb, RComponentInternal component, GameTime gameTime)
        {
            _effect.World = component.ComputedTransforms;

            //Start the spritebatch for the component
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
                SamplerState.PointWrap, DepthStencilState.Default,
                RasterizerState.CullNone, _effect);
            //Build a correctly sized rectangle to draw the asset on
            var drawRect = new Rectangle(
                -Scale(GameSettings.Instance.PhysicsCompensationForRendering),
                -Scale(GameSettings.Instance.PhysicsCompensationForRendering),
                ScaleForRendering(component.Component.Size.X),
                ScaleForRendering(component.Component.Size.Y));
            //Get the cached asset
            var asset = _cache.GetArtAsset(component.Component.SheetName, component.Component.FrameName, gameTime);
            //And draw
            sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, component.ComputedColor);

            sb.End();
        }

        private struct RComponentInternal
        {
            public Matrix ComputedTransforms;
            public MPTanks.Engine.Rendering.RenderableComponent Component;
            public Color ComputedColor;
        }
        private class SortListItem
        {
            public int Id;
            public List<RComponentInternal> List = new List<RComponentInternal>();
            public DateTime LastAccessed;
        }
        #endregion

        #region Animation Rendering
        private List<MPTanks.Engine.Rendering.Animations.Animation> _endedAnimations =
            new List<MPTanks.Engine.Rendering.Animations.Animation>();
        private void DrawAnimations(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            //And render the animations
            if (_game.AnimationEngine.Animations != null)
                foreach (var anim in _game.AnimationEngine.Animations)
                {
                    //Check if the animation will be done by next frame (approximately)
                    if (_cache.AnimEnded(anim.AnimationName, //name
                        anim.PositionInAnimationMs + (float)gameTime.ElapsedGameTime.TotalMilliseconds, //next frame time
                        anim.SpriteSheetName, anim.LoopCount)) //sheet and number of loops
                        _endedAnimations.Add(anim); //track it as ended
                    //cull if invisible from rendering
                    if (!IsVisible(anim.Position - (anim.Size / 2), anim.Size, boundsRect))
                        continue;

                    //Calculate the model's position in the world
                    var animMatrix =
                        Matrix.CreateTranslation(new Vector3(-Scale(anim.Size / 2), 0)) * //center it
                        Matrix.CreateRotationZ(anim.Rotation) * //rotate around center
                        Matrix.CreateTranslation( //and translate back into position
                            new Vector3(Scale((anim.Size / 2) + anim.Position), 0));

                    //It doesn't have configurable alpha so set the full opacity
                    _effect.Alpha = 1;
                    //upload the model matrix
                    _effect.World = animMatrix;
                    //Begin the draw call
                    sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
                        SamplerState.PointWrap, DepthStencilState.Default,
                        RasterizerState.CullNone, _effect);
                    //Load the sprite sheet and get it from the cache
                    var asset = _cache.GetAnimation(anim.AnimationName, anim.PositionInAnimationMs, anim.SpriteSheetName);
                    //For safety: if the asset's name is "loading", we set alpha to 0
                    if (asset.Name == "loading")
                        _effect.Alpha = 0;
                    //Compute the draw rectangle (size of the object, centered in world space)
                    var drawRect = new Rectangle(
                        Scale(-anim.Size.X / 2),
                        Scale(-anim.Size.Y / 2),
                        Scale(anim.Size.X),
                        Scale(anim.Size.X)
                        );
                    //And draw it with the correct texture space
                    sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, Color.White);
                    sb.End(); //End the spritebatch call
                }

            //Take all of the ended animations and track them
            foreach (var anim in _endedAnimations)
            {
                //Remove all the animations which are ended by now
                _game.AnimationEngine.MarkAnimationCompleted(anim);
            }

            _endedAnimations.Clear();
        }
        #endregion

        #region Particle Rendering
        private void DrawParticles(IEnumerable<MPTanks.Engine.Rendering.Particles.Particle> _particles,
            RectangleF boundsRect, GameTime gameTime, SpriteBatch sb, bool renderParticlesBelow)
        {
            //Tracker for how many particles we can render
            //This is decremented for each particle that is drawn *on screen*
            //but not for those that are outside of the view radius
            int remainingAllowedParticles = GameSettings.Instance.MaxParticlesToRender;
            //upload the world matrix
            _effect.World = Matrix.Identity;
            //and set the alpha back to max
            _effect.Alpha = 1;

            //Begin a draw call which handles sorting for us - NOTE: THIS DOES MEMORY ALLOCATIONS
            //in the form of SpriteBatchItems.
            sb.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.PointWrap,
                DepthStencilState.Default, RasterizerState.CullNone, _effect);

            //Iterate over the particle lists
            foreach (var particle in _particles)
            {
                //Stop drawing if we've hit the on screen particle limit
                if (remainingAllowedParticles == 0) break;
                //Ignore particles in wrong order
                if (particle.RenderBelowObjects != renderParticlesBelow) continue;
                //And ignore off screen particles
                if (!IsVisible(particle.Position, particle.Size, boundsRect)) continue;

                //Get the cached asset (or load it). See asset management below.
                var asset = _cache.GetArtAsset(particle.SheetName, particle.AssetName, gameTime);
                // A note to future me:
                // I'm aware that the rotation is incorrectly drawn, but to do it correctly requires
                // generating a matrix on the CPU which will be quite processor intensive. For now,
                // this looks good enough and is a bit faster.
                sb.Draw(asset.SpriteSheet.Texture, new Rectangle(
                    Scale(particle.Position.X), Scale(particle.Position.Y),
                    Scale(particle.Size.X), Scale(particle.Size.Y)
                    ),
                    asset.Bounds, particle.ColorMask, particle.Rotation, Vector2.Zero, SpriteEffects.None, 0);

                remainingAllowedParticles--; //Mark that another particle was drawn / note how many particles
                //we are still able to draw
            }

            //And do the deferred draw
            sb.End();
        }
        #endregion

        #region Scaling helpers
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Scale(float amount)
        {
            return (int)(amount * GameSettings.Instance.RenderScale);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float ScaleF(float amount)
        {
            return amount * GameSettings.Instance.RenderScale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 Scale(Vector2 amount)
        {
            return amount * GameSettings.Instance.RenderScale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ScaleForRendering(float amount)
        {
            //So, farseer keeps a small skin around objects (for whatever reason, only god knows)
            //so we have to artificially add the 0.0001 (or whatever) blocks around the object
            return (int)
                ((amount + (2 * GameSettings.Instance.PhysicsCompensationForRendering))
                * GameSettings.Instance.RenderScale);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsVisible(Vector2 pos, Vector2 size, RectangleF viewRect)
        {
            return pos.X < viewRect.Right &&
                pos.Y < viewRect.Bottom &&
                pos.X + size.X > viewRect.Left &&
                pos.Y + size.Y > viewRect.Top;
        }

        #endregion

        void IDisposable.Dispose()
        {
            _effect.Dispose();
            _cache.Dispose();
        }
        public override void Destroy()
        {
            ((IDisposable)this).Dispose();
        }
    }
}