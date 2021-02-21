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
    /// Interaction logic for CatalogUserControl.xaml
    /// </summary>
    public partial class CatalogCategoryUserControl : UserControl
    {

        public CatalogCategoryViewModel catalogCategoryVM { get; set; }
        public CatalogCategoryUserControl(Enums.TYPE categoryType)
        {
            InitializeComponent();
            setCategoryCatalog(categoryType);
            DataContext = catalogCategoryVM;
        }

        internal void setCategoryCatalog(Enums.TYPE categoryType)
        {
            if (catalogCategoryVM == null)
            {
                catalogCategoryVM = new CatalogCategoryViewModel(categoryType);
            }
            else
            {
                catalogCategoryVM.OnCatalogChange(categoryType);
            }

        }


    }
}
