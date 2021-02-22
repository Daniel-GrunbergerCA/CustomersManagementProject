using CustomerManagementPL.ViewModels;
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
using CustomersManagementDP;

namespace CustomerManagementPL.UserControls
{
    /// <summary>
    /// Interaction logic for CatalogStatisticsUserControl.xaml
    /// </summary>
    public partial class CatalogStatisticsUserControl : UserControl
    {
        public CatalogStatViewModel catalogStatVM { get; set; }

        public CatalogStatisticsUserControl(Enums.STAT categoryStat)
        {
            InitializeComponent();
            lineChartUC = new LineChartUserControl();
            barChartUC = new BarChartUserControl();
            setStatisticCatalog(categoryStat);
            this.DataContext = catalogStatVM;
            // init with line chart
            lineChartUC.SetValue(Grid.RowProperty, 2);
            lineChartUC.SetValue(Grid.ColumnProperty, 0);
            MainLayout.Children.Add(lineChartUC);
        }

        internal void setStatisticCatalog(Enums.STAT categoryStat)
        {
            if (catalogStatVM == null)
            {
                catalogStatVM = new CatalogStatViewModel(categoryStat);
            }
            else
            {
                catalogStatVM.OnCatalogChange(categoryStat);
            }
        }

        LineChartUserControl lineChartUC;
        BarChartUserControl barChartUC;

        public void LineChart_Click(object sender, RoutedEventArgs e)
        {
            MainLayout.Children.Remove(lineChartUC);
            MainLayout.Children.Remove(barChartUC);
            /* Past new*/
            lineChartUC.SetValue(Grid.RowProperty, 2);
            lineChartUC.SetValue(Grid.ColumnProperty, 0);
            MainLayout.Children.Add(lineChartUC);
        }

        public void BarChart_Click(object sender, RoutedEventArgs e)
        {
            MainLayout.Children.Remove(lineChartUC);
            MainLayout.Children.Remove(barChartUC);
            /* Past new*/
            barChartUC.SetValue(Grid.RowProperty, 2);
            barChartUC.SetValue(Grid.ColumnProperty, 0);
            MainLayout.Children.Add(barChartUC);
        }
    }
}
