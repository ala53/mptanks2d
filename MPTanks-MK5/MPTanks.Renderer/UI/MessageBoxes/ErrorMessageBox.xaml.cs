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
        
        private Grid e_31;
        
        private Border e_32;
        
        private StackPanel e_33;
        
        private Border e_34;
        
        private TextBlock e_35;
        
        private TextBlock e_36;
        
        private Border e_37;
        
        private StackPanel e_38;
        
        private Button e_39;
        
        private Button e_40;
        
        private Button e_41;
        
        private Button e_42;
        
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
            // e_31 element
            this.e_31 = new Grid();
            this.Content = this.e_31;
            this.e_31.Name = "e_31";
            // e_32 element
            this.e_32 = new Border();
            this.e_31.Children.Add(this.e_32);
            this.e_32.Name = "e_32";
            this.e_32.MaxWidth = 600F;
            this.e_32.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_32.VerticalAlignment = VerticalAlignment.Center;
            this.e_32.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_32.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_33 element
            this.e_33 = new StackPanel();
            this.e_32.Child = this.e_33;
            this.e_33.Name = "e_33";
            this.e_33.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_34 element
            this.e_34 = new Border();
            this.e_33.Children.Add(this.e_34);
            this.e_34.Name = "e_34";
            this.e_34.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_34.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_35 element
            this.e_35 = new TextBlock();
            this.e_34.Child = this.e_35;
            this.e_35.Name = "e_35";
            this.e_35.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_35.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_35.FontSize = 20F;
            Binding binding_e_35_Text = new Binding("Header");
            this.e_35.SetBinding(TextBlock.TextProperty, binding_e_35_Text);
            this.e_35.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_36 element
            this.e_36 = new TextBlock();
            this.e_33.Children.Add(this.e_36);
            this.e_36.Name = "e_36";
            this.e_36.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_36_Text = new Binding("Content");
            this.e_36.SetBinding(TextBlock.TextProperty, binding_e_36_Text);
            this.e_36.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_37 element
            this.e_37 = new Border();
            this.e_33.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_37.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_38 element
            this.e_38 = new StackPanel();
            this.e_37.Child = this.e_38;
            this.e_38.Name = "e_38";
            this.e_38.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_38.Orientation = Orientation.Horizontal;
            // e_39 element
            this.e_39 = new Button();
            this.e_38.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_39.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_39.Content = "Cancel";
            Binding binding_e_39_Visibility = new Binding("CancelButtonVisibility");
            this.e_39.SetBinding(Button.VisibilityProperty, binding_e_39_Visibility);
            Binding binding_e_39_Command = new Binding("CancelCommand");
            this.e_39.SetBinding(Button.CommandProperty, binding_e_39_Command);
            // e_40 element
            this.e_40 = new Button();
            this.e_38.Children.Add(this.e_40);
            this.e_40.Name = "e_40";
            this.e_40.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_40.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_40.Content = "No";
            Binding binding_e_40_Visibility = new Binding("NoButtonVisibility");
            this.e_40.SetBinding(Button.VisibilityProperty, binding_e_40_Visibility);
            Binding binding_e_40_Command = new Binding("NoCommand");
            this.e_40.SetBinding(Button.CommandProperty, binding_e_40_Command);
            // e_41 element
            this.e_41 = new Button();
            this.e_38.Children.Add(this.e_41);
            this.e_41.Name = "e_41";
            this.e_41.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_41.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_41.Content = "Yes";
            Binding binding_e_41_Visibility = new Binding("YesButtonVisibility");
            this.e_41.SetBinding(Button.VisibilityProperty, binding_e_41_Visibility);
            Binding binding_e_41_Command = new Binding("YesCommand");
            this.e_41.SetBinding(Button.CommandProperty, binding_e_41_Command);
            // e_42 element
            this.e_42 = new Button();
            this.e_38.Children.Add(this.e_42);
            this.e_42.Name = "e_42";
            this.e_42.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_42.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_42.Content = "Ok";
            Binding binding_e_42_Visibility = new Binding("OkButtonVisibility");
            this.e_42.SetBinding(Button.VisibilityProperty, binding_e_42_Visibility);
            Binding binding_e_42_Command = new Binding("OkCommand");
            this.e_42.SetBinding(Button.CommandProperty, binding_e_42_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
