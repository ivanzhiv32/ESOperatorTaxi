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
            LoadClients();
        }

        public ObservableCollection<Client> Clients { get; private set; }

        public void LoadClients() 
        {
            Clients = new ObservableCollection<Client>();

            string sql = "SELECT * FROM clients";
            MySqlDataReader reader = new MySqlCommand(sql, dbConnection).ExecuteReader();
            DataTable dt = new DataTable();
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
    }
}
