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
        
        private Grid e_34;
        
        private Border e_35;
        
        private StackPanel e_36;
        
        private Border e_37;
        
        private TextBlock e_38;
        
        private TextBlock e_39;
        
        private Border e_40;
        
        private StackPanel e_41;
        
        private Button e_42;
        
        private Button e_43;
        
        private Button e_44;
        
        private Button e_45;
        
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
            // e_34 element
            this.e_34 = new Grid();
            this.Content = this.e_34;
            this.e_34.Name = "e_34";
            // e_35 element
            this.e_35 = new Border();
            this.e_34.Children.Add(this.e_35);
            this.e_35.Name = "e_35";
            this.e_35.MaxWidth = 600F;
            this.e_35.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_35.VerticalAlignment = VerticalAlignment.Center;
            this.e_35.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_35.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_36 element
            this.e_36 = new StackPanel();
            this.e_35.Child = this.e_36;
            this.e_36.Name = "e_36";
            this.e_36.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_37 element
            this.e_37 = new Border();
            this.e_36.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_37.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_38 element
            this.e_38 = new TextBlock();
            this.e_37.Child = this.e_38;
            this.e_38.Name = "e_38";
            this.e_38.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_38.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_38.FontSize = 20F;
            Binding binding_e_38_Text = new Binding("Header");
            this.e_38.SetBinding(TextBlock.TextProperty, binding_e_38_Text);
            this.e_38.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_39 element
            this.e_39 = new TextBlock();
            this.e_36.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_39_Text = new Binding("Content");
            this.e_39.SetBinding(TextBlock.TextProperty, binding_e_39_Text);
            this.e_39.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_40 element
            this.e_40 = new Border();
            this.e_36.Children.Add(this.e_40);
            this.e_40.Name = "e_40";
            this.e_40.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_40.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_41 element
            this.e_41 = new StackPanel();
            this.e_40.Child = this.e_41;
            this.e_41.Name = "e_41";
            this.e_41.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_41.Orientation = Orientation.Horizontal;
            // e_42 element
            this.e_42 = new Button();
            this.e_41.Children.Add(this.e_42);
            this.e_42.Name = "e_42";
            this.e_42.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_42.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_42.Content = "Cancel";
            Binding binding_e_42_Visibility = new Binding("CancelButtonVisibility");
            this.e_42.SetBinding(Button.VisibilityProperty, binding_e_42_Visibility);
            Binding binding_e_42_Command = new Binding("CancelCommand");
            this.e_42.SetBinding(Button.CommandProperty, binding_e_42_Command);
            // e_43 element
            this.e_43 = new Button();
            this.e_41.Children.Add(this.e_43);
            this.e_43.Name = "e_43";
            this.e_43.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_43.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_43.Content = "No";
            Binding binding_e_43_Visibility = new Binding("NoButtonVisibility");
            this.e_43.SetBinding(Button.VisibilityProperty, binding_e_43_Visibility);
            Binding binding_e_43_Command = new Binding("NoCommand");
            this.e_43.SetBinding(Button.CommandProperty, binding_e_43_Command);
            // e_44 element
            this.e_44 = new Button();
            this.e_41.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_44.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_44.Content = "Yes";
            Binding binding_e_44_Visibility = new Binding("YesButtonVisibility");
            this.e_44.SetBinding(Button.VisibilityProperty, binding_e_44_Visibility);
            Binding binding_e_44_Command = new Binding("YesCommand");
            this.e_44.SetBinding(Button.CommandProperty, binding_e_44_Command);
            // e_45 element
            this.e_45 = new Button();
            this.e_41.Children.Add(this.e_45);
            this.e_45.Name = "e_45";
            this.e_45.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_45.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_45.Content = "Ok";
            Binding binding_e_45_Visibility = new Binding("OkButtonVisibility");
            this.e_45.SetBinding(Button.VisibilityProperty, binding_e_45_Visibility);
            Binding binding_e_45_Command = new Binding("OkCommand");
            this.e_45.SetBinding(Button.CommandProperty, binding_e_45_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
