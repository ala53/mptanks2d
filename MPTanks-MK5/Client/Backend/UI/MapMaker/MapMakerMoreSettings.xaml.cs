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
    public partial class MapMakerMoreSettings : UIRoot {
        
        private Grid e_0;
        
        private ScrollViewer e_1;
        
        private StackPanel e_2;
        
        private TextBlock e_3;
        
        private StackPanel e_4;
        
        private StackPanel e_5;
        
        private TextBlock e_6;
        
        private TextBlock e_7;
        
        private StackPanel e_8;
        
        private TextBox MapNameBox;
        
        private TextBox MapAuthorBox;
        
        private StackPanel e_9;
        
        private TextBlock e_10;
        
        private StackPanel e_11;
        
        private StackPanel e_12;
        
        private TextBlock e_13;
        
        private NumericTextBox BackgroundR;
        
        private StackPanel e_14;
        
        private TextBlock e_15;
        
        private NumericTextBox BackgroundG;
        
        private StackPanel e_16;
        
        private TextBlock e_17;
        
        private NumericTextBox BackgroundB;
        
        private StackPanel e_18;
        
        private TextBlock e_19;
        
        private StackPanel e_20;
        
        private StackPanel e_21;
        
        private TextBlock e_22;
        
        private TextBox ShadowX;
        
        private StackPanel e_23;
        
        private TextBlock e_24;
        
        private TextBox ShadowY;
        
        private TextBlock e_25;
        
        private StackPanel e_26;
        
        private StackPanel e_27;
        
        private TextBlock e_28;
        
        private NumericTextBox ShadowR;
        
        private StackPanel e_29;
        
        private TextBlock e_30;
        
        private NumericTextBox ShadowG;
        
        private StackPanel e_31;
        
        private TextBlock e_32;
        
        private NumericTextBox ShadowB;
        
        private StackPanel e_33;
        
        private TextBlock e_34;
        
        private NumericTextBox ShadowA;
        
        private StackPanel e_35;
        
        private CheckBox UseWhitelistCheckBox;
        
        private TextBlock e_36;
        
        private TextBlock e_37;
        
        private TextBlock e_38;
        
        private TextBlock e_39;
        
        private TextBox WhitelistTextBox;
        
        private Button GoBackBtn;
        
        public MapMakerMoreSettings() : 
                base() {
            this.Initialize();
        }
        
        public MapMakerMoreSettings(int width, int height) : 
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
            this.e_1 = new ScrollViewer();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Width = 200F;
            this.e_1.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_1.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_1.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.e_1.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_1.Content = this.e_2;
            this.e_2.Name = "e_2";
            this.e_2.Background = new SolidColorBrush(new ColorW(0, 0, 0, 79));
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_2.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_3.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_3.Text = "Press F8 to hide/show this menu";
            this.e_3.Padding = new Thickness(0F, 0F, 0F, 10F);
            this.e_3.FontFamily = new FontFamily("JHUF");
            this.e_3.FontSize = 12F;
            // e_4 element
            this.e_4 = new StackPanel();
            this.e_2.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Orientation = Orientation.Horizontal;
            // e_5 element
            this.e_5 = new StackPanel();
            this.e_4.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            // e_6 element
            this.e_6 = new TextBlock();
            this.e_5.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Height = 18F;
            this.e_6.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_6.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_6.Text = "Map Name";
            this.e_6.Padding = new Thickness(0F, 0F, 8F, 0F);
            this.e_6.FontFamily = new FontFamily("JHUF");
            this.e_6.FontSize = 12F;
            // e_7 element
            this.e_7 = new TextBlock();
            this.e_5.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Height = 18F;
            this.e_7.Margin = new Thickness(0F, 8F, 0F, 0F);
            this.e_7.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_7.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_7.Text = "Author";
            this.e_7.Padding = new Thickness(0F, 0F, 8F, 0F);
            this.e_7.FontFamily = new FontFamily("JHUF");
            this.e_7.FontSize = 12F;
            // e_8 element
            this.e_8 = new StackPanel();
            this.e_4.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.Width = 110F;
            // MapNameBox element
            this.MapNameBox = new TextBox();
            this.e_8.Children.Add(this.MapNameBox);
            this.MapNameBox.Name = "MapNameBox";
            this.MapNameBox.Height = 18F;
            this.MapNameBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.MapNameBox.FontFamily = new FontFamily("JHUF");
            this.MapNameBox.FontSize = 12F;
            this.MapNameBox.Text = "ZSB Unedited Map";
            // MapAuthorBox element
            this.MapAuthorBox = new TextBox();
            this.e_8.Children.Add(this.MapAuthorBox);
            this.MapAuthorBox.Name = "MapAuthorBox";
            this.MapAuthorBox.Height = 18F;
            this.MapAuthorBox.Margin = new Thickness(0F, 8F, 0F, 0F);
            this.MapAuthorBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.MapAuthorBox.FontFamily = new FontFamily("JHUF");
            this.MapAuthorBox.FontSize = 12F;
            this.MapAuthorBox.Text = "Not ZSB Games";
            // e_9 element
            this.e_9 = new StackPanel();
            this.e_2.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            // e_10 element
            this.e_10 = new TextBlock();
            this.e_9.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_10.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_10.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_10.Text = "Background color";
            this.e_10.FontFamily = new FontFamily("JHUF");
            this.e_10.FontSize = 12F;
            // e_11 element
            this.e_11 = new StackPanel();
            this.e_9.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            this.e_11.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_11.Orientation = Orientation.Vertical;
            // e_12 element
            this.e_12 = new StackPanel();
            this.e_11.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Orientation = Orientation.Horizontal;
            // e_13 element
            this.e_13 = new TextBlock();
            this.e_12.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.Foreground = new SolidColorBrush(new ColorW(255, 0, 0, 255));
            this.e_13.Text = "R:";
            this.e_13.FontFamily = new FontFamily("JHUF");
            this.e_13.FontSize = 12F;
            // BackgroundR element
            this.BackgroundR = new NumericTextBox();
            this.e_12.Children.Add(this.BackgroundR);
            this.BackgroundR.Name = "BackgroundR";
            this.BackgroundR.Width = 80F;
            this.BackgroundR.Text = "0";
            this.BackgroundR.Minimum = 0F;
            this.BackgroundR.Maximum = 255F;
            // e_14 element
            this.e_14 = new StackPanel();
            this.e_11.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            this.e_14.Orientation = Orientation.Horizontal;
            // e_15 element
            this.e_15 = new TextBlock();
            this.e_14.Children.Add(this.e_15);
            this.e_15.Name = "e_15";
            this.e_15.Foreground = new SolidColorBrush(new ColorW(0, 128, 0, 255));
            this.e_15.Text = "G:";
            this.e_15.FontFamily = new FontFamily("JHUF");
            this.e_15.FontSize = 12F;
            // BackgroundG element
            this.BackgroundG = new NumericTextBox();
            this.e_14.Children.Add(this.BackgroundG);
            this.BackgroundG.Name = "BackgroundG";
            this.BackgroundG.Width = 80F;
            this.BackgroundG.Text = "0";
            this.BackgroundG.Minimum = 0F;
            this.BackgroundG.Maximum = 255F;
            // e_16 element
            this.e_16 = new StackPanel();
            this.e_11.Children.Add(this.e_16);
            this.e_16.Name = "e_16";
            this.e_16.Orientation = Orientation.Horizontal;
            // e_17 element
            this.e_17 = new TextBlock();
            this.e_16.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            this.e_17.Foreground = new SolidColorBrush(new ColorW(0, 0, 255, 255));
            this.e_17.Text = "B:";
            this.e_17.FontFamily = new FontFamily("JHUF");
            this.e_17.FontSize = 12F;
            // BackgroundB element
            this.BackgroundB = new NumericTextBox();
            this.e_16.Children.Add(this.BackgroundB);
            this.BackgroundB.Name = "BackgroundB";
            this.BackgroundB.Width = 80F;
            this.BackgroundB.Text = "0";
            this.BackgroundB.Minimum = 0F;
            this.BackgroundB.Maximum = 255F;
            // e_18 element
            this.e_18 = new StackPanel();
            this.e_2.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            // e_19 element
            this.e_19 = new TextBlock();
            this.e_18.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_19.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_19.Text = "Shadow offset";
            this.e_19.FontFamily = new FontFamily("JHUF");
            this.e_19.FontSize = 12F;
            // e_20 element
            this.e_20 = new StackPanel();
            this.e_18.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_20.Orientation = Orientation.Vertical;
            // e_21 element
            this.e_21 = new StackPanel();
            this.e_20.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.Orientation = Orientation.Horizontal;
            // e_22 element
            this.e_22 = new TextBlock();
            this.e_21.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_22.Text = "X:";
            this.e_22.FontFamily = new FontFamily("JHUF");
            this.e_22.FontSize = 12F;
            // ShadowX element
            this.ShadowX = new TextBox();
            this.e_21.Children.Add(this.ShadowX);
            this.ShadowX.Name = "ShadowX";
            this.ShadowX.Width = 80F;
            this.ShadowX.Text = "1";
            // e_23 element
            this.e_23 = new StackPanel();
            this.e_20.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Margin = new Thickness(0F, 0F, 0F, 0F);
            this.e_23.Orientation = Orientation.Horizontal;
            // e_24 element
            this.e_24 = new TextBlock();
            this.e_23.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_24.Text = "Y:";
            this.e_24.FontFamily = new FontFamily("JHUF");
            this.e_24.FontSize = 12F;
            // ShadowY element
            this.ShadowY = new TextBox();
            this.e_23.Children.Add(this.ShadowY);
            this.ShadowY.Name = "ShadowY";
            this.ShadowY.Width = 80F;
            this.ShadowY.Text = "1";
            // e_25 element
            this.e_25 = new TextBlock();
            this.e_18.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            this.e_25.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_25.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_25.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_25.Text = "Shadow color";
            this.e_25.FontFamily = new FontFamily("JHUF");
            this.e_25.FontSize = 12F;
            // e_26 element
            this.e_26 = new StackPanel();
            this.e_18.Children.Add(this.e_26);
            this.e_26.Name = "e_26";
            this.e_26.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_26.Orientation = Orientation.Vertical;
            // e_27 element
            this.e_27 = new StackPanel();
            this.e_26.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.Orientation = Orientation.Horizontal;
            // e_28 element
            this.e_28 = new TextBlock();
            this.e_27.Children.Add(this.e_28);
            this.e_28.Name = "e_28";
            this.e_28.Foreground = new SolidColorBrush(new ColorW(255, 0, 0, 255));
            this.e_28.Text = "R:";
            this.e_28.FontFamily = new FontFamily("JHUF");
            this.e_28.FontSize = 12F;
            // ShadowR element
            this.ShadowR = new NumericTextBox();
            this.e_27.Children.Add(this.ShadowR);
            this.ShadowR.Name = "ShadowR";
            this.ShadowR.Width = 80F;
            this.ShadowR.Text = "50";
            this.ShadowR.Minimum = 0F;
            this.ShadowR.Maximum = 255F;
            // e_29 element
            this.e_29 = new StackPanel();
            this.e_26.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.Orientation = Orientation.Horizontal;
            // e_30 element
            this.e_30 = new TextBlock();
            this.e_29.Children.Add(this.e_30);
            this.e_30.Name = "e_30";
            this.e_30.Foreground = new SolidColorBrush(new ColorW(0, 128, 0, 255));
            this.e_30.Text = "G:";
            this.e_30.FontFamily = new FontFamily("JHUF");
            this.e_30.FontSize = 12F;
            // ShadowG element
            this.ShadowG = new NumericTextBox();
            this.e_29.Children.Add(this.ShadowG);
            this.ShadowG.Name = "ShadowG";
            this.ShadowG.Width = 80F;
            this.ShadowG.Text = "50";
            this.ShadowG.Minimum = 0F;
            this.ShadowG.Maximum = 255F;
            // e_31 element
            this.e_31 = new StackPanel();
            this.e_26.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
            this.e_31.Orientation = Orientation.Horizontal;
            // e_32 element
            this.e_32 = new TextBlock();
            this.e_31.Children.Add(this.e_32);
            this.e_32.Name = "e_32";
            this.e_32.Foreground = new SolidColorBrush(new ColorW(0, 0, 255, 255));
            this.e_32.Text = "B:";
            this.e_32.FontFamily = new FontFamily("JHUF");
            this.e_32.FontSize = 12F;
            // ShadowB element
            this.ShadowB = new NumericTextBox();
            this.e_31.Children.Add(this.ShadowB);
            this.ShadowB.Name = "ShadowB";
            this.ShadowB.Width = 80F;
            this.ShadowB.Text = "50";
            this.ShadowB.Minimum = 0F;
            this.ShadowB.Maximum = 255F;
            // e_33 element
            this.e_33 = new StackPanel();
            this.e_26.Children.Add(this.e_33);
            this.e_33.Name = "e_33";
            this.e_33.Orientation = Orientation.Horizontal;
            // e_34 element
            this.e_34 = new TextBlock();
            this.e_33.Children.Add(this.e_34);
            this.e_34.Name = "e_34";
            this.e_34.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_34.Text = "A:";
            this.e_34.FontFamily = new FontFamily("JHUF");
            this.e_34.FontSize = 12F;
            // ShadowA element
            this.ShadowA = new NumericTextBox();
            this.e_33.Children.Add(this.ShadowA);
            this.ShadowA.Name = "ShadowA";
            this.ShadowA.Width = 80F;
            this.ShadowA.Text = "100";
            this.ShadowA.Minimum = 0F;
            this.ShadowA.Maximum = 255F;
            // e_35 element
            this.e_35 = new StackPanel();
            this.e_2.Children.Add(this.e_35);
            this.e_35.Name = "e_35";
            this.e_35.Margin = new Thickness(0F, 10F, 0F, 0F);
            // UseWhitelistCheckBox element
            this.UseWhitelistCheckBox = new CheckBox();
            this.e_35.Children.Add(this.UseWhitelistCheckBox);
            this.UseWhitelistCheckBox.Name = "UseWhitelistCheckBox";
            this.UseWhitelistCheckBox.Margin = new Thickness(10F, 0F, 0F, 0F);
            this.UseWhitelistCheckBox.HorizontalAlignment = HorizontalAlignment.Left;
            this.UseWhitelistCheckBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.UseWhitelistCheckBox.FontFamily = new FontFamily("JHUF");
            this.UseWhitelistCheckBox.FontSize = 12F;
            this.UseWhitelistCheckBox.Content = "Whitelist gamemodes";
            // e_36 element
            this.e_36 = new TextBlock();
            this.e_35.Children.Add(this.e_36);
            this.e_36.Name = "e_36";
            this.e_36.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_36.Text = "List of whitelisted gamemodes";
            this.e_36.FontFamily = new FontFamily("JHUF");
            this.e_36.FontSize = 12F;
            // e_37 element
            this.e_37 = new TextBlock();
            this.e_35.Children.Add(this.e_37);
            this.e_37.Name = "e_37";
            this.e_37.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_37.Text = "Separated with commas,";
            this.e_37.FontFamily = new FontFamily("JHUF");
            this.e_37.FontSize = 12F;
            // e_38 element
            this.e_38 = new TextBlock();
            this.e_35.Children.Add(this.e_38);
            this.e_38.Name = "e_38";
            this.e_38.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_38.Text = "in the form of,";
            this.e_38.FontFamily = new FontFamily("JHUF");
            this.e_38.FontSize = 12F;
            // e_39 element
            this.e_39 = new TextBlock();
            this.e_35.Children.Add(this.e_39);
            this.e_39.Name = "e_39";
            this.e_39.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_39.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_39.Text = "Mod name+gamemode name";
            this.e_39.FontFamily = new FontFamily("JHUF");
            this.e_39.FontSize = 12F;
            // WhitelistTextBox element
            this.WhitelistTextBox = new TextBox();
            this.e_35.Children.Add(this.WhitelistTextBox);
            this.WhitelistTextBox.Name = "WhitelistTextBox";
            this.WhitelistTextBox.Margin = new Thickness(0F, 10F, 0F, 0F);
            this.WhitelistTextBox.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.WhitelistTextBox.FontFamily = new FontFamily("JHUF");
            this.WhitelistTextBox.FontSize = 12F;
            this.WhitelistTextBox.Text = "CoreAssets+DeathMatchGamemode";
            // GoBackBtn element
            this.GoBackBtn = new Button();
            this.e_2.Children.Add(this.GoBackBtn);
            this.GoBackBtn.Name = "GoBackBtn";
            this.GoBackBtn.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.GoBackBtn.Padding = new Thickness(10F, 10F, 10F, 10F);
            this.GoBackBtn.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.GoBackBtn.FontFamily = new FontFamily("JHUF");
            this.GoBackBtn.FontSize = 12F;
            this.GoBackBtn.Content = "Go back to menu";
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
