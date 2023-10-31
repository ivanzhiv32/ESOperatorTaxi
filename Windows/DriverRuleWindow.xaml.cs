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
    /// Логика взаимодействия для DriverRuleWindow.xaml
    /// </summary>
    public partial class DriverRuleWindow : Window
    {
        DriverSelectionRule driverRule = new DriverSelectionRule();
        DbManager dbManager;

        public DriverRuleWindow()
        {
            InitializeComponent();

            classOrder_cb.Items.Add(OrderClass.Econom);
            classOrder_cb.Items.Add(OrderClass.Comfort);
            classOrder_cb.Items.Add(OrderClass.Business);

            classCar_cb.Items.Add(CarClass.Econom);
            classCar_cb.Items.Add(CarClass.Comfort);
            classCar_cb.Items.Add(CarClass.Business);

            degreeCompliance_cb.Items.Add(DegreeCompliance.One);
            degreeCompliance_cb.Items.Add(DegreeCompliance.Two);
            degreeCompliance_cb.Items.Add(DegreeCompliance.Three);
            degreeCompliance_cb.Items.Add(DegreeCompliance.Four);
        }

        internal DriverRuleWindow(DbManager dbManager):this()
        {
            this.dbManager = dbManager;
        }

        internal DriverRuleWindow(DbManager dbManager, DriverSelectionRule driverRule):this()
        {
            this.dbManager = dbManager;
            this.driverRule = driverRule;
        }


        private void addDriverRuleWindow_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                driverRule.MaxDistanceToStartAddress = Convert.ToInt32(distance_tb.Text);
                driverRule.MinDriverRating = Convert.ToInt32(rating_tb.Text);
                driverRule.OrderClass = (OrderClass)classOrder_cb.SelectedItem;
                driverRule.CarClass = (CarClass)classCar_cb.SelectedItem;
                driverRule.DegreeCompliance = (DegreeCompliance)degreeCompliance_cb.SelectedItem;

                dbManager.Add<DriverSelectionRule>(driverRule);
                dbManager.LoadEntities<DriverSelectionRule>();
                MessageBox.Show("Правило успешно добавлено");
                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void changeDriverRuleWindow_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                driverRule.MaxDistanceToStartAddress = Convert.ToInt32(distance_tb.Text);
                driverRule.MinDriverRating = Convert.ToInt32(rating_tb.Text);
                driverRule.OrderClass = (OrderClass)classOrder_cb.SelectedItem;
                driverRule.CarClass = (CarClass)classCar_cb.SelectedItem;
                driverRule.DegreeCompliance = (DegreeCompliance)degreeCompliance_cb.SelectedItem;

                dbManager.Update(driverRule);
                //dbManager.LoadEntities<DriverSelectionRule>();
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
