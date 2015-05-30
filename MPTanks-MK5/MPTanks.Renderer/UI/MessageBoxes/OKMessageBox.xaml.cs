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
    public partial class OKMessageBox : UIRoot {
        
        private Grid e_38;
        
        private Border e_39;
        
        private StackPanel e_40;
        
        private Border e_41;
        
        private TextBlock e_42;
        
        private TextBlock e_43;
        
        private Border e_44;
        
        private StackPanel e_45;
        
        private Button e_46;
        
        private Button e_47;
        
        private Button e_48;
        
        private Button e_49;
        
        public OKMessageBox(int width, int height) : 
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
            // e_38 element
            this.e_38 = new Grid();
            this.Content = this.e_38;
            this.e_38.Name = "e_38";
            // e_39 element
            this.e_39 = new Border();
            this.e_38.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.MaxWidth = 600F;
            this.e_39.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_39.VerticalAlignment = VerticalAlignment.Center;
            this.e_39.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_39.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_40 element
            this.e_40 = new StackPanel();
            this.e_39.Child = this.e_40;
            this.e_40.Name = "e_40";
            this.e_40.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_41 element
            this.e_41 = new Border();
            this.e_40.Children.Add(this.e_41);
            this.e_41.Name = "e_41";
            this.e_41.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_41.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_42 element
            this.e_42 = new TextBlock();
            this.e_41.Child = this.e_42;
            this.e_42.Name = "e_42";
            this.e_42.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_42.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_42.FontSize = 20F;
            Binding binding_e_42_Text = new Binding("Header");
            this.e_42.SetBinding(TextBlock.TextProperty, binding_e_42_Text);
            this.e_42.SetResourceReference(TextBlock.ForegroundProperty, "SuccessTextColor");
            // e_43 element
            this.e_43 = new TextBlock();
            this.e_40.Children.Add(this.e_43);
            this.e_43.Name = "e_43";
            this.e_43.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_43_Text = new Binding("Content");
            this.e_43.SetBinding(TextBlock.TextProperty, binding_e_43_Text);
            this.e_43.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_44 element
            this.e_44 = new Border();
            this.e_40.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_44.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_45 element
            this.e_45 = new StackPanel();
            this.e_44.Child = this.e_45;
            this.e_45.Name = "e_45";
            this.e_45.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_45.Orientation = Orientation.Horizontal;
            // e_46 element
            this.e_46 = new Button();
            this.e_45.Children.Add(this.e_46);
            this.e_46.Name = "e_46";
            this.e_46.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_46.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_46.Content = "Cancel";
            Binding binding_e_46_Visibility = new Binding("CancelButtonVisibility");
            this.e_46.SetBinding(Button.VisibilityProperty, binding_e_46_Visibility);
            Binding binding_e_46_Command = new Binding("CancelCommand");
            this.e_46.SetBinding(Button.CommandProperty, binding_e_46_Command);
            // e_47 element
            this.e_47 = new Button();
            this.e_45.Children.Add(this.e_47);
            this.e_47.Name = "e_47";
            this.e_47.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_47.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_47.Content = "No";
            Binding binding_e_47_Visibility = new Binding("NoButtonVisibility");
            this.e_47.SetBinding(Button.VisibilityProperty, binding_e_47_Visibility);
            Binding binding_e_47_Command = new Binding("NoCommand");
            this.e_47.SetBinding(Button.CommandProperty, binding_e_47_Command);
            // e_48 element
            this.e_48 = new Button();
            this.e_45.Children.Add(this.e_48);
            this.e_48.Name = "e_48";
            this.e_48.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_48.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_48.Content = "Yes";
            Binding binding_e_48_Visibility = new Binding("YesButtonVisibility");
            this.e_48.SetBinding(Button.VisibilityProperty, binding_e_48_Visibility);
            Binding binding_e_48_Command = new Binding("YesCommand");
            this.e_48.SetBinding(Button.CommandProperty, binding_e_48_Command);
            // e_49 element
            this.e_49 = new Button();
            this.e_45.Children.Add(this.e_49);
            this.e_49.Name = "e_49";
            this.e_49.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_49.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_49.Content = "Ok";
            Binding binding_e_49_Visibility = new Binding("OkButtonVisibility");
            this.e_49.SetBinding(Button.VisibilityProperty, binding_e_49_Visibility);
            Binding binding_e_49_Command = new Binding("OkCommand");
            this.e_49.SetBinding(Button.CommandProperty, binding_e_49_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
