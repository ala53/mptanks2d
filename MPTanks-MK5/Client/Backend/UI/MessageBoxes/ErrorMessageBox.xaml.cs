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
        
        private Grid e_36;
        
        private Border e_37;
        
        private StackPanel e_38;
        
        private Border e_39;
        
        private TextBlock e_40;
        
        private TextBlock e_41;
        
        private Border e_42;
        
        private StackPanel e_43;
        
        private Button e_44;
        
        private Button e_45;
        
        private Button e_46;
        
        private Button e_47;
        
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
            // e_36 element
            this.e_36 = new Grid();
            this.Content = this.e_36;
            this.e_36.Name = "e_36";
            // e_37 element
            this.e_37 = new Border();
            this.e_36.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.MaxWidth = 600F;
            this.e_37.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_37.VerticalAlignment = VerticalAlignment.Center;
            this.e_37.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_37.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_38 element
            this.e_38 = new StackPanel();
            this.e_37.Child = this.e_38;
            this.e_38.Name = "e_38";
            this.e_38.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_39 element
            this.e_39 = new Border();
            this.e_38.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_39.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_40 element
            this.e_40 = new TextBlock();
            this.e_39.Child = this.e_40;
            this.e_40.Name = "e_40";
            this.e_40.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_40.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_40.FontSize = 20F;
            Binding binding_e_40_Text = new Binding("Header");
            this.e_40.SetBinding(TextBlock.TextProperty, binding_e_40_Text);
            this.e_40.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_41 element
            this.e_41 = new TextBlock();
            this.e_38.Children.Add(this.e_41);
            this.e_41.Name = "e_41";
            this.e_41.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_41_Text = new Binding("Content");
            this.e_41.SetBinding(TextBlock.TextProperty, binding_e_41_Text);
            this.e_41.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_42 element
            this.e_42 = new Border();
            this.e_38.Children.Add(this.e_42);
            this.e_42.Name = "e_42";
            this.e_42.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_42.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_43 element
            this.e_43 = new StackPanel();
            this.e_42.Child = this.e_43;
            this.e_43.Name = "e_43";
            this.e_43.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_43.Orientation = Orientation.Horizontal;
            // e_44 element
            this.e_44 = new Button();
            this.e_43.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_44.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_44.Content = "Cancel";
            Binding binding_e_44_Visibility = new Binding("CancelButtonVisibility");
            this.e_44.SetBinding(Button.VisibilityProperty, binding_e_44_Visibility);
            Binding binding_e_44_Command = new Binding("CancelCommand");
            this.e_44.SetBinding(Button.CommandProperty, binding_e_44_Command);
            // e_45 element
            this.e_45 = new Button();
            this.e_43.Children.Add(this.e_45);
            this.e_45.Name = "e_45";
            this.e_45.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_45.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_45.Content = "No";
            Binding binding_e_45_Visibility = new Binding("NoButtonVisibility");
            this.e_45.SetBinding(Button.VisibilityProperty, binding_e_45_Visibility);
            Binding binding_e_45_Command = new Binding("NoCommand");
            this.e_45.SetBinding(Button.CommandProperty, binding_e_45_Command);
            // e_46 element
            this.e_46 = new Button();
            this.e_43.Children.Add(this.e_46);
            this.e_46.Name = "e_46";
            this.e_46.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_46.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_46.Content = "Yes";
            Binding binding_e_46_Visibility = new Binding("YesButtonVisibility");
            this.e_46.SetBinding(Button.VisibilityProperty, binding_e_46_Visibility);
            Binding binding_e_46_Command = new Binding("YesCommand");
            this.e_46.SetBinding(Button.CommandProperty, binding_e_46_Command);
            // e_47 element
            this.e_47 = new Button();
            this.e_43.Children.Add(this.e_47);
            this.e_47.Name = "e_47";
            this.e_47.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_47.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_47.Content = "Ok";
            Binding binding_e_47_Visibility = new Binding("OkButtonVisibility");
            this.e_47.SetBinding(Button.VisibilityProperty, binding_e_47_Visibility);
            Binding binding_e_47_Command = new Binding("OkCommand");
            this.e_47.SetBinding(Button.CommandProperty, binding_e_47_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
