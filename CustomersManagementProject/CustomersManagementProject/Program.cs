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
     //       Categorie newCat = new Categorie("Food", 1);

            Categorie cat = bl.getAllCategories(x => x.CategorieId == 1).FirstOrDefault();
        //   Item it = new Item(1, "Banana", DateTime.Now, "Jerusalem", "OsherAd", 15, cat);
          //  bl.UpdateItem(it);
          
            bl.AddItem(new Item(200005, "Apple", DateTime.Now, "Jerusalem", "OsherAd", 10, cat));

          
            List<Item> items =  bl.getAllItems();
              foreach (var item in items)
              {
                  Console.WriteLine(item.ToString());
              }

            List<Categorie> cats = bl.getAllCategories();
            foreach (var c in cats)
            {
                Console.WriteLine(c.ToString());
            }
            Console.WriteLine("Hi");
            Console.ReadLine();

        }
    }
}
