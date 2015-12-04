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
        
        private Grid e_26;
        
        private StackPanel e_27;
        
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
            // e_26 element
            this.e_26 = new Grid();
            this.Content = this.e_26;
            this.e_26.Name = "e_26";
            // e_27 element
            this.e_27 = new StackPanel();
            this.e_26.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_27.VerticalAlignment = VerticalAlignment.Center;
            // Header element
            this.Header = new TextBlock();
            this.e_27.Children.Add(this.Header);
            this.Header.Name = "Header";
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.Header.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // ContentT element
            this.ContentT = new TextBlock();
            this.e_27.Children.Add(this.ContentT);
            this.ContentT.Name = "ContentT";
            this.ContentT.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            this.ContentT.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // ControlButton element
            this.ControlButton = new Button();
            this.e_27.Children.Add(this.ControlButton);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.ControlButton.HorizontalAlignment = HorizontalAlignment.Center;
            this.ControlButton.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.ControlButton.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
