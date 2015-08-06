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
    public partial class MainMenu : UIRoot {
        
        private Grid e_16;
        
        private StackPanel e_17;
        
        private TextBlock e_18;
        
        private TextBlock e_19;
        
        private StackPanel e_20;
        
        private Button e_21;
        
        private Button e_22;
        
        private Button e_23;
        
        private Button e_24;
        
        private Grid e_25;
        
        private Grid e_26;
        
        public MainMenu(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            InitializeElementResources(this);
            // e_16 element
            this.e_16 = new Grid();
            this.Content = this.e_16;
            this.e_16.Name = "e_16";
            RowDefinition row_e_16_0 = new RowDefinition();
            row_e_16_0.Height = new GridLength(0.2F, GridUnitType.Star);
            this.e_16.RowDefinitions.Add(row_e_16_0);
            RowDefinition row_e_16_1 = new RowDefinition();
            row_e_16_1.Height = new GridLength(0.8F, GridUnitType.Star);
            this.e_16.RowDefinitions.Add(row_e_16_1);
            ColumnDefinition col_e_16_0 = new ColumnDefinition();
            col_e_16_0.Width = new GridLength(0.3F, GridUnitType.Star);
            this.e_16.ColumnDefinitions.Add(col_e_16_0);
            ColumnDefinition col_e_16_1 = new ColumnDefinition();
            col_e_16_1.Width = new GridLength(0.65F, GridUnitType.Star);
            this.e_16.ColumnDefinitions.Add(col_e_16_1);
            // e_17 element
            this.e_17 = new StackPanel();
            this.e_16.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            Grid.SetRow(this.e_17, 0);
            // e_18 element
            this.e_18 = new TextBlock();
            this.e_17.Children.Add(this.e_18);
            this.e_18.Name = "e_18";
            this.e_18.Margin = new Thickness(20F, 20F, 20F, 0F);
            this.e_18.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_18.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_18.Text = "MP Tanks 2D";
            this.e_18.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 36F, FontStyle.Regular, "Segoe_UI_27_Regular");
            this.e_18.FontSize = 36F;
            // e_19 element
            this.e_19 = new TextBlock();
            this.e_17.Children.Add(this.e_19);
            this.e_19.Name = "e_19";
            this.e_19.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_19.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_19.Text = "Pre-Alpha (Version -1)";
            this.e_19.TextAlignment = TextAlignment.Center;
            FontManager.Instance.AddFont("Segoe UI", 18F, FontStyle.Regular, "Segoe_UI_13.5_Regular");
            this.e_19.FontSize = 18F;
            // e_20 element
            this.e_20 = new StackPanel();
            this.e_16.Children.Add(this.e_20);
            this.e_20.Name = "e_20";
            Grid.SetRow(this.e_20, 1);
            // e_21 element
            this.e_21 = new Button();
            this.e_20.Children.Add(this.e_21);
            this.e_21.Name = "e_21";
            this.e_21.Margin = new Thickness(20F, 20F, 20F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_21.Content = "Host game";
            Binding binding_e_21_Command = new Binding("HostGameCommand");
            this.e_21.SetBinding(Button.CommandProperty, binding_e_21_Command);
            // e_22 element
            this.e_22 = new Button();
            this.e_20.Children.Add(this.e_22);
            this.e_22.Name = "e_22";
            this.e_22.Margin = new Thickness(20F, 5F, 20F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_22.Content = "Join game";
            Binding binding_e_22_Command = new Binding("JoinGameCommand");
            this.e_22.SetBinding(Button.CommandProperty, binding_e_22_Command);
            // e_23 element
            this.e_23 = new Button();
            this.e_20.Children.Add(this.e_23);
            this.e_23.Name = "e_23";
            this.e_23.Margin = new Thickness(20F, 5F, 20F, 0F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_23.Content = "Settings";
            Binding binding_e_23_Command = new Binding("SettingsCommand");
            this.e_23.SetBinding(Button.CommandProperty, binding_e_23_Command);
            // e_24 element
            this.e_24 = new Button();
            this.e_20.Children.Add(this.e_24);
            this.e_24.Name = "e_24";
            this.e_24.Margin = new Thickness(20F, 5F, 20F, 20F);
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_24.Content = "Exit";
            Binding binding_e_24_Command = new Binding("ExitCommand");
            this.e_24.SetBinding(Button.CommandProperty, binding_e_24_Command);
            // e_25 element
            this.e_25 = new Grid();
            this.e_16.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            Grid.SetColumn(this.e_25, 1);
            Grid.SetRowSpan(this.e_25, 2);
            // e_26 element
            this.e_26 = new Grid();
            this.e_25.Children.Add(this.e_26);
            this.e_26.Name = "e_26";
            ColumnDefinition col_e_26_0 = new ColumnDefinition();
            col_e_26_0.Width = new GridLength(0.3F, GridUnitType.Star);
            this.e_26.ColumnDefinitions.Add(col_e_26_0);
            ColumnDefinition col_e_26_1 = new ColumnDefinition();
            col_e_26_1.Width = new GridLength(0.7F, GridUnitType.Star);
            this.e_26.ColumnDefinitions.Add(col_e_26_1);
            Binding binding_e_26_Visibility = new Binding("SettingsPaneVisibility");
            this.e_26.SetBinding(Grid.VisibilityProperty, binding_e_26_Visibility);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(UITemplateDictionary.Instance);
        }
    }
}
