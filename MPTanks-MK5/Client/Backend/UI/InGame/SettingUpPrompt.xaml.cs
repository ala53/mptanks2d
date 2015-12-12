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
    public partial class SettingUpPrompt : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock Header;
        
        private TextBlock ContentT;
        
        private Button ControlButton;
        
        public SettingUpPrompt() : 
                base() {
            this.Initialize();
        }
        
        public SettingUpPrompt(int width, int height) : 
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
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // Header element
            this.Header = new TextBlock();
            this.e_1.Children.Add(this.Header);
            this.Header.Name = "Header";
            this.Header.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            this.Header.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.Header.Text = "";
            this.Header.FontFamily = new FontFamily("JHUF");
            this.Header.FontSize = 30F;
            // ContentT element
            this.ContentT = new TextBlock();
            this.e_1.Children.Add(this.ContentT);
            this.ContentT.Name = "ContentT";
            this.ContentT.HorizontalAlignment = HorizontalAlignment.Center;
            this.ContentT.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.ContentT.Text = "";
            this.ContentT.FontFamily = new FontFamily("JHUF");
            this.ContentT.FontSize = 20F;
            this.ContentT.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // ControlButton element
            this.ControlButton = new Button();
            this.e_1.Children.Add(this.ControlButton);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.ControlButton.HorizontalAlignment = HorizontalAlignment.Center;
            this.ControlButton.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ControlButton.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.ControlButton.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ControlButton.FontFamily = new FontFamily("JHUF");
            this.ControlButton.Content = "";
            this.ControlButton.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            FontManager.Instance.AddFont("JHUF", 36F, FontStyle.Regular, "JHUF_27_Regular");
            FontManager.Instance.AddFont("JHUF", 18F, FontStyle.Regular, "JHUF_13.5_Regular");
            FontManager.Instance.AddFont("JHUF", 72F, FontStyle.Regular, "JHUF_54_Regular");
            FontManager.Instance.AddFont("JHUF", 96F, FontStyle.Regular, "JHUF_72_Regular");
            FontManager.Instance.AddFont("JHUF", 48F, FontStyle.Regular, "JHUF_36_Regular");
            FontManager.Instance.AddFont("JHUF", 20F, FontStyle.Regular, "JHUF_15_Regular");
            FontManager.Instance.AddFont("JHUF", 24F, FontStyle.Regular, "JHUF_18_Regular");
            FontManager.Instance.AddFont("JHUF", 40F, FontStyle.Regular, "JHUF_30_Regular");
            FontManager.Instance.AddFont("JHUF", 32F, FontStyle.Regular, "JHUF_24_Regular");
            FontManager.Instance.AddFont("JHUF", 30F, FontStyle.Regular, "JHUF_22.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
