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
        
        private TextBox e_14;
        
        private TextBlock e_15;
        
        private DockPanel e_16;
        
        private TextBlock e_17;
        
        private TextBox e_18;
        
        private DockPanel e_19;
        
        private TextBlock e_20;
        
        private TextBox e_21;
        
        private Button e_22;
        
        private Button e_23;
        
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
            this.e_14 = new TextBox();
            this.e_13.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            // e_15 element
            this.e_15 = new TextBlock();
            this.e_13.Children.Add(this.e_15);
            this.e_15.Name = "e_15";
            this.e_15.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_15.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_15.Text = "Connect to a server";
            this.e_15.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_15.FontSize = 48F;
            // e_16 element
            this.e_16 = new DockPanel();
            this.e_13.Children.Add(this.e_16);
            this.e_16.Name = "e_16";
            this.e_16.Margin = new Thickness(10F, 0F, 10F, 0F);
            this.e_16.LastChildFill = true;
            // e_17 element
            this.e_17 = new TextBlock();
            this.e_16.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            this.e_17.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_17.Text = "Server Address: ";
            this.e_17.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            this.e_17.FontSize = 16F;
            DockPanel.SetDock(this.e_17, Dock.Left);
            // e_18 element
            this.e_18 = new TextBox();
            this.e_16.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            this.e_18.MinWidth = 200F;
            this.e_18.Margin = new Thickness(10F, 0F, 0F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            DockPanel.SetDock(this.e_18, Dock.Right);
            Binding binding_e_18_Text = new Binding("ServerAddress");
            this.e_18.SetBinding(TextBox.TextProperty, binding_e_18_Text);
            // e_19 element
            this.e_19 = new DockPanel();
            this.e_13.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Margin = new Thickness(10F, 5F, 10F, 0F);
            this.e_19.LastChildFill = true;
            // e_20 element
            this.e_20 = new TextBlock();
            this.e_19.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_20.Text = "Password: ";
            this.e_20.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            this.e_20.FontSize = 16F;
            DockPanel.SetDock(this.e_20, Dock.Left);
            // e_21 element
            this.e_21 = new TextBox();
            this.e_19.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.MinWidth = 200F;
            this.e_21.Margin = new Thickness(10F, 0F, 0F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            DockPanel.SetDock(this.e_21, Dock.Right);
            Binding binding_e_21_Text = new Binding("ServerPassword");
            this.e_21.SetBinding(TextBox.TextProperty, binding_e_21_Text);
            // e_22 element
            this.e_22 = new Button();
            this.e_13.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Margin = new Thickness(10F, 5F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_22.Content = "Connect";
            Binding binding_e_22_Command = new Binding("ConnectButton");
            this.e_22.SetBinding(Button.CommandProperty, binding_e_22_Command);
            this.e_22.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            // e_23 element
            this.e_23 = new Button();
            this.e_13.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Margin = new Thickness(10F, 5F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_23.Content = "Go back";
            Binding binding_e_23_Command = new Binding("GoBackButton");
            this.e_23.SetBinding(Button.CommandProperty, binding_e_23_Command);
            this.e_23.SetResourceReference(Button.StyleProperty, "PrimaryButton");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
