using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomersManagementDP;
using CustomersManagementBL;

namespace CustomerManagementPL.Models
{
    public class ItemsModel
    {
        private IBL bl;
        public ItemsModel()
        {
            bl = new BL_imp();
        }

        public void Init()
        {
            bl.Init();
        }

        public List<Item> GetItems(Enums.TYPE category)
        {
            return bl.getAllItems(x => x.Categorie == category);
        }

        public List<Item> GetRecommendationsForToday()
        {
            return bl.getRecommendationsForToday();
        }
        public List<Item> GetYearItems(int year)
        {
            return bl.getAllItems(x => x.Date_of_purchase.Year == year);
        }

     
        public List<Item> GetMonthItems(int month, int year)
        {
            return bl.getAllItems(x => x.Date_of_purchase.Year == year &&
                                       x.Date_of_purchase.Month == month);
        }

        public List<Item> GetWeekItems(int week, int month, int year)
        {
            return bl.getAllItems(x => x.Date_of_purchase.Year == year &&
                                       x.Date_of_purchase.Month == month && 
                                       ((x.Date_of_purchase.Day / 7) == (week-1)));
        }

        public IEnumerable<IGrouping<DateTime,Item>> GetDateGroups()
        {
            return bl.GetDateGroups();
        }

        public List<Tuple<string, string>> getAllProductsTupleNameKey()
        {
            return bl.getAllProductsTupleNameKey();
        }

        public void CreatePdfForStoreRecomendations()
        {
            bl.CreatePdfForStoreRecomendations();
        }

        public void  CreatePdfForDayRecomendations()
        {
            bl.CreatePdfForDayRecomendations();
        }

        public IEnumerable<IGrouping<string, IGrouping<DateTime, Item>>> groupByDate()
        {
            return bl.groupByDate();
        }


        public void UpdateItem(Item item)
        {
            bl.UpdateItem(item);
        }

        public void RemoveItem(Item item)
        {
            bl.RemoveItem(item.ItemId);
        }

    }
}
