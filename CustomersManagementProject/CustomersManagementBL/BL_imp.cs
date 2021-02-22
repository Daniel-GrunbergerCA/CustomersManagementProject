using CustomersManagementDAL;
using CustomersManagementDP;
using CustomersManagementProjectML.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;


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

        public List<string> getAllStoreNames()
        {
            List<Item> items = idal.getAllItems();
            return (from item in items
                              select item.Store_name).Distinct().ToList();
        }

        public void CreatePdfForStoreRecomendations()
        {
            List<string> storeNames = getAllStoreNames();
            List<Tuple<string, string>> tuples = getAllProductsTupleNameKey();
            string text = "";

            foreach (var store in storeNames)
            {
                foreach (var tuple in tuples)
                {
                    ModelInput sampleData = new ModelInput()
                    {
                        Store_name = store,
                        SerialKey = tuple.Item1,
                    };
                    var predictionResult = ConsumeModel.Predict(sampleData);
                    text +=($"Store_name: {sampleData.Store_name}");
                    text += ($"SerialKey: {sampleData.SerialKey}");
                    text += ($"\n\nPredicted Rating: {predictionResult.Score}\n\n");
                }
               
            }
            Document doc = new Document(PageSize.A4, 7f, 5f, 5f, 0f);
            doc.AddTitle("Machine Learning results");
            PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + "CreatePdf.pdf", FileMode.Create));
            doc.Open();
            Paragraph p1 = new Paragraph(text);
            doc.Add(p1);
            doc.Close();

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
            return idal.getAllProductsTupleNameKey();
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
