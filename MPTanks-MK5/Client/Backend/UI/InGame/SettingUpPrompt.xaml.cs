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
        
        private Grid e_24;
        
        private StackPanel e_25;
        
        private TextBlock e_26;
        
        private TextBlock e_27;
        
        private Button e_28;
        
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
            // e_24 element
            this.e_24 = new Grid();
            this.Content = this.e_24;
            this.e_24.Name = "e_24";
            // e_25 element
            this.e_25 = new StackPanel();
            this.e_24.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            this.e_25.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_25.VerticalAlignment = VerticalAlignment.Center;
            // e_26 element
            this.e_26 = new TextBlock();
            this.e_25.Children.Add(this.e_26);
            this.e_26.Name = "e_26";
            this.e_26.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            Binding binding_e_26_Text = new Binding("Header");
            this.e_26.SetBinding(TextBlock.TextProperty, binding_e_26_Text);
            this.e_26.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_27 element
            this.e_27 = new TextBlock();
            this.e_25.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_27_Text = new Binding("Content");
            this.e_27.SetBinding(TextBlock.TextProperty, binding_e_27_Text);
            this.e_27.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // e_28 element
            this.e_28 = new Button();
            this.e_25.Children.Add(this.e_28);
            this.e_28.Name = "e_28";
            this.e_28.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_28.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_28.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            Binding binding_e_28_Visibility = new Binding("ControlButtonVisibility");
            this.e_28.SetBinding(Button.VisibilityProperty, binding_e_28_Visibility);
            Binding binding_e_28_Content = new Binding("ControlButtonInfo");
            this.e_28.SetBinding(Button.ContentProperty, binding_e_28_Content);
            Binding binding_e_28_Command = new Binding("ControlButtonCommand");
            this.e_28.SetBinding(Button.CommandProperty, binding_e_28_Command);
            this.e_28.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
