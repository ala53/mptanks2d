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
        
        private Grid e_41;
        
        private Border e_42;
        
        private StackPanel e_43;
        
        private Border e_44;
        
        private TextBlock e_45;
        
        private TextBlock e_46;
        
        private Border e_47;
        
        private StackPanel e_48;
        
        private Button e_49;
        
        private Button e_50;
        
        private Button e_51;
        
        private Button e_52;
        
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
            // e_41 element
            this.e_41 = new Grid();
            this.Content = this.e_41;
            this.e_41.Name = "e_41";
            // e_42 element
            this.e_42 = new Border();
            this.e_41.Children.Add(this.e_42);
            this.e_42.Name = "e_42";
            this.e_42.MaxWidth = 600F;
            this.e_42.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_42.VerticalAlignment = VerticalAlignment.Center;
            this.e_42.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_42.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_43 element
            this.e_43 = new StackPanel();
            this.e_42.Child = this.e_43;
            this.e_43.Name = "e_43";
            this.e_43.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_44 element
            this.e_44 = new Border();
            this.e_43.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_44.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_45 element
            this.e_45 = new TextBlock();
            this.e_44.Child = this.e_45;
            this.e_45.Name = "e_45";
            this.e_45.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_45.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_45.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_45.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_45.FontSize = 36F;
            Binding binding_e_45_Text = new Binding("Header");
            this.e_45.SetBinding(TextBlock.TextProperty, binding_e_45_Text);
            this.e_45.SetResourceReference(TextBlock.ForegroundProperty, "SuccessTextColor");
            // e_46 element
            this.e_46 = new TextBlock();
            this.e_43.Children.Add(this.e_46);
            this.e_46.Name = "e_46";
            this.e_46.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_46_Text = new Binding("Content");
            this.e_46.SetBinding(TextBlock.TextProperty, binding_e_46_Text);
            this.e_46.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_47 element
            this.e_47 = new Border();
            this.e_43.Children.Add(this.e_47);
            this.e_47.Name = "e_47";
            this.e_47.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_47.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_48 element
            this.e_48 = new StackPanel();
            this.e_47.Child = this.e_48;
            this.e_48.Name = "e_48";
            this.e_48.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_48.Orientation = Orientation.Horizontal;
            // e_49 element
            this.e_49 = new Button();
            this.e_48.Children.Add(this.e_49);
            this.e_49.Name = "e_49";
            this.e_49.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_49.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_49.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_49.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_49.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_49.FontSize = 20F;
            this.e_49.Content = "Cancel";
            Binding binding_e_49_Visibility = new Binding("CancelButtonVisibility");
            this.e_49.SetBinding(Button.VisibilityProperty, binding_e_49_Visibility);
            Binding binding_e_49_Command = new Binding("CancelCommand");
            this.e_49.SetBinding(Button.CommandProperty, binding_e_49_Command);
            // e_50 element
            this.e_50 = new Button();
            this.e_48.Children.Add(this.e_50);
            this.e_50.Name = "e_50";
            this.e_50.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_50.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_50.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_50.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_50.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_50.FontSize = 20F;
            this.e_50.Content = "No";
            Binding binding_e_50_Visibility = new Binding("NoButtonVisibility");
            this.e_50.SetBinding(Button.VisibilityProperty, binding_e_50_Visibility);
            Binding binding_e_50_Command = new Binding("NoCommand");
            this.e_50.SetBinding(Button.CommandProperty, binding_e_50_Command);
            // e_51 element
            this.e_51 = new Button();
            this.e_48.Children.Add(this.e_51);
            this.e_51.Name = "e_51";
            this.e_51.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_51.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_51.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_51.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_51.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_51.FontSize = 20F;
            this.e_51.Content = "Yes";
            Binding binding_e_51_Visibility = new Binding("YesButtonVisibility");
            this.e_51.SetBinding(Button.VisibilityProperty, binding_e_51_Visibility);
            Binding binding_e_51_Command = new Binding("YesCommand");
            this.e_51.SetBinding(Button.CommandProperty, binding_e_51_Command);
            // e_52 element
            this.e_52 = new Button();
            this.e_48.Children.Add(this.e_52);
            this.e_52.Name = "e_52";
            this.e_52.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_52.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_52.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_52.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_52.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_52.FontSize = 20F;
            this.e_52.Content = "Ok";
            Binding binding_e_52_Visibility = new Binding("OkButtonVisibility");
            this.e_52.SetBinding(Button.VisibilityProperty, binding_e_52_Visibility);
            Binding binding_e_52_Command = new Binding("OkCommand");
            this.e_52.SetBinding(Button.CommandProperty, binding_e_52_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
