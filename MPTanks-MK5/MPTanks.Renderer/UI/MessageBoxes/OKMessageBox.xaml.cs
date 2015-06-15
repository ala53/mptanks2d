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
        
        private Grid e_43;
        
        private Border e_44;
        
        private StackPanel e_45;
        
        private Border e_46;
        
        private TextBlock e_47;
        
        private TextBlock e_48;
        
        private Border e_49;
        
        private StackPanel e_50;
        
        private Button e_51;
        
        private Button e_52;
        
        private Button e_53;
        
        private Button e_54;
        
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
            // e_43 element
            this.e_43 = new Grid();
            this.Content = this.e_43;
            this.e_43.Name = "e_43";
            // e_44 element
            this.e_44 = new Border();
            this.e_43.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.MaxWidth = 600F;
            this.e_44.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_44.VerticalAlignment = VerticalAlignment.Center;
            this.e_44.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_44.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_45 element
            this.e_45 = new StackPanel();
            this.e_44.Child = this.e_45;
            this.e_45.Name = "e_45";
            this.e_45.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_46 element
            this.e_46 = new Border();
            this.e_45.Children.Add(this.e_46);
            this.e_46.Name = "e_46";
            this.e_46.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_46.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_47 element
            this.e_47 = new TextBlock();
            this.e_46.Child = this.e_47;
            this.e_47.Name = "e_47";
            this.e_47.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_47.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_47.FontSize = 20F;
            Binding binding_e_47_Text = new Binding("Header");
            this.e_47.SetBinding(TextBlock.TextProperty, binding_e_47_Text);
            this.e_47.SetResourceReference(TextBlock.ForegroundProperty, "SuccessTextColor");
            // e_48 element
            this.e_48 = new TextBlock();
            this.e_45.Children.Add(this.e_48);
            this.e_48.Name = "e_48";
            this.e_48.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_48_Text = new Binding("Content");
            this.e_48.SetBinding(TextBlock.TextProperty, binding_e_48_Text);
            this.e_48.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_49 element
            this.e_49 = new Border();
            this.e_45.Children.Add(this.e_49);
            this.e_49.Name = "e_49";
            this.e_49.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_49.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_50 element
            this.e_50 = new StackPanel();
            this.e_49.Child = this.e_50;
            this.e_50.Name = "e_50";
            this.e_50.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_50.Orientation = Orientation.Horizontal;
            // e_51 element
            this.e_51 = new Button();
            this.e_50.Children.Add(this.e_51);
            this.e_51.Name = "e_51";
            this.e_51.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_51.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_51.Content = "Cancel";
            Binding binding_e_51_Visibility = new Binding("CancelButtonVisibility");
            this.e_51.SetBinding(Button.VisibilityProperty, binding_e_51_Visibility);
            Binding binding_e_51_Command = new Binding("CancelCommand");
            this.e_51.SetBinding(Button.CommandProperty, binding_e_51_Command);
            // e_52 element
            this.e_52 = new Button();
            this.e_50.Children.Add(this.e_52);
            this.e_52.Name = "e_52";
            this.e_52.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_52.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_52.Content = "No";
            Binding binding_e_52_Visibility = new Binding("NoButtonVisibility");
            this.e_52.SetBinding(Button.VisibilityProperty, binding_e_52_Visibility);
            Binding binding_e_52_Command = new Binding("NoCommand");
            this.e_52.SetBinding(Button.CommandProperty, binding_e_52_Command);
            // e_53 element
            this.e_53 = new Button();
            this.e_50.Children.Add(this.e_53);
            this.e_53.Name = "e_53";
            this.e_53.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_53.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_53.Content = "Yes";
            Binding binding_e_53_Visibility = new Binding("YesButtonVisibility");
            this.e_53.SetBinding(Button.VisibilityProperty, binding_e_53_Visibility);
            Binding binding_e_53_Command = new Binding("YesCommand");
            this.e_53.SetBinding(Button.CommandProperty, binding_e_53_Command);
            // e_54 element
            this.e_54 = new Button();
            this.e_50.Children.Add(this.e_54);
            this.e_54.Name = "e_54";
            this.e_54.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_54.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_54.Content = "Ok";
            Binding binding_e_54_Visibility = new Binding("OkButtonVisibility");
            this.e_54.SetBinding(Button.VisibilityProperty, binding_e_54_Visibility);
            Binding binding_e_54_Command = new Binding("OkCommand");
            this.e_54.SetBinding(Button.CommandProperty, binding_e_54_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
