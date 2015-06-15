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
        
        private Grid e_55;
        
        private Border e_56;
        
        private StackPanel e_57;
        
        private Border e_58;
        
        private TextBlock e_59;
        
        private TextBlock e_60;
        
        private Border e_61;
        
        private StackPanel e_62;
        
        private Button e_63;
        
        private Button e_64;
        
        private Button e_65;
        
        private Button e_66;
        
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
            // e_55 element
            this.e_55 = new Grid();
            this.Content = this.e_55;
            this.e_55.Name = "e_55";
            // e_56 element
            this.e_56 = new Border();
            this.e_55.Children.Add(this.e_56);
            this.e_56.Name = "e_56";
            this.e_56.MaxWidth = 600F;
            this.e_56.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_56.VerticalAlignment = VerticalAlignment.Center;
            this.e_56.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_56.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_57 element
            this.e_57 = new StackPanel();
            this.e_56.Child = this.e_57;
            this.e_57.Name = "e_57";
            this.e_57.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_58 element
            this.e_58 = new Border();
            this.e_57.Children.Add(this.e_58);
            this.e_58.Name = "e_58";
            this.e_58.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_58.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_59 element
            this.e_59 = new TextBlock();
            this.e_58.Child = this.e_59;
            this.e_59.Name = "e_59";
            this.e_59.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_59.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_59.FontSize = 20F;
            Binding binding_e_59_Text = new Binding("Header");
            this.e_59.SetBinding(TextBlock.TextProperty, binding_e_59_Text);
            this.e_59.SetResourceReference(TextBlock.ForegroundProperty, "WarningTextColor");
            // e_60 element
            this.e_60 = new TextBlock();
            this.e_57.Children.Add(this.e_60);
            this.e_60.Name = "e_60";
            this.e_60.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_60_Text = new Binding("Content");
            this.e_60.SetBinding(TextBlock.TextProperty, binding_e_60_Text);
            this.e_60.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_61 element
            this.e_61 = new Border();
            this.e_57.Children.Add(this.e_61);
            this.e_61.Name = "e_61";
            this.e_61.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_61.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_62 element
            this.e_62 = new StackPanel();
            this.e_61.Child = this.e_62;
            this.e_62.Name = "e_62";
            this.e_62.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_62.Orientation = Orientation.Horizontal;
            // e_63 element
            this.e_63 = new Button();
            this.e_62.Children.Add(this.e_63);
            this.e_63.Name = "e_63";
            this.e_63.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_63.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_63.Content = "Cancel";
            Binding binding_e_63_Visibility = new Binding("CancelButtonVisibility");
            this.e_63.SetBinding(Button.VisibilityProperty, binding_e_63_Visibility);
            Binding binding_e_63_Command = new Binding("CancelCommand");
            this.e_63.SetBinding(Button.CommandProperty, binding_e_63_Command);
            // e_64 element
            this.e_64 = new Button();
            this.e_62.Children.Add(this.e_64);
            this.e_64.Name = "e_64";
            this.e_64.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_64.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_64.Content = "No";
            Binding binding_e_64_Visibility = new Binding("NoButtonVisibility");
            this.e_64.SetBinding(Button.VisibilityProperty, binding_e_64_Visibility);
            Binding binding_e_64_Command = new Binding("NoCommand");
            this.e_64.SetBinding(Button.CommandProperty, binding_e_64_Command);
            // e_65 element
            this.e_65 = new Button();
            this.e_62.Children.Add(this.e_65);
            this.e_65.Name = "e_65";
            this.e_65.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_65.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_65.Content = "Yes";
            Binding binding_e_65_Visibility = new Binding("YesButtonVisibility");
            this.e_65.SetBinding(Button.VisibilityProperty, binding_e_65_Visibility);
            Binding binding_e_65_Command = new Binding("YesCommand");
            this.e_65.SetBinding(Button.CommandProperty, binding_e_65_Command);
            // e_66 element
            this.e_66 = new Button();
            this.e_62.Children.Add(this.e_66);
            this.e_66.Name = "e_66";
            this.e_66.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_66.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_66.Content = "Ok";
            Binding binding_e_66_Visibility = new Binding("OkButtonVisibility");
            this.e_66.SetBinding(Button.VisibilityProperty, binding_e_66_Visibility);
            Binding binding_e_66_Command = new Binding("OkCommand");
            this.e_66.SetBinding(Button.CommandProperty, binding_e_66_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
