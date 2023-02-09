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
        public MainWindow()
        {
            InitializeComponent();

            DbManager dbManager = new DbManager();
            Client client = new Client()
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Patronymic = "Patronymic",
                PhoneNumber = 123
            };
            var list = dbManager.FindCollectionByType<Client>();
            list.Add(client);
            //dbManager.Add(client);

            //var entities = dbManager.LoadEntities<Client>();

            //dataGridOrders.AutoGenerateColumns = true;
            //dataGridOrders.ItemsSource = dataModel.Orders;
        }
    }
}
