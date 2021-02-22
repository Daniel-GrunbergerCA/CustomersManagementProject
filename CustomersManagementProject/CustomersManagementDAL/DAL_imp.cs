using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CustomersManagementDAL
{

    public class DAL_imp : IDAL
    {

        private GoogleDriveAPIManager googleDriveAPI_manager;

        public DAL_imp()
        {
            googleDriveAPI_manager = new GoogleDriveAPIManager(this);
            googleDriveAPI_manager.QuickStart();
        }

        public void AddItem(Item item)
        {

            using (var ctx = new CustomerContext())
            {
                ctx.Items.Add(item);
                ctx.SaveChanges();
            }
        }

       

        public List<Item> getAllItems(Func<Item, bool> pred = null)
        {
            using (var ctx = new CustomerContext())
            {
                if (pred == null)
                    return ctx.Items.ToList();
                else
                    return ctx.Items.Where(pred).ToList();
            }
        }

        public IEnumerable<IGrouping<DateTime,Item>> getGroupByDate()
        {
            using (var ctx = new CustomerContext())
            {
                var grpItms = from itm in ctx.Items
                              group itm by itm.Date_of_purchase into grpItm
                              select grpItm;
                return grpItms;
            }
        }

        public IEnumerable<IGrouping<string, Item>> getGroupBySerialKey()
        {
            using (var ctx = new CustomerContext())
            {
                var grpItms = from itm in ctx.Items
                              group itm by itm.SerialKey into grpItm
                              select grpItm;
                return grpItms.ToList();
            }
        }

        public List<Tuple<string, string>> getAllProductsTupleNameKey()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (var grpItm in getGroupBySerialKey())
            {
                list.Add(new Tuple<string, string>(grpItm.Key, grpItm.First().ItemName));
            }
            return list;
        }


        public IEnumerable<IGrouping<string, IGrouping<DateTime, Item>>> groupByDate()
        {
            var queryGroup = getGroupByDate();
            using (var ctx = new CustomerContext())
            {
                var r1 = from itm in ctx.Items
                         group itm by itm.Date_of_purchase into r2
                       select r2;
                return from itm in r1
                       from itm2 in ctx.Items
                       where itm.Key == itm2.Date_of_purchase
                       group itm by itm2.SerialKey into r3
                       select r3;
            }
        }


        public void RemoveItem(int itemId)
        {
            
           using (var ctx = new CustomerContext())
            {
                Item item = ctx.Items.Find(itemId);
                ctx.Items.Remove(item);
                ctx.SaveChanges();
            }
        }

      

        public void UpdateItem(Item item)
        {
            using (var ctx = new CustomerContext())
            {
                var itemToUpdate = ctx.Items.Find(item.ItemId);
                itemToUpdate.ItemName = item.ItemName;
                itemToUpdate.Quantity = item.Quantity;
                itemToUpdate.Store_location = item.Store_location;
                itemToUpdate.Store_name = item.Store_name;
                itemToUpdate.Categorie = item.Categorie;
                itemToUpdate.Description = item.Description;
                itemToUpdate.Date_of_purchase = item.Date_of_purchase;
                itemToUpdate.Rating = item.Rating;
                ctx.SaveChanges();
            }
        
        }







    }
}
