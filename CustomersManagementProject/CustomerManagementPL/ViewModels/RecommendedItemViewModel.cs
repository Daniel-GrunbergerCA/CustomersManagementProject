using CustomerManagementPL.Models;
using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomerManagementPL.ViewModels
{
    public class RecommendedItemViewModel
    {
        public Item Product  { get; set; }
          ItemsModel model = new ItemsModel();
        public string ProductImagePath { get; set; }
        public  ImageSource ProductImage { get; set; }
        public RecommendedItemViewModel(Item item, ItemsModel model)
        {
            this.Product = item;
            this.model = model;
            ProductImagePath = item.SerialKey + ".jpg";
            if (!File.Exists(ProductImagePath))
            {
                ProductImagePath = "nothing.jpg";
            }
            LoadImageFromURI();
            
        }
        public void LoadImageFromURI()
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(ProductImagePath, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            ProductImage = src;
        }
    }
}
