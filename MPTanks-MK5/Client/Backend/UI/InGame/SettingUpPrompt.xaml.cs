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
        
        private Grid e_21;
        
        private StackPanel e_22;
        
        private TextBlock e_23;
        
        private TextBlock e_24;
        
        private Button e_25;
        
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
            // e_21 element
            this.e_21 = new Grid();
            this.Content = this.e_21;
            this.e_21.Name = "e_21";
            // e_22 element
            this.e_22 = new StackPanel();
            this.e_21.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_22.VerticalAlignment = VerticalAlignment.Center;
            // e_23 element
            this.e_23 = new TextBlock();
            this.e_22.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            Binding binding_e_23_Text = new Binding("Header");
            this.e_23.SetBinding(TextBlock.TextProperty, binding_e_23_Text);
            this.e_23.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_24 element
            this.e_24 = new TextBlock();
            this.e_22.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_24_Text = new Binding("Content");
            this.e_24.SetBinding(TextBlock.TextProperty, binding_e_24_Text);
            this.e_24.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // e_25 element
            this.e_25 = new Button();
            this.e_22.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            this.e_25.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_25.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_25.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            Binding binding_e_25_Visibility = new Binding("ControlButtonVisibility");
            this.e_25.SetBinding(Button.VisibilityProperty, binding_e_25_Visibility);
            Binding binding_e_25_Content = new Binding("ControlButtonInfo");
            this.e_25.SetBinding(Button.ContentProperty, binding_e_25_Content);
            Binding binding_e_25_Command = new Binding("ControlButtonCommand");
            this.e_25.SetBinding(Button.CommandProperty, binding_e_25_Command);
            this.e_25.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
