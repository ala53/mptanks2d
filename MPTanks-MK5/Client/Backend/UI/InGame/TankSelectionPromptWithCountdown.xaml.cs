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
        
        private TextBlock Subscript;
        
        private StackPanel e_2;
        
        private StackPanel TankSelectionArea;
        
        private TextBlock e_3;
        
        private ScrollViewer e_4;
        
        private StackPanel TankOptions;
        
        private Button ConfirmButton;
        
        private StackPanel e_5;
        
        private TextBlock e_6;
        
        private TextBlock e_7;
        
        private StackPanel ReadyArea;
        
        private TextBlock e_8;
        
        private TextBlock e_9;
        
        private Button UnReadyButton;
        
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
            this.e_0.Background = new SolidColorBrush(new ColorW(0, 0, 0, 127));
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Margin = new Thickness(0F, 20F, 0F, 0F);
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Top;
            // Subscript element
            this.Subscript = new TextBlock();
            this.e_1.Children.Add(this.Subscript);
            this.Subscript.Name = "Subscript";
            this.Subscript.HorizontalAlignment = HorizontalAlignment.Center;
            this.Subscript.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.Subscript.Text = "0 seconds remaining to choose";
            this.Subscript.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.Subscript.FontFamily = new FontFamily("JHUF");
            this.Subscript.FontSize = 48F;
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_0.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.VerticalAlignment = VerticalAlignment.Center;
            // TankSelectionArea element
            this.TankSelectionArea = new StackPanel();
            this.e_2.Children.Add(this.TankSelectionArea);
            this.TankSelectionArea.Name = "TankSelectionArea";
            this.TankSelectionArea.Margin = new Thickness(0F, 0F, 0F, 0F);
            this.TankSelectionArea.HorizontalAlignment = HorizontalAlignment.Center;
            this.TankSelectionArea.VerticalAlignment = VerticalAlignment.Center;
            // e_3 element
            this.e_3 = new TextBlock();
            this.TankSelectionArea.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_3.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.e_3.Text = "Select a tank";
            this.e_3.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_3.FontFamily = new FontFamily("JHUF");
            this.e_3.FontSize = 24F;
            // e_4 element
            this.e_4 = new ScrollViewer();
            this.TankSelectionArea.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Height = 200F;
            this.e_4.Width = 420F;
            this.e_4.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.e_4.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            // TankOptions element
            this.TankOptions = new StackPanel();
            this.e_4.Content = this.TankOptions;
            this.TankOptions.Name = "TankOptions";
            // ConfirmButton element
            this.ConfirmButton = new Button();
            this.TankSelectionArea.Children.Add(this.ConfirmButton);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.ConfirmButton.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.ConfirmButton.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.ConfirmButton.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.ConfirmButton.FontFamily = new FontFamily("JHUF");
            this.ConfirmButton.FontSize = 24F;
            this.ConfirmButton.Content = "I\'m ready";
            // e_5 element
            this.e_5 = new StackPanel();
            this.TankSelectionArea.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Visibility = Visibility.Collapsed;
            // e_6 element
            this.e_6 = new TextBlock();
            this.e_5.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.MaxWidth = 420F;
            this.e_6.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_6.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_6.FontFamily = new FontFamily("JHUF");
            this.e_6.FontSize = 20F;
            // e_7 element
            this.e_7 = new TextBlock();
            this.e_5.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.MaxWidth = 420F;
            this.e_7.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_7.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.e_7.FontFamily = new FontFamily("JHUF");
            this.e_7.FontSize = 16F;
            // ReadyArea element
            this.ReadyArea = new StackPanel();
            this.e_2.Children.Add(this.ReadyArea);
            this.ReadyArea.Name = "ReadyArea";
            this.ReadyArea.Visibility = Visibility.Collapsed;
            // e_8 element
            this.e_8 = new TextBlock();
            this.ReadyArea.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_8.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_8.Text = "You\'re ready to go!";
            this.e_8.FontFamily = new FontFamily("JHUF");
            this.e_8.FontSize = 48F;
            // e_9 element
            this.e_9 = new TextBlock();
            this.ReadyArea.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_9.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_9.Text = "We\'re just waiting for some other players...";
            this.e_9.Padding = new Thickness(20F, 20F, 20F, 20F);
            this.e_9.FontFamily = new FontFamily("JHUF");
            this.e_9.FontSize = 24F;
            // UnReadyButton element
            this.UnReadyButton = new Button();
            this.ReadyArea.Children.Add(this.UnReadyButton);
            this.UnReadyButton.Name = "UnReadyButton";
            this.UnReadyButton.HorizontalAlignment = HorizontalAlignment.Center;
            this.UnReadyButton.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            this.UnReadyButton.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.UnReadyButton.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.UnReadyButton.FontFamily = new FontFamily("JHUF");
            this.UnReadyButton.FontSize = 24F;
            this.UnReadyButton.Content = "I lied. I\'m not ready.";
            FontManager.Instance.AddFont("JHUF", 36F, FontStyle.Regular, "JHUF_27_Regular");
            FontManager.Instance.AddFont("JHUF", 18F, FontStyle.Regular, "JHUF_13.5_Regular");
            FontManager.Instance.AddFont("JHUF", 72F, FontStyle.Regular, "JHUF_54_Regular");
            FontManager.Instance.AddFont("JHUF", 96F, FontStyle.Regular, "JHUF_72_Regular");
            FontManager.Instance.AddFont("JHUF", 48F, FontStyle.Regular, "JHUF_36_Regular");
            FontManager.Instance.AddFont("JHUF", 20F, FontStyle.Regular, "JHUF_15_Regular");
            FontManager.Instance.AddFont("JHUF", 24F, FontStyle.Regular, "JHUF_18_Regular");
            FontManager.Instance.AddFont("JHUF", 40F, FontStyle.Regular, "JHUF_30_Regular");
            FontManager.Instance.AddFont("JHUF", 32F, FontStyle.Regular, "JHUF_24_Regular");
            FontManager.Instance.AddFont("JHUF", 30F, FontStyle.Regular, "JHUF_22.5_Regular");
            FontManager.Instance.AddFont("JHUF", 16F, FontStyle.Regular, "JHUF_12_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
