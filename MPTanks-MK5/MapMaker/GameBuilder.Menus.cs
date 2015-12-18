using Microsoft.Xna.Framework;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPTanks.Clients.MapMaker
{
    public partial class GameBuilder
    {
        private void UI_ShowPrimaryMenu()
        {
            _ui.GoToPageIfNotThere("mapmakermainmenu", page =>
            {

            }, (page, state) =>
            {

            });
        }

        private void UI_EditExistingObject(GameObject obj)
        {
            _ui.GoToPageIfNotThere("mapmakereditobject", page =>
            {

            }, (page, state) =>
            {

            }, obj);

        }

        private void UI_LoadMod()
        {
            var fileBrowser = new OpenFileDialog();
            fileBrowser.Filter = "MP Tanks 2D mod files (*.mod)|*.mod";
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                var file = fileBrowser.FileName;
                if (!File.Exists(file)) return;

                //Prompt the user as to whether we should "trust" the mod's code or not
                bool fullTrust = false;
                if (MessageBox.Show(
                    "Should this mod be loaded in a \"full trust\" context? " +
                    "This means the mod will have privileged access to your computer. " +
                    "It is recommended not to do this unless you're sure, for security purposes.",
                    "Full trust?") == DialogResult.Yes)
                    fullTrust = true;

                //Load the mod
                string errors;
                var module = Modding.ModLoader.LoadMod(file,
                    GameSettings.Instance.ModUnpackPath,
                    GameSettings.Instance.ModMapPath,
                    GameSettings.Instance.ModAssetPath,
                    out errors,
                    fullTrust);
                if (module == null)
                {
                    MessageBox.Show("Could not load the specified mod\n\n" + errors, "Fatal mod loader error!");
                    return;
                }

                _map.Mods.Add(module.ModInfo);
                UpdateModsList();
            }
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
