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
            //Categorie cat = bl.getAllCategories(x => x.CategorieId == 1).FirstOrDefault();
            //  Categorie cat = new Categorie("Food", 1);
            //  bl.AddItem(new Item(657659, "Banana", DateTime.Now, "Jerusalem", "OsherAd", 15, cat));
            bl.AddItem(new Item(666, "Apple", DateTime.Now, "Jerusalem", "Rami Levi", 15, Enums.TYPE.Food, "JuicyKey","atge948"));

             List<Item> items =  bl.getAllItems();
            foreach (var item in items)
            {
                Console.WriteLine(item.ItemName + " " + item.Description + " " + item.SerialKey + " " + item.Store_location + " " +item.Store_name );
              }

           
            Console.ReadLine();

        }
    }
}
