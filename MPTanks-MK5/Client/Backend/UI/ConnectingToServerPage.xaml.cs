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
    public partial class ConnectingToServerPage : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private TextBlock e_3;
        
        private Border e_4;
        
        private StackPanel e_5;
        
        private Button e_6;
        
        private Border e_7;
        
        private StackPanel e_8;
        
        private TextBlock e_9;
        
        private TextBlock e_10;
        
        private Button e_11;
        
        public ConnectingToServerPage(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.SetResourceReference(UIRoot.BackgroundProperty, "MenuPageBGBrush");
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
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.Text = "Connecting to Server";
            FontManager.Instance.AddFont("Segoe UI", 48F, FontStyle.Regular, "Segoe_UI_36_Regular");
            this.e_2.SetResourceReference(TextBlock.StyleProperty, "MenuHeader");
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            Binding binding_e_3_Text = new Binding("AddressLabel");
            this.e_3.SetBinding(TextBlock.TextProperty, binding_e_3_Text);
            this.e_3.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            // e_4 element
            this.e_4 = new Border();
            this.e_1.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Margin = new Thickness(0F, 20F, 0F, 0F);
            this.e_4.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_4.SetResourceReference(Border.BorderBrushProperty, "MenuPageHeaderTextColor");
            // e_5 element
            this.e_5 = new StackPanel();
            this.e_4.Child = this.e_5;
            this.e_5.Name = "e_5";
            this.e_5.Margin = new Thickness(20F, 20F, 20F, 20F);
            Binding binding_e_5_Visibility = new Binding("CancelAreaVisibility");
            this.e_5.SetBinding(StackPanel.VisibilityProperty, binding_e_5_Visibility);
            // e_6 element
            this.e_6 = new Button();
            this.e_5.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_6.Content = "Cancel";
            Binding binding_e_6_Command = new Binding("CancelButtonCommand");
            this.e_6.SetBinding(Button.CommandProperty, binding_e_6_Command);
            this.e_6.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            // e_7 element
            this.e_7 = new Border();
            this.e_1.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Margin = new Thickness(0F, 20F, 0F, 0F);
            this.e_7.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_7.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_8 element
            this.e_8 = new StackPanel();
            this.e_7.Child = this.e_8;
            this.e_8.Name = "e_8";
            this.e_8.Margin = new Thickness(10F, 10F, 10F, 10F);
            Binding binding_e_8_Visibility = new Binding("ShowFailureArea");
            this.e_8.SetBinding(StackPanel.VisibilityProperty, binding_e_8_Visibility);
            // e_9 element
            this.e_9 = new TextBlock();
            this.e_8.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_9.Text = "Error";
            FontManager.Instance.AddFont("Segoe UI", 30F, FontStyle.Regular, "Segoe_UI_22.5_Regular");
            this.e_9.SetResourceReference(TextBlock.StyleProperty, "MenuSubHeader");
            this.e_9.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_10 element
            this.e_10 = new TextBlock();
            this.e_8.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Margin = new Thickness(20F, 0F, 20F, 10F);
            this.e_10.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_10_Text = new Binding("FailureReason");
            this.e_10.SetBinding(TextBlock.TextProperty, binding_e_10_Text);
            this.e_10.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            this.e_10.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_11 element
            this.e_11 = new Button();
            this.e_8.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            FontManager.Instance.AddFont("Segoe UI", 24F, FontStyle.Regular, "Segoe_UI_18_Regular");
            this.e_11.Content = "Return to Menu";
            Binding binding_e_11_Command = new Binding("ReturnButtonCommand");
            this.e_11.SetBinding(Button.CommandProperty, binding_e_11_Command);
            this.e_11.SetResourceReference(Button.StyleProperty, "PrimaryButton");
            this.e_11.SetResourceReference(Button.ForegroundProperty, "ErrorTextColor");
            this.e_11.SetResourceReference(Button.BorderBrushProperty, "ErrorTextColor");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
