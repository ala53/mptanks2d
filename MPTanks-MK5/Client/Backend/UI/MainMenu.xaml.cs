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
        
        private TextBlock _title;
        
        private TextBlock _subtitle;
        
        private StackPanel e_19;
        
        private StackPanel e_20;
        
        private Button HostBtn;
        
        private Button JoinBtn;
        
        private Button SettingsBtn;
        
        private Button ExitBtn;
        
        private StackPanel e_21;
        
        private Button MapMakerBtn;
        
        private Button ModsPageBtn;
        
        private Button LogOutBtn;
        
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
            // _title element
            this._title = new TextBlock();
            this.e_18.Children.Add(this._title);
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
            this.e_18.Children.Add(this._subtitle);
            this._subtitle.Name = "_subtitle";
            this._subtitle.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._subtitle.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this._subtitle.Text = "Pre-Alpha Version";
            this._subtitle.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._subtitle.FontFamily = new FontFamily("Karmatic Arcade");
            this._subtitle.FontSize = 36F;
            // e_19 element
            this.e_19 = new StackPanel();
            this.e_17.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Orientation = Orientation.Horizontal;
            // e_20 element
            this.e_20 = new StackPanel();
            this.e_19.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_20.Orientation = Orientation.Vertical;
            // HostBtn element
            this.HostBtn = new Button();
            this.e_20.Children.Add(this.HostBtn);
            this.HostBtn.Name = "HostBtn";
            this.HostBtn.Width = 350F;
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
            this.e_20.Children.Add(this.JoinBtn);
            this.JoinBtn.Name = "JoinBtn";
            this.JoinBtn.Width = 350F;
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
            this.e_20.Children.Add(this.SettingsBtn);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Width = 350F;
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
            this.e_20.Children.Add(this.ExitBtn);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Width = 350F;
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
            // e_21 element
            this.e_21 = new StackPanel();
            this.e_19.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_21.Orientation = Orientation.Vertical;
            // MapMakerBtn element
            this.MapMakerBtn = new Button();
            this.e_21.Children.Add(this.MapMakerBtn);
            this.MapMakerBtn.Name = "MapMakerBtn";
            this.MapMakerBtn.Width = 350F;
            this.MapMakerBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.MapMakerBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.MapMakerBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.MapMakerBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.MapMakerBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.MapMakerBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.MapMakerBtn.FontSize = 36F;
            this.MapMakerBtn.Content = "Map Maker";
            Binding binding_MapMakerBtn_Command = new Binding("ExitCommand");
            this.MapMakerBtn.SetBinding(Button.CommandProperty, binding_MapMakerBtn_Command);
            // ModsPageBtn element
            this.ModsPageBtn = new Button();
            this.e_21.Children.Add(this.ModsPageBtn);
            this.ModsPageBtn.Name = "ModsPageBtn";
            this.ModsPageBtn.Width = 350F;
            this.ModsPageBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.ModsPageBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ModsPageBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ModsPageBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ModsPageBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.ModsPageBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.ModsPageBtn.FontSize = 36F;
            this.ModsPageBtn.Content = "Mods";
            Binding binding_ModsPageBtn_Command = new Binding("ExitCommand");
            this.ModsPageBtn.SetBinding(Button.CommandProperty, binding_ModsPageBtn_Command);
            // LogOutBtn element
            this.LogOutBtn = new Button();
            this.e_21.Children.Add(this.LogOutBtn);
            this.LogOutBtn.Name = "LogOutBtn";
            this.LogOutBtn.Width = 350F;
            this.LogOutBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this.LogOutBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LogOutBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LogOutBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.LogOutBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.LogOutBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.LogOutBtn.FontSize = 36F;
            this.LogOutBtn.Content = "Log out";
            Binding binding_LogOutBtn_Command = new Binding("ExitCommand");
            this.LogOutBtn.SetBinding(Button.CommandProperty, binding_LogOutBtn_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
