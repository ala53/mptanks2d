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
        
        private Grid e_17;
        
        private StackPanel e_18;
        
        private TextBlock e_19;
        
        private TextBlock e_20;
        
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
            // e_17 element
            this.e_17 = new Grid();
            this.Content = this.e_17;
            this.e_17.Name = "e_17";
            // e_18 element
            this.e_18 = new StackPanel();
            this.e_17.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            this.e_18.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_18.VerticalAlignment = VerticalAlignment.Center;
            // e_19 element
            this.e_19 = new TextBlock();
            this.e_18.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_19.Text = "A game is currently active...";
            this.e_19.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_19.FontSize = 48F;
            // e_20 element
            this.e_20 = new TextBlock();
            this.e_18.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_20.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_20.Text = "You\'re in game right now.\nClose the game to get back to the main menu.\nOr, if you" +
                "\'re absolutely sure:";
            this.e_20.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_20.FontSize = 24F;
            // ForceCloseBtn element
            this.ForceCloseBtn = new Button();
            this.e_18.Children.Add(this.ForceCloseBtn);
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
