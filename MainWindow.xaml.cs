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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

using MySql.Data.MySqlClient;

namespace ESOperatorTaxi
{
    public partial class MainWindow : Window
    {
        internal DbManager dbManager = new DbManager();
        public MainWindow()
        {
            InitializeComponent();
            
            dbManager.Load();
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedTab;
            try
            {
                selectedTab = e.AddedItems[0] as TabItem;
            }
            catch
            {
                return;
            }
            
            if (selectedTab == null) return;
            else if (selectedTab.Name == "TabOrders")
            {
                dataGridOrders.Items.Clear();
                foreach (Order order in dbManager.Orders)
                {
                    dataGridOrders.Items.Add(order);
                }

                statusOrder_cb.Items.Clear();
                statusOrder_cb.Items.Add("Не выбрано");
                statusOrder_cb.Items.Add(OrderStatus.Creating);
                statusOrder_cb.Items.Add(OrderStatus.Running);
                statusOrder_cb.Items.Add(OrderStatus.Completed);
                statusOrder_cb.Items.Add(OrderStatus.Cancelled);

                classOrder_cb.Items.Clear();
                classOrder_cb.Items.Add("Не выбрано");
                classOrder_cb.Items.Add(OrderClass.Econom);
                classOrder_cb.Items.Add(OrderClass.Comfort);
                classOrder_cb.Items.Add(OrderClass.Business);
            }
            else if (selectedTab.Name == "TabDrivers")
            {
                dataGridDrivers.Items.Clear();
                foreach (Driver driver in dbManager.Drivers)
                {
                    dataGridDrivers.Items.Add(driver);
                }

                statusDriver_cb.Items.Clear();
                statusDriver_cb.Items.Add("Не выбрано");
                statusDriver_cb.Items.Add("Свободен");
                statusDriver_cb.Items.Add("Занят");
            }
            else if (selectedTab.Name == "TabCars")
            {
                dataGridCars.Items.Clear();
                foreach (Car car in dbManager.Cars)
                {
                    dataGridCars.Items.Add(car);
                }

                classCar_cb.Items.Clear();
                classCar_cb.Items.Add("Не выбрано");
                classCar_cb.Items.Add(CarClass.Econom);
                classCar_cb.Items.Add(CarClass.Comfort);
                classCar_cb.Items.Add(CarClass.Business);
            }
        }

        private void addOrder_btn_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(dbManager);
            orderWindow.Title = "Создание заказа";
            orderWindow.create_btn.Visibility = Visibility.Visible;
            orderWindow.Show();
        }

        private void deleteOrder_btn_Click(object sender, RoutedEventArgs e)
        {
            Order order = (Order)dataGridOrders.SelectedItem;
            if (order == null)
            {
                MessageBox.Show("Выберите заказ для удаления");
                return;
            }
            Review review = dbManager.Reviews.FirstOrDefault(c => c.OrderId == order.Id);
            if(review != null) dbManager.Delete<Review>(review);
            dbManager.Delete<Order>(order);
            dataGridOrders.Items.Remove(order);
        }

        private void changeOrder_btn_Click(object sender, RoutedEventArgs e)
        {
            Order order = (Order)dataGridOrders.SelectedItem;
            if (order == null)
            {
                MessageBox.Show("Выберите заказ для изменения");
                return;
            }

            OrderWindow orderWindow = new OrderWindow(dbManager, order);
            orderWindow.Title = "Изменение заказа";
            orderWindow.change_btn.Visibility = Visibility.Visible;

            orderWindow.Show();
        }

        private void updateOrder_btn_Click(object sender, RoutedEventArgs e)
        {
            dataGridOrders.Items.Clear();
            foreach (Order order in dbManager.Orders)
            {
                dataGridOrders.Items.Add(order);
            }
        }
        private void applyOrderFilter_btn_Click(object sender, RoutedEventArgs e)
        {
            var statusOrder = statusOrder_cb.SelectedItem;
            var classOrder = classOrder_cb.SelectedItem;
            var numberDriver = driverNumber_tb.Text;
            DateTime? dateOrder = dateOrder_dp.SelectedDate;

            dataGridOrders.Items.Clear();
            foreach (Order order in dbManager.Orders)
            {
                dataGridOrders.Items.Add(order);
            }

            if (statusOrder.GetType() == typeof(OrderStatus))
            {
                foreach(Order order in dbManager.Orders)
                {
                    if (order.StatusId != (OrderStatus)statusOrder) dataGridOrders.Items.Remove(order);
                }
            }
            if (classOrder.GetType() == typeof(OrderClass))
            {
                foreach (Order order in dbManager.Orders)
                {
                    if(order.OrderClassId != (OrderClass)classOrder) dataGridOrders.Items.Remove(order);
                }
            }
            if(numberDriver != "")
            {
                foreach (Order order in dbManager.Orders)
                {
                    if (Convert.ToString(order.Driver.PhoneNumber) != numberDriver) dataGridOrders.Items.Remove(order);
                }
            }

            if (dateOrder != null)
            {
                foreach (Order order in dbManager.Orders)
                {
                    if (dateOrder.Value.Date != order.Date.Date) dataGridOrders.Items.Remove(order);
                }
            }
        }

        private void addDriver_btn_Click(object sender, RoutedEventArgs e)
        {
            DriverWindow driverWindow = new DriverWindow(dbManager);
            driverWindow.Title = "Добавление водителя";
            driverWindow.changeDataDriver_btn.Visibility = Visibility.Visible;
            driverWindow.Show();
        }

