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
            // Resource - [ErrorTextColor] SolidColorBrush
            this.Add("ErrorTextColor", new SolidColorBrush(new ColorW(255, 0, 0, 255)));
            // Resource - [MenuContent] Style
            Style r_1_s = new Style(typeof(TextBlock));
            Setter r_1_s_S_0 = new Setter(TextBlock.ForegroundProperty, new ResourceReferenceExpression("MenuPageContentTextColor"));
            r_1_s.Setters.Add(r_1_s_S_0);
            Setter r_1_s_S_1 = new Setter(TextBlock.FontSizeProperty, 16F);
            r_1_s.Setters.Add(r_1_s_S_1);
            this.Add("MenuContent", r_1_s);
            // Resource - [MenuHeader] Style
            Style r_2_s = new Style(typeof(TextBlock));
            Setter r_2_s_S_0 = new Setter(TextBlock.ForegroundProperty, new ResourceReferenceExpression("MenuPageHeaderTextColor"));
            r_2_s.Setters.Add(r_2_s_S_0);
            Setter r_2_s_S_1 = new Setter(TextBlock.FontSizeProperty, 48F);
            r_2_s.Setters.Add(r_2_s_S_1);
            this.Add("MenuHeader", r_2_s);
            // Resource - [MenuPageBGBrush] SolidColorBrush
            this.Add("MenuPageBGBrush", new SolidColorBrush(new ColorW(20, 20, 20, 255)));
            // Resource - [MenuPageContentTextColor] SolidColorBrush
            this.Add("MenuPageContentTextColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
            // Resource - [MenuPageHeaderTextColor] SolidColorBrush
            this.Add("MenuPageHeaderTextColor", new SolidColorBrush(new ColorW(255, 240, 150, 255)));
            // Resource - [MenuPageSubHeaderTextColor] SolidColorBrush
            this.Add("MenuPageSubHeaderTextColor", new SolidColorBrush(new ColorW(255, 230, 100, 255)));
            // Resource - [MenuSubHeader] Style
            Style r_7_s = new Style(typeof(TextBlock));
            Setter r_7_s_S_0 = new Setter(TextBlock.ForegroundProperty, new ResourceReferenceExpression("MenuPageSubHeaderTextColor"));
            r_7_s.Setters.Add(r_7_s_S_0);
            Setter r_7_s_S_1 = new Setter(TextBlock.FontSizeProperty, 30F);
            r_7_s.Setters.Add(r_7_s_S_1);
            this.Add("MenuSubHeader", r_7_s);
            // Resource - [PrimaryButton] Style
            Style r_8_s = new Style(typeof(Button));
            Setter r_8_s_S_0 = new Setter(Button.FontSizeProperty, 24F);
            r_8_s.Setters.Add(r_8_s_S_0);
            this.Add("PrimaryButton", r_8_s);
            // Resource - [SecondaryButton] Style
            Style r_9_s = new Style(typeof(Button));
            this.Add("SecondaryButton", r_9_s);
            // Resource - [SuccessTextColor] SolidColorBrush
            this.Add("SuccessTextColor", new SolidColorBrush(new ColorW(79, 138, 16, 255)));
            // Resource - [WarningTextColor] SolidColorBrush
            this.Add("WarningTextColor", new SolidColorBrush(new ColorW(200, 200, 40, 255)));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 72F, FontStyle.Regular, "Karmatic_Arcade_54_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 96F, FontStyle.Regular, "Karmatic_Arcade_72_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 48F, FontStyle.Regular, "Karmatic_Arcade_36_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
        }
    }
}
