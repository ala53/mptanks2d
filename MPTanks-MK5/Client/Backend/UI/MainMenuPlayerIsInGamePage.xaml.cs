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
    public partial class MainMenuPlayerIsInGamePage : UIRoot {
        
        private Grid e_22;
        
        private StackPanel e_23;
        
        private TextBlock e_24;
        
        private TextBlock e_25;
        
        private Button ForceCloseBtn;
        
        public MainMenuPlayerIsInGamePage(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            InitializeElementResources(this);
            // e_22 element
            this.e_22 = new Grid();
            this.Content = this.e_22;
            this.e_22.Name = "e_22";
            // e_23 element
            this.e_23 = new StackPanel();
            this.e_22.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_23.VerticalAlignment = VerticalAlignment.Center;
            // e_24 element
            this.e_24 = new TextBlock();
            this.e_23.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_24.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_24.Text = "A game is currently active...";
            this.e_24.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_24.FontSize = 48F;
            // e_25 element
            this.e_25 = new TextBlock();
            this.e_23.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            this.e_25.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_25.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_25.Text = "You\'re in game right now.\nClose the game to get back to the main menu.\nOr, if you" +
                "\'re absolutely sure:";
            this.e_25.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_25.FontSize = 24F;
            // ForceCloseBtn element
            this.ForceCloseBtn = new Button();
            this.e_23.Children.Add(this.ForceCloseBtn);
            this.ForceCloseBtn.Name = "ForceCloseBtn";
            this.ForceCloseBtn.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.ForceCloseBtn.Content = "Click to forcibly close game";
            this.ForceCloseBtn.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
