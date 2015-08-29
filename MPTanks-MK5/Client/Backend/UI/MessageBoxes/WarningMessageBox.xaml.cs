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
        
        private Grid e_58;
        
        private Border e_59;
        
        private StackPanel e_60;
        
        private Border e_61;
        
        private TextBlock e_62;
        
        private TextBlock e_63;
        
        private Border e_64;
        
        private StackPanel e_65;
        
        private Button e_66;
        
        private Button e_67;
        
        private Button e_68;
        
        private Button e_69;
        
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
            // e_58 element
            this.e_58 = new Grid();
            this.Content = this.e_58;
            this.e_58.Name = "e_58";
            // e_59 element
            this.e_59 = new Border();
            this.e_58.Children.Add(this.e_59);
            this.e_59.Name = "e_59";
            this.e_59.MaxWidth = 600F;
            this.e_59.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_59.VerticalAlignment = VerticalAlignment.Center;
            this.e_59.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_59.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_60 element
            this.e_60 = new StackPanel();
            this.e_59.Child = this.e_60;
            this.e_60.Name = "e_60";
            this.e_60.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_61 element
            this.e_61 = new Border();
            this.e_60.Children.Add(this.e_61);
            this.e_61.Name = "e_61";
            this.e_61.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_61.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_62 element
            this.e_62 = new TextBlock();
            this.e_61.Child = this.e_62;
            this.e_62.Name = "e_62";
            this.e_62.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_62.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_62.FontSize = 20F;
            Binding binding_e_62_Text = new Binding("Header");
            this.e_62.SetBinding(TextBlock.TextProperty, binding_e_62_Text);
            this.e_62.SetResourceReference(TextBlock.ForegroundProperty, "WarningTextColor");
            // e_63 element
            this.e_63 = new TextBlock();
            this.e_60.Children.Add(this.e_63);
            this.e_63.Name = "e_63";
            this.e_63.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_63_Text = new Binding("Content");
            this.e_63.SetBinding(TextBlock.TextProperty, binding_e_63_Text);
            this.e_63.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_64 element
            this.e_64 = new Border();
            this.e_60.Children.Add(this.e_64);
            this.e_64.Name = "e_64";
            this.e_64.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_64.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_65 element
            this.e_65 = new StackPanel();
            this.e_64.Child = this.e_65;
            this.e_65.Name = "e_65";
            this.e_65.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_65.Orientation = Orientation.Horizontal;
            // e_66 element
            this.e_66 = new Button();
            this.e_65.Children.Add(this.e_66);
            this.e_66.Name = "e_66";
            this.e_66.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_66.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_66.Content = "Cancel";
            Binding binding_e_66_Visibility = new Binding("CancelButtonVisibility");
            this.e_66.SetBinding(Button.VisibilityProperty, binding_e_66_Visibility);
            Binding binding_e_66_Command = new Binding("CancelCommand");
            this.e_66.SetBinding(Button.CommandProperty, binding_e_66_Command);
            // e_67 element
            this.e_67 = new Button();
            this.e_65.Children.Add(this.e_67);
            this.e_67.Name = "e_67";
            this.e_67.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_67.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_67.Content = "No";
            Binding binding_e_67_Visibility = new Binding("NoButtonVisibility");
            this.e_67.SetBinding(Button.VisibilityProperty, binding_e_67_Visibility);
            Binding binding_e_67_Command = new Binding("NoCommand");
            this.e_67.SetBinding(Button.CommandProperty, binding_e_67_Command);
            // e_68 element
            this.e_68 = new Button();
            this.e_65.Children.Add(this.e_68);
            this.e_68.Name = "e_68";
            this.e_68.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_68.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_68.Content = "Yes";
            Binding binding_e_68_Visibility = new Binding("YesButtonVisibility");
            this.e_68.SetBinding(Button.VisibilityProperty, binding_e_68_Visibility);
            Binding binding_e_68_Command = new Binding("YesCommand");
            this.e_68.SetBinding(Button.CommandProperty, binding_e_68_Command);
            // e_69 element
            this.e_69 = new Button();
            this.e_65.Children.Add(this.e_69);
            this.e_69.Name = "e_69";
            this.e_69.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_69.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_69.Content = "Ok";
            Binding binding_e_69_Visibility = new Binding("OkButtonVisibility");
            this.e_69.SetBinding(Button.VisibilityProperty, binding_e_69_Visibility);
            Binding binding_e_69_Command = new Binding("OkCommand");
            this.e_69.SetBinding(Button.CommandProperty, binding_e_69_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
