using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ESOperatorTaxi.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportClientWindow.xaml
    /// </summary>
    public partial class ReportClientWindow : Window
    {
        Client client = new Client();
        private ObservableCollection<Order> orders = new ObservableCollection<Order>();

        internal ReportClientWindow(DbManager dbManager, Client client)
        {
            InitializeComponent();

            this.client = client;

            nameClient_lb.Content = client.Surname + " " + client.Name + " " + client.Patronymic;
            numberClient_lb.Content = client.PhoneNumber;

            foreach (Order order in dbManager.Orders)
            {
                if (order.ClientId == client.Id)
                {
                    orders.Add(order);
                }
            }

            var xAxis = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12);
            ordersClient_lb.Content = orders.Count;
            ChartValues<int> values = new ChartValues<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < values.Count; i++)
            {
                values[i] = orders.Count(c => c.Date.Month == i);
            }

            SeriesCollection series = new SeriesCollection
            {
                new ColumnSeries
                    {
                    Title = "Кол-во заказов",
                    Values = values,
                    Fill = Brushes.CornflowerBlue
                    }
            };

            chartClient.Series = series;
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog p = new PrintDialog();
            if (p.ShowDialog() == true) p.PrintVisual(stackPanel, client.Surname);
        }
    }
}
