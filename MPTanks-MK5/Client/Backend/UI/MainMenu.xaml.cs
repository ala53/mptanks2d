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
        
        private Grid e_28;
        
        private StackPanel e_29;
        
        private StackPanel e_30;
        
        private TextBlock _title;
        
        private TextBlock _subtitle;
        
        private StackPanel e_31;
        
        private Button _hostBtn;
        
        private Button _joinBtn;
        
        private Button _settingsBtn;
        
        private Button _exitBtn;
        
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
            // e_28 element
            this.e_28 = new Grid();
            this.Content = this.e_28;
            this.e_28.Name = "e_28";
            // e_29 element
            this.e_29 = new StackPanel();
            this.e_28.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.Orientation = Orientation.Vertical;
            // e_30 element
            this.e_30 = new StackPanel();
            this.e_29.Children.Add(this.e_30);
            this.e_30.Name = "e_30";
            // _title element
            this._title = new TextBlock();
            this.e_30.Children.Add(this._title);
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
            this.e_30.Children.Add(this._subtitle);
            this._subtitle.Name = "_subtitle";
            this._subtitle.HorizontalAlignment = HorizontalAlignment.Stretch;
            this._subtitle.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this._subtitle.Text = "Pre-Alpha Version";
            this._subtitle.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._subtitle.FontFamily = new FontFamily("Karmatic Arcade");
            this._subtitle.FontSize = 36F;
            // e_31 element
            this.e_31 = new StackPanel();
            this.e_29.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
            this.e_31.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_31.Orientation = Orientation.Vertical;
            // _hostBtn element
            this._hostBtn = new Button();
            this.e_31.Children.Add(this._hostBtn);
            this._hostBtn.Name = "_hostBtn";
            this._hostBtn.Width = 500F;
            this._hostBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this._hostBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._hostBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._hostBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this._hostBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._hostBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this._hostBtn.FontSize = 36F;
            this._hostBtn.Content = "Host game";
            Binding binding__hostBtn_Command = new Binding("HostGameCommand");
            this._hostBtn.SetBinding(Button.CommandProperty, binding__hostBtn_Command);
            // _joinBtn element
            this._joinBtn = new Button();
            this.e_31.Children.Add(this._joinBtn);
            this._joinBtn.Name = "_joinBtn";
            this._joinBtn.Width = 500F;
            this._joinBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this._joinBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._joinBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._joinBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this._joinBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._joinBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this._joinBtn.FontSize = 36F;
            this._joinBtn.Content = "Join game";
            Binding binding__joinBtn_Command = new Binding("JoinGameCommand");
            this._joinBtn.SetBinding(Button.CommandProperty, binding__joinBtn_Command);
            // _settingsBtn element
            this._settingsBtn = new Button();
            this.e_31.Children.Add(this._settingsBtn);
            this._settingsBtn.Name = "_settingsBtn";
            this._settingsBtn.Width = 500F;
            this._settingsBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this._settingsBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._settingsBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._settingsBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this._settingsBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._settingsBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this._settingsBtn.FontSize = 36F;
            this._settingsBtn.Content = "Settings";
            Binding binding__settingsBtn_Command = new Binding("SettingsCommand");
            this._settingsBtn.SetBinding(Button.CommandProperty, binding__settingsBtn_Command);
            // _exitBtn element
            this._exitBtn = new Button();
            this.e_31.Children.Add(this._exitBtn);
            this._exitBtn.Name = "_exitBtn";
            this._exitBtn.Width = 500F;
            this._exitBtn.HorizontalAlignment = HorizontalAlignment.Left;
            this._exitBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._exitBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this._exitBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this._exitBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this._exitBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this._exitBtn.FontSize = 36F;
            this._exitBtn.Content = "Exit";
            Binding binding__exitBtn_Command = new Binding("ExitCommand");
            this._exitBtn.SetBinding(Button.CommandProperty, binding__exitBtn_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
