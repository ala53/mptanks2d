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
    public partial class MainMenu : UIRoot {
        
        private Grid e_16;
        
        private StackPanel e_17;
        
        private StackPanel e_18;
        
        private TextBlock e_19;
        
        private TextBlock e_20;
        
        private StackPanel e_21;
        
        private Button e_22;
        
        private Button e_23;
        
        private Button e_24;
        
        private Button ExitButton;
        
        public MainMenu(int width, int height) : 
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
            // e_16 element
            this.e_16 = new Grid();
            this.Content = this.e_16;
            this.e_16.Name = "e_16";
            // e_17 element
            this.e_17 = new StackPanel();
            this.e_16.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            this.e_17.Orientation = Orientation.Vertical;
            // e_18 element
            this.e_18 = new StackPanel();
            this.e_17.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            // e_19 element
            this.e_19 = new TextBlock();
            this.e_18.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_19.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_19.Text = "MP Tanks 2D";
            this.e_19.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Karmatic Arcade", 96F, FontStyle.Regular, "Karmatic_Arcade_72_Regular");
            this.e_19.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_19.FontSize = 96F;
            // e_20 element
            this.e_20 = new TextBlock();
            this.e_18.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_20.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_20.Text = "Pre-Alpha Version";
            this.e_20.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_20.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_20.FontSize = 36F;
            // e_21 element
            this.e_21 = new StackPanel();
            this.e_17.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_21.Orientation = Orientation.Vertical;
            // e_22 element
            this.e_22 = new Button();
            this.e_21.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Width = 500F;
            this.e_22.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_22.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            this.e_22.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_22.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_22.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_22.FontSize = 36F;
            this.e_22.Content = "Host game";
            Binding binding_e_22_Command = new Binding("HostGameCommand");
            this.e_22.SetBinding(Button.CommandProperty, binding_e_22_Command);
            // e_23 element
            this.e_23 = new Button();
            this.e_21.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Width = 500F;
            this.e_23.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_23.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            this.e_23.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_23.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_23.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_23.FontSize = 36F;
            this.e_23.Content = "Join game";
            Binding binding_e_23_Command = new Binding("JoinGameCommand");
            this.e_23.SetBinding(Button.CommandProperty, binding_e_23_Command);
            // e_24 element
            this.e_24 = new Button();
            this.e_21.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.Width = 500F;
            this.e_24.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_24.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            this.e_24.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_24.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_24.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_24.FontSize = 36F;
            this.e_24.Content = "Settings";
            Binding binding_e_24_Command = new Binding("SettingsCommand");
            this.e_24.SetBinding(Button.CommandProperty, binding_e_24_Command);
            // ExitButton element
            this.ExitButton = new Button();
            this.e_21.Children.Add(this.ExitButton);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Width = 500F;
            this.ExitButton.HorizontalAlignment = HorizontalAlignment.Left;
            this.ExitButton.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            this.ExitButton.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ExitButton.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.ExitButton.FontFamily = new FontFamily("Karmatic Arcade");
            this.ExitButton.FontSize = 36F;
            this.ExitButton.Content = "Exit";
            Binding binding_ExitButton_Command = new Binding("ExitCommand");
            this.ExitButton.SetBinding(Button.CommandProperty, binding_ExitButton_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
