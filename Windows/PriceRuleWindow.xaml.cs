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
    /// Логика взаимодействия для PriceRuleWindow.xaml
    /// </summary>
    public partial class PriceRuleWindow : Window
    {
        DbManager dbManager;
        PriceRule priceRule = new PriceRule();
        public PriceRuleWindow()
        {
            InitializeComponent();
            classOrder_cb.Items.Add(OrderClass.Econom);
            classOrder_cb.Items.Add(OrderClass.Comfort);
            classOrder_cb.Items.Add(OrderClass.Business);
        }
        internal PriceRuleWindow(DbManager dbManager):this()
        {
            this.dbManager = dbManager;
        }
        internal PriceRuleWindow(DbManager dbManager, PriceRule priceRule): this()
        {
            this.dbManager = dbManager;
            this.priceRule = priceRule;
        }

        private void addPriceRuleWindow_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                priceRule.StartRange = Convert.ToInt32(startRange_tb.Text);
                priceRule.EndRange = Convert.ToInt32(endRange_tb.Text);
                priceRule.OrderClass = (OrderClass)classOrder_cb.SelectedItem;
                priceRule.PricePerKm = Convert.ToDecimal(pricePerKm_tb.Text);
                priceRule.Boarding = Convert.ToInt32(boarding_tb.Text);

                dbManager.Add(priceRule);
                dbManager.LoadEntities<PriceRule>();

                MessageBox.Show("Правило успешно добавлено");
                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }

        private void changePriceRuleWindow_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                priceRule.StartRange = Convert.ToInt32(startRange_tb.Text);
                priceRule.EndRange = Convert.ToInt32(endRange_tb.Text);
                priceRule.OrderClass = (OrderClass)classOrder_cb.SelectedItem;
                priceRule.PricePerKm = Convert.ToDecimal(pricePerKm_tb.Text);
                priceRule.Boarding = Convert.ToInt32(boarding_tb.Text);

                dbManager.Update(priceRule);
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
