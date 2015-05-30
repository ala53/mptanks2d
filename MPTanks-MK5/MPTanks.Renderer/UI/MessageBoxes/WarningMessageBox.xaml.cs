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
        
        private Grid e_50;
        
        private Border e_51;
        
        private StackPanel e_52;
        
        private Border e_53;
        
        private TextBlock e_54;
        
        private TextBlock e_55;
        
        private Border e_56;
        
        private StackPanel e_57;
        
        private Button e_58;
        
        private Button e_59;
        
        private Button e_60;
        
        private Button e_61;
        
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
            // e_50 element
            this.e_50 = new Grid();
            this.Content = this.e_50;
            this.e_50.Name = "e_50";
            // e_51 element
            this.e_51 = new Border();
            this.e_50.Children.Add(this.e_51);
            this.e_51.Name = "e_51";
            this.e_51.MaxWidth = 600F;
            this.e_51.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_51.VerticalAlignment = VerticalAlignment.Center;
            this.e_51.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_51.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_52 element
            this.e_52 = new StackPanel();
            this.e_51.Child = this.e_52;
            this.e_52.Name = "e_52";
            this.e_52.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_53 element
            this.e_53 = new Border();
            this.e_52.Children.Add(this.e_53);
            this.e_53.Name = "e_53";
            this.e_53.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_53.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_54 element
            this.e_54 = new TextBlock();
            this.e_53.Child = this.e_54;
            this.e_54.Name = "e_54";
            this.e_54.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_54.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_54.FontSize = 20F;
            Binding binding_e_54_Text = new Binding("Header");
            this.e_54.SetBinding(TextBlock.TextProperty, binding_e_54_Text);
            this.e_54.SetResourceReference(TextBlock.ForegroundProperty, "WarningTextColor");
            // e_55 element
            this.e_55 = new TextBlock();
            this.e_52.Children.Add(this.e_55);
            this.e_55.Name = "e_55";
            this.e_55.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_55_Text = new Binding("Content");
            this.e_55.SetBinding(TextBlock.TextProperty, binding_e_55_Text);
            this.e_55.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_56 element
            this.e_56 = new Border();
            this.e_52.Children.Add(this.e_56);
            this.e_56.Name = "e_56";
            this.e_56.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_56.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_57 element
            this.e_57 = new StackPanel();
            this.e_56.Child = this.e_57;
            this.e_57.Name = "e_57";
            this.e_57.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_57.Orientation = Orientation.Horizontal;
            // e_58 element
            this.e_58 = new Button();
            this.e_57.Children.Add(this.e_58);
            this.e_58.Name = "e_58";
            this.e_58.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_58.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_58.Content = "Cancel";
            Binding binding_e_58_Visibility = new Binding("CancelButtonVisibility");
            this.e_58.SetBinding(Button.VisibilityProperty, binding_e_58_Visibility);
            Binding binding_e_58_Command = new Binding("CancelCommand");
            this.e_58.SetBinding(Button.CommandProperty, binding_e_58_Command);
            // e_59 element
            this.e_59 = new Button();
            this.e_57.Children.Add(this.e_59);
            this.e_59.Name = "e_59";
            this.e_59.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_59.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_59.Content = "No";
            Binding binding_e_59_Visibility = new Binding("NoButtonVisibility");
            this.e_59.SetBinding(Button.VisibilityProperty, binding_e_59_Visibility);
            Binding binding_e_59_Command = new Binding("NoCommand");
            this.e_59.SetBinding(Button.CommandProperty, binding_e_59_Command);
            // e_60 element
            this.e_60 = new Button();
            this.e_57.Children.Add(this.e_60);
            this.e_60.Name = "e_60";
            this.e_60.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_60.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_60.Content = "Yes";
            Binding binding_e_60_Visibility = new Binding("YesButtonVisibility");
            this.e_60.SetBinding(Button.VisibilityProperty, binding_e_60_Visibility);
            Binding binding_e_60_Command = new Binding("YesCommand");
            this.e_60.SetBinding(Button.CommandProperty, binding_e_60_Command);
            // e_61 element
            this.e_61 = new Button();
            this.e_57.Children.Add(this.e_61);
            this.e_61.Name = "e_61";
            this.e_61.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_61.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_61.Content = "Ok";
            Binding binding_e_61_Visibility = new Binding("OkButtonVisibility");
            this.e_61.SetBinding(Button.VisibilityProperty, binding_e_61_Visibility);
            Binding binding_e_61_Command = new Binding("OkCommand");
            this.e_61.SetBinding(Button.CommandProperty, binding_e_61_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
