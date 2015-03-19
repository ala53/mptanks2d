using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ruminate.DataStructures;

namespace Ruminate.GUI.Framework {

    public class Gui {
       
        // Internal System Managers
        internal InputManager InputManager { get; private set; }
        internal RenderManager RenderManager { get; private set; }

        /*####################################################################*/
        /*                           Initialization                           */
        /*####################################################################*/

        public Gui(Game game, Skin defaultSkin, Text defaultText,
            IEnumerable<Tuple<string, Skin>> skins = null, 
            IEnumerable<Tuple<string, Text>> textRenderers = null) {

            InitDom();

            NewState = OldState = new MouseState();

            InputManager = new InputManager(Dom);
            RenderManager = new RenderManager(game.GraphicsDevice);

            SetDefaultSettings(game, defaultSkin, defaultText);

            if (skins != null) {
                foreach (var skin in skins) {
                    AddSkin(skin.Item1, skin.Item2);
                }
            }

            if (textRenderers != null) {
                foreach (var textRenderer in textRenderers) {
                    AddText(textRenderer.Item1, textRenderer.Item2);
                }
            }
        }

        /*####################################################################*/
        /*                            Dom Management                          */
        /*####################################################################*/

        //Dom Management
        private Root<Widget> Dom { get; set; }

        private void InitDom() {
            Dom = new Root<Widget>();
            Dom.OnAttachedToRoot += node => {
                node.DfsOperationChildren(prepareNode => prepareNode.Data.Prepare(this));
                node.DfsOperationChildren(childNode => {
                    if (childNode.Parent != null && childNode.Parent.Root != childNode.Parent) {
                        childNode.Parent.Data.Layout();
                    }
                });
            };
            Dom.OnChildrenChanged += node => node.DfsOperation(innerNode => innerNode.Data.Layout());
        }

        public Widget[] Widgets {
            get {
                return Dom.Children.ConvertAll(input => input.Data).ToArray();
            } set {
                Dom.Children.Clear(); 
                AddWidgets(value);
            }
        }

        public void AddWidget(Widget widget) {
            Dom.AddChild(widget);
        }

        public void AddWidgets(IEnumerable<Widget> widget) {
            Dom.AddChildren(widget);
        }

        public void RemoveWidget(Widget widget) {
            Dom.RemoveChild(widget);
        }

        /*####################################################################*/
        /*                              Settings                              */
        /*####################################################################*/

        #region Settings

        private void SetDefaultSettings(Game game, Skin defaultSkin, Text defaultText) {

            DefaultScrollSpeed = 3;
            DefaultWheelSpeed = 6;

            SelectionColor = new Texture2D(game.GraphicsDevice, 1, 1);
            HighlightingColor = Color.LightSkyBlue * 0.3f;

            AddSkin("Default", defaultSkin);
            DefaultSkin = "Default";
           
            AddText("Default", defaultText);
            DefaultText = "Default";            
        }

        public Rectangle ScreenBounds { get { return RenderManager.GraphicsDevice.Viewport.Bounds; } }

        public int DefaultScrollSpeed { get; set; }
        public int DefaultWheelSpeed { get; set; }

        public Texture2D SelectionColor {
            get { return RenderManager.SelectionColor; }
            set { RenderManager.SelectionColor = value; }
        }

        public Color HighlightingColor {
            get { return RenderManager.HighlightingColor; }
            set { RenderManager.HighlightingColor = value; }
        }

        public string DefaultSkin {
            get { return RenderManager.DefaultSkin; }
            set { RenderManager.DefaultSkin = value; }
        }

        public string DefaultText {
            get { return RenderManager.DefaultText; }
            set { RenderManager.DefaultText = value; }
        }

        public SpriteFont DefaultFont {
            get { return RenderManager.Texts[DefaultText].SpriteFont; }
        }

        public void AddSkin(string name, Skin skin) {
            RenderManager.AddSkin(name, skin);
        }

        public void AddText(string name, Text renderer) {
            RenderManager.AddText(name, renderer);
        }

        #endregion

        /*####################################################################*/
        /*                        Event Based Input                           */
        /*####################################################################*/

        #region Input

        public bool HasMouse { get { return InputManager.HoverWidget != null; } }

        //KeyBoard
        public event CharEnteredHandler CharacterPress {
            add { KeyboardEvents.KeyTyped += value; }
            remove { KeyboardEvents.KeyTyped -= value; }
        }

        public event KeyEventHandler KeyDown {
            add { KeyboardEvents.KeyPressed += value; }
            remove { KeyboardEvents.KeyPressed -= value; }
        }

        public event KeyEventHandler KeyUp {
            add { KeyboardEvents.KeyReleased += value; }
            remove { KeyboardEvents.KeyReleased -= value; }
        }


        //Mouse
        public event MouseEventHandler MouseDoubleClick {
            add { MouseEvents.ButtonDoubleClicked += value; }
            remove { MouseEvents.ButtonDoubleClicked -= value; }
        }

        public event MouseEventHandler MouseDown {
            add { MouseEvents.ButtonPressed += value; }
            remove { MouseEvents.ButtonPressed -= value; }
        }

        public event MouseEventHandler MouseUp {
            add { MouseEvents.ButtonReleased += value; }
            remove { MouseEvents.ButtonReleased -= value; }
        }

        public event MouseEventHandler MouseWheel {
            add { MouseEvents.MouseWheelMoved += value; }
            remove { MouseEvents.MouseWheelMoved -= value; }
        }

        #endregion

        /*####################################################################*/
        /*                             Game Loop                              */
        /*####################################################################*/

        public void Resize() {
                           
            Dom.DfsOperationChildren(node => {                
                node.Data.SissorArea = RenderManager.GraphicsDevice.Viewport.Bounds;
            });

            Dom.DfsOperationChildren(node => node.Data.Layout());
        }

        internal MouseState NewState, OldState;

        public void Update(GameTime time) {            

            NewState = Mouse.GetState();

            InputManager.Update(time, NewState);

            Dom.DfsOperationChildren(node => {
                if (!node.Data.Active) return;
                node.Data.Update();
            });

            OldState = NewState;
        }

        public void Draw() {

            RenderManager.Draw(Dom);
        }
    }
}
