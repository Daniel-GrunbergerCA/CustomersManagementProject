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
        public void AddCategorie(Categorie categorie)
        {

            using (var ctx = new CustomerContext())
            {
                ctx.Categories.Add(categorie);
                ctx.SaveChanges();
            }
        }

        public void AddItem(Item item)
        {

            using (var ctx = new CustomerContext())
            {
                Categorie cat = ctx.Categories.Where(x => x.CategorieId == item.Categorie.CategorieId).FirstOrDefault();
                ctx.Items.Add(item);
                ctx.SaveChanges();
            }
        }

        public List<Categorie> getAllCategories(Func<Categorie, bool> pred = null)
        {
            using (var ctx = new CustomerContext())
            {
              
                if (pred == null)
                    return ctx.Categories.ToList();
                else
                    return ctx.Categories.Where(pred).ToList();
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

        public void RemoveItem(int itemId)
        {
            
           using (var ctx = new CustomerContext())
            {
                Item item = ctx.Items.Find(itemId);
                ctx.Items.Remove(item);
                ctx.SaveChanges();
            }
        }

        public void RemoveCategorie(int id)
        {
            using (var ctx = new CustomerContext())
            {
                Categorie cat = ctx.Categories.Find(id);
                ctx.Categories.Remove(cat);
                ctx.SaveChanges();
            }

        }

        public void UpdateCategorie(Categorie categorie)
        {
            using (var ctx = new CustomerContext())
            {
                var categorieToUpdate = ctx.Categories.Find(categorie.CategorieId);
                categorieToUpdate.CategorieName = categorie.CategorieName;
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
                itemToUpdate.Date_of_purchase = item.Date_of_purchase;
                ctx.SaveChanges();
            }
        
        }
    }
}
