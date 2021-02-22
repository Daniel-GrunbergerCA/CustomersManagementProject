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
        public CreatePDFCommand CreatePDF { get; set; }
        ItemsModel itemsModel = new ItemsModel();

        public RecomendationViewModel()
        {
            CreatePDF = new CreatePDFCommand();
            CreatePDF.GeneratePdfEvent += CreatePDF_function;
        }

        public void CreatePDF_function()
        {
            itemsModel.CreatePdfForStoreRecomendations();
        }
    }
}
