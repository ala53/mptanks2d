// -----------------------------------------------------------
//  
//  This file was generated, please do not modify.
//  
// -----------------------------------------------------------
namespace EmptyKeys.UserInterface.Generated {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.ObjectModel;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Charts;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Controls.Primitives;
    using EmptyKeys.UserInterface.Input;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "1.11.0.0")]
    public partial class TankSelectionPromptWithCountdown : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private StackPanel e_2;
        
        private Image Star;
        
        private TextBlock Header;
        
        private TextBlock Subscript;
        
        private TextBlock PlayerList;
        
        public TankSelectionPromptWithCountdown() : 
                base() {
            this.Initialize();
        }
        
        public TankSelectionPromptWithCountdown(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            this.e_0.Background = new SolidColorBrush(new ColorW(0, 0, 0, 159));
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.Orientation = Orientation.Horizontal;
            // Star element
            this.Star = new Image();
            this.e_2.Children.Add(this.Star);
            this.Star.Name = "Star";
            this.Star.Height = 90F;
            this.Star.Width = 90F;
            BitmapImage Star_bm = new BitmapImage();
            Star_bm.TextureAsset = "assets/ui/imgs/star";
            this.Star.Source = Star_bm;
            // Header element
            this.Header = new TextBlock();
            this.e_2.Children.Add(this.Header);
            this.Header.Name = "Header";
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            this.Header.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.Header.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.Header.FontFamily = new FontFamily("Karmatic Arcade");
            this.Header.FontSize = 72F;
            // Subscript element
            this.Subscript = new TextBlock();
            this.e_1.Children.Add(this.Subscript);
            this.Subscript.Name = "Subscript";
            this.Subscript.HorizontalAlignment = HorizontalAlignment.Center;
            this.Subscript.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.Subscript.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.Subscript.FontFamily = new FontFamily("Karmatic Arcade");
            this.Subscript.FontSize = 40F;
            // PlayerList element
            this.PlayerList = new TextBlock();
            this.e_1.Children.Add(this.PlayerList);
            this.PlayerList.Name = "PlayerList";
            this.PlayerList.HorizontalAlignment = HorizontalAlignment.Center;
            this.PlayerList.Foreground = new SolidColorBrush(new ColorW(0, 0, 255, 255));
            this.PlayerList.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.PlayerList.FontFamily = new FontFamily("Karmatic Arcade");
            this.PlayerList.FontSize = 18F;
            ImageManager.Instance.AddImage("assets/ui/imgs/star");
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 72F, FontStyle.Regular, "Karmatic_Arcade_54_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 96F, FontStyle.Regular, "Karmatic_Arcade_72_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 48F, FontStyle.Regular, "Karmatic_Arcade_36_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 40F, FontStyle.Regular, "Karmatic_Arcade_30_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 32F, FontStyle.Regular, "Karmatic_Arcade_24_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 30F, FontStyle.Regular, "Karmatic_Arcade_22.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
