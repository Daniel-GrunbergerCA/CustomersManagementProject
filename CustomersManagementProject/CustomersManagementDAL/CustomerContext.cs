using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersManagementDAL
{
    public class CustomerContext : DbContext
    {

        public CustomerContext() : base("CustomersDB")
        {
            Database.SetInitializer<CustomerContext>(new DropCreateDatabaseIfModelChanges<CustomerContext>());
        }

        public DbSet<Item> Items { get; set; }

        public DbSet<Categorie> Categories { get; set; }

    }
}
