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
        
        private Grid e_52;
        
        private Border e_53;
        
        private StackPanel e_54;
        
        private Border e_55;
        
        private TextBlock e_56;
        
        private TextBlock e_57;
        
        private Border e_58;
        
        private StackPanel e_59;
        
        private Button e_60;
        
        private Button e_61;
        
        private Button e_62;
        
        private Button e_63;
        
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
            // e_52 element
            this.e_52 = new Grid();
            this.Content = this.e_52;
            this.e_52.Name = "e_52";
            // e_53 element
            this.e_53 = new Border();
            this.e_52.Children.Add(this.e_53);
            this.e_53.Name = "e_53";
            this.e_53.MaxWidth = 600F;
            this.e_53.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_53.VerticalAlignment = VerticalAlignment.Center;
            this.e_53.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_53.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_54 element
            this.e_54 = new StackPanel();
            this.e_53.Child = this.e_54;
            this.e_54.Name = "e_54";
            this.e_54.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_55 element
            this.e_55 = new Border();
            this.e_54.Children.Add(this.e_55);
            this.e_55.Name = "e_55";
            this.e_55.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_55.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_56 element
            this.e_56 = new TextBlock();
            this.e_55.Child = this.e_56;
            this.e_56.Name = "e_56";
            this.e_56.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_56.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_56.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.e_56.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_56.FontSize = 36F;
            Binding binding_e_56_Text = new Binding("Header");
            this.e_56.SetBinding(TextBlock.TextProperty, binding_e_56_Text);
            this.e_56.SetResourceReference(TextBlock.ForegroundProperty, "SuccessTextColor");
            // e_57 element
            this.e_57 = new TextBlock();
            this.e_54.Children.Add(this.e_57);
            this.e_57.Name = "e_57";
            this.e_57.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_57.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_57.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_57.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_57.FontSize = 20F;
            Binding binding_e_57_Text = new Binding("Content");
            this.e_57.SetBinding(TextBlock.TextProperty, binding_e_57_Text);
            this.e_57.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_58 element
            this.e_58 = new Border();
            this.e_54.Children.Add(this.e_58);
            this.e_58.Name = "e_58";
            this.e_58.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_58.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_59 element
            this.e_59 = new StackPanel();
            this.e_58.Child = this.e_59;
            this.e_59.Name = "e_59";
            this.e_59.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_59.Orientation = Orientation.Horizontal;
            // e_60 element
            this.e_60 = new Button();
            this.e_59.Children.Add(this.e_60);
            this.e_60.Name = "e_60";
            this.e_60.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_60.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_60.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_60.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_60.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_60.FontSize = 20F;
            this.e_60.Content = "Cancel";
            Binding binding_e_60_Visibility = new Binding("CancelButtonVisibility");
            this.e_60.SetBinding(Button.VisibilityProperty, binding_e_60_Visibility);
            Binding binding_e_60_Command = new Binding("CancelCommand");
            this.e_60.SetBinding(Button.CommandProperty, binding_e_60_Command);
            // e_61 element
            this.e_61 = new Button();
            this.e_59.Children.Add(this.e_61);
            this.e_61.Name = "e_61";
            this.e_61.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_61.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_61.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_61.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_61.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_61.FontSize = 20F;
            this.e_61.Content = "No";
            Binding binding_e_61_Visibility = new Binding("NoButtonVisibility");
            this.e_61.SetBinding(Button.VisibilityProperty, binding_e_61_Visibility);
            Binding binding_e_61_Command = new Binding("NoCommand");
            this.e_61.SetBinding(Button.CommandProperty, binding_e_61_Command);
            // e_62 element
            this.e_62 = new Button();
            this.e_59.Children.Add(this.e_62);
            this.e_62.Name = "e_62";
            this.e_62.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_62.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_62.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_62.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_62.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_62.FontSize = 20F;
            this.e_62.Content = "Yes";
            Binding binding_e_62_Visibility = new Binding("YesButtonVisibility");
            this.e_62.SetBinding(Button.VisibilityProperty, binding_e_62_Visibility);
            Binding binding_e_62_Command = new Binding("YesCommand");
            this.e_62.SetBinding(Button.CommandProperty, binding_e_62_Command);
            // e_63 element
            this.e_63 = new Button();
            this.e_59.Children.Add(this.e_63);
            this.e_63.Name = "e_63";
            this.e_63.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_63.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.e_63.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.e_63.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.e_63.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_63.FontSize = 20F;
            this.e_63.Content = "Ok";
            Binding binding_e_63_Visibility = new Binding("OkButtonVisibility");
            this.e_63.SetBinding(Button.VisibilityProperty, binding_e_63_Visibility);
            Binding binding_e_63_Command = new Binding("OkCommand");
            this.e_63.SetBinding(Button.CommandProperty, binding_e_63_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
