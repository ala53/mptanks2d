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
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private Button LoadMapBtn;
        
        private Button SaveMapBtn;
        
        private StackPanel e_3;
        
        private TextBlock e_4;
        
        private ScrollViewer e_5;
        
        private StackPanel ModsListPanel;
        
        private Button LoadModBtn;
        
        private StackPanel e_6;
        
        private CheckBox LockToGridChkBox;
        
        private StackPanel e_7;
        
        private TextBlock e_8;
        
        private TextBox SearchBox;
        
        private ScrollViewer e_9;
        
        private StackPanel MapObjectSelectorPanel;
        
        private Button MoreSettingsBtn;
        
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
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Width = 200F;
            this.e_1.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_1.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_1.Background = new SolidColorBrush(new ColorW(0, 0, 0, 79));
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_2.Text = "Press F8 to hide/show this menu";
            this.e_2.FontFamily = new FontFamily("JHUF");
            this.e_2.FontSize = 12F;
            // LoadMapBtn element
            this.LoadMapBtn = new Button();
            this.e_1.Children.Add(this.LoadMapBtn);
            this.LoadMapBtn.Name = "LoadMapBtn";
            this.LoadMapBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.LoadMapBtn.FontFamily = new FontFamily("JHUF");
            this.LoadMapBtn.FontSize = 12F;
            this.LoadMapBtn.Content = "Load map";
            // SaveMapBtn element
            this.SaveMapBtn = new Button();
            this.e_1.Children.Add(this.SaveMapBtn);
            this.SaveMapBtn.Name = "SaveMapBtn";
            this.SaveMapBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.SaveMapBtn.FontFamily = new FontFamily("JHUF");
            this.SaveMapBtn.FontSize = 12F;
            this.SaveMapBtn.Content = "Save (autosaves every 1 min)";
            // e_3 element
            this.e_3 = new StackPanel();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_3.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_4.Text = "Mods this map depends on:";
            this.e_4.Padding = new Thickness(5F, 5F, 5F, 5F);
            this.e_4.FontFamily = new FontFamily("JHUF");
            this.e_4.FontSize = 12F;
            // e_5 element
            this.e_5 = new ScrollViewer();
            this.e_3.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Height = 90F;
            this.e_5.MaxHeight = 90F;
            // ModsListPanel element
            this.ModsListPanel = new StackPanel();
            this.e_5.Content = this.ModsListPanel;
            this.ModsListPanel.Name = "ModsListPanel";
            // LoadModBtn element
            this.LoadModBtn = new Button();
            this.e_3.Children.Add(this.LoadModBtn);
            this.LoadModBtn.Name = "LoadModBtn";
            this.LoadModBtn.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.LoadModBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.LoadModBtn.FontFamily = new FontFamily("JHUF");
            this.LoadModBtn.FontSize = 12F;
            this.LoadModBtn.Content = "Load another mod";
            // e_6 element
            this.e_6 = new StackPanel();
            this.e_1.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Margin = new Thickness(5F, 5F, 0F, 5F);
            this.e_6.HorizontalAlignment = HorizontalAlignment.Left;
            // LockToGridChkBox element
            this.LockToGridChkBox = new CheckBox();
            this.e_6.Children.Add(this.LockToGridChkBox);
            this.LockToGridChkBox.Name = "LockToGridChkBox";
            this.LockToGridChkBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.LockToGridChkBox.FontFamily = new FontFamily("JHUF");
            this.LockToGridChkBox.FontSize = 12F;
            this.LockToGridChkBox.Content = "Lock to grid";
            // e_7 element
            this.e_7 = new StackPanel();
            this.e_1.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Margin = new Thickness(0F, 0F, 0F, 10F);
            this.e_7.Orientation = Orientation.Horizontal;
            // e_8 element
            this.e_8 = new TextBlock();
            this.e_7.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_8.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_8.Text = "Search:";
            this.e_8.Padding = new Thickness(0F, 8F, 8F, 8F);
            this.e_8.FontFamily = new FontFamily("JHUF");
            this.e_8.FontSize = 12F;
            // SearchBox element
            this.SearchBox = new TextBox();
            this.e_7.Children.Add(this.SearchBox);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Width = 150F;
            // e_9 element
            this.e_9 = new ScrollViewer();
            this.e_1.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Height = 180F;
            this.e_9.MaxHeight = 180F;
            this.e_9.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.e_9.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            // MapObjectSelectorPanel element
            this.MapObjectSelectorPanel = new StackPanel();
            this.e_9.Content = this.MapObjectSelectorPanel;
            this.MapObjectSelectorPanel.Name = "MapObjectSelectorPanel";
            this.MapObjectSelectorPanel.Width = 175F;
            this.MapObjectSelectorPanel.Background = new SolidColorBrush(new ColorW(0, 0, 0, 144));
            // MoreSettingsBtn element
            this.MoreSettingsBtn = new Button();
            this.e_1.Children.Add(this.MoreSettingsBtn);
            this.MoreSettingsBtn.Name = "MoreSettingsBtn";
            this.MoreSettingsBtn.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.MoreSettingsBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.MoreSettingsBtn.FontFamily = new FontFamily("JHUF");
            this.MoreSettingsBtn.FontSize = 12F;
            this.MoreSettingsBtn.Content = "Map Settings";
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
            FontManager.Instance.AddFont("JHUF", 12F, FontStyle.Regular, "JHUF_9_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
