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
    public partial class LoginForm : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private StackPanel e_3;
        
        private StackPanel e_4;
        
        private TextBlock e_5;
        
        private TextBox UsernameBox;
        
        private StackPanel e_6;
        
        private TextBlock e_7;
        
        private PasswordBox PasswordBox;
        
        private Button LoginBtn;
        
        private Button ForgotPasswordBtn;
        
        private Button NoAccountBtn;
        
        public LoginForm() : 
                base() {
            this.Initialize();
        }
        
        public LoginForm(int width, int height) : 
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
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            this.e_1.Orientation = Orientation.Vertical;
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Margin = new Thickness(10F, 10F, 10F, 30F);
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_2.Text = "MP Tanks 2D";
            this.e_2.FontFamily = new FontFamily("JHUF");
            this.e_2.FontSize = 72F;
            // e_3 element
            this.e_3 = new StackPanel();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_3.VerticalAlignment = VerticalAlignment.Center;
            this.e_3.Orientation = Orientation.Vertical;
            // e_4 element
            this.e_4 = new StackPanel();
            this.e_3.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Orientation = Orientation.Horizontal;
            // e_5 element
            this.e_5 = new TextBlock();
            this.e_4.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Width = 150F;
            this.e_5.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_5.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_5.Text = "Email";
            this.e_5.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_5.FontFamily = new FontFamily("JHUF");
            this.e_5.FontSize = 18F;
            // UsernameBox element
            this.UsernameBox = new TextBox();
            this.e_4.Children.Add(this.UsernameBox);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Width = 300F;
            this.UsernameBox.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.UsernameBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            // e_6 element
            this.e_6 = new StackPanel();
            this.e_3.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.e_6.Orientation = Orientation.Horizontal;
            // e_7 element
            this.e_7 = new TextBlock();
            this.e_6.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Width = 150F;
            this.e_7.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_7.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_7.Text = "Password";
            this.e_7.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_7.FontFamily = new FontFamily("JHUF");
            this.e_7.FontSize = 18F;
            // PasswordBox element
            this.PasswordBox = new PasswordBox();
            this.e_6.Children.Add(this.PasswordBox);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Width = 300F;
            this.PasswordBox.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.PasswordBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            // LoginBtn element
            this.LoginBtn = new Button();
            this.e_3.Children.Add(this.LoginBtn);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Width = 450F;
            this.LoginBtn.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.LoginBtn.HorizontalAlignment = HorizontalAlignment.Center;
            this.LoginBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LoginBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.LoginBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.LoginBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.LoginBtn.FontFamily = new FontFamily("JHUF");
            this.LoginBtn.FontSize = 36F;
            this.LoginBtn.Content = "Log in";
            Binding binding_LoginBtn_Command = new Binding("HostGameCommand");
            this.LoginBtn.SetBinding(Button.CommandProperty, binding_LoginBtn_Command);
            // ForgotPasswordBtn element
            this.ForgotPasswordBtn = new Button();
            this.e_3.Children.Add(this.ForgotPasswordBtn);
            this.ForgotPasswordBtn.Name = "ForgotPasswordBtn";
            this.ForgotPasswordBtn.Width = 450F;
            this.ForgotPasswordBtn.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.ForgotPasswordBtn.HorizontalAlignment = HorizontalAlignment.Center;
            this.ForgotPasswordBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ForgotPasswordBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ForgotPasswordBtn.Padding = new Thickness(5F, 5F, 5F, 5F);
            this.ForgotPasswordBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.ForgotPasswordBtn.FontFamily = new FontFamily("JHUF");
            this.ForgotPasswordBtn.FontSize = 18F;
            this.ForgotPasswordBtn.Content = "Forgot your password?";
            // NoAccountBtn element
            this.NoAccountBtn = new Button();
            this.e_3.Children.Add(this.NoAccountBtn);
            this.NoAccountBtn.Name = "NoAccountBtn";
            this.NoAccountBtn.Width = 450F;
            this.NoAccountBtn.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.NoAccountBtn.HorizontalAlignment = HorizontalAlignment.Center;
            this.NoAccountBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.NoAccountBtn.BorderBrush = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.NoAccountBtn.Padding = new Thickness(5F, 5F, 5F, 5F);
            this.NoAccountBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.NoAccountBtn.FontFamily = new FontFamily("JHUF");
            this.NoAccountBtn.FontSize = 18F;
            this.NoAccountBtn.Content = "No account?  Buy MP Tanks!";
            FontManager.Instance.AddFont("JHUF", 36F, FontStyle.Regular, "JHUF_27_Regular");
            FontManager.Instance.AddFont("JHUF", 18F, FontStyle.Regular, "JHUF_13.5_Regular");
            FontManager.Instance.AddFont("JHUF", 72F, FontStyle.Regular, "JHUF_54_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
