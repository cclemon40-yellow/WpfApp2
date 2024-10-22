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
    public partial class MainWindow : Window
    {
        // 飲料菜單，包含飲料名稱和價格
        Dictionary<string, int> drinks = new Dictionary<string, int>
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

        // 訂單，包含飲料名稱和數量
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string takeout = ""; // 取餐方式

        public MainWindow()
        {
            InitializeComponent();
            DisplayDrinkMenu(drinks); // 顯示飲料菜單
        }

        // 顯示飲料菜單
        private void DisplayDrinkMenu(Dictionary<string, int> drinks)
        {
            stackpanel_DrinkMenu.Children.Clear(); // 清空現有的菜單項目
            stackpanel_DrinkMenu.Height = drinks.Count * 40; // 設定StackPanel的高度
            foreach (var drink in drinks)
            {
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal, // 水平排列
                    Margin = new Thickness(2), // 外邊距
                    Height = 35, // 高度
                    VerticalAlignment = VerticalAlignment.Center, // 垂直置中
                    Background = Brushes.AliceBlue // 背景顏色
                };

                var cb = new CheckBox
                {
                    Content = $"{drink.Key} {drink.Value}元", // 顯示飲料名稱和價格
                    FontFamily = new FontFamily("微軟正黑體"), // 字體
                    FontSize = 18, // 字體大小
                    Foreground = Brushes.Blue, // 字體顏色
                    Margin = new Thickness(10, 0, 40, 0), // 外邊距
                    VerticalContentAlignment = VerticalAlignment.Center // 垂直置中
                };

                var sl = new Slider
                {
                    Width = 150, // 寬度
                    Value = 0, // 初始值
                    Minimum = 0, // 最小值
                    Maximum = 10, // 最大值
                    IsSnapToTickEnabled = true, // 啟用刻度對齊
                    VerticalAlignment = VerticalAlignment.Center, // 垂直置中
                };

                var lb = new Label
                {
                    Width = 30, // 寬度
                    Content = "0", // 初始內容
                    FontFamily = new FontFamily("微軟正黑體"), // 字體
                    FontSize = 18 // 字體大小
                };

                // 綁定Slider的值到Label
                Binding myBinding = new Binding("Value")
                {
                    Source = sl, // 綁定來源
                    Mode = BindingMode.OneWay // 單向綁定
                };
                lb.SetBinding(ContentProperty, myBinding); // 設定綁定

                sp.Children.Add(cb); // 將CheckBox加入StackPanel
                sp.Children.Add(sl); // 將Slider加入StackPanel
                sp.Children.Add(lb); // 將Label加入StackPanel

                stackpanel_DrinkMenu.Children.Add(sp); // 將StackPanel加入主StackPanel
            }
        }

        // 處理取餐方式的選擇
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true)
            {
                takeout = rb.Content.ToString(); // 設定取餐方式
            }
        }

        // 處理訂單按鈕點擊事件
        private void OlderButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = ""; // 清空結果顯示區域
            string discoutMessage = ""; // 折扣訊息
            // 確認所有訂單的品項
            orders.Clear(); // 清空現有訂單
            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[1] as Slider;
                var lb = sp.Children[2] as Label;
                if (cb.IsChecked == true && sl.Value > 0)
                {
                    string drinkName = cb.Content.ToString().Split(' ')[0]; // 取得飲料名稱
                    orders.Add(drinkName, int.Parse(lb.Content.ToString())); // 加入訂單
                }
            }

            // 顯示訂單細項，並計算金額
            double total = 0.0; // 總金額
            double sellPrice = 0.0; // 折扣後金額

            ResultTextBlock.Text += $"取餐方式:{takeout}\n"; // 顯示取餐方式

            int num = 1;
            foreach (var item in orders)
            {
                string drinkName = item.Key;
                int quantity = item.Value;
                int price = drinks[drinkName];

                int subTotal = price * quantity; // 計算小計
                total += subTotal; // 累加到總金額
                ResultTextBlock.Text += $"{num}，{drinkName}x {quantity}杯，共{subTotal}元\n"; // 顯示訂單細項
                num++;
            }

            // 計算折扣
            if (total >= 500)
            {
                discoutMessage = "滿500元打8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                discoutMessage = "滿300元打9折";
                sellPrice = total * 0.9;
            }
            else
            {
                discoutMessage = "無折扣";
                sellPrice = total;
            }

            ResultTextBlock.Text += $"總金額:{total}元\n"; // 顯示總金額
            ResultTextBlock.Text += $"{discoutMessage}，需付金額:{sellPrice}元\n"; // 顯示折扣訊息和折扣後金額
        }
    }
}
