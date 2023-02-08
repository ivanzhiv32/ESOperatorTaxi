﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace ESOperatorTaxi
{
    class DataModel
    {
        private MySqlConnection dbConnection;

        public DataModel() 
        {
            dbConnection = new MySqlConnection("server=s2.kts.tu-bryansk.ru;port=3306;user=IAS18.ZHivII;database=IAS18_ZHivII;password=3q%Md=Q2/4;");
            dbConnection.Open();
            LoadDrivers();
            LoadCars();
            LoadClients();
            LoadOrders();
        }

        public ObservableCollection<Client> Clients { get; private set; }
        public ObservableCollection<Car> Cars { get; private set; }
        public ObservableCollection<Driver> Drivers { get; private set; }
        public ObservableCollection<Order> Orders { get; private set; }
        public Dictionary<int, string> StatusesOrders = new Dictionary<int, string>();
        public Dictionary<int, string> CarsClasses = new Dictionary<int, string>();
        public Dictionary<int, string> FitDegree = new Dictionary<int, string>();
        public Dictionary<int, string> ClassesOrders = new Dictionary<int, string>();

        public void LoadClients() 
        {
            Clients = new ObservableCollection<Client>();

            string sql = "SELECT * FROM clients";
            MySqlCommand sqlCommand = new MySqlCommand(sql, dbConnection);
            DataTable dt = new DataTable();
            using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                dt.Load(reader);

            foreach (DataRow row in dt.Rows)
            {
                Clients.Add(
                    new Client()
                    { 
                        Id = (int)row["ID"], 
                        Name = (string)row["Name"],
                        Patronymic = (string)row["Patronymic"],
                        Surname = (string)row["Surname"],
                        PhoneNumber = (decimal)row["PhoneNumber"],
                    });
            }
        }

        public void LoadCars()
        {
            Cars = new ObservableCollection<Car>();

            string sql = "SELECT * FROM cars";
            MySqlCommand sqlCommand = new MySqlCommand(sql, dbConnection);
            DataTable dt = new DataTable();
            using (MySqlDataReader reader = sqlCommand.ExecuteReader()) 
                dt.Load(reader);


            foreach (DataRow row in dt.Rows)
            {
                string sqlClass = "SELECT Name FROM car_classes WHERE ID=" + row["IdClass"];
                string carClass = null;
                sqlCommand = new MySqlCommand(sqlClass, dbConnection);
                using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                    if (reader.Read()) carClass = (string)reader.GetValue(0);              

                Cars.Add(
                    new Car()
                    {
                        Id = (int)row["ID"],
                        Brand = (string)row["Brand"],
                        Number = (string)row["Number"],
                        Colour = (string)row["Colour"],
                        ClassName = carClass,
                    });
            }
        }

        public void LoadDrivers()
        {
            Drivers = new ObservableCollection<Driver>();

            string sql = "SELECT * FROM drivers";
            MySqlCommand sqlCommand = new MySqlCommand(sql, dbConnection);
            DataTable dt = new DataTable();
            using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                dt.Load(reader);


            foreach (DataRow row in dt.Rows)
            {
                Drivers.Add(
                    new Driver()
                    {
                        Id = (int)row["ID"],
                        Surname = (string)row["Surname"],
                        Name = (string)row["Name"],
                        Patronymic = (string)row["Patronomic"],
                        IsFree = Convert.ToBoolean(row["isFree"]),
                    });
            }
        }

        public void LoadOrders()
        {
            Orders = new ObservableCollection<Order>();

            string sql = "SELECT * FROM orders";
            //string sql = "SELECT orders.ID, IdClient, IdDriver, Comment, Price, Name, Status FROM orders, order_classes, statuses_order " +
            //    "WHERE order_classes.ID = orders.IdOrderClass AND statuses_order.ID = orders.IdOrderClass";
            MySqlCommand sqlCommand = new MySqlCommand(sql, dbConnection);
            DataTable dt = new DataTable();
            using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                dt.Load(reader);

            foreach (DataRow row in dt.Rows)
            {
                Orders.Add(
                    new Order()
                    {
                        Id = (int)row["ID"],
                        Comment = (string)row["Comment"],
                        Price = (int)row["Price"],
                        OrderClassId = (int)row["IdOrderClass"],
                        StatusId = (int)row["IdStatus"],
                        DriverId = (int)row["IdDriver"],
                        ClientId = (int)row["IdClient"]
                    });
            }
        }

        public void LoadDictionaries()
        {
            string[] namesTables = new string[] { "car_classes", "fit_degree", "order_classes", "statuses_order" };
            string sql;
            foreach(string table in namesTables)
            {
                sql = "SELECT * FROM" + table;
                MySqlCommand sqlCommand = new MySqlCommand(sql, dbConnection);
                DataTable dt = new DataTable();
                using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                    dt.Load(reader);

                Dictionary<int, string> entity = new Dictionary<int, string>();
                foreach (DataRow row in dt.Rows)
                {
                    entity.Add((int)row["ID"], (string)row["Name"]);
                }

                switch (table)
                {
                    case "car_classes":
                        CarsClasses = entity;
                        break;
                    case "fit_degree":
                        FitDegree = entity;
                        break;
                    case "order_classes":
                        ClassesOrders = entity;
                        break;
                    case "statuses_order":
                        StatusesOrders = entity;
                        break;
                }
            }
            
        }
    }
}
