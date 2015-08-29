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
        
        private Grid e_46;
        
        private Border e_47;
        
        private StackPanel e_48;
        
        private Border e_49;
        
        private TextBlock e_50;
        
        private TextBlock e_51;
        
        private Border e_52;
        
        private StackPanel e_53;
        
        private Button e_54;
        
        private Button e_55;
        
        private Button e_56;
        
        private Button e_57;
        
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
            // e_46 element
            this.e_46 = new Grid();
            this.Content = this.e_46;
            this.e_46.Name = "e_46";
            // e_47 element
            this.e_47 = new Border();
            this.e_46.Children.Add(this.e_47);
            this.e_47.Name = "e_47";
            this.e_47.MaxWidth = 600F;
            this.e_47.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_47.VerticalAlignment = VerticalAlignment.Center;
            this.e_47.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_47.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_48 element
            this.e_48 = new StackPanel();
            this.e_47.Child = this.e_48;
            this.e_48.Name = "e_48";
            this.e_48.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_49 element
            this.e_49 = new Border();
            this.e_48.Children.Add(this.e_49);
            this.e_49.Name = "e_49";
            this.e_49.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_49.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_50 element
            this.e_50 = new TextBlock();
            this.e_49.Child = this.e_50;
            this.e_50.Name = "e_50";
            this.e_50.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_50.HorizontalAlignment = HorizontalAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 20F, FontStyle.Regular, "Segoe_UI_15_Regular");
            this.e_50.FontSize = 20F;
            Binding binding_e_50_Text = new Binding("Header");
            this.e_50.SetBinding(TextBlock.TextProperty, binding_e_50_Text);
            this.e_50.SetResourceReference(TextBlock.ForegroundProperty, "SuccessTextColor");
            // e_51 element
            this.e_51 = new TextBlock();
            this.e_48.Children.Add(this.e_51);
            this.e_51.Name = "e_51";
            this.e_51.Margin = new Thickness(10F, 10F, 10F, 10F);
            FontManager.Instance.AddFont("Segoe UI", 16F, FontStyle.Regular, "Segoe_UI_12_Regular");
            Binding binding_e_51_Text = new Binding("Content");
            this.e_51.SetBinding(TextBlock.TextProperty, binding_e_51_Text);
            this.e_51.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_52 element
            this.e_52 = new Border();
            this.e_48.Children.Add(this.e_52);
            this.e_52.Name = "e_52";
            this.e_52.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_52.SetResourceReference(Border.BorderBrushProperty, "SuccessTextColor");
            // e_53 element
            this.e_53 = new StackPanel();
            this.e_52.Child = this.e_53;
            this.e_53.Name = "e_53";
            this.e_53.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_53.Orientation = Orientation.Horizontal;
            // e_54 element
            this.e_54 = new Button();
            this.e_53.Children.Add(this.e_54);
            this.e_54.Name = "e_54";
            this.e_54.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_54.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_54.Content = "Cancel";
            Binding binding_e_54_Visibility = new Binding("CancelButtonVisibility");
            this.e_54.SetBinding(Button.VisibilityProperty, binding_e_54_Visibility);
            Binding binding_e_54_Command = new Binding("CancelCommand");
            this.e_54.SetBinding(Button.CommandProperty, binding_e_54_Command);
            // e_55 element
            this.e_55 = new Button();
            this.e_53.Children.Add(this.e_55);
            this.e_55.Name = "e_55";
            this.e_55.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_55.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_55.Content = "No";
            Binding binding_e_55_Visibility = new Binding("NoButtonVisibility");
            this.e_55.SetBinding(Button.VisibilityProperty, binding_e_55_Visibility);
            Binding binding_e_55_Command = new Binding("NoCommand");
            this.e_55.SetBinding(Button.CommandProperty, binding_e_55_Command);
            // e_56 element
            this.e_56 = new Button();
            this.e_53.Children.Add(this.e_56);
            this.e_56.Name = "e_56";
            this.e_56.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_56.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_56.Content = "Yes";
            Binding binding_e_56_Visibility = new Binding("YesButtonVisibility");
            this.e_56.SetBinding(Button.VisibilityProperty, binding_e_56_Visibility);
            Binding binding_e_56_Command = new Binding("YesCommand");
            this.e_56.SetBinding(Button.CommandProperty, binding_e_56_Command);
            // e_57 element
            this.e_57 = new Button();
            this.e_53.Children.Add(this.e_57);
            this.e_57.Name = "e_57";
            this.e_57.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_57.Padding = new Thickness(10F, 0F, 10F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_57.Content = "Ok";
            Binding binding_e_57_Visibility = new Binding("OkButtonVisibility");
            this.e_57.SetBinding(Button.VisibilityProperty, binding_e_57_Visibility);
            Binding binding_e_57_Command = new Binding("OkCommand");
            this.e_57.SetBinding(Button.CommandProperty, binding_e_57_Command);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
