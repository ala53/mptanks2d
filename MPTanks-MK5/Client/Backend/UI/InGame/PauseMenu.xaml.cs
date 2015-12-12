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
    public partial class PauseMenu : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private StackPanel e_2;
        
        private TextBlock e_3;
        
        private Border e_4;
        
        private Button ServerInfoBtn;
        
        private Button SettingsBtn;
        
        private Button LeaveServerBtn;
        
        private StackPanel e_5;
        
        public PauseMenu() : 
                base() {
            this.Initialize();
        }
        
        public PauseMenu(int width, int height) : 
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
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            this.e_0.Background = new SolidColorBrush(new ColorW(0, 0, 0, 159));
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Orientation = Orientation.Horizontal;
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Width = 300F;
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_2.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_3.Text = "MP Tanks 2D";
            this.e_3.FontFamily = new FontFamily("JHUF");
            this.e_3.FontSize = 32F;
            // e_4 element
            this.e_4 = new Border();
            this.e_2.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.e_4.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_4.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            // ServerInfoBtn element
            this.ServerInfoBtn = new Button();
            this.e_2.Children.Add(this.ServerInfoBtn);
            this.ServerInfoBtn.Name = "ServerInfoBtn";
            this.ServerInfoBtn.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.ServerInfoBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ServerInfoBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.ServerInfoBtn.FontFamily = new FontFamily("JHUF");
            this.ServerInfoBtn.FontSize = 24F;
            this.ServerInfoBtn.Content = "Server Info";
            // SettingsBtn element
            this.SettingsBtn = new Button();
            this.e_2.Children.Add(this.SettingsBtn);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.SettingsBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.SettingsBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.SettingsBtn.FontFamily = new FontFamily("JHUF");
            this.SettingsBtn.FontSize = 24F;
            this.SettingsBtn.Content = "Settings";
            // LeaveServerBtn element
            this.LeaveServerBtn = new Button();
            this.e_2.Children.Add(this.LeaveServerBtn);
            this.LeaveServerBtn.Name = "LeaveServerBtn";
            this.LeaveServerBtn.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.LeaveServerBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LeaveServerBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.LeaveServerBtn.FontFamily = new FontFamily("JHUF");
            this.LeaveServerBtn.FontSize = 24F;
            this.LeaveServerBtn.Content = "Leave Server";
            // e_5 element
            this.e_5 = new StackPanel();
            this.e_1.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            FontManager.Instance.AddFont("JHUF", 36F, FontStyle.Regular, "JHUF_27_Regular");
            FontManager.Instance.AddFont("JHUF", 18F, FontStyle.Regular, "JHUF_13.5_Regular");
            FontManager.Instance.AddFont("JHUF", 72F, FontStyle.Regular, "JHUF_54_Regular");
            FontManager.Instance.AddFont("JHUF", 96F, FontStyle.Regular, "JHUF_72_Regular");
            FontManager.Instance.AddFont("JHUF", 48F, FontStyle.Regular, "JHUF_36_Regular");
            FontManager.Instance.AddFont("JHUF", 20F, FontStyle.Regular, "JHUF_15_Regular");
            FontManager.Instance.AddFont("JHUF", 24F, FontStyle.Regular, "JHUF_18_Regular");
            FontManager.Instance.AddFont("JHUF", 40F, FontStyle.Regular, "JHUF_30_Regular");
            FontManager.Instance.AddFont("JHUF", 32F, FontStyle.Regular, "JHUF_24_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
