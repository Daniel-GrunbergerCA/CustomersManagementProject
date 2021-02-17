using CustomersManagementDAL;
using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersManagementBL
{
    public class BL_imp : IBL
    {
        public IDAL idal { get; set; }

        public BL_imp()
        {
            idal = new DAL_imp();
        }
  

        public void AddItem(Item item)
        {
            idal.AddItem(item);
        }

       
        public List<Item> getAllItems(Func<Item, bool> pred = null)
        {
           return idal.getAllItems(pred);
        }

        public void RemoveItem(int itemId)
        {
            idal.RemoveItem(itemId);
        }

    

        public void UpdateItem(Item item)
        {
            idal.UpdateItem(item);
        }
    }
}
