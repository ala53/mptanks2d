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
        
        private Grid e_24;
        
        private Border e_25;
        
        private StackPanel e_26;
        
        private Border e_27;
        
        private TextBlock Header;
        
        private TextBlock ContentT;
        
        private Border e_28;
        
        private StackPanel e_29;
        
        private Button Cancel;
        
        private Button No;
        
        private Button Yes;
        
        private Button Ok;
        
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
            // e_24 element
            this.e_24 = new Grid();
            this.Content = this.e_24;
            this.e_24.Name = "e_24";
            // e_25 element
            this.e_25 = new Border();
            this.e_24.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            this.e_25.MaxWidth = 600F;
            this.e_25.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_25.VerticalAlignment = VerticalAlignment.Center;
            this.e_25.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_25.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_26 element
            this.e_26 = new StackPanel();
            this.e_25.Child = this.e_26;
            this.e_26.Name = "e_26";
            this.e_26.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_27 element
            this.e_27 = new Border();
            this.e_26.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_27.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // Header element
            this.Header = new TextBlock();
            this.e_27.Child = this.Header;
            this.Header.Name = "Header";
            this.Header.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            this.Header.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.Header.FontFamily = new FontFamily("Karmatic Arcade");
            this.Header.FontSize = 36F;
            this.Header.SetResourceReference(TextBlock.ForegroundProperty, "ErrorTextColor");
            // ContentT element
            this.ContentT = new TextBlock();
            this.e_26.Children.Add(this.ContentT);
            this.ContentT.Name = "ContentT";
            this.ContentT.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.ContentT.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ContentT.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.ContentT.FontFamily = new FontFamily("Karmatic Arcade");
            this.ContentT.FontSize = 20F;
            this.ContentT.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_28 element
            this.e_28 = new Border();
            this.e_26.Children.Add(this.e_28);
            this.e_28.Name = "e_28";
            this.e_28.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_28.SetResourceReference(Border.BorderBrushProperty, "ErrorTextColor");
            // e_29 element
            this.e_29 = new StackPanel();
            this.e_28.Child = this.e_29;
            this.e_29.Name = "e_29";
            this.e_29.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_29.Orientation = Orientation.Horizontal;
            // Cancel element
            this.Cancel = new Button();
            this.e_29.Children.Add(this.Cancel);
            this.Cancel.Name = "Cancel";
            this.Cancel.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.Cancel.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.Cancel.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.Cancel.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.Cancel.FontFamily = new FontFamily("Karmatic Arcade");
            this.Cancel.FontSize = 20F;
            this.Cancel.Content = "Cancel";
            Binding binding_Cancel_Visibility = new Binding("CancelButtonVisibility");
            this.Cancel.SetBinding(Button.VisibilityProperty, binding_Cancel_Visibility);
            // No element
            this.No = new Button();
            this.e_29.Children.Add(this.No);
            this.No.Name = "No";
            this.No.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.No.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.No.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.No.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.No.FontFamily = new FontFamily("Karmatic Arcade");
            this.No.FontSize = 20F;
            this.No.Content = "No";
            Binding binding_No_Visibility = new Binding("NoButtonVisibility");
            this.No.SetBinding(Button.VisibilityProperty, binding_No_Visibility);
            // Yes element
            this.Yes = new Button();
            this.e_29.Children.Add(this.Yes);
            this.Yes.Name = "Yes";
            this.Yes.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.Yes.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.Yes.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.Yes.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.Yes.FontFamily = new FontFamily("Karmatic Arcade");
            this.Yes.FontSize = 20F;
            this.Yes.Content = "Yes";
            Binding binding_Yes_Visibility = new Binding("YesButtonVisibility");
            this.Yes.SetBinding(Button.VisibilityProperty, binding_Yes_Visibility);
            // Ok element
            this.Ok = new Button();
            this.e_29.Children.Add(this.Ok);
            this.Ok.Name = "Ok";
            this.Ok.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.Ok.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.Ok.Padding = new Thickness(10F, 0F, 10F, 0F);
            this.Ok.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.Ok.FontFamily = new FontFamily("Karmatic Arcade");
            this.Ok.FontSize = 20F;
            this.Ok.Content = "Ok";
            Binding binding_Ok_Visibility = new Binding("OkButtonVisibility");
            this.Ok.SetBinding(Button.VisibilityProperty, binding_Ok_Visibility);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
