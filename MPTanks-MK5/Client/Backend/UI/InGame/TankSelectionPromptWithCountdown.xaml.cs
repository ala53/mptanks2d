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
    using EmptyKeys.UserInterface.Charts;
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
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "1.11.0.0")]
    public partial class TankSelectionPromptWithCountdown : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private StackPanel e_2;
        
        private TextBlock Header;
        
        private TextBlock Subscript;
        
        private StackPanel e_3;
        
        private TextBlock e_4;
        
        private ScrollViewer e_5;
        
        private StackPanel TankOptions;
        
        private Button ConfirmButton;
        
        public TankSelectionPromptWithCountdown() : 
                base() {
            this.Initialize();
        }
        
        public TankSelectionPromptWithCountdown(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            this.e_0.Background = new SolidColorBrush(new ColorW(0, 0, 0, 159));
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Margin = new Thickness(0F, 20F, 0F, 0F);
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Top;
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.Orientation = Orientation.Horizontal;
            // Header element
            this.Header = new TextBlock();
            this.e_2.Children.Add(this.Header);
            this.Header.Name = "Header";
            this.Header.HorizontalAlignment = HorizontalAlignment.Center;
            this.Header.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.Header.Text = "Counting down...";
            this.Header.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.Header.FontFamily = new FontFamily("Karmatic Arcade");
            this.Header.FontSize = 36F;
            // Subscript element
            this.Subscript = new TextBlock();
            this.e_1.Children.Add(this.Subscript);
            this.Subscript.Name = "Subscript";
            this.Subscript.HorizontalAlignment = HorizontalAlignment.Center;
            this.Subscript.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.Subscript.Text = "-0 seconds remaining";
            this.Subscript.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.Subscript.FontFamily = new FontFamily("Karmatic Arcade");
            this.Subscript.FontSize = 24F;
            // e_3 element
            this.e_3 = new StackPanel();
            this.e_0.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Margin = new Thickness(0F, 70F, 0F, 0F);
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_3.VerticalAlignment = VerticalAlignment.Center;
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_3.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_4.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.e_4.Text = "Select a tank";
            this.e_4.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_4.FontFamily = new FontFamily("Karmatic Arcade");
            this.e_4.FontSize = 24F;
            // e_5 element
            this.e_5 = new ScrollViewer();
            this.e_3.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Height = 200F;
            this.e_5.MinWidth = 400F;
            // TankOptions element
            this.TankOptions = new StackPanel();
            this.e_5.Content = this.TankOptions;
            this.TankOptions.Name = "TankOptions";
            // ConfirmButton element
            this.ConfirmButton = new Button();
            this.e_3.Children.Add(this.ConfirmButton);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.HorizontalAlignment = HorizontalAlignment.Center;
            this.ConfirmButton.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ConfirmButton.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ConfirmButton.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ConfirmButton.FontFamily = new FontFamily("Karmatic Arcade");
            this.ConfirmButton.FontSize = 24F;
            this.ConfirmButton.Content = "Confirm Selection";
            FontManager.Instance.AddFont("Karmatic Arcade", 36F, FontStyle.Regular, "Karmatic_Arcade_27_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 18F, FontStyle.Regular, "Karmatic_Arcade_13.5_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 72F, FontStyle.Regular, "Karmatic_Arcade_54_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 96F, FontStyle.Regular, "Karmatic_Arcade_72_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 48F, FontStyle.Regular, "Karmatic_Arcade_36_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 20F, FontStyle.Regular, "Karmatic_Arcade_15_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 24F, FontStyle.Regular, "Karmatic_Arcade_18_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 40F, FontStyle.Regular, "Karmatic_Arcade_30_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 32F, FontStyle.Regular, "Karmatic_Arcade_24_Regular");
            FontManager.Instance.AddFont("Karmatic Arcade", 30F, FontStyle.Regular, "Karmatic_Arcade_22.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
