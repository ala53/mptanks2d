using MPTanks_MK4.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.Rendering
{
    class FrameRenderer
    {
        struct DrawObject
        {
            public Rectangle Rectangle;
            public Rectangle Sprite;
            public Matrix4 Matrix;
            public Color4 Mask;
        }
        public void Render(Vector2 offset, Vector2 size, float rotation, params GameObjects.GameObject[] Objects)
        {
            //First, compute the viewport
            var viewMatrix = Matrix4.CreateOrthographicOffCenter(
                offset.X, offset.X + size.X, offset.Y + size.Y, offset.Y, 0.01f, 1);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref viewMatrix);

            //Second, run all the animation loops
            foreach (var obj in Objects)
                foreach (var component in obj.Components)
                    component.CurrentSprite = component.CurrentAnimationState.GetNextFrame();

            //Third, sort all of the components by sprite sheet so we can have smart batching
            var componentBatches = new Dictionary<Sprites.SpriteSheet, List<DrawObject>>();
            
            foreach (var obj in Objects)
                foreach (var component in obj.Components)
                    if (component.Visible)
                    {
                        if (componentBatches.ContainsKey(component.CurrentSprite.Sheet))
                            componentBatches[component.CurrentSprite.Sheet].Add(new DrawObject()
                            {
                                Rectangle = component.CurrentSprite.Rectangle,
                                Sprite = component.CurrentSprite.Normalized,
                                Matrix = component.Offset * obj.Matrix,
                                Mask = component.Mask
                            });
                        else
                            componentBatches.Add(component.CurrentSprite.Sheet,
                                new List<DrawObject>(new[] { new DrawObject () {
                                Rectangle = component.CurrentSprite.Rectangle, 
                                Sprite = component.CurrentSprite.Normalized,
                                Matrix = component.Offset * obj.Matrix,
                                Mask = component.Mask
                                }}));
                    }

            //Fourth, draw all of the objects with some batching
            foreach (var coll in componentBatches)
            {
                BindSheet(coll.Key);

                foreach (var obj in coll.Value)
                {
                    DrawComponent(obj);
                }

                EndDraw();
            }
        }

        private void BindSheet(Sprites.SpriteSheet spritesheet)
        {
            GL.BindTexture(TextureTarget.Texture2D, spritesheet.Texture.GLTextureId);
            GL.Begin(BeginMode.Quads);
        }

        private void DrawComponent(DrawObject component)
        {
            GL.Color4(component.Mask);

            var x = component.Rectangle.X / 2;
            var y = component.Rectangle.Y / 2;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref component.Matrix);

            GL.TexCoord2(component.Sprite.Position);
            GL.Vertex2(new Vector2(-x, -y));
            GL.TexCoord2(component.Sprite.Position + new Vector2(0, component.Sprite.Size.Y));
            GL.Vertex2(new Vector2(-x, y));
            GL.TexCoord2(component.Sprite.Position + component.Sprite.Size);
            GL.Vertex2(new Vector2(x, y));
            GL.TexCoord2(component.Sprite.Position + new Vector2(component.Sprite.Size.X, 0));
            GL.Vertex2(new Vector2(x, -y));
        }

        private void EndDraw()
        {
            GL.End();
        }
    }
}

