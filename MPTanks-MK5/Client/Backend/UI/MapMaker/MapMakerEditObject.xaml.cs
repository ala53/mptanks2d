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
    public partial class MapMakerEditObject : UIRoot {
        
        private Grid e_0;
        
        private Grid e_1;
        
        private ScrollViewer e_2;
        
        private StackPanel e_3;
        
        private TextBlock e_4;
        
        private TextBlock e_5;
        
        private StackPanel e_6;
        
        private StackPanel e_7;
        
        private TextBlock e_8;
        
        private TextBox WidthBox;
        
        private StackPanel e_9;
        
        private TextBlock e_10;
        
        private TextBox HeightBox;
        
        private TextBlock e_11;
        
        private StackPanel e_12;
        
        private StackPanel e_13;
        
        private TextBlock e_14;
        
        private TextBox PosXBox;
        
        private StackPanel e_15;
        
        private TextBlock e_16;
        
        private TextBox PosYBox;
        
        private TextBlock e_17;
        
        private StackPanel e_18;
        
        private StackPanel e_19;
        
        private TextBox RotationBox;
        
        private TextBlock e_20;
        
        private StackPanel e_21;
        
        private StackPanel e_22;
        
        private TextBox DrawLayerBox;
        
        private TextBlock e_23;
        
        private StackPanel e_24;
        
        private StackPanel e_25;
        
        private TextBlock e_26;
        
        private NumericTextBox ColorR;
        
        private StackPanel e_27;
        
        private TextBlock e_28;
        
        private NumericTextBox ColorG;
        
        private StackPanel e_29;
        
        private TextBlock e_30;
        
        private NumericTextBox ColorB;
        
        private StackPanel e_31;
        
        private TextBlock e_32;
        
        private NumericTextBox ColorA;
        
        private TextBlock e_33;
        
        private ScrollViewer e_34;
        
        private StackPanel SettingsPanel;
        
        public MapMakerEditObject() : 
                base() {
            this.Initialize();
        }
        
        public MapMakerEditObject(int width, int height) : 
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
            this.e_1 = new Grid();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Width = 200F;
            this.e_1.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_1.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_1.Background = new SolidColorBrush(new ColorW(0, 0, 0, 79));
            // e_2 element
            this.e_2 = new ScrollViewer();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_2.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.e_2.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            // e_3 element
            this.e_3 = new StackPanel();
            this.e_2.Content = this.e_3;
            this.e_3.Name = "e_3";
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_3.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_4.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_4.Text = "Press F8 to hide/show this menu";
            this.e_4.Padding = new Thickness(0F, 0F, 0F, 10F);
            this.e_4.FontFamily = new FontFamily("JHUF");
            this.e_4.FontSize = 12F;
            // e_5 element
            this.e_5 = new TextBlock();
            this.e_3.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_5.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_5.Text = "Size";
            this.e_5.FontFamily = new FontFamily("JHUF");
            this.e_5.FontSize = 12F;
            // e_6 element
            this.e_6 = new StackPanel();
            this.e_3.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_6.HorizontalAlignment = HorizontalAlignment.Center;
            // e_7 element
            this.e_7 = new StackPanel();
            this.e_6.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Orientation = Orientation.Horizontal;
            // e_8 element
            this.e_8 = new TextBlock();
            this.e_7.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_8.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_8.Text = "X:";
            this.e_8.FontFamily = new FontFamily("JHUF");
            this.e_8.FontSize = 12F;
            // WidthBox element
            this.WidthBox = new TextBox();
            this.e_7.Children.Add(this.WidthBox);
            this.WidthBox.Name = "WidthBox";
            this.WidthBox.Width = 80F;
            this.WidthBox.FontFamily = new FontFamily("JHUF");
            this.WidthBox.FontSize = 12F;
            // e_9 element
            this.e_9 = new StackPanel();
            this.e_6.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Orientation = Orientation.Horizontal;
            // e_10 element
            this.e_10 = new TextBlock();
            this.e_9.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_10.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_10.Text = "Y:";
            this.e_10.FontFamily = new FontFamily("JHUF");
            this.e_10.FontSize = 12F;
            // HeightBox element
            this.HeightBox = new TextBox();
            this.e_9.Children.Add(this.HeightBox);
            this.HeightBox.Name = "HeightBox";
            this.HeightBox.Width = 80F;
            this.HeightBox.FontFamily = new FontFamily("JHUF");
            this.HeightBox.FontSize = 12F;
            // e_11 element
            this.e_11 = new TextBlock();
            this.e_3.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            this.e_11.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_11.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_11.Text = "Position";
            this.e_11.FontFamily = new FontFamily("JHUF");
            this.e_11.FontSize = 12F;
            // e_12 element
            this.e_12 = new StackPanel();
            this.e_3.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_12.HorizontalAlignment = HorizontalAlignment.Center;
            // e_13 element
            this.e_13 = new StackPanel();
            this.e_12.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.Orientation = Orientation.Horizontal;
            // e_14 element
            this.e_14 = new TextBlock();
            this.e_13.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            this.e_14.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_14.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_14.Text = "X:";
            this.e_14.FontFamily = new FontFamily("JHUF");
            this.e_14.FontSize = 12F;
            // PosXBox element
            this.PosXBox = new TextBox();
            this.e_13.Children.Add(this.PosXBox);
            this.PosXBox.Name = "PosXBox";
            this.PosXBox.Width = 80F;
            this.PosXBox.FontFamily = new FontFamily("JHUF");
            this.PosXBox.FontSize = 12F;
            // e_15 element
            this.e_15 = new StackPanel();
            this.e_12.Children.Add(this.e_15);
            this.e_15.Name = "e_15";
            this.e_15.Orientation = Orientation.Horizontal;
            // e_16 element
            this.e_16 = new TextBlock();
            this.e_15.Children.Add(this.e_16);
            this.e_16.Name = "e_16";
            this.e_16.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_16.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_16.Text = "Y:";
            this.e_16.FontFamily = new FontFamily("JHUF");
            this.e_16.FontSize = 12F;
            // PosYBox element
            this.PosYBox = new TextBox();
            this.e_15.Children.Add(this.PosYBox);
            this.PosYBox.Name = "PosYBox";
            this.PosYBox.Width = 80F;
            this.PosYBox.FontFamily = new FontFamily("JHUF");
            this.PosYBox.FontSize = 12F;
            // e_17 element
            this.e_17 = new TextBlock();
            this.e_3.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            this.e_17.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_17.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_17.Text = "Rotation (Degrees)";
            this.e_17.FontFamily = new FontFamily("JHUF");
            this.e_17.FontSize = 12F;
            // e_18 element
            this.e_18 = new StackPanel();
            this.e_3.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            this.e_18.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_18.HorizontalAlignment = HorizontalAlignment.Center;
            // e_19 element
            this.e_19 = new StackPanel();
            this.e_18.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.Orientation = Orientation.Horizontal;
            // RotationBox element
            this.RotationBox = new TextBox();
            this.e_19.Children.Add(this.RotationBox);
            this.RotationBox.Name = "RotationBox";
            this.RotationBox.Width = 80F;
            this.RotationBox.FontFamily = new FontFamily("JHUF");
            this.RotationBox.FontSize = 12F;
            // e_20 element
            this.e_20 = new TextBlock();
            this.e_3.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            this.e_20.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_20.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_20.Text = "Draw layer";
            this.e_20.FontFamily = new FontFamily("JHUF");
            this.e_20.FontSize = 12F;
            // e_21 element
            this.e_21 = new StackPanel();
            this.e_3.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.Margin = new Thickness(0F, 0F, 10F, 0F);
            this.e_21.HorizontalAlignment = HorizontalAlignment.Center;
            // e_22 element
            this.e_22 = new StackPanel();
            this.e_21.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Orientation = Orientation.Horizontal;
            // DrawLayerBox element
            this.DrawLayerBox = new TextBox();
            this.e_22.Children.Add(this.DrawLayerBox);
            this.DrawLayerBox.Name = "DrawLayerBox";
            this.DrawLayerBox.Width = 80F;
            this.DrawLayerBox.FontFamily = new FontFamily("JHUF");
            this.DrawLayerBox.FontSize = 12F;
            // e_23 element
            this.e_23 = new TextBlock();
            this.e_3.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.e_23.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_23.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_23.Text = "Color mask";
            this.e_23.FontFamily = new FontFamily("JHUF");
            this.e_23.FontSize = 12F;
            // e_24 element
            this.e_24 = new StackPanel();
            this.e_3.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_24.Orientation = Orientation.Vertical;
            // e_25 element
            this.e_25 = new StackPanel();
            this.e_24.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            this.e_25.Orientation = Orientation.Horizontal;
            // e_26 element
            this.e_26 = new TextBlock();
            this.e_25.Children.Add(this.e_26);
            this.e_26.Name = "e_26";
            this.e_26.Foreground = new SolidColorBrush(new ColorW(255, 0, 0, 255));
            this.e_26.Text = "R:";
            this.e_26.FontFamily = new FontFamily("JHUF");
            this.e_26.FontSize = 12F;
            // ColorR element
            this.ColorR = new NumericTextBox();
            this.e_25.Children.Add(this.ColorR);
            this.ColorR.Name = "ColorR";
            this.ColorR.Width = 80F;
            this.ColorR.Text = "50";
            this.ColorR.Minimum = 0F;
            this.ColorR.Maximum = 255F;
            // e_27 element
            this.e_27 = new StackPanel();
            this.e_24.Children.Add(this.e_27);
            this.e_27.Name = "e_27";
            this.e_27.Orientation = Orientation.Horizontal;
            // e_28 element
            this.e_28 = new TextBlock();
            this.e_27.Children.Add(this.e_28);
            this.e_28.Name = "e_28";
            this.e_28.Foreground = new SolidColorBrush(new ColorW(0, 128, 0, 255));
            this.e_28.Text = "G:";
            this.e_28.FontFamily = new FontFamily("JHUF");
            this.e_28.FontSize = 12F;
            // ColorG element
            this.ColorG = new NumericTextBox();
            this.e_27.Children.Add(this.ColorG);
            this.ColorG.Name = "ColorG";
            this.ColorG.Width = 80F;
            this.ColorG.Text = "50";
            this.ColorG.Minimum = 0F;
            this.ColorG.Maximum = 255F;
            // e_29 element
            this.e_29 = new StackPanel();
            this.e_24.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.Orientation = Orientation.Horizontal;
            // e_30 element
            this.e_30 = new TextBlock();
            this.e_29.Children.Add(this.e_30);
            this.e_30.Name = "e_30";
            this.e_30.Foreground = new SolidColorBrush(new ColorW(0, 0, 255, 255));
            this.e_30.Text = "B:";
            this.e_30.FontFamily = new FontFamily("JHUF");
            this.e_30.FontSize = 12F;
            // ColorB element
            this.ColorB = new NumericTextBox();
            this.e_29.Children.Add(this.ColorB);
            this.ColorB.Name = "ColorB";
            this.ColorB.Width = 80F;
            this.ColorB.Text = "50";
            this.ColorB.Minimum = 0F;
            this.ColorB.Maximum = 255F;
            // e_31 element
            this.e_31 = new StackPanel();
            this.e_24.Children.Add(this.e_31);
            this.e_31.Name = "e_31";
            this.e_31.Orientation = Orientation.Horizontal;
            // e_32 element
            this.e_32 = new TextBlock();
            this.e_31.Children.Add(this.e_32);
            this.e_32.Name = "e_32";
            this.e_32.Foreground = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            this.e_32.Text = "A:";
            this.e_32.FontFamily = new FontFamily("JHUF");
            this.e_32.FontSize = 12F;
            // ColorA element
            this.ColorA = new NumericTextBox();
            this.e_31.Children.Add(this.ColorA);
            this.ColorA.Name = "ColorA";
            this.ColorA.Width = 80F;
            this.ColorA.Text = "100";
            this.ColorA.Minimum = 0F;
            this.ColorA.Maximum = 255F;
            // e_33 element
            this.e_33 = new TextBlock();
            this.e_3.Children.Add(this.e_33);
            this.e_33.Name = "e_33";
            this.e_33.Margin = new Thickness(0F, 20F, 0F, 0F);
            this.e_33.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_33.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_33.Text = "Object Settings";
            this.e_33.FontFamily = new FontFamily("JHUF");
            this.e_33.FontSize = 12F;
            // e_34 element
            this.e_34 = new ScrollViewer();
            this.e_3.Children.Add(this.e_34);
            this.e_34.Name = "e_34";
            this.e_34.Margin = new Thickness(0F, 0F, 0F, 0F);
            this.e_34.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.e_34.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            // SettingsPanel element
            this.SettingsPanel = new StackPanel();
            this.e_34.Content = this.SettingsPanel;
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Height = 150F;
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
