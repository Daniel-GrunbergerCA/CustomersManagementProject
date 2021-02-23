using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CustomersManagementDP.Enums;

namespace CustomersManagementDP
{
    /// <summary>
    /// This class describes an Item that was purchased.
    /// </summary>
    public class Item
    {
        public Item() { }



        public Item(string name, DateTime date, string location, string store_name, int quant, Enums.TYPE cat, string description, string serialKey, double price, int rating)
        {
            this.itemName = name;
            this.date_of_purchase = date;
            this.store_location = location;
            this.store_name = store_name;
            this.quantity = quant;
            this.categorie = cat;
            this.description = description;
            this.serialKey = serialKey;
            this.price = price;
            this.rating = rating;
        }

        public Item(Item item)
        {
            this.itemId = item.itemId;
            this.itemName = item.itemName;
            this.date_of_purchase = item.date_of_purchase;
            this.store_location = item.store_location;
            this.store_name = item.store_name;
            this.quantity = item.quantity;
            this.categorie = item.categorie;
            this.description = item.description;
            this.serialKey = item.serialKey;
            this.price = item.price;
            this.rating = item.rating;
        }

        private int itemId;
        private string itemName;
        private DateTime date_of_purchase = new DateTime();
        private string store_location;
        private string store_name;
        private int quantity;
        private Enums.TYPE categorie;
        private string serialKey;
        private string description;
        private double price;
        private int rating = 4; // Default is 4

        #region setters & getters

        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public Enums.TYPE Categorie
            {
            get { return categorie; }
            set {
                
                categorie = value; }
            }


        public int ItemId
            {
                get { return itemId; }
                set
                {
                   

                    itemId = value;
                }
            }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

            public int Quantity
            {
                get { return quantity; }
                set
                {
                    if (quantity < 0)
                        throw new Exception("Quantity cannot be negative!");

                    quantity = value;
                }
            }

            public string Store_name
            {
                get { return store_name; }
                set { store_name = value; }
            }
            public string Store_location
            {
                get { return store_location; }
                set { store_location = value; }
            }
            
            public string SerialKey
            {
                get { return serialKey; }
                set { serialKey = value; }
            }
            public string ItemName
            {
                get { return itemName; }
                set { itemName = value; }
            }
            public double Price
            {
                get { return price; }
                set { price = value; }
            }
            public DateTime Date_of_purchase
            {
                get
                {
                    return date_of_purchase;
                }
                set
                {
                    if (value > DateTime.Now)
                        throw new Exception("The date cannot be in the future!");

                    date_of_purchase = value;
                }
            }
            #endregion

        
    }
}

