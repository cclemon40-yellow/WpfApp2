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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetTextBox = sender as TextBox;
            int amount;
            bool success = int.TryParse(targetTextBox.Text, out amount);
            if (!success)
            {
                MessageBox.Show("請輸入正整數", "輸入錯誤");
            }
            else if (amount <= 0) 
            {
                MessageBox.Show("請輸入正整數", "輸入錯誤");
            }
            else
            {
                var targetStackPanel = targetTextBox.Parent as StackPanel;
                var targetLabel = targetStackPanel.Children[0] as Label;
                var drinkName = targetLabel.Content.ToString();
                //MessageBox.Show($"您選擇了{drinkName}，數量為{amount}杯", "訂購成功");
                ResultTextBlock.Text += $"您選擇了{drinkName}，數量為{amount}杯\n";
            }

        }
    }
}