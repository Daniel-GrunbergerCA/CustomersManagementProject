using CustomerManagementPL.Commands;
using CustomerManagementPL.Models;
using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementPL.ViewModels
{
    public class RecomendationViewModel
    {
        public CreatePDFCommand CreatePDFStores { get; set; }
        public CreatePDFCommand CreatePDFDays { get; set; }

        public string today { get; set; }
        ItemsModel itemsModel = new ItemsModel();
        public ObservableCollection<RecommendedItemViewModel> ItemsVM { get; set; }

        public RecomendationViewModel()
        {
            today = "Today is : " + DateTime.Now.DayOfWeek + ". Here are our recommendations for today:  ";
            CreatePDFStores = new CreatePDFCommand();
            CreatePDFDays = new CreatePDFCommand();
            CreatePDFStores.GeneratePdfEvent += CreatePDFStores_function;
            CreatePDFDays.GeneratePdfEvent += CreatePDFDays_function;
            generateCollectionFromModel();
        }

        public void CreatePDFStores_function()
        {
            itemsModel.CreatePdfForStoreRecomendations();
        }

        public void CreatePDFDays_function()
        {
            itemsModel.CreatePdfForDayRecomendations();
        }


        private void generateCollectionFromModel()
        {
            ItemsVM = new ObservableCollection<RecommendedItemViewModel>();
            foreach (var item in itemsModel.GetRecommendationsForToday()) 
            {
                ItemsVM.Add(new RecommendedItemViewModel(new Item(item), itemsModel));
            }
        }
    }
}
