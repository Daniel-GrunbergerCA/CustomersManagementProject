using CustomerManagementPL.UserControls;
using CustomerManagementPL.ViewModels;
using CustomersManagementDP;
using System;
using System.Windows;
using System.Windows.Controls;




namespace CustomerManagementPL
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const short PRODUCT_CATEGORIES = 0;
        private const short PRODUCT_STATISTICS = 1;
        private const short PRODUCT_RECOMMENDED = 2;

        public ViewModel mViewModel { get; set; }
        private CatalogCategoryUserControl catalogBuy;
        private CatalogStatisticsUserControl catalogStat;
        private CategoryListUserControl categoriesUC;
        private RecomendationsUserControl recommendationUC;
        public MainWindow()
        {
            InitializeComponent();
            mViewModel = new ViewModel(this);
            this.DataContext = mViewModel;
            mViewModel.Init();
        }

        // [___]  [   ]  [   ]
        public void onCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            /* Clean all user controls and past those two which are revant */
            //remove if exist
            if (categoriesUC != null)
                MainLayout.Children.Remove(categoriesUC);
            //remove if exist
            if (catalogBuy != null)
                MainLayout.Children.Remove(catalogBuy);
            //remove if exist
            if (catalogStat != null)
                MainLayout.Children.Remove(catalogStat);
            /* Past new*/
            categoriesUC = new CategoryListUserControl(PRODUCT_CATEGORIES);
            categoriesUC.SetValue(Grid.RowProperty, 1);
            categoriesUC.SetValue(Grid.ColumnProperty, 0);
            MainLayout.Children.Add(categoriesUC);
        }
        // [   ]  [___]  [   ]
        public void onStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            /* Clean all user controls and past those two which are revant */
            //remove if exist
            if (categoriesUC != null)
                MainLayout.Children.Remove(categoriesUC);
            //remove if exist
            if (catalogBuy != null)
                MainLayout.Children.Remove(catalogBuy);
            //remove if exist
            if (catalogStat != null)
                MainLayout.Children.Remove(catalogStat);
            /* Past new*/
            categoriesUC = new CategoryListUserControl(PRODUCT_STATISTICS);
            categoriesUC.SetValue(Grid.RowProperty, 1);
            categoriesUC.SetValue(Grid.ColumnProperty, 0);
            MainLayout.Children.Add(categoriesUC);
        }

        // [    ]
        // [____]
        // [    ]
        // [    ]
        public void onCatalog_Click(object sender, RoutedPropertyChangedEventArgs<string> e)
        {


            // the TaskManager will lead this category to the correct usercontrol
            e.Handled = true;
            if (e.NewValue != null && categoriesUC.categoryVM.Title == "My Categories")
            {
                Enum.TryParse((string)e.NewValue, out Enums.TYPE category_catalog);

                //remove if other exist
                if (catalogBuy != null)
                {
                    MainLayout.Children.Remove(catalogBuy);
                }
                //remove if other exist
                if (catalogStat != null)
                {
                    MainLayout.Children.Remove(catalogStat);
                }

                /* Past new*/
                catalogBuy = new CatalogCategoryUserControl(category_catalog);
                //catalogBuy.setCategoryCatalog(category_catalog);
                catalogBuy.SetValue(Grid.RowProperty, 1);
                catalogBuy.SetValue(Grid.ColumnProperty, 1);
                MainLayout.Children.Add(catalogBuy);
            }
            else if(e.NewValue != null && categoriesUC.categoryVM.Title == "My Statistics")
            {
                Enum.TryParse((string)e.NewValue, out Enums.STAT statistic_catalog);

                //remove if other exist
                if (catalogBuy != null)
                {
                    MainLayout.Children.Remove(catalogBuy);
                }
                //remove if other exist
                if (catalogStat != null)
                {
                    MainLayout.Children.Remove(catalogStat);
                }

                /* Past new*/
                catalogStat = new CatalogStatisticsUserControl(statistic_catalog);
                //catalogStat.setStatisticCatalog(statistic_catalog);
                catalogStat.SetValue(Grid.RowProperty, 1);
                catalogStat.SetValue(Grid.ColumnProperty, 1);
                MainLayout.Children.Add(catalogStat);
            }
        }

        private void Recommended_Click(object sender, RoutedEventArgs e)
        {
            /* Clean all user controls and past those two which are revant */
            //remove if exist
            if (categoriesUC != null)
                MainLayout.Children.Remove(categoriesUC);
            //remove if exist
            if (catalogBuy != null)
                MainLayout.Children.Remove(catalogBuy);
            //remove if exist
            if (catalogStat != null)
                MainLayout.Children.Remove(catalogStat);
            /* Past new*/

            recommendationUC = new RecomendationsUserControl();
            recommendationUC.SetValue(Grid.RowProperty, 1);
            recommendationUC.SetValue(Grid.ColumnProperty, 0);
            recommendationUC.SetValue(Grid.ColumnSpanProperty, 2);
            MainLayout.Children.Add(recommendationUC);
        }
    }
}
