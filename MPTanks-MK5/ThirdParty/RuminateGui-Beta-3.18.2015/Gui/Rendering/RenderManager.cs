using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ruminate.DataStructures;

namespace Ruminate.GUI.Framework {

    public class RenderManager {

        private RasterizerState RasterizerState { get; set; }

        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public string DefaultSkin { get; set; }
        public string DefaultText { get; set; }

        public Dictionary<string, Skin> Skins { get; private set; }
        public Dictionary<string, Text> Texts { get; private set; }

        private DepthStencilState ApplyStencil { get; set; }
        private DepthStencilState SampleStencil { get; set; }

        public Texture2D SelectionColor { get; set; }
        public Color HighlightingColor {
            get {
                var data = new Color[1];
                SelectionColor.GetData(data);
                return data[0];
            }
            set {
                SelectionColor.SetData(new[] { value });
            }
        }

        internal RenderManager(GraphicsDevice device) {

            Skins = new Dictionary<string, Skin>();
            Texts = new Dictionary<string, Text>();

            ApplyStencil = new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            SampleStencil = new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false,
            };

            GraphicsDevice = device;
            RasterizerState = new RasterizerState { ScissorTestEnable = true };
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /*####################################################################*/
        /*                         State Management                           */
        /*####################################################################*/

        internal void Draw(Root<Widget> dom) {

            SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState);

            dom.DfsOperationChildren(node => {
                if (!node.Data.Visible) return;
                GraphicsDevice.ScissorRectangle = node.Data.SissorArea;
                node.Data.Draw();
            });
            
            dom.DfsOperationChildren(node => {
                if (!node.Data.Visible) return;
                GraphicsDevice.ScissorRectangle = node.Data.ClippingArea;
                node.Data.DrawNoClipping();
            });

            SpriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        /*####################################################################*/
        /*                          Skin Management                           */
        /*####################################################################*/

        internal void AddSkin(string name, Skin skin) {
            Skins.Add(name, skin);
        }

        internal void AddText(string name, Text textRenderer) {
            Texts.Add(name, textRenderer);
        }
    }
}
