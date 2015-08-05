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
        
        private Grid e_35;
        
        private Border e_36;
        
        private StackPanel e_37;
        
        private Border e_38;
        
        private TextBlock e_39;
        
        private TextBlock e_40;
        
        private Border e_41;
        
        private StackPanel e_42;
        
        private Button e_43;
        
        private Button e_44;
        
        private Button e_45;
        
        private Button e_46;
        
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
            // e_35 element
            this.e_35 = new Grid();
            this.Content = this.e_35;
            this.e_35.Name = "e_35";
            // e_36 element
            this.e_36 = new Border();
            this.e_35.Children.Add(this.e_36);
            this.e_36.Name = "e_36";
            this.e_36.MaxWidth = 600F;
            this.e_36.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_36.VerticalAlignment = VerticalAlignment.Center;
            this.e_36.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_36.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_37 element
            this.e_37 = new StackPanel();
            this.e_36.Child = this.e_37;
            this.e_37.Name = "e_37";
            this.e_37.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_38 element
            this.e_38 = new Border();
            this.e_37.Children.Add(this.e_38);
            this.e_38.Name = "e_38";
            this.e_38.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_38.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_39 element
            this.e_39 = new TextBlock();
            this.e_38.Child = this.e_39;
            this.e_39.Name = "e_39";
            this.e_39.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_39.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_39.FontSize = 20F;
            Binding binding_e_39_Text = new Binding("Header");
            this.e_39.SetBinding(TextBlock.TextProperty, binding_e_39_Text);
            this.e_39.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_40 element
            this.e_40 = new TextBlock();
            this.e_37.Children.Add(this.e_40);
            this.e_40.Name = "e_40";
            this.e_40.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_40_Text = new Binding("Content");
            this.e_40.SetBinding(TextBlock.TextProperty, binding_e_40_Text);
            this.e_40.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_41 element
            this.e_41 = new Border();
            this.e_37.Children.Add(this.e_41);
            this.e_41.Name = "e_41";
            this.e_41.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_41.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_42 element
            this.e_42 = new StackPanel();
            this.e_41.Child = this.e_42;
            this.e_42.Name = "e_42";
            this.e_42.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_42.Orientation = Orientation.Horizontal;
            // e_43 element
            this.e_43 = new Button();
            this.e_42.Children.Add(this.e_43);
            this.e_43.Name = "e_43";
            this.e_43.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_43.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_43.Content = "Cancel";
            Binding binding_e_43_Visibility = new Binding("CancelButtonVisibility");
            this.e_43.SetBinding(Button.VisibilityProperty, binding_e_43_Visibility);
            Binding binding_e_43_Command = new Binding("CancelCommand");
            this.e_43.SetBinding(Button.CommandProperty, binding_e_43_Command);
            // e_44 element
            this.e_44 = new Button();
            this.e_42.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_44.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_44.Content = "No";
            Binding binding_e_44_Visibility = new Binding("NoButtonVisibility");
            this.e_44.SetBinding(Button.VisibilityProperty, binding_e_44_Visibility);
            Binding binding_e_44_Command = new Binding("NoCommand");
            this.e_44.SetBinding(Button.CommandProperty, binding_e_44_Command);
            // e_45 element
            this.e_45 = new Button();
            this.e_42.Children.Add(this.e_45);
            this.e_45.Name = "e_45";
            this.e_45.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_45.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_45.Content = "Yes";
            Binding binding_e_45_Visibility = new Binding("YesButtonVisibility");
            this.e_45.SetBinding(Button.VisibilityProperty, binding_e_45_Visibility);
            Binding binding_e_45_Command = new Binding("YesCommand");
            this.e_45.SetBinding(Button.CommandProperty, binding_e_45_Command);
            // e_46 element
            this.e_46 = new Button();
            this.e_42.Children.Add(this.e_46);
            this.e_46.Name = "e_46";
            this.e_46.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_46.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_46.Content = "Ok";
            Binding binding_e_46_Visibility = new Binding("OkButtonVisibility");
            this.e_46.SetBinding(Button.VisibilityProperty, binding_e_46_Visibility);
            Binding binding_e_46_Command = new Binding("OkCommand");
            this.e_46.SetBinding(Button.CommandProperty, binding_e_46_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
