using CustomerManagementPL.Commands;
using CustomerManagementPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementPL.ViewModels
{
    public class RecomendationViewModel
    {
        public CreatePDFCommand CreatePDFStores { get; set; }
        public CreatePDFCommand CreatePDFDays { get; set; }
        ItemsModel itemsModel = new ItemsModel();

        public RecomendationViewModel()
        {
            CreatePDFStores = new CreatePDFCommand();
            CreatePDFDays = new CreatePDFCommand();
            CreatePDFStores.GeneratePdfEvent += CreatePDFStores_function;
            CreatePDFDays.GeneratePdfEvent += CreatePDFDays_function;
        }

        public void CreatePDFStores_function()
        {
            itemsModel.CreatePdfForStoreRecomendations();
        }

        public void CreatePDFDays_function()
        {
            itemsModel.CreatePdfForDayRecomendations();
        }
    }
}
