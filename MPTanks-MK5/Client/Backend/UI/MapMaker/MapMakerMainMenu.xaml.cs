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
    public partial class MapMakerMainMenu : UIRoot {
        
        private Grid e_0;
        
        private TextBlock e_1;
        
        public MapMakerMainMenu() : 
                base() {
            this.Initialize();
        }
        
        public MapMakerMainMenu(int width, int height) : 
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
            // e_1 element
            this.e_1 = new TextBlock();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_1.Text = "AAAAAAAAAAA";
            this.e_1.FontFamily = new FontFamily("JHUF");
            this.e_1.FontSize = 36F;
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
