using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
            Dictionary<string,int> drinks = new Dictionary<string, int>
            {
                {"紅茶大杯",60 },
                {"紅茶小杯",40 },
                {"綠茶大杯",60 },
                {"綠茶小杯",40 },
                {"可樂大杯",50 },
                {"可樂小杯",30 },
                {"咖啡大杯",70 },
                {"咖啡小杯",50 }
            };

        Dictionary<string, int> orders = new Dictionary<string, int>();
            string takeout = "";
            public MainWindow()
            {
                InitializeComponent();
                DisplayDrinkMenu(drinks);
            }

        

        private void DisplayDrinkMenu(Dictionary<string, int> drinks)
            {
            stackpanel_DrinkMenu.Children.Clear();
            stackpanel_DrinkMenu.Height = drinks.Count * 40;
                foreach(var drink in drinks)
                {
                    var sp = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(2),
                        Height = 35,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = Brushes.AliceBlue
                    };

                    var cb = new CheckBox
                    {
                        Content = $"{drink.Key} {drink.Value}元",
                        FontFamily = new FontFamily("微軟正黑體"),
                        FontSize = 18,
                        Foreground = Brushes.Blue,
                        Margin = new Thickness(10, 0, 40, 0),
                        VerticalContentAlignment = VerticalAlignment.Center
                    };

                    var sl = new Slider
                    {
                        Width = 150,
                        Value = 0,
                        Minimum = 0,
                        Maximum = 10,
                        IsSnapToTickEnabled = true,
                        VerticalAlignment = VerticalAlignment.Center,
                    };

                    var lb = new Label
                    {
                        Width = 30,
                        Content = "0",
                        FontFamily = new FontFamily("微軟正黑體"),
                        FontSize = 18
                    };

                    Binding myBinding = new Binding("Value")
                    {
                        Source = sl,
                        Mode = BindingMode.OneWay
                    };
                    lb.SetBinding(ContentProperty, myBinding);

                    sp.Children.Add(cb);
                    sp.Children.Add(sl);
                    sp.Children.Add(lb);

                    stackpanel_DrinkMenu.Children.Add(sp);
                    
                }
            }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true)
            {
                takeout = rb.Content.ToString();
                //MessageBox.Show($"方式:{takeout}");
            }
        }
        
        
        private void OlderButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "";
            string discoutMessage = "";
            // 確認所有訂單的品項
            orders.Clear();
            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
                var sp1 = sp as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[1] as Slider;
                var lb = sp.Children[2] as Label;
                if (cb.IsChecked == true && sl.Value > 0)
                {
                    orders.Add(cb.Content.ToString(), int.Parse(lb.Content.ToString()));
                }
            }

            //顯示訂單細項，並計算金額
            double total = 0.0;
            double sellPrice = 0.0;

            ResultTextBlock.Text += $"取餐方式:{takeout}\n";

            int num = 1;
            foreach (var item in orders)
            {
                string drinkName = item.Key.Split(' ')[0];
                int quantity = item.Value;
                int price = drinks[drinkName];

                int subTotal = price * quantity;
                total += subTotal;
                ResultTextBlock.Text += $"{num}，{drinkName}x {quantity}杯，共{subTotal}元\n";
                num++;
            }

            if (total >= 500)
            {
                discoutMessage = "滿500元打8折";
                sellPrice = total * 0.8;
            }else if(total >= 300)
            {
                discoutMessage = "滿300元打9折";
                sellPrice = total * 0.9;
            }
            else
            {
                discoutMessage = "無折扣";
                sellPrice = total;
            }

            ResultTextBlock.Text += $"總金額:{total}元\n";
            ResultTextBlock.Text += $"{discoutMessage}，需付金額:{sellPrice}元\n";
        }
    }
}