        private void deleteDriver_btn_Click(object sender, RoutedEventArgs e)
        {
            Driver driver = (Driver)dataGridDrivers.SelectedItem;

            if (driver == null)
            {
                MessageBox.Show("Выберите водителя для удаления");
                return;
            }

            foreach(TripTicket ticket in dbManager.TripTickets)
            {
                if (ticket.Driver == driver) dbManager.Delete<TripTicket>(ticket);
            }
            foreach (DriverRating driverRating in dbManager.DriverRatings)
            {
                if (driverRating.Driver == driver) dbManager.Delete<DriverRating>(driverRating);
            }
            foreach (Order order in dbManager.Orders)
            {
                if (order.Driver == driver) dbManager.Delete<Order>(order);
            }

            dbManager.Delete<Driver>(driver);
            dataGridDrivers.Items.Remove(driver);
        }

        private void changeDriver_btn_Click(object sender, RoutedEventArgs e)
        {
            Driver driver = (Driver)dataGridDrivers.SelectedItem;
            if (driver == null)
            {
                MessageBox.Show("Выберите водителя из таблицы");
                return;
            }

            DriverWindow driverWindow = new DriverWindow(dbManager, driver);
            driverWindow.Title = "Изменение данных водителя";
            driverWindow.changeDataDriver_btn.Visibility = Visibility.Visible;

            driverWindow.surname_tb.Text = driver.Surname;
            driverWindow.name_tb.Text = driver.Name;
            driverWindow.patronymic_tb.Text = driver.Patronymic;
            driverWindow.phoneNumber_tb.Text = Convert.ToString(driver.PhoneNumber);
            driverWindow.car_tb.Text = driver.Car.Number;

            driverWindow.Show();
        }

        private void updateDriver_btn_Click(object sender, RoutedEventArgs e)
        {
            dataGridDrivers.Items.Clear();
            foreach (Driver driver in dbManager.Drivers)
            {
                dataGridDrivers.Items.Add(driver);
            }
        }

        private void applyDriverFilter_btn_Click(object sender, RoutedEventArgs e)
        {
            var status = statusDriver_cb.SelectedItem;
            var surname = driverSurname_tb.Text;
            var phoneNumber = driverPhoneNumber_tb.Text;
            var carNumber = numberCar_tb.Text;

            dataGridDrivers.Items.Clear();
            foreach (Driver driver in dbManager.Drivers)
            {
                dataGridDrivers.Items.Add(driver);
            }

            if (status != "Не выбрано")
            {
                foreach (Driver driver in dbManager.Drivers)
                {
                    if (driver.Status != status) dataGridDrivers.Items.Remove(driver);
                }
            }

            if (surname != "")
            {
                foreach (Driver driver in dbManager.Drivers)
                {
                    if (driver.Surname.ToLower() != surname.ToLower()) dataGridDrivers.Items.Remove(driver);
                }
            }

            if (phoneNumber != "")
            {
                foreach (Driver driver in dbManager.Drivers)
                {
                    if (Convert.ToString(driver.PhoneNumber) != phoneNumber) dataGridDrivers.Items.Remove(driver);
                }
            }
            if (carNumber != "")
            {
                foreach (Driver driver in dbManager.Drivers)
                {
                    if (driver.Car.Number.ToLower() != carNumber.ToLower()) dataGridDrivers.Items.Remove(driver);
                }
            }           
        }

        private void addCar_btn_Click(object sender, RoutedEventArgs e)
        {
            CarWindow carWindow = new CarWindow(dbManager);
            carWindow.Title = "Добавление авто";
            carWindow.addCar_btn.Visibility = Visibility.Visible;
            carWindow.Show();
        }

        private void changeCar_btn_Click(object sender, RoutedEventArgs e)
        {
            Car car = (Car)dataGridCars.SelectedItem;
            if (car == null)
            {
                MessageBox.Show("Выберите автомобиль из таблицы");
                return;
            }

            CarWindow carWindow = new CarWindow(dbManager, car);
            carWindow.Title = "Редактирование";
            carWindow.updateCar_btn.Visibility = Visibility.Visible;
            carWindow.brand_tb.IsReadOnly = true;
            carWindow.model_tb.IsReadOnly = true;
            carWindow.number_tb.IsReadOnly = true;

            carWindow.brand_tb.Text = car.Brand;
            carWindow.model_tb.Text = car.Model;
            carWindow.number_tb.Text = car.Number;
            carWindow.carClass_cb.SelectedItem = car.CarClass;
            carWindow.colour_tb.Text = car.Colour;

            carWindow.Show();
        }

        private void applyCarFilter_btn_Click(object sender, RoutedEventArgs e)
        {
            var classCar = classCar_cb.SelectedItem;
            var brand = carBrand_tb.Text;
            var model = carModel_tb.Text;
            var number = carNumber_tb.Text;

            dataGridCars.Items.Clear();
            foreach (Car car in dbManager.Cars)
            {
                dataGridCars.Items.Add(car);
            }

            if (classCar.GetType() == typeof(CarClass))
            {
                foreach (Car car in dbManager.Cars)
                {
                    if (car.CarClass != (CarClass)classCar) dataGridCars.Items.Remove(car);
                }
            }
            if (brand != "")
            {
                foreach (Car car in dbManager.Cars)
                {
                    if (car.Brand.ToLower() != brand.ToLower()) dataGridCars.Items.Remove(car);
                }
            }
            if (model != "")
            {
                foreach (Car car in dbManager.Cars)
                {
                    if (car.Model.ToLower() != model.ToLower()) dataGridCars.Items.Remove(car);
                }
            }
            if (number != "")
            {
                foreach (Car car in dbManager.Cars)
                {
                    if (car.Number.ToLower() != number.ToLower()) dataGridCars.Items.Remove(car);
                }
            }
        }

        
    }
}
