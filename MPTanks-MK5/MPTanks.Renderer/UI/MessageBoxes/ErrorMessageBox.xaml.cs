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
    public partial class ErrorMessageBox : UIRoot {
        
        private Grid e_26;
        
        private Border e_27;
        
        private StackPanel e_28;
        
        private Border e_29;
        
        private TextBlock e_30;
        
        private TextBlock e_31;
        
        private Border e_32;
        
        private StackPanel e_33;
        
        private Button e_34;
        
        private Button e_35;
        
        private Button e_36;
        
        private Button e_37;
        
        public ErrorMessageBox(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(0, 0, 0, 51));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            InitializeElementResources(this);
            // e_26 element
            this.e_26 = new Grid();
            this.Content = this.e_26;
            this.e_26.Name = "e_26";
            // e_27 element
            this.e_27 = new Border();
            this.e_26.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.MaxWidth = 600F;
            this.e_27.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_27.VerticalAlignment = VerticalAlignment.Center;
            this.e_27.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_27.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_28 element
            this.e_28 = new StackPanel();
            this.e_27.Child = this.e_28;
            this.e_28.Name = "e_28";
            this.e_28.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_29 element
            this.e_29 = new Border();
            this.e_28.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_29.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_30 element
            this.e_30 = new TextBlock();
            this.e_29.Child = this.e_30;
            this.e_30.Name = "e_30";
            this.e_30.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_30.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_30.FontSize = 20F;
            Binding binding_e_30_Text = new Binding("Header");
            this.e_30.SetBinding(TextBlock.TextProperty, binding_e_30_Text);
            this.e_30.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_31 element
            this.e_31 = new TextBlock();
            this.e_28.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
            this.e_31.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_31_Text = new Binding("Content");
            this.e_31.SetBinding(TextBlock.TextProperty, binding_e_31_Text);
            this.e_31.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_32 element
            this.e_32 = new Border();
            this.e_28.Children.Add(this.e_32);
            this.e_32.Name = "e_32";
            this.e_32.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_32.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_33 element
            this.e_33 = new StackPanel();
            this.e_32.Child = this.e_33;
            this.e_33.Name = "e_33";
            this.e_33.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_33.Orientation = Orientation.Horizontal;
            // e_34 element
            this.e_34 = new Button();
            this.e_33.Children.Add(this.e_34);
            this.e_34.Name = "e_34";
            this.e_34.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_34.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_34.Content = "Cancel";
            Binding binding_e_34_Visibility = new Binding("CancelButtonVisibility");
            this.e_34.SetBinding(Button.VisibilityProperty, binding_e_34_Visibility);
            Binding binding_e_34_Command = new Binding("CancelCommand");
            this.e_34.SetBinding(Button.CommandProperty, binding_e_34_Command);
            // e_35 element
            this.e_35 = new Button();
            this.e_33.Children.Add(this.e_35);
            this.e_35.Name = "e_35";
            this.e_35.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_35.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_35.Content = "No";
            Binding binding_e_35_Visibility = new Binding("NoButtonVisibility");
            this.e_35.SetBinding(Button.VisibilityProperty, binding_e_35_Visibility);
            Binding binding_e_35_Command = new Binding("NoCommand");
            this.e_35.SetBinding(Button.CommandProperty, binding_e_35_Command);
            // e_36 element
            this.e_36 = new Button();
            this.e_33.Children.Add(this.e_36);
            this.e_36.Name = "e_36";
            this.e_36.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_36.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_36.Content = "Yes";
            Binding binding_e_36_Visibility = new Binding("YesButtonVisibility");
            this.e_36.SetBinding(Button.VisibilityProperty, binding_e_36_Visibility);
            Binding binding_e_36_Command = new Binding("YesCommand");
            this.e_36.SetBinding(Button.CommandProperty, binding_e_36_Command);
            // e_37 element
            this.e_37 = new Button();
            this.e_33.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_37.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_37.Content = "Ok";
            Binding binding_e_37_Visibility = new Binding("OkButtonVisibility");
            this.e_37.SetBinding(Button.VisibilityProperty, binding_e_37_Visibility);
            Binding binding_e_37_Command = new Binding("OkCommand");
            this.e_37.SetBinding(Button.CommandProperty, binding_e_37_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
