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
using System.Drawing;
using Font = iTextSharp.text.Font;

namespace CustomersManagementBL
{
    public class BL_imp : IBL
    {
        public IDAL idal { get; set; }
        private GoogleDriveAPIManager googleDriveAPI_manager;

        public BL_imp()
        {
            idal = new DAL_imp();
        }

        public void Init()
        {
            googleDriveAPI_manager = new GoogleDriveAPIManager(this);
            googleDriveAPI_manager.QuickStart();
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

        public void CreatePdfForDayRecomendations()
        {
            List<Tuple<string, string>> tuples = getAllProductsTupleNameKey();
            List<DayOfWeek> days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Sunday);
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);
            days.Add(DayOfWeek.Saturday);

            PdfPTable table = new PdfPTable(2);

            PdfPCell cell = new PdfPCell(new Phrase("Item Name"));

            cell.Colspan = 1;

            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

            table.AddCell(cell);
            PdfPCell cell2 = new PdfPCell(new Phrase("Best Day"));

            cell2.Colspan = 1;

            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

            table.AddCell(cell2);
            foreach (var tuple in tuples)
            {
                int quantity = 0;
                int maxQuantity = 0;
                DayOfWeek dayOfWeek = new DayOfWeek();
                foreach(DayOfWeek day in days)
                {
                    List<Item> items = idal.getAllItems(y => y.Date_of_purchase.DayOfWeek == day && y.SerialKey == tuple.Item1);
                    foreach(var item in items)
                    {
                        quantity += item.Quantity;
                    }
                    if (quantity > maxQuantity)
                    {
                        maxQuantity = quantity;
                        dayOfWeek = day;
                    }
                }
                table.AddCell(tuple.Item2);
                table.AddCell(dayOfWeek.ToString());
                Document doc = new Document(PageSize.A4, 7f, 5f, 5f, 0f);
                doc.AddTitle("Machine Learning results");
                PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Recommended Days.pdf", FileMode.Create));
                doc.Open();
                //     Paragraph p1 = new Paragraph(text);
                //   doc.Add(p1);

                doc.Add(table);
                Font x = FontFactory.GetFont("nina fett");

                x.Size = 19;

                x.SetStyle("Italic");

                x.SetColor(0, 42, 255);


                Paragraph c2 = new Paragraph(@"Based on our recommendations for which products to buy on which  day", x);
                c2.IndentationLeft = 30;
                doc.Add(c2);


                doc.Close();
            }
        }
        public void CreatePdfForStoreRecomendations()
        {
            List<string> storeNames = getAllStoreNames();
            List<Tuple<string, string>> tuples = getAllProductsTupleNameKey();
            string text = "";
            
             PdfPTable table = new PdfPTable(2);

             PdfPCell cell = new PdfPCell(new Phrase("Item Name"));

            cell.Colspan = 1;

            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

            table.AddCell(cell);
            PdfPCell cell2 = new PdfPCell(new Phrase("Store Name"));

            cell2.Colspan = 1;

            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

            table.AddCell(cell2);
            foreach (var tuple in tuples)
            {
                float bestScore = 0;
                string storeName = "";
                foreach (var store in storeNames)
                {
                    ModelInput sampleData = new ModelInput()
                    {
                        Store_name = store,
                        SerialKey = tuple.Item1,
                    };
                    var predictionResult = ConsumeModel.Predict(sampleData);
                    if (predictionResult.Score>=bestScore || bestScore==0)
                    {
                        bestScore = predictionResult.Score;
                        storeName = store;
                    }
        }
                table.AddCell(tuple.Item2);
                table.AddCell(storeName);
            }
            Document doc = new Document(PageSize.A4, 7f, 5f, 5f, 0f);
            doc.AddTitle("Machine Learning results");
            PdfWriter.GetInstance(doc, new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Recommended Stores.pdf", FileMode.Create));
            doc.Open();
            //     Paragraph p1 = new Paragraph(text);
            //   doc.Add(p1);
           
            doc.Add(table);
            Font x = FontFactory.GetFont("nina fett");

            x.Size = 19;

            x.SetStyle("Italic");

            x.SetColor(0, 42, 255);


            Paragraph c2 = new Paragraph(@"Based on our recommendations for which products to buy in which store", x);
            c2.IndentationLeft = 30;
            doc.Add(c2);


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
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (var grpItm in idal.getGroupBySerialKey())
            {
                list.Add(new Tuple<string, string>(grpItm.Key, grpItm.First().ItemName));
            }
            return list;
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
