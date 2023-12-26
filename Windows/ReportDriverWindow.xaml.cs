using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace ESOperatorTaxi.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportDriver.xaml
    /// </summary>
    public partial class ReportDriver : Window
    {
        Driver driver = new Driver();
        private ObservableCollection<Order> orders = new ObservableCollection<Order>();

        internal ReportDriver(DbManager dbManager, Driver driver)
        {
            InitializeComponent();

            this.driver = driver;

            nameDriver_lb.Content = driver.Surname +" "  + driver.Name +  " " + driver.Patronymic;
            numberDriver_lb.Content = driver.PhoneNumber;
            carDriver_lb.Content = driver.Car.Brand + " " + driver.Car.Model + " " + driver.Car.Number + ". Цвет: " + driver.Car.Colour;
            ratingDriver_lb.Content = 5;

            foreach(Order order in dbManager.Orders)
            {
                if (order.DriverId == driver.Id)
                {
                    orders.Add(order);
                }
            }
            var xAxis = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12);
            ordersDriver_lb.Content = orders.Count;
            ChartValues<int> values = new ChartValues<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            for(int i = 0; i < values.Count; i++)
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
            
            chart.Series = series;
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog p = new PrintDialog();
            if (p.ShowDialog() == true) p.PrintVisual(stackPanel, driver.Surname);

        }
    }
}
