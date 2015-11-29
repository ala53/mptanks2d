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
        
        private Grid e_11;
        
        private StackPanel e_12;
        
        private StackPanel e_13;
        
        private TextBlock _title;
        
        private TextBlock _subtitle;
        
        private StackPanel e_14;
        
        private Button HostBtn;
        
        private Button JoinBtn;
        
        private Button SettingsBtn;
        
        private Button ExitBtn;
        
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
            // e_11 element
            this.e_11 = new Grid();
            this.Content = this.e_11;
            this.e_11.Name = "e_11";
            // e_12 element
            this.e_12 = new StackPanel();
            this.e_11.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Orientation = Orientation.Vertical;
            // e_13 element
            this.e_13 = new StackPanel();
            this.e_12.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            // _title element
            this._title = new TextBlock();
            this.e_13.Children.Add(this._title);
            this._title.Name = "_title";
            this._title.Margin = new Thickness(20F, 20F, 20F, 0F);
            this._title.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._title.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this._title.Text = "MP Tanks 2D";
            this._title.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Karmatic Arcade", 96F, FontStyle.Regular, "Karmatic_Arcade_72_Regular");
            this._title.FontFamily = new FontFamily("Karmatic Arcade");
            this._title.FontSize = 96F;
            // _subtitle element
            this._subtitle = new TextBlock();
            this.e_13.Children.Add(this._subtitle);
            this._subtitle.Name = "_subtitle";
            this._subtitle.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._subtitle.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this._subtitle.Text = "Pre-Alpha Version";
            this._subtitle.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._subtitle.FontFamily = new FontFamily("Karmatic Arcade");
            this._subtitle.FontSize = 36F;
            // e_14 element
            this.e_14 = new StackPanel();
            this.e_12.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            this.e_14.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_14.Orientation = Orientation.Vertical;
            // HostBtn element
            this.HostBtn = new Button();
            this.e_14.Children.Add(this.HostBtn);
            this.HostBtn.Name = "HostBtn";
            this.HostBtn.Width = 500F;
            this.HostBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.HostBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.HostBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.HostBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.HostBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.HostBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.HostBtn.FontSize = 36F;
            this.HostBtn.Content = "Host game";
            Binding binding_HostBtn_Command = new Binding("HostGameCommand");
            this.HostBtn.SetBinding(Button.CommandProperty, binding_HostBtn_Command);
            // JoinBtn element
            this.JoinBtn = new Button();
            this.e_14.Children.Add(this.JoinBtn);
            this.JoinBtn.Name = "JoinBtn";
            this.JoinBtn.Width = 500F;
            this.JoinBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.JoinBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.JoinBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.JoinBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.JoinBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.JoinBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.JoinBtn.FontSize = 36F;
            this.JoinBtn.Content = "Join game";
            Binding binding_JoinBtn_Command = new Binding("JoinGameCommand");
            this.JoinBtn.SetBinding(Button.CommandProperty, binding_JoinBtn_Command);
            // SettingsBtn element
            this.SettingsBtn = new Button();
            this.e_14.Children.Add(this.SettingsBtn);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Width = 500F;
            this.SettingsBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.SettingsBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.SettingsBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.SettingsBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.SettingsBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.SettingsBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.SettingsBtn.FontSize = 36F;
            this.SettingsBtn.Content = "Settings";
            Binding binding_SettingsBtn_Command = new Binding("SettingsCommand");
            this.SettingsBtn.SetBinding(Button.CommandProperty, binding_SettingsBtn_Command);
            // ExitBtn element
            this.ExitBtn = new Button();
            this.e_14.Children.Add(this.ExitBtn);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Width = 500F;
            this.ExitBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.ExitBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ExitBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ExitBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ExitBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.ExitBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.ExitBtn.FontSize = 36F;
            this.ExitBtn.Content = "Exit";
            Binding binding_ExitBtn_Command = new Binding("ExitCommand");
            this.ExitBtn.SetBinding(Button.CommandProperty, binding_ExitBtn_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
