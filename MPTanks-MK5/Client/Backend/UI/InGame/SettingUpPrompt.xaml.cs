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
        
        private Grid e_36;
        
        private StackPanel e_37;
        
        private TextBlock e_38;
        
        private TextBlock e_39;
        
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
            // e_36 element
            this.e_36 = new Grid();
            this.Content = this.e_36;
            this.e_36.Name = "e_36";
            // e_37 element
            this.e_37 = new StackPanel();
            this.e_36.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_37.VerticalAlignment = VerticalAlignment.Center;
            // e_38 element
            this.e_38 = new TextBlock();
            this.e_37.Children.Add(this.e_38);
            this.e_38.Name = "e_38";
            this.e_38.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_38.Text = "Setting Up Game...";
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_38.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_39 element
            this.e_39 = new TextBlock();
            this.e_37.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_39_Text = new Binding("SecondsRemainingText");
            this.e_39.SetBinding(TextBlock.TextProperty, binding_e_39_Text);
            this.e_39.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
