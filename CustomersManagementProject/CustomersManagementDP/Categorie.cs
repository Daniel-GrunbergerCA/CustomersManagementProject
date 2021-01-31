using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersManagementDP
{
    public class Categorie
    {
        private string categorieName;
        public string CategorieName { 
            get
            {
                return categorieName;
            }

            set 
            {
                this.categorieName = value;
            }
        }


        private int categorieId;
        public int CategorieId
        {
            get
            {
                return categorieId;
            }
            set
            {
                this.categorieId = value;
            }
        }
       public  Categorie(string name, int id)
        {
            this.categorieId = id;
            this.categorieName = name;
        }

        public Categorie() { }
    }
}
