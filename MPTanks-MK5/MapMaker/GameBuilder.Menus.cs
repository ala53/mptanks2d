using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MPTanks.Client.Backend.UI;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Clients.MapMaker
{
    public partial class GameBuilder
    {
        private bool UI_MousePressed => Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
        private Vector2 UI_MousePosition => new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        private bool UI_HasSelectedObject;
        private Vector2 UI_MouseDragOffset;
        private GameObject UI_SelectedObject;
        private void UI_ShowPrimaryMenu()
        {
            _ui.UnwindAndEmpty();
            _ui.GoToPageIfNotThere("mapmakermainmenu", page =>
            {
                page.Element<Button>("LoadMapBtn");
                page.Element<Button>("SaveMapBtn");
                page.Element<Button>("GenerateMapBtn").Click += (a, b) =>
                {
                    File.WriteAllText("mpa.json", _map.GenerateMap(_game));
                };
                page.Element<StackPanel>("ModsListPanel");
                page.Element<Button>("LoadModBtn");
                page.Element<CheckBox>("LockToGridChkBox");
                page.Element<TextBox>("SearchBox");
                page.Element<StackPanel>("MapObjectSelectorPanel");
            }, (page, state) =>
            {
                //update types
                var modListPanel = page.Element<StackPanel>("ModsListPanel");
                foreach (var mod in _map.Mods)
                {
                    var existing = modListPanel.Children.FirstOrDefault(a => a.Tag.Equals(mod));
                    if (existing != null)
                    {
                        //it exists, ignore it
                    }
                    else
                    {
                        var tblk = new TextBlock();
                        tblk.Tag = mod;
                        tblk.Text = mod.ModName + "(v" + mod.ModMajor + "." + mod.ModMinor + ")";
                        tblk.FontFamily = new FontFamily("JHUF");
                        tblk.FontSize = 12;
                        tblk.Foreground = new SolidColorBrush(ColorW.White);
                        tblk.HorizontalContentAlignment = EmptyKeys.UserInterface.HorizontalAlignment.Center;
                        tblk.Padding = new EmptyKeys.UserInterface.Thickness(5);
                        modListPanel.Children.Add(tblk);
                    }
                }
                //update map object types
                var selector = page.Element<StackPanel>("MapObjectSelectorPanel");
                //get all map objects and their information
                foreach (var mapObjReflName in Engine.Maps.MapObjects.MapObject.AvailableTypes.Keys)
                {
                    var info = Engine.Helpers.ReflectionHelper.GetGameObjectInfo(mapObjReflName);
                    var existing = (Button)selector.Children.FirstOrDefault(a => a.Tag as string == mapObjReflName);
                    if (existing != null)
                    {
                        //It exists, ignore it
                    }
                    else
                    {
                        //create one
                        var btn = new Button();
                        var panel = new StackPanel();
                        var head = new TextBlock();
                        var desc = new TextBlock();

                        head.FontFamily = new FontFamily("JHUF");
                        head.FontSize = 12;
                        head.Text = info.DisplayName;
                        head.Width = 175;
                        desc.FontFamily = new FontFamily("JHUF");
                        desc.FontSize = 12;
                        desc.Text = UserInterface.SplitStringIntoLines(info.DisplayDescription, 28);
                        desc.Width = 175;
                        desc.HorizontalAlignment = EmptyKeys.UserInterface.HorizontalAlignment.Center;
                        desc.Foreground = new SolidColorBrush(new ColorW(Color.Gray.R, Color.Gray.G, Color.Gray.B));

                        panel.Orientation = EmptyKeys.UserInterface.Orientation.Vertical;
                        panel.Width = 175;
                        panel.Children.Add(head);
                        panel.Children.Add(desc);

                        btn.Content = panel;
                        btn.Width = 175;

                        btn.Tag = mapObjReflName;
                        btn.Padding = new EmptyKeys.UserInterface.Thickness(0, 0, 0, 10);

                        btn.Click += (a, b) =>
                        {
                            //Spawn and select an object
                            var reflName = (string)btn.Tag;
                            var obj = _game.AddMapObject(reflName);
                            obj.Position = UI_ComputeWorldSpaceFromMouse(
                                new Vector2(
                                    Window.ClientBounds.Width / 2,
                                    Window.ClientBounds.Height / 2
                                ));
                        };

                        selector.Children.Add(btn);
                    }
                }
            });

            _ui.UpdateState(new object());
        }

        private void UI_PrimaryMenu_SpawnItem(string reflectionName)
        {

        }

        private void UI_ProcessClickInGameArea(Vector2 clickArea)
        {
            if (clickArea.X < 200) //it's in the sidebar
                return;

            //Check if it's intersecting an object
            var obj = _game.GetIntersectingGameObject(UI_ComputeWorldSpaceFromMouse(clickArea));
            if (obj == null)
            {
                //Click with nothing selected. Let's go back to the main menu
                UI_ShowPrimaryMenu();
                UI_SelectedObject = null;
                UI_HasSelectedObject = false;
                return;
            }

            //Mark a selection
            UI_HasSelectedObject = true;
            UI_MouseDragOffset = clickArea - UI_ComputeScreenSpaceFromWorld(obj.Position);
            UI_SelectedObject = obj;

            UI_IsDragging = true;

            //And switch to the selection menu
            UI_EditExistingObject(obj);
        }

        private Vector2 UI_ComputeWorldSpaceFromMouse(Vector2 mouseClickPoint)
        {
            var viewRect = ComputeDrawRectangle();
            var gameClick = mouseClickPoint;
            //Scale down
            gameClick.X *= (viewRect.Width / GraphicsDevice.Viewport.Width);
            gameClick.Y *= (viewRect.Height / GraphicsDevice.Viewport.Height);
            //And move to center
            gameClick += viewRect.TopLeft;
            return gameClick;
        }

        private Vector2 UI_ComputeScreenSpaceFromWorld(Vector2 worldPoint)
        {
            var viewRect = ComputeDrawRectangle();
            var gameClick = worldPoint;
            //And move to center
            gameClick -= viewRect.TopLeft;
            //Scale up
            gameClick.X *= (GraphicsDevice.Viewport.Width / viewRect.Width);
            gameClick.Y *= (GraphicsDevice.Viewport.Height / viewRect.Height);
            return gameClick;
        }

        private void UI_EditExistingObject(GameObject obj)
        {
            _ui.GoToPageIfNotThere("mapmakereditobject", page =>
            {

            }, (page, state) =>
            {

            });

            _ui.UpdateState(obj);
        }

        private void UI_MouseReleased()
        {
            UI_IsDragging = false;
        }

        private void UI_EscapePressed()
        {
            UI_SelectedObject = null;
            UI_HasSelectedObject = false;
            UI_IsDragging = false;
            UI_ShowPrimaryMenu();
        }

        private void UI_Update(GameTime gameTime)
        {
            UI_ProcessDraggingObject();
        }

        private bool UI_IsDragging;
        private void UI_ProcessDraggingObject()
        {
            if (!UI_MousePressed || //If the mouse was released
                !UI_HasSelectedObject || //If no object is selected
                !UI_IsDragging ||
                Mouse.GetState().X < 200 ||//Or if we're in the side bar
                UI_SelectedObject == null //If someone was an idiot
                )
                return;

            //Move the object to the mouse center
            UI_SelectedObject.Position = UI_ComputeWorldSpaceFromMouse(UI_MousePosition - UI_MouseDragOffset);
        }

        private void UI_LoadMod()
        {
            _ui.ShowMessageBox("Warning!",
                "Loading a mod is irreversible. Once you've added the mod to " +
                "the map, the only way to restore is to reset to a previous save. " +
                "Are you sure you want to continue?",
                Client.Backend.UI.UserInterface.MessageBoxType.WarningMessageBox,
                Client.Backend.UI.UserInterface.MessageBoxButtons.YesNo,
                a =>
                {
                    if (a == Client.Backend.UI.UserInterface.MessageBoxResult.No)
                        return;
                    var fileBrowser = new System.Windows.Forms.OpenFileDialog();
                    fileBrowser.Filter = "MP Tanks 2D mod files (*.mod)|*.mod";
                    if (fileBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var file = fileBrowser.FileName;
                        if (!File.Exists(file)) return;

                        //Prompt the user as to whether we should "trust" the mod's code or not
                        _ui.ShowMessageBox("Full trust?",
                            "Should this mod be loaded in a \"full trust\" context? " +
                            "This means the mod will have privileged access to your computer. " +
                            "It is recommended not to do this unless you're sure, for security purposes.",
                            Client.Backend.UI.UserInterface.MessageBoxType.OKMessageBox,
                            Client.Backend.UI.UserInterface.MessageBoxButtons.YesNo, b =>
                            {
                                bool fullTrust = (b == Client.Backend.UI.UserInterface.MessageBoxResult.Yes);
                                //Load the mod
                                string errors;
                                var module = Modding.ModLoader.LoadCompressedModFile(file,
                                    GameSettings.Instance.ModUnpackPath,
                                    GameSettings.Instance.ModMapPath,
                                    GameSettings.Instance.ModAssetPath,
                                    out errors,
                                    fullTrust);
                                if (module == null)
                                {
                                    Logger.Error("MOD LOADING ERROR (NON FATAL)");
                                    Logger.Error(errors);
                                    _ui.ShowMessageBox("Mod loading error",
                                        "Some errors occurred while loading the mod you selected. The errors have been written to the log.",
                                        Client.Backend.UI.UserInterface.MessageBoxType.ErrorMessageBox);
                                    return;
                                }
                                //If successful:
                                _map.Mods.Add(module.ModInfo);
                                UpdateModsList();
                            });

                    }
                });
        }

        private void UI_DrawXYPositionAndZoom()
        {
            var rect = ComputeDrawRectangle();
            var inf = $"Camera: \nCenter: {_cameraPosition.X.ToString("N1")}, {_cameraPosition.Y.ToString("N1")}\n" +
                $"View: L: {rect.Left.ToString("N0")}, R: {rect.Right.ToString("N0")}, T: {rect.Top.ToString("N0")}, B: {rect.Bottom.ToString("N0")}\n" +
                $"Zoom: {_cameraZoom.ToString("N2")}\n" +
                $"Lines every {GridLineBlockSize()} blocks";
            var size = _ui.DefaultFont.MeasureString(inf);
            //10,10 from top right
            var pos = new Vector2(Window.ClientBounds.Right - 10 - size.X, Window.ClientBounds.Top + 10);

            _sb.Begin();
            _sb.DrawString(_ui.DefaultFont, inf, pos - new Vector2(1), Color.Black);
            _sb.DrawString(_ui.DefaultFont, inf, pos - new Vector2(-1), Color.Black);
            _sb.DrawString(_ui.DefaultFont, inf, pos, Color.White);
            _sb.End();
        }
    }
}
