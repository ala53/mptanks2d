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
    public partial class ConnectToServerPage : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private DockPanel e_3;
        
        private TextBlock e_4;
        
        private TextBox ServerAddress;
        
        private DockPanel e_5;
        
        private TextBlock e_6;
        
        private TextBox ServerPassword;
        
        private Button ConnectBtn;
        
        private Button GoBackBtn;
        
        public ConnectToServerPage() : 
                base() {
            this.Initialize();
        }
        
        public ConnectToServerPage(int width, int height) : 
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
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Margin = new Thickness(20F, 20F, 20F, 20F);
            this.e_2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_2.Text = "Connect to a server";
            this.e_2.TextAlignment = TextAlignment.Center;
            this.e_2.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_2.FontSize = 36F;
            // e_3 element
            this.e_3 = new DockPanel();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Margin = new Thickness(10F, 0F, 10F, 0F);
            this.e_3.LastChildFill = true;
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_3.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Width = 150F;
            this.e_4.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_4.Text = "Address";
            this.e_4.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_4.TextAlignment = TextAlignment.Left;
            this.e_4.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_4.FontSize = 18F;
            DockPanel.SetDock(this.e_4, Dock.Left);
            // ServerAddress element
            this.ServerAddress = new TextBox();
            this.e_3.Children.Add(this.ServerAddress);
            this.ServerAddress.Name = "ServerAddress";
            this.ServerAddress.MinWidth = 200F;
            this.ServerAddress.Margin = new Thickness(10F, 0F, 0F, 0F);
            this.ServerAddress.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            DockPanel.SetDock(this.ServerAddress, Dock.Right);
            // e_5 element
            this.e_5 = new DockPanel();
            this.e_1.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Margin = new Thickness(10F, 5F, 10F, 0F);
            this.e_5.LastChildFill = true;
            // e_6 element
            this.e_6 = new TextBlock();
            this.e_5.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Width = 150F;
            this.e_6.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_6.Text = "Password";
            this.e_6.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_6.TextAlignment = TextAlignment.Left;
            this.e_6.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_6.FontSize = 18F;
            DockPanel.SetDock(this.e_6, Dock.Left);
            // ServerPassword element
            this.ServerPassword = new TextBox();
            this.e_5.Children.Add(this.ServerPassword);
            this.ServerPassword.Name = "ServerPassword";
            this.ServerPassword.MinWidth = 200F;
            this.ServerPassword.Margin = new Thickness(10F, 0F, 0F, 0F);
            this.ServerPassword.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            DockPanel.SetDock(this.ServerPassword, Dock.Right);
            // ConnectBtn element
            this.ConnectBtn = new Button();
            this.e_1.Children.Add(this.ConnectBtn);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Margin = new Thickness(10F, 10F, 10F, 0F);
            this.ConnectBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ConnectBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ConnectBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.ConnectBtn.FontSize = 36F;
            this.ConnectBtn.Content = "Connect";
            this.ConnectBtn.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            // GoBackBtn element
            this.GoBackBtn = new Button();
            this.e_1.Children.Add(this.GoBackBtn);
            this.GoBackBtn.Name = "GoBackBtn";
            this.GoBackBtn.Margin = new Thickness(10F, 10F, 10F, 0F);
            this.GoBackBtn.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.GoBackBtn.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.GoBackBtn.FontFamily = new FontFamily("Karmatic Arcade");
            this.GoBackBtn.FontSize = 36F;
            this.GoBackBtn.Content = "Go back";
            this.GoBackBtn.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
