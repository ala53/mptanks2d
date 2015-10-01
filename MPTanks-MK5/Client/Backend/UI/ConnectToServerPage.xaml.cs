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
        
        private Grid e_12;
        
        private StackPanel e_13;
        
        private TextBlock e_14;
        
        private DockPanel e_15;
        
        private TextBlock e_16;
        
        private TextBox e_17;
        
        private DockPanel e_18;
        
        private TextBlock e_19;
        
        private TextBox e_20;
        
        private Button e_21;
        
        private Button e_22;
        
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
            // e_12 element
            this.e_12 = new Grid();
            this.Content = this.e_12;
            this.e_12.Name = "e_12";
            // e_13 element
            this.e_13 = new StackPanel();
            this.e_12.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_13.VerticalAlignment = VerticalAlignment.Center;
            // e_14 element
            this.e_14 = new TextBlock();
            this.e_13.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            this.e_14.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_14.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_14.Text = "Connect to a server";
            this.e_14.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_14.FontSize = 48F;
            // e_15 element
            this.e_15 = new DockPanel();
            this.e_13.Children.Add(this.e_15);
            this.e_15.Name = "e_15";
            this.e_15.Margin = new Thickness(10F, 0F, 10F, 0F);
            this.e_15.LastChildFill = true;
            // e_16 element
            this.e_16 = new TextBlock();
            this.e_15.Children.Add(this.e_16);
            this.e_16.Name = "e_16";
            this.e_16.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_16.Text = "Server Address: ";
            this.e_16.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            this.e_16.FontSize = 16F;
            DockPanel.SetDock(this.e_16, Dock.Left);
            // e_17 element
            this.e_17 = new TextBox();
            this.e_15.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            this.e_17.MinWidth = 200F;
            this.e_17.Margin = new Thickness(10F, 0F, 0F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            DockPanel.SetDock(this.e_17, Dock.Right);
            Binding binding_e_17_Text = new Binding("ServerAddress");
            this.e_17.SetBinding(TextBox.TextProperty, binding_e_17_Text);
            // e_18 element
            this.e_18 = new DockPanel();
            this.e_13.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            this.e_18.Margin = new Thickness(10F, 5F, 10F, 0F);
            this.e_18.LastChildFill = true;
            // e_19 element
            this.e_19 = new TextBlock();
            this.e_18.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_19.Text = "Password: ";
            this.e_19.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            this.e_19.FontSize = 16F;
            DockPanel.SetDock(this.e_19, Dock.Left);
            // e_20 element
            this.e_20 = new TextBox();
            this.e_18.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.MinWidth = 200F;
            this.e_20.Margin = new Thickness(10F, 0F, 0F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            DockPanel.SetDock(this.e_20, Dock.Right);
            Binding binding_e_20_Text = new Binding("ServerPassword");
            this.e_20.SetBinding(TextBox.TextProperty, binding_e_20_Text);
            // e_21 element
            this.e_21 = new Button();
            this.e_13.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.Margin = new Thickness(10F, 5F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_21.Content = "Connect";
            Binding binding_e_21_Command = new Binding("ConnectButton");
            this.e_21.SetBinding(Button.CommandProperty, binding_e_21_Command);
            this.e_21.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            // e_22 element
            this.e_22 = new Button();
            this.e_13.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Margin = new Thickness(10F, 5F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_22.Content = "Go back";
            Binding binding_e_22_Command = new Binding("GoBackButton");
            this.e_22.SetBinding(Button.CommandProperty, binding_e_22_Command);
            this.e_22.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
