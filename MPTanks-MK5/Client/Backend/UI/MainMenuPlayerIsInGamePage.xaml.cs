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
        
        private Grid e_20;
        
        private StackPanel e_21;
        
        private TextBlock e_22;
        
        private TextBlock e_23;
        
        private Button e_24;
        
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
            // e_20 element
            this.e_20 = new Grid();
            this.Content = this.e_20;
            this.e_20.Name = "e_20";
            // e_21 element
            this.e_21 = new StackPanel();
            this.e_20.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_21.VerticalAlignment = VerticalAlignment.Center;
            // e_22 element
            this.e_22 = new TextBlock();
            this.e_21.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_22.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_22.Text = "A game is currently active...";
            this.e_22.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_22.FontSize = 48F;
            // e_23 element
            this.e_23 = new TextBlock();
            this.e_21.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_23.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_23.Text = "You\'re in game right now.\nClose the game to get back to the main menu.\nOr, if you" +
                "\'re absolutely sure:";
            this.e_23.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_23.FontSize = 24F;
            // e_24 element
            this.e_24 = new Button();
            this.e_21.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_24.Content = "Click to forcibly close game";
            Binding binding_e_24_Command = new Binding("ForciblyCloseButtonCommand");
            this.e_24.SetBinding(Button.CommandProperty, binding_e_24_Command);
            this.e_24.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
