using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Toolkit.SpriteSheets
{
    /// <summary>
    /// Interaction logic for SpritePreview.xaml
    /// </summary>
    public partial class SpritePreview : UserControl
    {
        public event EventHandler OnResizeCalled = delegate { };
        public event EventHandler OnDeleteCalled = delegate { };
        public event EventHandler OnSpritePropertyChanged = delegate { };

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                IsHitTestVisible = _enabled;
                if (Enabled)
                    Opacity = 1;
                else Opacity = 0.33;
            }
        }
        public ImageSource Image
        {
            get { return image.Source; }
            set
            {
                OnSpritePropertyChanged(this, new EventArgs());
                image.Source = value;
            }
        }
        public string SpriteName { get { return title.Text; } set { title.Text = value; } }
        private Rect _rectangle;
        public Rect Rectangle
        {
            get { return _rectangle; }
            set
            {
                OnSpritePropertyChanged(this, new EventArgs());
                _rectangle = value;
                image.Clip = new RectangleGeometry(_rectangle);
            }
        }
        private RuntimeSprite _sprite;
        internal RuntimeSprite Sprite
        {
            get { return _sprite; }
            set
            {
                _sprite = value;
                UpdateSprite();
            }
        }


        public SpritePreview()
        {
            InitializeComponent();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Enabled)
                if (MessageBox.Show("Warning!", "Are you sure you want to delete this sprite? This cannot be reversed.",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (Sprite != null)
                    {
                        Sprite.Parent.Sprites.Remove(Sprite);
                    }
                    OnDeleteCalled(this, new EventArgs());
                }
        }

        private void resizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Enabled)
                OnResizeCalled(this, new EventArgs());
        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnSpritePropertyChanged(this, new EventArgs());
            if (Sprite != null)
            {
                Sprite.Name = title.Text;
                if (Sprite.Parent.SpriteNameExists(title.Text))
                {
                    title.Foreground = Brushes.Red;
                    title.ToolTip = "Name already exists in this sprite sheet";
                }
                else
                {
                    title.Foreground = Brushes.White;
                    title.ToolTip = null;
                }
            }
        }

        public void UpdateSprite()
        {
            if (_sprite == null) return;
            Rectangle = _sprite.Rectangle;
            Image = _sprite.Parent.Image;
            Name = _sprite.Name;
        }
    }
}
