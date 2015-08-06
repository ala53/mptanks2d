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
        
        private TextBlock e_34;
        
        private TextBlock e_35;
        
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
            // e_34 element
            this.e_34 = new TextBlock();
            this.e_33.Children.Add(this.e_34);
            this.e_34.Name = "e_34";
            this.e_34.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_34.Text = "Setting Up Game...";
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_34.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_35 element
            this.e_35 = new TextBlock();
            this.e_33.Children.Add(this.e_35);
            this.e_35.Name = "e_35";
            this.e_35.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_35_Text = new Binding("SecondsRemainingText");
            this.e_35.SetBinding(TextBlock.TextProperty, binding_e_35_Text);
            this.e_35.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
