using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MPTanks.Client.Backend.UI;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using MPTanks.Engine.Maps.Serialization;
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
        private bool UI_LockToGrid;
        private bool UI_MousePressed => Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
        private Vector2 UI_MousePosition => new Vector2(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y);
        private bool UI_HasSelectedObject;
        private Vector2 UI_MouseDragOffset;
        private GameObject UI_SelectedObject;
        private void UI_ShowPrimaryMenu()
        {
            _ui.UnwindAndEmpty();
            _ui.GoToPageIfNotThere("mapmakermainmenu", page =>
            {
                page.Element<Button>("LoadMapBtn").Click += (a, b) =>
                {
                    var fd = new System.Windows.Forms.OpenFileDialog();
                    fd.Filter = "MP Tanks 2D map files (*.json)|*.json";
                    if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!File.Exists(fd.FileName))
                            return;
                        //try
                        //{
                        _game = _map.CreateFromMap(MapJSON.Load(
                            File.ReadAllText(fd.FileName)));
                        OnMapChanged();
                        //}
                        // catch
                        //  {
                        _ui.ShowMessageBox("Load error",
                            "An error occurred while loading that map.",
                            UserInterface.MessageBoxType.ErrorMessageBox);
                        //  }
                    }
                };
                page.Element<Button>("SaveMapBtn").Click += (a, b) =>
                {
                    var fd = new System.Windows.Forms.SaveFileDialog();
                    fd.Filter = "MP Tanks 2D map files (*.json)|*.json";
                    if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!Directory.Exists(new FileInfo(fd.FileName).Directory.FullName))
                            return;
                        try
                        { File.WriteAllText(fd.FileName, _map.GenerateMap(_game)); }
                        catch
                        {
                            _ui.ShowMessageBox("Save error!",
                                "An error occurred while saving that map for you.",
                                UserInterface.MessageBoxType.ErrorMessageBox);
                        }
                    }
                };
                page.Element<StackPanel>("ModsListPanel");
                page.Element<Button>("LoadModBtn").Click += (a, b) => UI_LoadMod();
                page.Element<CheckBox>("LockToGridChkBox", elem =>
                {
                    elem.Checked += (a, b) => UI_LockToGrid = true;
                    elem.Unchecked += (a, b) => UI_LockToGrid = false;
                });
                page.Element<TextBox>("SearchBox", elem =>
                {
                    elem.KeyUp += (a, b) =>
                    {
                        var selector = page.Element<StackPanel>("MapObjectSelectorPanel");
                        foreach (var itm in selector.Children)
                        {
                            var info = Engine.Helpers.ReflectionHelper.GetGameObjectInfo((string)itm.Tag);
                            if (info.DisplayName.ToLower().Contains(elem.Text.ToLower()) ||
                            info.DisplayDescription.ToLower().Contains(elem.Text.ToLower()))
                            {
                                //Visible
                                itm.Visibility = EmptyKeys.UserInterface.Visibility.Visible;
                            }
                            else
                            {
                                itm.Visibility = EmptyKeys.UserInterface.Visibility.Collapsed;
                            }
                        }
                    };
                });
                page.Element<StackPanel>("MapObjectSelectorPanel");

                page.Element<Button>("MoreSettingsBtn").Click += (a, b) => UI_ShowMoreSettingsMenu();
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

        private void UI_ShowMoreSettingsMenu()
        {
            _ui.GoToPageIfNotThere("mapmakermoresettings", page =>
            {
                if (_map.MapName != null)
                    page.Element<TextBox>("MapNameBox").Text = _map.MapName;
                if (_map.MapAuthor != null)
                    page.Element<TextBox>("MapAuthorBox").Text = _map.MapAuthor;
                page.Element<TextBox>("MapNameBox").KeyUp += (a, b) =>
                {
                    _map.MapName = (a as TextBox).Text;
                };
                page.Element<TextBox>("MapAuthorBox").KeyUp += (a, b) =>
                {
                    _map.MapAuthor = (a as TextBox).Text;
                };

                page.Element<NumericTextBox>("BackgroundR").Text = _map.BackgroundColor.R.ToString();
                page.Element<NumericTextBox>("BackgroundG").Text = _map.BackgroundColor.G.ToString();
                page.Element<NumericTextBox>("BackgroundB").Text = _map.BackgroundColor.B.ToString();

                page.RegisterUpdater(() =>
                {

                    _map.BackgroundColor.R = (byte)page.Element<NumericTextBox>("BackgroundR").Value;
                    _map.BackgroundColor.G = (byte)page.Element<NumericTextBox>("BackgroundG").Value;
                    _map.BackgroundColor.B = (byte)page.Element<NumericTextBox>("BackgroundB").Value;
                    _game.Map.BackgroundColor = _map.BackgroundColor;

                    var shadowX = page.Element<TextBox>("ShadowX").Text;
                    var shadowY = page.Element<TextBox>("ShadowY").Text;
                    if (!float.TryParse(shadowX, out _map.ShadowOffset.X))
                        page.Element<TextBox>("ShadowX").Text = _map.ShadowOffset.X.ToString();
                    if (!float.TryParse(shadowY, out _map.ShadowOffset.Y))
                        page.Element<TextBox>("ShadowY").Text = _map.ShadowOffset.Y.ToString();
                    _map.ShadowOffset.X = Math.Max(Math.Min(_map.ShadowOffset.X, 10), -10);
                    _map.ShadowOffset.Y = Math.Max(Math.Min(_map.ShadowOffset.Y, 10), -10);

                    _game.Map.ShadowOffset = _map.ShadowOffset;

                    _map.ShadowColor.R = (byte)page.Element<NumericTextBox>("ShadowR").Value;
                    _map.ShadowColor.G = (byte)page.Element<NumericTextBox>("ShadowG").Value;
                    _map.ShadowColor.B = (byte)page.Element<NumericTextBox>("ShadowB").Value;
                    _map.ShadowColor.A = (byte)page.Element<NumericTextBox>("ShadowA").Value;
                    _game.Map.ShadowColor = _map.ShadowColor;
                });

                page.Element<TextBox>("ShadowX").Text = _map.ShadowOffset.X.ToString();
                page.Element<TextBox>("ShadowY").Text = _map.ShadowOffset.Y.ToString();

                page.Element<NumericTextBox>("ShadowR").Text = _map.ShadowColor.R.ToString();
                page.Element<NumericTextBox>("ShadowG").Text = _map.ShadowColor.G.ToString();
                page.Element<NumericTextBox>("ShadowB").Text = _map.ShadowColor.B.ToString();
                page.Element<NumericTextBox>("ShadowA").Text = _map.ShadowColor.A.ToString();

                page.Element<CheckBox>("UseWhitelistCheckBox");
                page.Element<TextBlock>("WhitelistTextBox");
                page.Element<Button>("GoBackBtn").Click += (a, b) => UI_ShowPrimaryMenu();
            });
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
            _ui.GoToPage("mapmakereditobject", page =>
            {
                page.Element<TextBox>("WidthBox").Text = obj.Size.X.ToString();
                page.Element<TextBox>("HeightBox").Text = obj.Size.Y.ToString();

                page.Element<TextBox>("PosXBox").Text = obj.Position.X.ToString();
                page.Element<TextBox>("PosYBox").Text = obj.Position.Y.ToString();

                page.Element<TextBox>("RotationBox").Text = MathHelper.ToDegrees(obj.Rotation).ToString();
                page.Element<TextBox>("DrawLayerBox").Text = obj.DrawLayer.ToString();

                page.Element<TextBox>("WidthBox").KeyUp += (a, b) =>
                {
                    float size;
                    if (float.TryParse(page.Element<TextBox>("WidthBox").Text, out size) && size > 0.01 && size < 10000)
                        obj.Size = new Vector2(size, obj.Size.Y);
                    else page.Element<TextBox>("WidthBox").Text = obj.Size.X.ToString();
                };
                page.Element<TextBox>("HeightBox").KeyUp += (a, b) =>
                {
                    float size;
                    if (float.TryParse(page.Element<TextBox>("HeightBox").Text, out size) && size > 0.01 && size < 10000)
                        obj.Size = new Vector2(obj.Size.X, size);
                    else page.Element<TextBox>("HeightBox").Text = obj.Size.Y.ToString();
                };

                page.Element<TextBox>("PosXBox").KeyUp += (a, b) =>
                {
                    float pos;
                    if (float.TryParse(page.Element<TextBox>("PosXBox").Text, out pos))
                        obj.Position = new Vector2(pos, obj.Position.Y);
                    else page.Element<TextBox>("PosXBox").Text = obj.Position.X.ToString();
                };
                page.Element<TextBox>("PosYBox").KeyUp += (a, b) =>
                {
                    float pos;
                    if (float.TryParse(page.Element<TextBox>("PosYBox").Text, out pos))
                        obj.Position = new Vector2(obj.Position.X, pos);
                    else page.Element<TextBox>("PosYBox").Text = obj.Position.Y.ToString();
                };

                page.Element<TextBox>("RotationBox").KeyUp += (a, b) =>
                {
                    float rotation;
                    if (float.TryParse(page.Element<TextBox>("RotationBox").Text, out rotation))
                        obj.Rotation = MathHelper.ToRadians(rotation);
                    else page.Element<TextBox>("RotationBox").Text = MathHelper.ToDegrees(obj.Rotation).ToString();
                };
                page.Element<TextBox>("DrawLayerBox").KeyUp += (a, b) =>
                {
                    int layer;
                    if (int.TryParse(page.Element<TextBox>("DrawLayerBox").Text, out layer) &&
                        layer > -100000000 && layer < 100000000)
                        obj.DrawLayer = layer;
                    else page.Element<TextBox>("DrawLayerBox").Text = obj.DrawLayer.ToString();
                };
                page.RegisterUpdater(() =>
                {
                    obj.ColorMask = new Color(
                        (byte)page.Element<NumericTextBox>("ColorR").Value,
                        (byte)page.Element<NumericTextBox>("ColorG").Value,
                        (byte)page.Element<NumericTextBox>("ColorB").Value,
                        (byte)page.Element<NumericTextBox>("ColorA").Value);

                });

                page.Element<NumericTextBox>("ColorR").Text = obj.ColorMask.R.ToString();
                page.Element<NumericTextBox>("ColorG").Text = obj.ColorMask.G.ToString();
                page.Element<NumericTextBox>("ColorB").Text = obj.ColorMask.B.ToString();
                page.Element<NumericTextBox>("ColorA").Text = obj.ColorMask.A.ToString();

            });
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
                Microsoft.Xna.Framework.Input.Mouse.GetState().X < 200 ||//Or if we're in the side bar
                UI_SelectedObject == null //If someone was an idiot
                )
                return;

            //Move the object to the mouse center
            UI_SelectedObject.Position = UI_ComputeWorldSpaceFromMouse(UI_MousePosition - UI_MouseDragOffset);
            if (UI_LockToGrid)
            {
                UI_SelectedObject.Position = new Vector2(
                       (float)Math.Round(UI_SelectedObject.Position.X, MidpointRounding.AwayFromZero),
                       (float)Math.Round(UI_SelectedObject.Position.Y, MidpointRounding.AwayFromZero));
            }
        }

        private void UI_LoadMod()
        {
            _ui.ShowMessageBox("Warning!",
                "Loading a mod is irreversible. Once you've added the mod to " +
                "the map, the only way to restore is to reset to a previous save. " +
                "Are you sure you want to continue?",
                UserInterface.MessageBoxType.WarningMessageBox,
                UserInterface.MessageBoxButtons.YesNo,
                a =>
                {
                    if (a == UserInterface.MessageBoxResult.No)
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
                            UserInterface.MessageBoxType.OKMessageBox,
                            UserInterface.MessageBoxButtons.YesNo, b =>
                            {
                                bool fullTrust = (b == UserInterface.MessageBoxResult.Yes);
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
                                        UserInterface.MessageBoxType.ErrorMessageBox);
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
