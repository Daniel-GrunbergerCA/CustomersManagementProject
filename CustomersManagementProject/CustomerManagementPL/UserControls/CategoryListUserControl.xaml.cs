using CustomerManagementPL.ViewModels;
using CustomersManagementDP;
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

namespace CustomerManagementPL.UserControls
{
    /// <summary>
    /// Interaction logic for CategoryListUserControl.xaml
    /// </summary>
    public partial class CategoryListUserControl : UserControl
    {
        public CategoryViewModel categoryVM { get; set; }

        string lastCategory = "";
        public CategoryListUserControl(int v)
        {
            InitializeComponent();
            setSideTitle(v);
            DataContext = categoryVM;
        }


        internal void setSideTitle(int v)
        {
            if (v == 0)
            {
                List<string> productCategories = new List<string>();
                foreach (var value in Enum.GetValues(typeof(Enums.TYPE)))
                {
                    productCategories.Add(value.ToString());
                };
                categoryVM = new CategoryViewModel("Categories", productCategories);
            }

            /*
             Sections: long-term product, long-term store, long-term category, total purchase cost over time.
                       -> Aggregation can be done at the day, week and month level.
             - View a profile of products that are usually bought together.
             */
            else if (v == 1)
            {
                List<string> productStatistics = new List<string>();
                foreach (var value in Enum.GetValues(typeof(Enums.STAT)))
                {
                    productStatistics.Add(value.ToString());
                };
                categoryVM = new CategoryViewModel("Statistics", productStatistics);
            }
        }



        public void CategoryOnClick(object sender, RoutedEventArgs e)
        {
            string newCategory = (sender as Button).DataContext as string;
            /* prevent from useless call back to the same catalog's category */
            if (lastCategory == newCategory) { return; }
            /* Call the TaskManager by this category */
            /* Using Bubbling Routed Events */
            RoutedPropertyChangedEventArgs<string> newEventArgs = new RoutedPropertyChangedEventArgs<string>(lastCategory, newCategory, CategoryListUserControl.CatalogSwitchEvent);
            this.RaiseEvent(newEventArgs);
            lastCategory = newCategory;
        }

        #region Routed event for category change
        public static readonly RoutedEvent CatalogSwitchEvent = EventManager.RegisterRoutedEvent("CatalogSwitch",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(CategoryListUserControl));

        // Provide CLR accessors for the event
        public event RoutedPropertyChangedEventHandler<string> CatalogSwitch
        {
            add { AddHandler(CatalogSwitchEvent, value); }
            remove { RemoveHandler(CatalogSwitchEvent, value); }
        }
        #endregion

    }
}
