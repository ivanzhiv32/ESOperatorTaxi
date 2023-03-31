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
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        MainWindow mainWindow = new MainWindow();
        DbManager dbManager;
        public ClientWindow()
        {
            InitializeComponent();
        }
        internal ClientWindow(DbManager dbManager)
        {
            InitializeComponent();

            this.dbManager = dbManager;
        }

        private void addClient_btn_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client();
            //DbManager dbManager = new DbManager();
            try
            {
                client.Surname = surname_tb.Text;
                client.Name = name_tb.Text;
                client.Patronymic = patronymic_tb.Text;
                client.PhoneNumber = Convert.ToInt64(phoneNumber_tb.Text);

                dbManager.Add<Client>(client);
                MessageBox.Show("Клиент успешно добавлен");
                Close();
            }
            catch
            {
                MessageBox.Show("Введены некорректные данные клиента");
            }
        }
    }
}
