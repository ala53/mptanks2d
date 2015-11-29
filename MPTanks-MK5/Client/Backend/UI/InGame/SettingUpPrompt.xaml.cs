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
        
        private Grid e_19;
        
        private StackPanel e_20;
        
        private TextBlock e_21;
        
        private TextBlock e_22;
        
        private Button e_23;
        
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
            // e_19 element
            this.e_19 = new Grid();
            this.Content = this.e_19;
            this.e_19.Name = "e_19";
            // e_20 element
            this.e_20 = new StackPanel();
            this.e_19.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_20.VerticalAlignment = VerticalAlignment.Center;
            // e_21 element
            this.e_21 = new TextBlock();
            this.e_20.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            Binding binding_e_21_Text = new Binding("Header");
            this.e_21.SetBinding(TextBlock.TextProperty, binding_e_21_Text);
            this.e_21.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_22 element
            this.e_22 = new TextBlock();
            this.e_20.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_22_Text = new Binding("Content");
            this.e_22.SetBinding(TextBlock.TextProperty, binding_e_22_Text);
            this.e_22.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // e_23 element
            this.e_23 = new Button();
            this.e_20.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_23.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_23.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            Binding binding_e_23_Visibility = new Binding("ControlButtonVisibility");
            this.e_23.SetBinding(Button.VisibilityProperty, binding_e_23_Visibility);
            Binding binding_e_23_Content = new Binding("ControlButtonInfo");
            this.e_23.SetBinding(Button.ContentProperty, binding_e_23_Content);
            Binding binding_e_23_Command = new Binding("ControlButtonCommand");
            this.e_23.SetBinding(Button.CommandProperty, binding_e_23_Command);
            this.e_23.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
