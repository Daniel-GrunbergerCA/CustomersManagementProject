using CustomersManagementDP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
          //   Database.SetInitializer<CustomerContext>(null); Use when changing the model and there is an Exception (connection opened to the old db)
        }

        public DbSet<Item> Items { get; set; }

        

    }
}
