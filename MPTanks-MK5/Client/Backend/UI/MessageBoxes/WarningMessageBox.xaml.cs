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
        
        private Grid e_40;
        
        private Border e_41;
        
        private StackPanel e_42;
        
        private Border e_43;
        
        private TextBlock Header;
        
        private TextBlock ContentT;
        
        private Border e_44;
        
        private StackPanel e_45;
        
        private Button Cancel;
        
        private Button No;
        
        private Button Yes;
        
        private Button Ok;
        
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
            // e_40 element
            this.e_40 = new Grid();
            this.Content = this.e_40;
            this.e_40.Name = "e_40";
            // e_41 element
            this.e_41 = new Border();
            this.e_40.Children.Add(this.e_41);
            this.e_41.Name = "e_41";
            this.e_41.MaxWidth = 600F;
            this.e_41.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_41.VerticalAlignment = VerticalAlignment.Center;
            this.e_41.BorderThickness = new Thickness(2F, 2F, 2F, 2F);
            this.e_41.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_42 element
            this.e_42 = new StackPanel();
            this.e_41.Child = this.e_42;
            this.e_42.Name = "e_42";
            this.e_42.Background = new SolidColorBrush(new ColorW(0, 0, 0, 204));
            // e_43 element
            this.e_43 = new Border();
            this.e_42.Children.Add(this.e_43);
            this.e_43.Name = "e_43";
            this.e_43.BorderThickness = new Thickness(0F, 0F, 0F, 2F);
            this.e_43.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // Header element
            this.Header = new TextBlock();
            this.e_43.Child = this.Header;
            this.Header.Name = "Header";
            this.Header.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            this.Header.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            this.Header.FontFamily = new FontFamily("Karmatic Arcade");
            this.Header.FontSize = 36F;
            this.Header.SetResourceReference(TextBlock.ForegroundProperty, "WarningTextColor");
            // ContentT element
            this.ContentT = new TextBlock();
            this.e_42.Children.Add(this.ContentT);
            this.ContentT.Name = "ContentT";
            this.ContentT.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.ContentT.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ContentT.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            this.ContentT.FontFamily = new FontFamily("Karmatic Arcade");
            this.ContentT.FontSize = 20F;
            this.ContentT.SetResourceReference(TextBlock.StyleProperty, "MenuContent");
            // e_44 element
            this.e_44 = new Border();
            this.e_42.Children.Add(this.e_44);
            this.e_44.Name = "e_44";
            this.e_44.BorderThickness = new Thickness(0F, 2F, 0F, 0F);
            this.e_44.SetResourceReference(Border.BorderBrushProperty, "WarningTextColor");
            // e_45 element
            this.e_45 = new StackPanel();
            this.e_44.Child = this.e_45;
            this.e_45.Name = "e_45";
            this.e_45.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_45.Orientation = Orientation.Horizontal;
            // Cancel element
            this.Cancel = new Button();
            this.e_45.Children.Add(this.Cancel);
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
            this.e_45.Children.Add(this.No);
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
            this.e_45.Children.Add(this.Yes);
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
            this.e_45.Children.Add(this.Ok);
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
