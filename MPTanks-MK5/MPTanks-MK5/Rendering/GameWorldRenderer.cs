using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MPTanks_MK5.Rendering
{
    class GameWorldRenderer : Renderer
    {
        private GameObject[] _objects;
        private Engine.Rendering.AnimationEngine _animEngine;

        private RectangleF _viewRect;

        private BasicEffect _effect;
        private AssetCache _cache;

        public GameWorldRenderer(GameClient game)
            : base(game)
        {
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
        public void SetAnimations(Engine.Rendering.AnimationEngine engine)
        {
            _animEngine = engine;
        }
        public void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, RectangleF viewRect, GameTime gameTime)
        {
            const float overdraw = 0.15f;

            var _boundsRect = new RectangleF(
                ScaleF(viewRect.X - (viewRect.Width * overdraw)),
                ScaleF(viewRect.Y - (viewRect.Height * overdraw)),
                ScaleF(viewRect.Width * (1 + 2 * overdraw)),
                ScaleF(viewRect.Height * (1 + 2 * overdraw)));

            viewRect = new RectangleF(
                ScaleF(viewRect.X),
                ScaleF(viewRect.Y),
                ScaleF(viewRect.Width),
                ScaleF(viewRect.Height));

            _viewRect = viewRect;

            //compute the projection matrix
            var projectionMatrix = Matrix.Identity *
                Matrix.CreateOrthographicOffCenter(viewRect.Left, viewRect.Right,
                viewRect.Bottom, viewRect.Top, -1, 1);

            //upload the projection matrix
            _effect.Projection = projectionMatrix;

            //First, we cull out off screen stuff and compute the world matrices for what we need
            //to draw so we can do them in bulk
            if (_objects != null)
                foreach (var obj in _objects)
                {
                    //Cache position and size for perf reasons
                    var objPos = Scale(obj.Position);
                    var objSize = Scale(obj.Size);

                    //View culling
                    if (!IsVisible(objPos, objSize, _boundsRect))
                        continue;

                    //Compute the global model matrix
                    var modelMatrix = Matrix.Identity *
                        Matrix.CreateTranslation(
                            new Vector3(-objSize / 2, 0)) *
                        Matrix.CreateRotationZ(obj.Rotation) *
                        Matrix.CreateTranslation(
                            new Vector3(objPos, 0));

                    foreach (var component in obj.Components.Values)
                    {
                        var cmpColor = component.Mask.ToVector4() * obj.ColorMask.ToVector4();
                        var mask = new Color(cmpColor);

                        //upload transform matrix for the object
                        var cmpMatrix = Matrix.CreateScale(component.Scale) *
                            Matrix.CreateTranslation(new Vector3(-Scale(component.RotationOrigin), 0)) *
                            Matrix.CreateRotationZ(component.Rotation) *
                            Matrix.CreateTranslation(
                            new Vector3(Scale(component.RotationOrigin + component.Offset), 0)) *
                            modelMatrix;
                        _effect.World = cmpMatrix;
                        _effect.Alpha = mask.A / 255f;

                        //Start the spritebatch for the object
                        sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                            SamplerState.AnisotropicClamp, DepthStencilState.Default,
                            RasterizerState.CullNone, _effect);


                        var drawRect = new Rectangle(
                            -Scale(Settings.PhysicsCompensationForRendering),
                            -Scale(Settings.PhysicsCompensationForRendering),
                            ScaleForRendering(component.Size.X),
                            ScaleForRendering(component.Size.Y));

                        var asset = _cache.GetArtAsset(component, gameTime);
                        sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, new Color(mask, 255));

                        sb.End();
                    }
                }

            var endedAnimations = new List<Engine.Rendering.Animation>();
            //And render the animations
            if (_animEngine != null)
                foreach (var anim in _animEngine.Animations)
                {
                    //Check if the animation is done
                    if (_cache.AnimEnded(anim.AnimationName, anim.PositionInAnimationMs, anim.SpriteSheetName))
                        endedAnimations.Add(anim);
                    //cull if invisible
                    if (!IsVisible(Scale(anim.Position - (anim.Size / 2)), Scale(anim.Size), viewRect))
                        continue;

                    var animMatrix =
                        Matrix.CreateTranslation(new Vector3(-Scale(anim.Size / 2), 0)) *
                        Matrix.CreateRotationZ(anim.Rotation) *
                        Matrix.CreateTranslation(
                            new Vector3(Scale((anim.Size / 2) + anim.Position), 0));

                    _effect.Alpha = 1;
                    _effect.World = animMatrix;

                    sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                        SamplerState.AnisotropicClamp, DepthStencilState.Default,
                        RasterizerState.CullNone, _effect);

                    var asset = _cache.GetAnimation(anim.AnimationName, anim.PositionInAnimationMs, anim.SpriteSheetName);

                    var drawRect = new Rectangle(
                        Scale(-anim.Size.X / 2),
                        Scale(-anim.Size.Y / 2),
                        Scale(anim.Size.X),
                        Scale(anim.Size.X)
                        );

                    sb.Draw(asset.SpriteSheet.Texture, drawRect, asset.Bounds, Color.White);

                    sb.End();
                }

            //TODO Figure out why the first animation drawn breaks
            foreach (var anim in endedAnimations)
            {
                _animEngine.MarkAnimationCompleted(anim);
            }
        }


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

        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            Render(sb, _viewRect, new GameTime());
        }

        public override void Destroy()
        {

        }
    }
}