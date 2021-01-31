using CustomersManagementBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomersManagementDP;
using System.Data.SqlClient;

namespace CustomersManagementProject
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL bl = new BL_imp();
            Categorie cat = bl.getAllCategories(x => x.CategorieId == 1).FirstOrDefault();
            bl.AddItem(new Item(657657, "Banana", DateTime.Now, "Jerusalem", "OsherAd", 15, cat));

           List<Item> items =  bl.getAllItems();
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadLine();

        }
    }
}
