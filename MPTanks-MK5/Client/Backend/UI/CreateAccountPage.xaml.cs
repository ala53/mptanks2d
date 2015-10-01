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
    public partial class CreateAccountPage : UIRoot {
        
        private Grid e_24;
        
        private StackPanel e_25;
        
        private TextBox TbNumber1;
        
        private Button e_26;
        
        public CreateAccountPage(int width, int height) : 
                base(width, height) {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.Background = new SolidColorBrush(new ColorW(0, 128, 0, 255));
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            // e_24 element
            this.e_24 = new Grid();
            this.Content = this.e_24;
            this.e_24.Name = "e_24";
            RowDefinition row_e_24_0 = new RowDefinition();
            row_e_24_0.Height = new GridLength(14F, GridUnitType.Star);
            this.e_24.RowDefinitions.Add(row_e_24_0);
            RowDefinition row_e_24_1 = new RowDefinition();
            row_e_24_1.Height = new GridLength(11F, GridUnitType.Star);
            this.e_24.RowDefinitions.Add(row_e_24_1);
            ColumnDefinition col_e_24_0 = new ColumnDefinition();
            col_e_24_0.Width = new GridLength(47F, GridUnitType.Star);
            this.e_24.ColumnDefinitions.Add(col_e_24_0);
            ColumnDefinition col_e_24_1 = new ColumnDefinition();
            col_e_24_1.Width = new GridLength(103F, GridUnitType.Star);
            this.e_24.ColumnDefinitions.Add(col_e_24_1);
            // e_25 element
            this.e_25 = new StackPanel();
            this.e_24.Children.Add(this.e_25);
            this.e_25.Name = "e_25";
            Grid.SetRowSpan(this.e_25, 2);
            // TbNumber1 element
            this.TbNumber1 = new TextBox();
            this.e_25.Children.Add(this.TbNumber1);
            this.TbNumber1.Name = "TbNumber1";
            this.TbNumber1.Height = 23F;
            this.TbNumber1.Width = 94F;
            this.TbNumber1.HorizontalAlignment = HorizontalAlignment.Left;
            this.TbNumber1.VerticalAlignment = VerticalAlignment.Top;
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            Binding binding_TbNumber1_Text = new Binding("TextBoxText");
            this.TbNumber1.SetBinding(TextBox.TextProperty, binding_TbNumber1_Text);
            // e_26 element
            this.e_26 = new Button();
            this.e_25.Children.Add(this.e_26);
            this.e_26.Name = "e_26";
            this.e_26.Width = 75F;
            this.e_26.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_26.VerticalAlignment = VerticalAlignment.Top;
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_26.Content = "Button";
        }
    }
}
