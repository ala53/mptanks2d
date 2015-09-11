using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Toolkit.SpriteSheets
{
    /// <summary>
    /// Interaction logic for SpriteSheetWindow.xaml
    /// </summary>
    public partial class SpriteSheetWindow : Window
    {
        private bool __hasChanges;
        private bool _hasChanges
        {
            get { return __hasChanges; }
            set
            {
                __hasChanges = value;
                _outFile = _outFile; // force a title update
            }
        }
        private string __outFile = null;
        private string _outFile
        {
            get { return __outFile; }
            set
            {
                __outFile = value;
                if (_hasChanges)
                {
                    if (__outFile != null)
                        Title = $"*MP Tanks Sprite Sheet Editor ({(new FileInfo(__outFile)).Name})";
                    else Title = "*MP Tanks Sprite Sheet Editor (untitled)";
                }
                else
                {
                    if (__outFile != null)
                        Title = $"MP Tanks Sprite Sheet Editor ({(new FileInfo(__outFile)).Name})";
                    else Title = "MP Tanks Sprite Sheet Editor (untitled)";
                }
            }
        }
        private RuntimeSpriteSheet _sheet = new RuntimeSpriteSheet();
        private List<SpritePreview> _activePreviews = new List<SpritePreview>();
        public SpriteSheetWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the window as a dialog and returns the output file 
        /// </summary>
        /// <returns></returns>
        public string ShowAsDialog()
        {
            ShowDialog();
            return _outFile;
        }

        private void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();

            dlg.Filter = "MP Tanks Sprite sheets (*.ssjson)|*.ssjson";

            dlg.ShowDialog();

            if (dlg.FileName == null || !File.Exists(dlg.FileName))
            {
                MessageBox.Show("Chosen file is invalid.");
                return;
            }

            _outFile = dlg.FileName;

            Save(_outFile);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_outFile == null)
                saveAsBtn_Click(sender, e);
            else Save(_outFile);
        }

        private void Save(string filename)
        {
            if (_sheet.Image == null)
            {
                MessageBox.Show("No image has been chosen; cannot save.");
                return;
            }

            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                var saved = _sheet.SerializeWithImage();
                fs.Write(saved, 0, saved.Length);
                fs.Flush();
            }
            _hasChanges = false;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_hasChanges)
            {
                var result = MessageBox.Show("Warning!", "You have unsaved changes. Would you like to save them first?",
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    saveBtn_Click(sender, e);
                }
                if (result == MessageBoxResult.Cancel)
                    return;
            }

            var dlg = new OpenFileDialog();

            dlg.Filter = "MP Tanks Sprite sheets (*.ssjson)|*.ssjson";

            dlg.ShowDialog();

            var path = dlg.FileName;

            if (path == null || !File.Exists(path))
            {
                MessageBox.Show("Error", "Could not load the selected file");
                return; //Couldn't load
            }

            try
            {
                ClearSheet();
                _sheet = RuntimeSpriteSheet.LoadWithImage(File.ReadAllBytes(path));
                _outFile = path;
                ReloadSheet();
            }
            catch
            {
                MessageBox.Show("Something's wrong with the file you chose; we couldn't load it.");
                return;
            }
        }

        private void ClearSheet()
        {
            SpritesList.Children.Clear();
            _outFile = null;
            _sheet = new RuntimeSpriteSheet();
            _activePreviews.Clear();
            ReloadSheet();
        }
        private void ReloadSheet()
        {
            foreach (var sprite in _sheet.Sprites)
            {
                var info = FindByRTSprite(sprite);
                if (info != null)
                {
                    info.UpdateSprite();
                }
                else
                {
                    info = new SpritePreview();
                    info.Sprite = sprite;
                    info.UpdateSprite();
                    info.OnSpritePropertyChanged += (a, e) =>
                    {
                        _hasChanges = true;
                    };
                    info.OnDeleteCalled += (a, e) =>
                    {
                        _hasChanges = true;
                        var sp = _activePreviews.First(b => b == info);
                        SpritesList.Children.Remove(sp);
                        _activePreviews.Remove(sp);
                        _sheet.Sprites.Remove(sp.Sprite);
                        ReloadSheet();
                    };
                    info.OnResizeCalled += (a, e) =>
                    {
                        _hasChanges = true;
                    };
                    _activePreviews.Add(info);
                    SpritesList.Children.Add(info);
                }
            }

            var removables = new List<SpritePreview>();
            foreach (var sprite in _activePreviews)
                if (!_sheet.Sprites.Contains(sprite.Sprite))
                    removables.Add(sprite);

            foreach (var rem in removables)
            {
                _activePreviews.Remove(rem);
                SpritesList.Children.Remove(rem);
            }
        }

        private SpritePreview FindByRTSprite(RuntimeSprite sprite)
        {
            foreach (var child in _activePreviews)
                if (child.Sprite == sprite)
                    return child;

            return null;
        }

        private void SelectSprite(SpritePreview prev = null)
        {
            foreach (var child in SpritesList.Children)
                if (child as SpritePreview != null)
                    (child as SpritePreview).Enabled = false;

            if (prev != null)
                prev.Enabled = true;
        }

        private void DoResize(SpritePreview prev)
        {

        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_hasChanges)
            {
                var result = MessageBox.Show("Warning!", "You have unsaved changes. Would you like to save them first?",
                    MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    saveBtn_Click(sender, e);
                }
                if (result == MessageBoxResult.Cancel)
                    return;
            }
            ClearSheet();

        }

        private void setImgBtn_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = "Images (*.png, *.jpg, *.gif, *.tiff, *.jpeg)|*.jpeg;*.png;*.gif;*.tiff;*.jpeg";
            ofd.ShowDialog();

            if (ofd.FileName == null || !File.Exists(ofd.FileName))
            {
                MessageBox.Show("Could not find the file specified.");
                return;
            }
            try
            {
                _sheet.Image = RuntimeSpriteSheet.DecodePhoto(File.ReadAllBytes(ofd.FileName));
                ReloadSheet();
            }
            catch
            {
                MessageBox.Show("An unspecified error occurred while trying to load the image.");
            }
        }
    }
}
