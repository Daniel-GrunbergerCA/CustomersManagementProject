using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CustomersManagementDP.Enums;

namespace CustomersManagementDP
{
    public class Item
    {


            private string itemId;
            private string itemName;
            private DateTime date_of_purchase;
            private string store_location;
            private string store_name;
            private int quantity;
            private Categorie categorie;


            #region setters & getters
            

            public Categorie Categorie
            {
            get { return categorie; }
            set { categorie = value; }
            }
            public string ItemId
            {
                get { return itemId; }
                set
                {
                    if (value.Length != 6)
                        throw new Exception("Id must contain 6 characters!");

                    itemId = value;
                }
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
            
            public string ItemName
            {
                get { return itemName; }
                set { itemName = value; }
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
}

