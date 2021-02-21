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

            //bl.UpdateItem(new Item(2, "Pasta", DateTime.Now, "Tel-Aviv", "Rami Levi", 8, Enums.TYPE.Food, "Sweet Pasta!!!", "iuor643"));
            //bl.AddItem(new Item("Pasta", DateTime.Now, "Tel-Aviv", "Rami Levi", 8, Enums.TYPE.Food, "Sweet Pasta!!!", "iuor643", 25.00));
            //bl.RemoveItem(2);
             List<Item> items =  bl.getAllItems();
            foreach (var item in items)
            {
                Console.WriteLine(item.ItemId + " " +item.ItemName + " " + item.Description + " " + item.SerialKey + " " + item.Store_location + " " +item.Store_name + " "+item.Price);
            }

           
            Console.ReadLine();

        }
    }
}
