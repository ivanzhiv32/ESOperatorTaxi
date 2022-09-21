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
            //string connStr = "server=s2.kts.tu-bryansk.ru;port=3306;user=IAS18.ZHivII;database=IAS18_ZHivII;password=3q%Md=Q2/4;";
            //MySqlConnection conn = new MySqlConnection(connStr);
            //conn.Open();
            //string sql = "SELECT * FROM clients";
            //MySqlCommand command = new MySqlCommand(sql, conn);
            //MySqlDataReader reader = command.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Load(reader);

            //dataGrid.AutoGenerateColumns = true;
            //dataGrid.ItemsSource = dt.DefaultView;
            //conn.Close();

            DataModel dataModel = new DataModel();
            dataGrid.AutoGenerateColumns = true;
            dataGrid.ItemsSource = dataModel.Clients;
        }
    }
}
