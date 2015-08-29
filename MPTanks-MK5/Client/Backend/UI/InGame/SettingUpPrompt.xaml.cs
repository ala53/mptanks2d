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
        
        private Grid e_30;
        
        private StackPanel e_31;
        
        private TextBlock e_32;
        
        private TextBlock e_33;
        
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
            // e_30 element
            this.e_30 = new Grid();
            this.Content = this.e_30;
            this.e_30.Name = "e_30";
            // e_31 element
            this.e_31 = new StackPanel();
            this.e_30.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
            this.e_31.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_31.VerticalAlignment = VerticalAlignment.Center;
            // e_32 element
            this.e_32 = new TextBlock();
            this.e_31.Children.Add(this.e_32);
            this.e_32.Name = "e_32";
            this.e_32.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_32.Text = "Setting Up Game...";
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_32.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_33 element
            this.e_33 = new TextBlock();
            this.e_31.Children.Add(this.e_33);
            this.e_33.Name = "e_33";
            this.e_33.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_33_Text = new Binding("SecondsRemainingText");
            this.e_33.SetBinding(TextBlock.TextProperty, binding_e_33_Text);
            this.e_33.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
