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
        
        private Grid e_29;
        
        private Border e_30;
        
        private StackPanel e_31;
        
        private Border e_32;
        
        private TextBlock e_33;
        
        private TextBlock e_34;
        
        private Border e_35;
        
        private StackPanel e_36;
        
        private Button e_37;
        
        private Button e_38;
        
        private Button e_39;
        
        private Button e_40;
        
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
            // e_29 element
            this.e_29 = new Grid();
            this.Content = this.e_29;
            this.e_29.Name = "e_29";
            // e_30 element
            this.e_30 = new Border();
            this.e_29.Children.Add(this.e_30);
            this.e_30.Name = "e_30";
            this.e_30.MaxWidth = 600F;
            this.e_30.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_30.VerticalAlignment = VerticalAlignment.Center;
            this.e_30.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_30.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_31 element
            this.e_31 = new StackPanel();
            this.e_30.Child = this.e_31;
            this.e_31.Name = "e_31";
            this.e_31.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_32 element
            this.e_32 = new Border();
            this.e_31.Children.Add(this.e_32);
            this.e_32.Name = "e_32";
            this.e_32.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_32.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_33 element
            this.e_33 = new TextBlock();
            this.e_32.Child = this.e_33;
            this.e_33.Name = "e_33";
            this.e_33.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_33.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_33.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_33.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_33.FontSize = 36F;
            Binding binding_e_33_Text = new Binding("Header");
            this.e_33.SetBinding(TextBlock.TextProperty, binding_e_33_Text);
            this.e_33.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // e_34 element
            this.e_34 = new TextBlock();
            this.e_31.Children.Add(this.e_34);
            this.e_34.Name = "e_34";
            this.e_34.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_34.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_34.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_34.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_34.FontSize = 20F;
            Binding binding_e_34_Text = new Binding("Content");
            this.e_34.SetBinding(TextBlock.TextProperty, binding_e_34_Text);
            this.e_34.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_35 element
            this.e_35 = new Border();
            this.e_31.Children.Add(this.e_35);
            this.e_35.Name = "e_35";
            this.e_35.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_35.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_36 element
            this.e_36 = new StackPanel();
            this.e_35.Child = this.e_36;
            this.e_36.Name = "e_36";
            this.e_36.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_36.Orientation = Orientation.Horizontal;
            // e_37 element
            this.e_37 = new Button();
            this.e_36.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_37.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_37.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_37.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_37.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_37.FontSize = 20F;
            this.e_37.Content = "Cancel";
            Binding binding_e_37_Visibility = new Binding("CancelButtonVisibility");
            this.e_37.SetBinding(Button.VisibilityProperty, binding_e_37_Visibility);
            Binding binding_e_37_Command = new Binding("CancelCommand");
            this.e_37.SetBinding(Button.CommandProperty, binding_e_37_Command);
            // e_38 element
            this.e_38 = new Button();
            this.e_36.Children.Add(this.e_38);
            this.e_38.Name = "e_38";
            this.e_38.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_38.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_38.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_38.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_38.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_38.FontSize = 20F;
            this.e_38.Content = "No";
            Binding binding_e_38_Visibility = new Binding("NoButtonVisibility");
            this.e_38.SetBinding(Button.VisibilityProperty, binding_e_38_Visibility);
            Binding binding_e_38_Command = new Binding("NoCommand");
            this.e_38.SetBinding(Button.CommandProperty, binding_e_38_Command);
            // e_39 element
            this.e_39 = new Button();
            this.e_36.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_39.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_39.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_39.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_39.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_39.FontSize = 20F;
            this.e_39.Content = "Yes";
            Binding binding_e_39_Visibility = new Binding("YesButtonVisibility");
            this.e_39.SetBinding(Button.VisibilityProperty, binding_e_39_Visibility);
            Binding binding_e_39_Command = new Binding("YesCommand");
            this.e_39.SetBinding(Button.CommandProperty, binding_e_39_Command);
            // e_40 element
            this.e_40 = new Button();
            this.e_36.Children.Add(this.e_40);
            this.e_40.Name = "e_40";
            this.e_40.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_40.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_40.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_40.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_40.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_40.FontSize = 20F;
            this.e_40.Content = "Ok";
            Binding binding_e_40_Visibility = new Binding("OkButtonVisibility");
            this.e_40.SetBinding(Button.VisibilityProperty, binding_e_40_Visibility);
            Binding binding_e_40_Command = new Binding("OkCommand");
            this.e_40.SetBinding(Button.CommandProperty, binding_e_40_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
