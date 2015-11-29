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
        
        private Grid e_7;
        
        private StackPanel e_8;
        
        private TextBox TbNumber1;
        
        private Button e_9;
        
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
            // e_7 element
            this.e_7 = new Grid();
            this.Content = this.e_7;
            this.e_7.Name = "e_7";
            RowDefinition row_e_7_0 = new RowDefinition();
            row_e_7_0.Height = new GridLength(14F, GridUnitType.Star);
            this.e_7.RowDefinitions.Add(row_e_7_0);
            RowDefinition row_e_7_1 = new RowDefinition();
            row_e_7_1.Height = new GridLength(11F, GridUnitType.Star);
            this.e_7.RowDefinitions.Add(row_e_7_1);
            ColumnDefinition col_e_7_0 = new ColumnDefinition();
            col_e_7_0.Width = new GridLength(47F, GridUnitType.Star);
            this.e_7.ColumnDefinitions.Add(col_e_7_0);
            ColumnDefinition col_e_7_1 = new ColumnDefinition();
            col_e_7_1.Width = new GridLength(103F, GridUnitType.Star);
            this.e_7.ColumnDefinitions.Add(col_e_7_1);
            // e_8 element
            this.e_8 = new StackPanel();
            this.e_7.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            Grid.SetRowSpan(this.e_8, 2);
            // TbNumber1 element
            this.TbNumber1 = new TextBox();
            this.e_8.Children.Add(this.TbNumber1);
            this.TbNumber1.Name = "TbNumber1";
            this.TbNumber1.Height = 23F;
            this.TbNumber1.Width = 94F;
            this.TbNumber1.HorizontalAlignment = HorizontalAlignment.Left;
            this.TbNumber1.VerticalAlignment = VerticalAlignment.Top;
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            Binding binding_TbNumber1_Text = new Binding("TextBoxText");
            this.TbNumber1.SetBinding(TextBox.TextProperty, binding_TbNumber1_Text);
            // e_9 element
            this.e_9 = new Button();
            this.e_8.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Width = 75F;
            this.e_9.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_9.VerticalAlignment = VerticalAlignment.Top;
            FontManager.Instance.AddFont("Segoe UI", 12F, FontStyle.Regular, "Segoe_UI_9_Regular");
            this.e_9.Content = "Button";
        }
    }
}
