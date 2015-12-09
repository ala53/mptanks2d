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
    public partial class SpectatorOverlay : UIRoot {
        
        private Grid e_0;
        
        private TextBlock SpectatingHeader;
        
        public SpectatorOverlay() : 
                base() {
            this.Initialize();
        }
        
        public SpectatorOverlay(int width, int height) : 
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
            this.SetResourceReference(UIRoot.BackgroundProperty, "MenuPageBGBrush");
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // SpectatingHeader element
            this.SpectatingHeader = new TextBlock();
            this.e_0.Children.Add(this.SpectatingHeader);
            this.SpectatingHeader.Name = "SpectatingHeader";
            this.SpectatingHeader.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.SpectatingHeader.HorizontalAlignment = HorizontalAlignment.Center;
            this.SpectatingHeader.VerticalAlignment = VerticalAlignment.Top;
            this.SpectatingHeader.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.SpectatingHeader.Text = "Spectating";
            this.SpectatingHeader.FontFamily = new FontFamily("Karmatic Arcade");
            this.SpectatingHeader.FontSize = 48F;
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
