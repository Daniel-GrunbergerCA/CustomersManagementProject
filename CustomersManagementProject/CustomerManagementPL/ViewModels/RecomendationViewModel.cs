using CustomerManagementPL.Commands;
using CustomerManagementPL.Models;
using CustomersManagementDP;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CustomerManagementPL.ViewModels
{
    public class RecomendationViewModel
    {
        public CreatePDFCommand CreatePDFStores { get; set; }
        public CreatePDFCommand CreatePDFDays { get; set; }
        public string PopUpbarMessage { get; set; }
        public bool PopUpEnabled { get; set; }

        public SnackbarMessageQueue PDFMessageQueue { get; set; }

        public string today { get; set; }
        ItemsModel itemsModel = new ItemsModel();
        public ObservableCollection<RecommendedItemViewModel> ItemsVM { get; set; }

        public RecomendationViewModel()
        {
            today =  "Here are our recommendations for today (" + DateTime.Now.DayOfWeek+")";
            CreatePDFStores = new CreatePDFCommand();
            CreatePDFDays = new CreatePDFCommand();
            CreatePDFStores.GeneratePdfEvent += CreatePDFStores_function;
            CreatePDFDays.GeneratePdfEvent += CreatePDFDays_function;
            generateCollectionFromModel();
            PopUpEnabled = false;
            PDFMessageQueue = new SnackbarMessageQueue();
        }

        public void CreatePDFStores_function()
        {
            itemsModel.CreatePdfForStoreRecomendations();
            PDFMessageQueue.Enqueue("The recommendations saved as 'Recommended Stores.pdf'");
            PDFMessageQueue.Enqueue("path: " + AppDomain.CurrentDomain.BaseDirectory + "Recommended Stores.pdf");
        }

        public void CreatePDFDays_function()
        {
            itemsModel.CreatePdfForDayRecomendations();
            PDFMessageQueue.Enqueue("The recommendations saved as 'Recommended Days.pdf'");
            PDFMessageQueue.Enqueue("path: " + AppDomain.CurrentDomain.BaseDirectory + "Recommended Days.pdf");
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
