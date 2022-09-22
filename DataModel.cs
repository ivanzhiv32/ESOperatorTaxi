using System;
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
        }

        public ObservableCollection<Client> Clients { get; private set; }
        public ObservableCollection<Car> Cars { get; private set; }
        public ObservableCollection<Driver> Drivers { get; private set; }

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
                string sqlNumberCar = "SELECT Number FROM cars WHERE ID=" + row["IdCar"];
                string numberCar = null;
                sqlCommand = new MySqlCommand(sqlNumberCar, dbConnection);
                using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                    if (reader.Read()) numberCar = (string)reader.GetValue(0);
                //Drivers.Add(new Driver());

                Drivers.Add(
                    new Driver()
                    {
                        Id = (int)row["ID"],
                        Surname = (string)row["Surname"],
                        Name = (string)row["Name"],
                        Patronymic = (string)row["Patronomic"],
                        NumberCar = numberCar,
                        //IsFree = (bool)row["isFree"],
                    });
            }
        }
    }
}
