using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersManagementBL
{
    public interface IBL
    {
        void AddItem(Item item);

        void RemoveItem(int itemId);

        void UpdateItem(Item item);

        List<Item> getAllItems(Func<Item, bool> pred = null);
        void AddCategorie(Categorie categorie);

        void UpdateCategorie(Categorie categorie);

        List<Categorie> getAllCategories(Func<Categorie, bool> pred = null);



    }
}
