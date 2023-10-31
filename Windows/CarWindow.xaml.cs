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

namespace ESOperatorTaxi
{
    /// <summary>
    /// Логика взаимодействия для CarWindow.xaml
    /// </summary>
    public partial class CarWindow : Window
    {
        DbManager dbManager;
        Car car = new Car();
        public CarWindow()
        {
            InitializeComponent();

            carClass_cb.Items.Add(CarClass.Econom);
            carClass_cb.Items.Add(CarClass.Comfort);
            carClass_cb.Items.Add(CarClass.Business);
        }
        internal CarWindow(DbManager dbManager)
        {
            InitializeComponent();

            this.dbManager = dbManager;

            carClass_cb.Items.Add(CarClass.Econom);
            carClass_cb.Items.Add(CarClass.Comfort);
            carClass_cb.Items.Add(CarClass.Business);
        }
        internal CarWindow(DbManager dbManager,  Car car)
        {
            InitializeComponent();

            this.dbManager = dbManager;
            this.car = car;

            carClass_cb.Items.Add(CarClass.Econom);
            carClass_cb.Items.Add(CarClass.Comfort);
            carClass_cb.Items.Add(CarClass.Business);
        }

        private void addCar_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                car.Brand = brand_tb.Text;
                car.Model = model_tb.Text;
                car.Number = number_tb.Text;
                car.CarClass = (CarClass)carClass_cb.SelectedItem;
                car.Colour = colour_tb.Text;

                dbManager.Add<Car>(car);
                dbManager.LoadEntities<Car>();
                MessageBox.Show("Автомобиль успешно добавлен");
                Close();
            }
            catch
            {
                MessageBox.Show("Введены некорректные данные автомобиля");
            }
        }

        private void updateCar_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                car.CarClass = (CarClass)carClass_cb.SelectedItem;
                dbManager.Update<Car>(car);
                MessageBox.Show("Данные обновлены");
                Close();
            }
            catch
            {
                MessageBox.Show("Введены некорректные данные");
            }
        }
    }
}
