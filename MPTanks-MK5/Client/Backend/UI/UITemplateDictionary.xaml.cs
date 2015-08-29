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
    public sealed class UITemplateDictionary : ResourceDictionary {
        
        private static UITemplateDictionary singleton = new UITemplateDictionary();
        
        public UITemplateDictionary() {
            this.InitializeResources();
        }
        
        public static UITemplateDictionary Instance {
            get {
                return singleton;
            }
        }
        
        private void InitializeResources() {
            // Resource - [MenuSubHeader] Style
            Style r_0_s = new Style(typeof(TextBlock));
            Setter r_0_s_S_0 = new Setter(TextBlock.ForegroundProperty, new ResourceReferenceExpression("MenuPageSubHeaderTextColor"));
            r_0_s.Setters.Add(r_0_s_S_0);
            Setter r_0_s_S_1 = new Setter(TextBlock.FontSizeProperty, 30F);
            r_0_s.Setters.Add(r_0_s_S_1);
            this.Add("MenuSubHeader", r_0_s);
            // Resource - [MenuPageSubHeaderTextColor] SolidColorBrush
            this.Add("MenuPageSubHeaderTextColor", new SolidColorBrush(new ColorW(255, 230, 100, 255)));
            // Resource - [SuccessTextColor] SolidColorBrush
            this.Add("SuccessTextColor", new SolidColorBrush(new ColorW(79, 138, 16, 255)));
            // Resource - [MenuPageBGBrush] SolidColorBrush
            this.Add("MenuPageBGBrush", new SolidColorBrush(new ColorW(20, 20, 20, 255)));
            // Resource - [SecondaryButton] Style
            Style r_4_s = new Style(typeof(Button));
            this.Add("SecondaryButton", r_4_s);
            // Resource - [ErrorTextColor] SolidColorBrush
            this.Add("ErrorTextColor", new SolidColorBrush(new ColorW(255, 0, 0, 255)));
            // Resource - [MenuPageContentTextColor] SolidColorBrush
            this.Add("MenuPageContentTextColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
            // Resource - [PrimaryButton] Style
            Style r_7_s = new Style(typeof(Button));
            Setter r_7_s_S_0 = new Setter(Button.FontSizeProperty, 24F);
            r_7_s.Setters.Add(r_7_s_S_0);
            this.Add("PrimaryButton", r_7_s);
            // Resource - [MenuContent] Style
            Style r_8_s = new Style(typeof(TextBlock));
            Setter r_8_s_S_0 = new Setter(TextBlock.ForegroundProperty, new ResourceReferenceExpression("MenuPageContentTextColor"));
            r_8_s.Setters.Add(r_8_s_S_0);
            Setter r_8_s_S_1 = new Setter(TextBlock.FontSizeProperty, 16F);
            r_8_s.Setters.Add(r_8_s_S_1);
            this.Add("MenuContent", r_8_s);
            // Resource - [WarningTextColor] SolidColorBrush
            this.Add("WarningTextColor", new SolidColorBrush(new ColorW(200, 200, 40, 255)));
            // Resource - [MenuPageHeaderTextColor] SolidColorBrush
            this.Add("MenuPageHeaderTextColor", new SolidColorBrush(new ColorW(255, 240, 150, 255)));
            // Resource - [MenuHeader] Style
            Style r_11_s = new Style(typeof(TextBlock));
            Setter r_11_s_S_0 = new Setter(TextBlock.ForegroundProperty, new ResourceReferenceExpression("MenuPageHeaderTextColor"));
            r_11_s.Setters.Add(r_11_s_S_0);
            Setter r_11_s_S_1 = new Setter(TextBlock.FontSizeProperty, 48F);
            r_11_s.Setters.Add(r_11_s_S_1);
            this.Add("MenuHeader", r_11_s);
        }
    }
}
