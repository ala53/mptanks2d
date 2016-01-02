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
    public partial class MainMenu : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private StackPanel e_2;
        
        private TextBlock _title;
        
        private TextBlock _subtitle;
        
        private StackPanel e_3;
        
        private StackPanel e_4;
        
        private Button HostBtn;
        
        private Button JoinBtn;
        
        private Button SettingsBtn;
        
        private Button ExitBtn;
        
        private StackPanel e_5;
        
        private Button MapMakerBtn;
        
        private Button ModsPageBtn;
        
        private Button LogOutBtn;
        
        public MainMenu() : 
                base() {
            this.Initialize();
        }
        
        public MainMenu(int width, int height) : 
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
            this.e_1.Orientation = Orientation.Vertical;
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            // _title element
            this._title = new TextBlock();
            this.e_2.Children.Add(this._title);
            this._title.Name = "_title";
            this._title.Margin = new Thickness(20F, 20F, 20F, 0F);
            this._title.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._title.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this._title.Text = "MP Tanks 2D";
            this._title.TextAlignment = TextAlignment.Center;
            this._title.FontFamily = new FontFamily("JHUF");
            this._title.FontSize = 96F;
            // _subtitle element
            this._subtitle = new TextBlock();
            this.e_2.Children.Add(this._subtitle);
            this._subtitle.Name = "_subtitle";
            this._subtitle.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._subtitle.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this._subtitle.Text = "Pre-Alpha Version";
            this._subtitle.TextAlignment = TextAlignment.Center;
            this._subtitle.FontFamily = new FontFamily("JHUF");
            this._subtitle.FontSize = 36F;
            // e_3 element
            this.e_3 = new StackPanel();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Orientation = Orientation.Horizontal;
            // e_4 element
            this.e_4 = new StackPanel();
            this.e_3.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_4.Orientation = Orientation.Vertical;
            // HostBtn element
            this.HostBtn = new Button();
            this.e_4.Children.Add(this.HostBtn);
            this.HostBtn.Name = "HostBtn";
            this.HostBtn.Width = 350F;
            this.HostBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.HostBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.HostBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.HostBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.HostBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.HostBtn.FontFamily = new FontFamily("JHUF");
            this.HostBtn.FontSize = 36F;
            this.HostBtn.Content = "Host game";
            Binding binding_HostBtn_Command = new Binding("HostGameCommand");
            this.HostBtn.SetBinding(Button.CommandProperty, binding_HostBtn_Command);
            // JoinBtn element
            this.JoinBtn = new Button();
            this.e_4.Children.Add(this.JoinBtn);
            this.JoinBtn.Name = "JoinBtn";
            this.JoinBtn.Width = 350F;
            this.JoinBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.JoinBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.JoinBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.JoinBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.JoinBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.JoinBtn.FontFamily = new FontFamily("JHUF");
            this.JoinBtn.FontSize = 36F;
            this.JoinBtn.Content = "Join game";
            Binding binding_JoinBtn_Command = new Binding("JoinGameCommand");
            this.JoinBtn.SetBinding(Button.CommandProperty, binding_JoinBtn_Command);
            // SettingsBtn element
            this.SettingsBtn = new Button();
            this.e_4.Children.Add(this.SettingsBtn);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Width = 350F;
            this.SettingsBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.SettingsBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.SettingsBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.SettingsBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.SettingsBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.SettingsBtn.FontFamily = new FontFamily("JHUF");
            this.SettingsBtn.FontSize = 36F;
            this.SettingsBtn.Content = "Settings";
            Binding binding_SettingsBtn_Command = new Binding("SettingsCommand");
            this.SettingsBtn.SetBinding(Button.CommandProperty, binding_SettingsBtn_Command);
            // ExitBtn element
            this.ExitBtn = new Button();
            this.e_4.Children.Add(this.ExitBtn);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Width = 350F;
            this.ExitBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.ExitBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ExitBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ExitBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ExitBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ExitBtn.FontFamily = new FontFamily("JHUF");
            this.ExitBtn.FontSize = 36F;
            this.ExitBtn.Content = "Exit";
            Binding binding_ExitBtn_Command = new Binding("ExitCommand");
            this.ExitBtn.SetBinding(Button.CommandProperty, binding_ExitBtn_Command);
            // e_5 element
            this.e_5 = new StackPanel();
            this.e_3.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_5.Orientation = Orientation.Vertical;
            // MapMakerBtn element
            this.MapMakerBtn = new Button();
            this.e_5.Children.Add(this.MapMakerBtn);
            this.MapMakerBtn.Name = "MapMakerBtn";
            this.MapMakerBtn.Width = 350F;
            this.MapMakerBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.MapMakerBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.MapMakerBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.MapMakerBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.MapMakerBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.MapMakerBtn.FontFamily = new FontFamily("JHUF");
            this.MapMakerBtn.FontSize = 36F;
            this.MapMakerBtn.Content = "Map Maker";
            Binding binding_MapMakerBtn_Command = new Binding("ExitCommand");
            this.MapMakerBtn.SetBinding(Button.CommandProperty, binding_MapMakerBtn_Command);
            // ModsPageBtn element
            this.ModsPageBtn = new Button();
            this.e_5.Children.Add(this.ModsPageBtn);
            this.ModsPageBtn.Name = "ModsPageBtn";
            this.ModsPageBtn.Width = 350F;
            this.ModsPageBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.ModsPageBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ModsPageBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ModsPageBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ModsPageBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ModsPageBtn.FontFamily = new FontFamily("JHUF");
            this.ModsPageBtn.FontSize = 36F;
            this.ModsPageBtn.Content = "Mods";
            Binding binding_ModsPageBtn_Command = new Binding("ExitCommand");
            this.ModsPageBtn.SetBinding(Button.CommandProperty, binding_ModsPageBtn_Command);
            // LogOutBtn element
            this.LogOutBtn = new Button();
            this.e_5.Children.Add(this.LogOutBtn);
            this.LogOutBtn.Name = "LogOutBtn";
            this.LogOutBtn.Width = 350F;
            this.LogOutBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.LogOutBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LogOutBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LogOutBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.LogOutBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.LogOutBtn.FontFamily = new FontFamily("JHUF");
            this.LogOutBtn.FontSize = 36F;
            this.LogOutBtn.Content = "Log out";
            Binding binding_LogOutBtn_Command = new Binding("ExitCommand");
            this.LogOutBtn.SetBinding(Button.CommandProperty, binding_LogOutBtn_Command);
            FontManager.Instance.AddFont("JHUF", 36F, FontStyle.Regular, "JHUF_27_Regular");
            FontManager.Instance.AddFont("JHUF", 18F, FontStyle.Regular, "JHUF_13.5_Regular");
            FontManager.Instance.AddFont("JHUF", 24F, FontStyle.Regular, "JHUF_18_Regular");
            FontManager.Instance.AddFont("JHUF", 72F, FontStyle.Regular, "JHUF_54_Regular");
            FontManager.Instance.AddFont("JHUF", 96F, FontStyle.Regular, "JHUF_72_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
