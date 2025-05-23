﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        DbManager dbManager;
        Order order = new Order();
        OperatorService operatorService;
        //internal static readonly OperatorService operatorService;
        public OrderWindow()
        {
            InitializeComponent();

            carClass_cb.Items.Add(CarClass.Econom);
            carClass_cb.Items.Add(CarClass.Comfort);
            carClass_cb.Items.Add(CarClass.Business);
        }

        internal OrderWindow(DbManager dbManager, OperatorService operatorService):this()
        {
            this.dbManager = dbManager;
            this.operatorService = operatorService;
        }

        internal OrderWindow(DbManager dbManager, OperatorService operatorService, Order order):this()
        {
            this.order = order;
            this.dbManager = dbManager;
            this.operatorService = operatorService;

            phoneNumberClient_tb.Text = Convert.ToString(order.Client.PhoneNumber);
            phoneNumberClient_tb.IsReadOnly = true;
            comment_tb.Text = order.Comment;
            carClass_cb.SelectedItem = order.Driver.Car.CarClass;
            price_tb.Text = Convert.ToString(order.Price);
            driver_tb.Text = order.Driver.Surname;
            startStreet_tb.Text = order.StartStreet.Name;
            startHouseNumber_tb.Text = Convert.ToString(order.StartHouseNumber);
            startEntranseNumber_tb.Text = Convert.ToString(order.StartEntranseNumber);
            finishStreet_tb.Text = order.FinishStreet.Name;
            finishHouseNumber_tb.Text = Convert.ToString(order.FinishHouseNumber);
            finishEntranseNumber_tb.Text = Convert.ToString(order.FinishEntranseNumber);

            
            
        }

        private void carClass_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGridSuitableDrivers.Items.Clear();

            foreach (Driver driver in dbManager.Drivers)
            {
                if (driver.IsFree == true && driver.Car.CarClass == (CarClass)carClass_cb.SelectedItem)
                {
                    dataGridSuitableDrivers.Items.Add(driver);
                }
            }
        }

        private void dataGridAddDrivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridSuitableDrivers.SelectedItem != null)
            {
                Driver driver = (Driver)dataGridSuitableDrivers.SelectedItem;
                driver_tb.Text = driver.Surname;
            }
            else driver_tb.Text = "";
        }

        private void create_btn_Click(object sender, RoutedEventArgs e)
        {
            order.Client = dbManager.Clients.FirstOrDefault(c => Convert.ToString(c.PhoneNumber) == phoneNumberClient_tb.Text);
            if(order.Client == null)
            {
                MessageBox.Show("Клиент с таким номером не найден.\nДобавьте нового клиента");
                ClientWindow clientWindow = new ClientWindow(dbManager);
                clientWindow.phoneNumber_tb.Text = phoneNumberClient_tb.Text;
                clientWindow.Show();
                return;
            }
            order.ClientId = order.Client.Id;

            order.Comment = comment_tb.Text;
            order.OrderClassId = (OrderClass)carClass_cb.SelectedItem;
            //Обработать автоматическое определение цены и водителя
            order.Price = Convert.ToInt32(price_tb.Text);
            if(dataGridSuitableDrivers.SelectedItem == null)
            {
                MessageBox.Show("Выберите подходящего водителя");
                return;
            }
            order.Driver = (Driver)dataGridSuitableDrivers.SelectedItem;
            order.DriverId = order.Driver.Id;

            Street startStreet = dbManager.Streets.FirstOrDefault(c => c.Name == startStreet_tb.Text);
            if(startStreet == null)
            {
                startStreet = new Street();
                startStreet.Name = startStreet_tb.Text;
                startStreet.CityId = 1;
                dbManager.Add<Street>(startStreet);
            }
            order.StartStreet = startStreet;
            order.StartStreetId = order.StartStreet.Id;
            order.StartHouseNumber = Convert.ToInt32(startHouseNumber_tb.Text);
            try
            {
                order.StartEntranseNumber = Convert.ToInt32(startEntranseNumber_tb.Text);
            }
            catch
            {
                order.StartEntranseNumber = 0;
            }

            Street finishStreet = dbManager.Streets.FirstOrDefault(c => c.Name == finishStreet_tb.Text);
            if (finishStreet == null)
            {
                finishStreet = new Street();
                finishStreet.Name = finishStreet_tb.Text;
                finishStreet.CityId = 1;
                dbManager.Add<Street>(finishStreet);
            }
            order.FinishStreet = finishStreet;
            order.FinishStreetId = order.FinishStreet.Id;
            order.FinishHouseNumber = Convert.ToInt32(finishHouseNumber_tb.Text);
            try
            {
                order.FinishEntranseNumber = Convert.ToInt32(finishEntranseNumber_tb.Text);
            }
            catch
            {
                order.FinishEntranseNumber = 0;
            }

            order.StatusId = OrderStatus.Creating;
            order.Date = DateTime.Now;

            dbManager.Add<Order>(order);
            
            Close();
        }

        private void change_btn_Click(object sender, RoutedEventArgs e)
        {
            order.Comment = comment_tb.Text;
            order.OrderClassId = (OrderClass)carClass_cb.SelectedItem;
            //Обработать автоматическое определение цены и водителя
            order.Price = Convert.ToInt32(price_tb.Text);
            if(dataGridSuitableDrivers.SelectedItem != null)
            {
                order.Driver = (Driver)dataGridSuitableDrivers.SelectedItem;
                order.DriverId = order.Driver.Id;
            }

            Street startStreet = dbManager.Streets.FirstOrDefault(c => c.Name == startStreet_tb.Text);
            if (startStreet == null)
            {
                startStreet = new Street();
                startStreet.Name = startStreet_tb.Text;
                startStreet.CityId = 1;
                dbManager.Add<Street>(startStreet);
            }
            order.StartStreet = startStreet;
            order.StartStreetId = order.StartStreet.Id;
            order.StartHouseNumber = Convert.ToInt32(startHouseNumber_tb.Text);
            order.StartEntranseNumber = Convert.ToInt32(startEntranseNumber_tb.Text);

            Street finishStreet = dbManager.Streets.FirstOrDefault(c => c.Name == finishStreet_tb.Text);
            if (finishStreet == null)
            {
                finishStreet = new Street();
                finishStreet.Name = finishStreet_tb.Text;
                finishStreet.CityId = 1;
                dbManager.Add<Street>(finishStreet);
            }
            order.FinishStreet = finishStreet;
            order.FinishStreetId = order.FinishStreet.Id;
            order.FinishHouseNumber = Convert.ToInt32(finishHouseNumber_tb.Text);
            order.FinishEntranseNumber = Convert.ToInt32(finishEntranseNumber_tb.Text);
            order.StatusId = OrderStatus.Creating;

            dbManager.Update<Order>(order);
            Close();
        }
        private void autoSelectDriver_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Geopoint start = new Geopoint("Брянск", startStreet_tb.Text, Convert.ToInt32(startHouseNumber_tb.Text));

            try
            {
                var driver = operatorService.ChooseDrivers(start, (OrderClass)carClass_cb.SelectedItem);
                if((CarClass)carClass_cb.SelectedItem != driver.Keys.First().Car.CarClass)
                {
                    carClass_cb.SelectedItem = driver.Keys.First().Car.CarClass;
                    MessageBox.Show("Класс заказа был повышен в связи с отсутствием подходящих водителей");
                }
                
                dataGridSuitableDrivers.SelectedItem = driver.Keys.First();
                driver_tb.Text = driver.Keys.First().Surname;
            }
            catch
            {
                autoSelectDriver_checkBox.IsChecked = false;
                MessageBox.Show("Подходящие водители не найдены");
            }
            
        }

        private void autoSelectPrice_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Geopoint start = new Geopoint("Брянск", startStreet_tb.Text, Convert.ToInt32(startHouseNumber_tb.Text));
                Geopoint finish = new Geopoint("Брянск", finishStreet_tb.Text, Convert.ToInt32(finishHouseNumber_tb.Text));

                price_tb.Text = Convert.ToString((int)operatorService.CalculateBasePrice(start, finish, (OrderClass)carClass_cb.SelectedItem));
            }
            catch
            {
                autoSelectPrice_checkBox.IsChecked = false;
                MessageBox.Show("Не удалось посчитать стоимость поездки.\nПроверьте правильность введенного адреса.");
            }
        }
    }
}
