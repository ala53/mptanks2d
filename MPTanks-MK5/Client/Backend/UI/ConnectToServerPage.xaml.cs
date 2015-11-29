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
    public partial class ConnectToServerPage : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private DockPanel e_3;
        
        private TextBlock e_4;
        
        private TextBox e_5;
        
        private DockPanel e_6;
        
        private TextBlock e_7;
        
        private TextBox e_8;
        
        private Button e_9;
        
        private Button e_10;
        
        public ConnectToServerPage(int width, int height) : 
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
            this.e_2.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_2.Text = "Connect to a server";
            this.e_2.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_2.FontSize = 48F;
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
            this.e_4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_4.Text = "Server Address: ";
            this.e_4.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            this.e_4.FontSize = 16F;
            DockPanel.SetDock(this.e_4, Dock.Left);
            // e_5 element
            this.e_5 = new TextBox();
            this.e_3.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.MinWidth = 200F;
            this.e_5.Margin = new Thickness(10F, 0F, 0F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            DockPanel.SetDock(this.e_5, Dock.Right);
            Binding binding_e_5_Text = new Binding("ServerAddress");
            this.e_5.SetBinding(TextBox.TextProperty, binding_e_5_Text);
            // e_6 element
            this.e_6 = new DockPanel();
            this.e_1.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Margin = new Thickness(10F, 5F, 10F, 0F);
            this.e_6.LastChildFill = true;
            // e_7 element
            this.e_7 = new TextBlock();
            this.e_6.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_7.Text = "Password: ";
            this.e_7.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            this.e_7.FontSize = 16F;
            DockPanel.SetDock(this.e_7, Dock.Left);
            // e_8 element
            this.e_8 = new TextBox();
            this.e_6.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.MinWidth = 200F;
            this.e_8.Margin = new Thickness(10F, 0F, 0F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            DockPanel.SetDock(this.e_8, Dock.Right);
            Binding binding_e_8_Text = new Binding("ServerPassword");
            this.e_8.SetBinding(TextBox.TextProperty, binding_e_8_Text);
            // e_9 element
            this.e_9 = new Button();
            this.e_1.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Margin = new Thickness(10F, 5F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_9.Content = "Connect";
            Binding binding_e_9_Command = new Binding("ConnectButton");
            this.e_9.SetBinding(Button.CommandProperty, binding_e_9_Command);
            this.e_9.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            // e_10 element
            this.e_10 = new Button();
            this.e_1.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Margin = new Thickness(10F, 5F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_10.Content = "Go back";
            Binding binding_e_10_Command = new Binding("GoBackButton");
            this.e_10.SetBinding(Button.CommandProperty, binding_e_10_Command);
            this.e_10.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
