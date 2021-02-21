using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagementPL.Commands;
using CustomerManagementPL.Models;
using CustomersManagementDP;
using Microsoft.Win32;

namespace CustomerManagementPL.ViewModels
{
    public class CatalogCategoryViewModel : IViewModel// : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public string Title
        {
            get;
            set;
        }

        public ObservableCollection<ItemViewModel> ItemsVM { get; set; }

        private ItemsModel itemsModel;
        private Enums.TYPE category;

        public UploadImageCommand UploadImage { get; set; }
        public DeleteItemCommand DeleteItem { get; set; }



        public CatalogCategoryViewModel(Enums.TYPE categoryType)
        {
            this.itemsModel = new ItemsModel();
            this.category = categoryType;
            Title = ((Enums.TYPE)categoryType).ToString();
            ItemsVM = new ObservableCollection<ItemViewModel>();
            ItemsVM.CollectionChanged += ItemsVM_CollectionChanged;
            generateAndFilterCollectionFromModel();

            UploadImage = new UploadImageCommand();
            UploadImage.UploadEvent += Image_UploadEvent;

            DeleteItem = new DeleteItemCommand();
            DeleteItem.DeleteItemViewModelEvent += ItemVM_DeleteEvent;
        }

        public void OnCatalogChange(Enums.TYPE categoryType)
        {
            this.category = categoryType;
            this.Title = ((Enums.TYPE)categoryType).ToString();
            generateAndFilterCollectionFromModel();
        }


        private void generateAndFilterCollectionFromModel()
        {
            ItemsVM.Clear();
            foreach (var item in itemsModel.GetItems(category))
            {
                ItemsVM.Add(new ItemViewModel(new Item(item), itemsModel));
            }
        }


        private void ItemsVM_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //var newData = e.NewItems[0] as ItemViewModel;
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                ItemViewModel removedItem = e.OldItems[0] as ItemViewModel;
                itemsModel.RemoveItem(removedItem.Product);
            }
            //if (e.Action == NotifyCollectionChangedAction.Add)
            //    Model.Students.Add(newData);

        }



        public void Image_UploadEvent(Item product)
        {
            string ProductImagePath = product.SerialKey + ".jpg";
            // open file dialog   
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // image filters  
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";   
            if (openFileDialog.ShowDialog() == true)
                System.IO.File.Copy(openFileDialog.FileName, ProductImagePath, true);
            generateAndFilterCollectionFromModel();
        }





        public void ItemVM_DeleteEvent(ItemViewModel itemVM)
        {
            ItemsVM.Remove(itemVM);
        }
    }
}
