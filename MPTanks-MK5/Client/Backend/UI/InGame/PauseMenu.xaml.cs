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
    public partial class PauseMenu : UIRoot {
        
        private Grid e_26;
        
        private StackPanel e_27;
        
        private StackPanel e_28;
        
        private TextBlock e_29;
        
        private Border e_30;
        
        private Button ServerInfoBtn;
        
        private Button SettingsBtn;
        
        private Button LeaveServerBtn;
        
        private StackPanel e_31;
        
        public PauseMenu(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            InitializeElementResources(this);
            // e_26 element
            this.e_26 = new Grid();
            this.Content = this.e_26;
            this.e_26.Name = "e_26";
            this.e_26.Background = new SolidColorBrush(new ColorW(128, 128, 128, 127));
            // e_27 element
            this.e_27 = new StackPanel();
            this.e_26.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.Orientation = Orientation.Horizontal;
            // e_28 element
            this.e_28 = new StackPanel();
            this.e_27.Children.Add(this.e_28);
            this.e_28.Name = "e_28";
            this.e_28.Width = 300F;
            // e_29 element
            this.e_29 = new TextBlock();
            this.e_28.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_29.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_29.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_29.Text = "MP Tanks 2D";
            FontManager.Instance.AddFont("Karmatic Arcade", 32F, FontStyle.Regular, "Karmatic_Arcade_24_Regular");
            this.e_29.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_29.FontSize = 32F;
            // e_30 element
            this.e_30 = new Border();
            this.e_28.Children.Add(this.e_30);
            this.e_30.Name = "e_30";
            this.e_30.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.e_30.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_30.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            // ServerInfoBtn element
            this.ServerInfoBtn = new Button();
            this.e_28.Children.Add(this.ServerInfoBtn);
            this.ServerInfoBtn.Name = "ServerInfoBtn";
            this.ServerInfoBtn.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.ServerInfoBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ServerInfoBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
            this.ServerInfoBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.ServerInfoBtn.FontSize = 24F;
            this.ServerInfoBtn.Content = "Server Info";
            // SettingsBtn element
            this.SettingsBtn = new Button();
            this.e_28.Children.Add(this.SettingsBtn);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.SettingsBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.SettingsBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
            this.SettingsBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.SettingsBtn.FontSize = 24F;
            this.SettingsBtn.Content = "Settings";
            // LeaveServerBtn element
            this.LeaveServerBtn = new Button();
            this.e_28.Children.Add(this.LeaveServerBtn);
            this.LeaveServerBtn.Name = "LeaveServerBtn";
            this.LeaveServerBtn.Margin = new Thickness(10F, 0F, 10F, 10F);
            this.LeaveServerBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LeaveServerBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
            this.LeaveServerBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.LeaveServerBtn.FontSize = 24F;
            this.LeaveServerBtn.Content = "Leave Server";
            // e_31 element
            this.e_31 = new StackPanel();
            this.e_27.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
