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
        
        private Grid e_27;
        
        private StackPanel e_28;
        
        private TextBlock e_29;
        
        private TextBlock e_30;
        
        private Button e_31;
        
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
            // e_27 element
            this.e_27 = new Grid();
            this.Content = this.e_27;
            this.e_27.Name = "e_27";
            // e_28 element
            this.e_28 = new StackPanel();
            this.e_27.Children.Add(this.e_28);
            this.e_28.Name = "e_28";
            this.e_28.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_28.VerticalAlignment = VerticalAlignment.Center;
            // e_29 element
            this.e_29 = new TextBlock();
            this.e_28.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_29.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_29.Text = "A game is currently active...";
            this.e_29.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_29.FontSize = 48F;
            // e_30 element
            this.e_30 = new TextBlock();
            this.e_28.Children.Add(this.e_30);
            this.e_30.Name = "e_30";
            this.e_30.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_30.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_30.Text = "You\'re in game right now.\nClose the game to get back to the main menu.\nOr, if you" +
                "\'re absolutely sure:";
            this.e_30.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_30.FontSize = 24F;
            // e_31 element
            this.e_31 = new Button();
            this.e_28.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
            this.e_31.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_31.Content = "Click to forcibly close game";
            Binding binding_e_31_Command = new Binding("ForciblyCloseButtonCommand");
            this.e_31.SetBinding(Button.CommandProperty, binding_e_31_Command);
            this.e_31.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
