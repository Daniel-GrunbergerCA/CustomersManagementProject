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

        public IEnumerable<IGrouping<DateTime,Item>> GetDateGroups()
        {
            return idal.getGroupByDate();
        }

        public IEnumerable<IGrouping<string, IGrouping<DateTime, Item>>> groupByDate()
        {
            return idal.groupByDate();
        }

        public List<Tuple<string, string>> getAllProductsTupleNameKey()
        {
            List<Tuple<string,string>> productsNameKey = new List<Tuple<string,string>>();
            foreach (var group in idal.getGroupBySerialKey())
            {
                productsNameKey.Add(new Tuple<string,string>(group.Key,group.First().ItemName));
            }
            return productsNameKey;
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
