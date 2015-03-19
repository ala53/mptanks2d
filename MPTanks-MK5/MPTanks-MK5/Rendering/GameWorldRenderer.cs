using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MPTanks_MK5.Rendering
{
    class GameWorldRenderer : Renderer
    {
        private Engine.GameCore _game;

        private BasicEffect _effect;
        private AssetCache _cache;
        private RectangleF _viewRect;

        public GameWorldRenderer(Screens.Screen screen, Engine.GameCore game)
            :base(screen)
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

        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, GameTime gameTime)
        {
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

            //Draw game objects
            DrawObjects(_boundsRect, gameTime, sb);
            //And animations
            DrawAnimations(_boundsRect, gameTime, sb);
            //And particles
            DrawParticles(_boundsRect, gameTime, sb);
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
                    var modelMatrix = Matrix.Identity *
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
                        var cmpMatrix = Matrix.CreateScale(component.Scale) *
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
                -Scale(Settings.PhysicsCompensationForRendering),
                -Scale(Settings.PhysicsCompensationForRendering),
                ScaleForRendering(component.Component.Size.X),
                ScaleForRendering(component.Component.Size.Y));
            //Get the cached asset
            var asset = _cache.GetArtAsset(component.Component.SpriteSheetName, component.Component.AssetName, gameTime);
            //And draw
            sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, component.ComputedColor);

            sb.End();
        }

        private struct RComponentInternal
        {
            public Matrix ComputedTransforms;
            public Engine.Rendering.RenderableComponent Component;
            public Color ComputedColor;
        }
        private class SortListItem
        {
            public int Id;
            public List<RComponentInternal> List = new List<RComponentInternal>();
            public DateTime LastAccessed;
        }
        #endregion

        private List<Engine.Rendering.Animations.Animation> _endedAnimations =
            new List<Engine.Rendering.Animations.Animation>();
        private void DrawAnimations(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            //And render the animations
            if (_game.AnimationEngine.Animations != null)
                foreach (var anim in _game.AnimationEngine.Animations)
                {
                    //Check if the animation will be done by next frame (approximately)
                    if (_cache.AnimEnded(anim.AnimationName,
                        anim.PositionInAnimationMs + (float)gameTime.ElapsedGameTime.TotalMilliseconds,
                        anim.SpriteSheetName, anim.LoopCount))
                        _endedAnimations.Add(anim);
                    //cull if invisible
                    if (!IsVisible(anim.Position - (anim.Size / 2), anim.Size, boundsRect))
                        continue;

                    //Calculate the model's position in the world
                    var animMatrix =
                        Matrix.CreateTranslation(new Vector3(-Scale(anim.Size / 2), 0)) *
                        Matrix.CreateRotationZ(anim.Rotation) *
                        Matrix.CreateTranslation(
                            new Vector3(Scale((anim.Size / 2) + anim.Position), 0));

                    //It doesn't have configurable alpha so set the full opacity
                    _effect.Alpha = 1;
                    //upload the model matrix
                    _effect.World = animMatrix;
                    //Begin the draw call
                    sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,
                        SamplerState.PointWrap, DepthStencilState.Default,
                        RasterizerState.CullNone, _effect);
                    //Load the sprite sheet
                    var asset = _cache.GetAnimation(anim.AnimationName, anim.PositionInAnimationMs, anim.SpriteSheetName);
                    //Compute the draw rectangle
                    var drawRect = new Rectangle(
                        Scale(-anim.Size.X / 2),
                        Scale(-anim.Size.Y / 2),
                        Scale(anim.Size.X),
                        Scale(anim.Size.X)
                        );
                    //And draw
                    sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, Color.White);
                    sb.End();
                }

            foreach (var anim in _endedAnimations)
            {
                //Remove all the animations which are ended by now
                _game.AnimationEngine.MarkAnimationCompleted(anim);
            }

            _endedAnimations.Clear();
        }

        private void DrawParticles(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            if (_game.ParticleEngine.Particles == null)
                return;

            int remainingAllowedParticles = ClientSettings.MaxParticlesToRender;
            _effect.World = Matrix.Identity;
            _effect.Alpha = 1;

            sb.Begin(SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.PointWrap,
                DepthStencilState.Default, RasterizerState.CullNone, _effect);

            foreach (var particle in _game.ParticleEngine.Particles)
            {
                if (remainingAllowedParticles == 0) break; //Stop drawing if too many particles
                //And ignore off screen particles
                if (!IsVisible(particle.Position, particle.Size, boundsRect)) continue;
                var pos = Scale(particle.Position);
                var size = Scale(particle.Size);

                //Get the cached asset
                var asset = _cache.GetArtAsset(particle.SheetName, particle.AssetName, gameTime);
                // A note to future me:
                // I'm aware that the rotation is incorrectly drawn, but to do it correctly requires
                // generating a matrix on the CPU which will be quite processor intensive. For now,
                // this looks good enough and is a bit faster.
                sb.Draw(asset.SpriteSheet.Texture, new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y),
                    asset.Bounds, particle.ColorMask, particle.Rotation, Vector2.Zero, SpriteEffects.None, 0);

                remainingAllowedParticles--; //Mark that another particle was drawn
            }

            sb.End();
        }

        #region Scaling helpers
        private int Scale(float amount)
        {
            return (int)(amount * Settings.RenderScale);
        }
        private float ScaleF(float amount)
        {
            return amount * Settings.RenderScale;
        }

        private Vector2 Scale(Vector2 amount)
        {
            return amount * Settings.RenderScale;
        }

        private int ScaleForRendering(float amount)
        {
            return (int)
                ((amount + (2 * Settings.PhysicsCompensationForRendering))
                * Settings.RenderScale);
        }
        private bool IsVisible(Vector2 pos, Vector2 size, RectangleF viewRect)
        {
            return pos.X < viewRect.Right &&
                pos.Y < viewRect.Bottom &&
                pos.X + size.X > viewRect.Left &&
                pos.Y + size.Y > viewRect.Top;
        }

        #endregion
        public override void Destroy()
        {
            _effect.Dispose();
            _cache.Dispose();
        }
    }
}