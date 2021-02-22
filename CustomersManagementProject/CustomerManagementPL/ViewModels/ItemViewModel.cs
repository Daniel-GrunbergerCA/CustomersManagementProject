using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CustomerManagementPL.Commands;
using CustomerManagementPL.Models;
using CustomerManagementPL.UserControls;
using CustomersManagementDP;

namespace CustomerManagementPL.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged, IViewModel
    {

        private ItemsModel itemsModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ProductImagePath { get; set; }
        private ImageSource productImage;

        public ImageSource ProductImage
        {
            get
            {
                return productImage;
            }
            set
            {
                productImage = value;
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductImage"));
            }
        }

        public string Description
        {
            get
            {
                return Product.Description;
            }
            set
            {
                Product.Description = value;
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public int Rating
        {
            get
            {
                return Product.Rating;
            }
            set
            {
                Product.Rating = value;
                if (null != PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs("Rating"));
            }
        }
        public Item Product { get; set; }

        public ItemViewModel(Item item, ItemsModel itemsModel)
        {
            Product = item;
            this.itemsModel = itemsModel;
            UpdateProduct = new UpdateItemCommand();
            UpdateProduct.UpdateEvent += Item_UpdateEvent;
            ProductImagePath = Product.SerialKey + ".jpg";
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




        public UpdateItemCommand UpdateProduct { get; set; }

        public void Item_UpdateEvent()
        {
            itemsModel.UpdateItem(Product);
        }







    }
}
