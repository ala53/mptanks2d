using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MPTanks_MK5.Rendering
{
    class GameWorldRenderer
    {
        public GameClient Game { get; private set; }
        private GameObject[] _objects;
        private Engine.Rendering.Animations.AnimationEngine _animEngine;
        private IEnumerable<Engine.Rendering.Particles.Particle> _particles;

        private BasicEffect _effect;
        private AssetCache _cache;

        public GameWorldRenderer(GameClient game)
        {
            Game = game;
            _cache = new AssetCache(game);
            _effect = new BasicEffect(game.GraphicsDevice);
            _effect.View = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            _effect.TextureEnabled = true;
        }

        public void SetObjects(params GameObject[] objects)
        {
            _objects = objects;
        }
        public void SetAnimations(Engine.Rendering.Animations.AnimationEngine engine)
        {
            _animEngine = engine;
        }
        public void SetParticles(IEnumerable<Engine.Rendering.Particles.Particle> particles)
        {
            _particles = particles;
        }
        public void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, RectangleF viewRect, GameTime gameTime)
        {
            const float overdraw = 0.15f;

            var _boundsRect = new RectangleF( //Compute the bounds for visibility checks which have overdraw for simplicity
                ScaleF(viewRect.X - (viewRect.Width * overdraw)),
                ScaleF(viewRect.Y - (viewRect.Height * overdraw)),
                ScaleF(viewRect.Width * (1 + 2 * overdraw)),
                ScaleF(viewRect.Height * (1 + 2 * overdraw)));

            viewRect = new RectangleF( //Scale the view rectangle to world space (for now, 100x)
                ScaleF(viewRect.X),
                ScaleF(viewRect.Y),
                ScaleF(viewRect.Width),
                ScaleF(viewRect.Height));

            //compute the projection matrix to offset the screen view
            var projectionMatrix = Matrix.Identity *
                Matrix.CreateOrthographicOffCenter(viewRect.Left, viewRect.Right,
                viewRect.Bottom, viewRect.Top, -1, 1);

            //upload the projection matrix 
            _effect.Projection = projectionMatrix;

            //Draw game objects
            DrawObjects(_boundsRect, gameTime, sb);
            //And animations
            DrawAnimations(_boundsRect, gameTime, sb);
            //And particles
            DrawParticles(_boundsRect, gameTime, sb);
        }

        private void DrawObjects(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            if (_objects != null)
                foreach (var obj in _objects)
                {
                    //Cache position and size for perf reasons
                    var objPos = Scale(obj.Position);
                    var objSize = Scale(obj.Size);

                    //View culling
                    if (!IsVisible(objPos, objSize, boundsRect))
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
                        //Upload the transformation matrix for the object
                        _effect.World = cmpMatrix;
                        //This is here because the effect requires manual alpha blending for some reason
                        //_effect.Alpha = mask.A / 255f;

                        //Start the spritebatch for the component
                        sb.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied,
                            SamplerState.AnisotropicClamp, DepthStencilState.Default,
                            RasterizerState.CullNone, _effect);
                        //Build a correctly sized rectangle to draw the asset on
                        var drawRect = new Rectangle(
                            -Scale(Settings.PhysicsCompensationForRendering),
                            -Scale(Settings.PhysicsCompensationForRendering),
                            ScaleForRendering(component.Size.X),
                            ScaleForRendering(component.Size.Y));
                        //Get the cached asset
                        var asset = _cache.GetArtAsset(component.SpriteSheetName, component.AssetName, gameTime);
                        //And draw
                        sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, new Color(mask, 255));

                        sb.End();
                    }
                }

        }

        private void DrawAnimations(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            var endedAnimations = new List<Engine.Rendering.Animations.Animation>();
            //And render the animations
            if (_animEngine != null)
                foreach (var anim in _animEngine.Animations)
                {
                    //Check if the animation will be done by next frame (approximately)
                    if (_cache.AnimEnded(anim.AnimationName,
                        anim.PositionInAnimationMs + (float)gameTime.ElapsedGameTime.TotalMilliseconds,
                        anim.SpriteSheetName, anim.LoopCount))
                        endedAnimations.Add(anim);
                    //cull if invisible
                    if (!IsVisible(Scale(anim.Position - (anim.Size / 2)), Scale(anim.Size), boundsRect))
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
                        SamplerState.AnisotropicClamp, DepthStencilState.Default,
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

            foreach (var anim in endedAnimations)
            {
                //Remove all the animations which are ended by now
                _animEngine.MarkAnimationCompleted(anim);
            }
        }

        private void DrawParticles(RectangleF boundsRect, GameTime gameTime, SpriteBatch sb)
        {
            if (_particles == null)
                return;

            _effect.World = Matrix.Identity;
            _effect.Alpha = 1;

            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap,
                DepthStencilState.Default, RasterizerState.CullNone, _effect);

            foreach (var particle in _particles)
            {
                //ignore dead particles
                if (!particle.Alive) continue;

                var pos = Scale(particle.Position);
                var size = Scale(particle.Size);
                //And ignore off screen particles
                if (!IsVisible(pos, size, boundsRect)) continue;
                
                //Get the cached asset
                var asset = _cache.GetArtAsset(particle.SheetName, particle.AssetName, gameTime);
                // A note to future me:
                // I'm aware that the rotation is incorrectly drawn, but to do it correctly requires
                // generating a matrix on the CPU which will be quite processor intensive. For now,
                // this looks good enough and is a bit faster.
                sb.Draw(asset.SpriteSheet.Texture, new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y),
                    asset.Bounds, particle.ColorMask,particle.Rotation, Vector2.Zero, SpriteEffects.None, 0);
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
        public void Destroy()
        {
            _effect.Dispose();
            _cache.Dispose();
        }
    }
}