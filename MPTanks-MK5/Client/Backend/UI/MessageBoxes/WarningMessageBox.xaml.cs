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
    public partial class WarningMessageBox : UIRoot {
        
        private Grid e_64;
        
        private Border e_65;
        
        private StackPanel e_66;
        
        private Border e_67;
        
        private TextBlock e_68;
        
        private TextBlock e_69;
        
        private Border e_70;
        
        private StackPanel e_71;
        
        private Button e_72;
        
        private Button e_73;
        
        private Button e_74;
        
        private Button e_75;
        
        public WarningMessageBox(int width, int height) : 
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
            // e_64 element
            this.e_64 = new Grid();
            this.Content = this.e_64;
            this.e_64.Name = "e_64";
            // e_65 element
            this.e_65 = new Border();
            this.e_64.Children.Add(this.e_65);
            this.e_65.Name = "e_65";
            this.e_65.MaxWidth = 600F;
            this.e_65.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_65.VerticalAlignment = VerticalAlignment.Center;
            this.e_65.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_65.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_66 element
            this.e_66 = new StackPanel();
            this.e_65.Child = this.e_66;
            this.e_66.Name = "e_66";
            this.e_66.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_67 element
            this.e_67 = new Border();
            this.e_66.Children.Add(this.e_67);
            this.e_67.Name = "e_67";
            this.e_67.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_67.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_68 element
            this.e_68 = new TextBlock();
            this.e_67.Child = this.e_68;
            this.e_68.Name = "e_68";
            this.e_68.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_68.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_68.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_68.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_68.FontSize = 36F;
            Binding binding_e_68_Text = new Binding("Header");
            this.e_68.SetBinding(TextBlock.TextProperty, binding_e_68_Text);
            this.e_68.SetResourceReference(TextBlock.ForegroundProperty, "WarningTextColor");
            // e_69 element
            this.e_69 = new TextBlock();
            this.e_66.Children.Add(this.e_69);
            this.e_69.Name = "e_69";
            this.e_69.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_69.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_69.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_69.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_69.FontSize = 20F;
            Binding binding_e_69_Text = new Binding("Content");
            this.e_69.SetBinding(TextBlock.TextProperty, binding_e_69_Text);
            this.e_69.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_70 element
            this.e_70 = new Border();
            this.e_66.Children.Add(this.e_70);
            this.e_70.Name = "e_70";
            this.e_70.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_70.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_71 element
            this.e_71 = new StackPanel();
            this.e_70.Child = this.e_71;
            this.e_71.Name = "e_71";
            this.e_71.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_71.Orientation = Orientation.Horizontal;
            // e_72 element
            this.e_72 = new Button();
            this.e_71.Children.Add(this.e_72);
            this.e_72.Name = "e_72";
            this.e_72.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_72.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_72.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_72.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_72.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_72.FontSize = 20F;
            this.e_72.Content = "Cancel";
            Binding binding_e_72_Visibility = new Binding("CancelButtonVisibility");
            this.e_72.SetBinding(Button.VisibilityProperty, binding_e_72_Visibility);
            Binding binding_e_72_Command = new Binding("CancelCommand");
            this.e_72.SetBinding(Button.CommandProperty, binding_e_72_Command);
            // e_73 element
            this.e_73 = new Button();
            this.e_71.Children.Add(this.e_73);
            this.e_73.Name = "e_73";
            this.e_73.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_73.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_73.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_73.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_73.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_73.FontSize = 20F;
            this.e_73.Content = "No";
            Binding binding_e_73_Visibility = new Binding("NoButtonVisibility");
            this.e_73.SetBinding(Button.VisibilityProperty, binding_e_73_Visibility);
            Binding binding_e_73_Command = new Binding("NoCommand");
            this.e_73.SetBinding(Button.CommandProperty, binding_e_73_Command);
            // e_74 element
            this.e_74 = new Button();
            this.e_71.Children.Add(this.e_74);
            this.e_74.Name = "e_74";
            this.e_74.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_74.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_74.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_74.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_74.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_74.FontSize = 20F;
            this.e_74.Content = "Yes";
            Binding binding_e_74_Visibility = new Binding("YesButtonVisibility");
            this.e_74.SetBinding(Button.VisibilityProperty, binding_e_74_Visibility);
            Binding binding_e_74_Command = new Binding("YesCommand");
            this.e_74.SetBinding(Button.CommandProperty, binding_e_74_Command);
            // e_75 element
            this.e_75 = new Button();
            this.e_71.Children.Add(this.e_75);
            this.e_75.Name = "e_75";
            this.e_75.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_75.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_75.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_75.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_75.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_75.FontSize = 20F;
            this.e_75.Content = "Ok";
            Binding binding_e_75_Visibility = new Binding("OkButtonVisibility");
            this.e_75.SetBinding(Button.VisibilityProperty, binding_e_75_Visibility);
            Binding binding_e_75_Command = new Binding("OkCommand");
            this.e_75.SetBinding(Button.CommandProperty, binding_e_75_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
