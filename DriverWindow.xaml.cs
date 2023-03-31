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
    /// Логика взаимодействия для DriverWindow.xaml
    /// </summary>
    public partial class DriverWindow : Window
    {
        DbManager dbManager;
        Driver driver = new Driver();
        public DriverWindow()
        {
            InitializeComponent();
        }
        internal DriverWindow(DbManager dbManager)
        {
            InitializeComponent();

            this.dbManager = dbManager;
        }
        internal DriverWindow(DbManager dbManager, Driver driver)
        {
            InitializeComponent();

            this.driver = driver;
            this.dbManager = dbManager;            
        }

        private void addDriver_btn_Click(object sender, RoutedEventArgs e)
        {
            Driver driver = new Driver();
            try
            {
                driver.Surname = surname_tb.Text;
                driver.Name = name_tb.Text;
                driver.Patronymic = patronymic_tb.Text;
                driver.Car = dbManager.Cars.FirstOrDefault(c => c.Number == car_tb.Text);
                if (driver.Car == null)
                {
                    MessageBox.Show("Автомобиль с таким номером не найден.\nДобавьте новый автомобиль");
                    CarWindow carWindow = new CarWindow(dbManager);
                    carWindow.number_tb.Text = car_tb.Text;
                    carWindow.Show();
                    return;
                }
                driver.CarId = driver.Car.Id;
                driver.IsFree = true;
                driver.PhoneNumber = Convert.ToInt64(phoneNumber_tb.Text);

                dbManager.Add<Driver>(driver);
                dbManager.LoadEntities<Driver>();
                MessageBox.Show("Водитель успешно добавлен");
                Close();
            }
            catch
            {
                MessageBox.Show("Введены некорректные данные водителя");
            }
        }
        private void changeDataDriver_btn_Click(object sender, RoutedEventArgs e)
        {
            driver.Surname = surname_tb.Text;
            driver.Name = name_tb.Text;
            driver.Patronymic = patronymic_tb.Text;
            driver.PhoneNumber = Convert.ToInt64(phoneNumber_tb.Text);
            driver.Car = dbManager.Cars.FirstOrDefault(c => c.Number == car_tb.Text);
            if (driver.Car == null)
            {
                MessageBox.Show("Автомобиль с таким номером не найден.\nДобавьте новый автомобиль");
                CarWindow carWindow = new CarWindow(dbManager);
                carWindow.number_tb.Text = car_tb.Text;
                carWindow.Show();
                return;
            }
            driver.CarId = driver.Car.Id;
            driver.IsFree = true;

            dbManager.Update<Driver>(driver);
            dbManager.LoadEntities<Driver>();
            MessageBox.Show("Данные о водителе успешно изменены");
            Close();
        }
    }
}
