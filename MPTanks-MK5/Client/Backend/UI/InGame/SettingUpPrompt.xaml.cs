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
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "1.6.7.0")]
    public partial class SettingUpPrompt : UIRoot {
        
        private Grid e_32;
        
        private StackPanel e_33;
        
        private TextBlock Header;
        
        private TextBlock ContentT;
        
        private Button ControlButton;
        
        public SettingUpPrompt(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.SetResourceReference(UIRoot.BackgroundProperty, "MenuPageBGBrush");
            InitializeElementResources(this);
            // e_32 element
            this.e_32 = new Grid();
            this.Content = this.e_32;
            this.e_32.Name = "e_32";
            // e_33 element
            this.e_33 = new StackPanel();
            this.e_32.Children.Add(this.e_33);
            this.e_33.Name = "e_33";
            this.e_33.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_33.VerticalAlignment = VerticalAlignment.Center;
            // Header element
            this.Header = new TextBlock();
            this.e_33.Children.Add(this.Header);
            this.Header.Name = "Header";
            this.Header.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            this.Header.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.Header.Text = "";
            FontManager.Instance.AddFont("Karmatic Arcade", 30F, FontStyle.Regular, "Karmatic_Arcade_22.5_Regular");
            this.Header.FontFamily = new FontFamily("Karmatic Arcade");
            this.Header.FontSize = 30F;
            // ContentT element
            this.ContentT = new TextBlock();
            this.e_33.Children.Add(this.ContentT);
            this.ContentT.Name = "ContentT";
            this.ContentT.HorizontalAlignment = HorizontalAlignment.Center;
            this.ContentT.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.ContentT.Text = "";
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.ContentT.FontFamily = new FontFamily("Karmatic Arcade");
            this.ContentT.FontSize = 20F;
            this.ContentT.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // ControlButton element
            this.ControlButton = new Button();
            this.e_33.Children.Add(this.ControlButton);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.ControlButton.HorizontalAlignment = HorizontalAlignment.Center;
            this.ControlButton.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ControlButton.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.ControlButton.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
            this.ControlButton.FontFamily = new FontFamily("Karmatic Arcade");
            this.ControlButton.Content = "";
            this.ControlButton.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
