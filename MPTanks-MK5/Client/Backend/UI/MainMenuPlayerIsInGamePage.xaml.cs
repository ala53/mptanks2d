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
    public partial class MainMenuPlayerIsInGamePage : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private TextBlock e_3;
        
        private Button ForceCloseBtn;
        
        public MainMenuPlayerIsInGamePage() : 
                base() {
            this.Initialize();
        }
        
        public MainMenuPlayerIsInGamePage(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_2.Text = "A game is running";
            this.e_2.TextAlignment = TextAlignment.Center;
            this.e_2.FontFamily = new FontFamily("JHUF");
            this.e_2.FontSize = 48F;
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_3.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_3.Text = "You\'re in game right now.\nClose the game to get back to the main menu.\nOr, if you" +
                "\'re absolutely sure:";
            this.e_3.TextAlignment = TextAlignment.Center;
            this.e_3.FontFamily = new FontFamily("JHUF");
            this.e_3.FontSize = 20F;
            // ForceCloseBtn element
            this.ForceCloseBtn = new Button();
            this.e_1.Children.Add(this.ForceCloseBtn);
            this.ForceCloseBtn.Name = "ForceCloseBtn";
            this.ForceCloseBtn.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.ForceCloseBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ForceCloseBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ForceCloseBtn.FontFamily = new FontFamily("JHUF");
            this.ForceCloseBtn.Content = "Click to forcibly close game";
            this.ForceCloseBtn.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            FontManager.Instance.AddFont("JHUF", 36F, FontStyle.Regular, "JHUF_27_Regular");
            FontManager.Instance.AddFont("JHUF", 18F, FontStyle.Regular, "JHUF_13.5_Regular");
            FontManager.Instance.AddFont("JHUF", 24F, FontStyle.Regular, "JHUF_18_Regular");
            FontManager.Instance.AddFont("JHUF", 72F, FontStyle.Regular, "JHUF_54_Regular");
            FontManager.Instance.AddFont("JHUF", 96F, FontStyle.Regular, "JHUF_72_Regular");
            FontManager.Instance.AddFont("JHUF", 48F, FontStyle.Regular, "JHUF_36_Regular");
            FontManager.Instance.AddFont("JHUF", 20F, FontStyle.Regular, "JHUF_15_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
