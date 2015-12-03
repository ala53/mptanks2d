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
    public partial class LoginForm : UIRoot {
        
        private Grid e_8;
        
        private StackPanel e_9;
        
        private StackPanel e_10;
        
        private TextBlock e_11;
        
        private TextBox UsernameBox;
        
        private StackPanel e_12;
        
        private TextBlock e_13;
        
        private PasswordBox PasswordBox;
        
        private Button LoginBtn;
        
        private Button ForgotPasswordBtn;
        
        private Button NoAccountBtn;
        
        public LoginForm(int width, int height) : 
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
            // e_8 element
            this.e_8 = new Grid();
            this.Content = this.e_8;
            this.e_8.Name = "e_8";
            // e_9 element
            this.e_9 = new StackPanel();
            this.e_8.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_9.VerticalAlignment = VerticalAlignment.Center;
            this.e_9.Orientation = Orientation.Vertical;
            // e_10 element
            this.e_10 = new StackPanel();
            this.e_9.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Orientation = Orientation.Horizontal;
            // e_11 element
            this.e_11 = new TextBlock();
            this.e_10.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            this.e_11.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_11.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_11.Text = "Username";
            this.e_11.Padding = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            this.e_11.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_11.FontSize = 18F;
            // UsernameBox element
            this.UsernameBox = new TextBox();
            this.e_10.Children.Add(this.UsernameBox);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Width = 300F;
            this.UsernameBox.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.UsernameBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            // e_12 element
            this.e_12 = new StackPanel();
            this.e_9.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.e_12.Orientation = Orientation.Horizontal;
            // e_13 element
            this.e_13 = new TextBlock();
            this.e_12.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_13.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_13.Text = "Password";
            this.e_13.Padding = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            this.e_13.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_13.FontSize = 18F;
            // PasswordBox element
            this.PasswordBox = new PasswordBox();
            this.e_12.Children.Add(this.PasswordBox);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Width = 300F;
            this.PasswordBox.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.PasswordBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            // LoginBtn element
            this.LoginBtn = new Button();
            this.e_9.Children.Add(this.LoginBtn);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Width = 450F;
            this.LoginBtn.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.LoginBtn.HorizontalAlignment = HorizontalAlignment.Center;
            this.LoginBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LoginBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LoginBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.LoginBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.LoginBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.LoginBtn.FontSize = 36F;
            this.LoginBtn.Content = "Log in";
            Binding binding_LoginBtn_Command = new Binding("HostGameCommand");
            this.LoginBtn.SetBinding(Button.CommandProperty, binding_LoginBtn_Command);
            // ForgotPasswordBtn element
            this.ForgotPasswordBtn = new Button();
            this.e_9.Children.Add(this.ForgotPasswordBtn);
            this.ForgotPasswordBtn.Name = "ForgotPasswordBtn";
            this.ForgotPasswordBtn.Width = 450F;
            this.ForgotPasswordBtn.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.ForgotPasswordBtn.HorizontalAlignment = HorizontalAlignment.Center;
            this.ForgotPasswordBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ForgotPasswordBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ForgotPasswordBtn.Padding = new Thickness(5F, 5F, 5F, 5F);
            this.ForgotPasswordBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            this.ForgotPasswordBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.ForgotPasswordBtn.FontSize = 18F;
            this.ForgotPasswordBtn.Content = "Forgot your password?";
            // NoAccountBtn element
            this.NoAccountBtn = new Button();
            this.e_9.Children.Add(this.NoAccountBtn);
            this.NoAccountBtn.Name = "NoAccountBtn";
            this.NoAccountBtn.Width = 450F;
            this.NoAccountBtn.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.NoAccountBtn.HorizontalAlignment = HorizontalAlignment.Center;
            this.NoAccountBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.NoAccountBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.NoAccountBtn.Padding = new Thickness(5F, 5F, 5F, 5F);
            this.NoAccountBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            this.NoAccountBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.NoAccountBtn.FontSize = 18F;
            this.NoAccountBtn.Content = "No account?  Buy MP Tanks!";
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
