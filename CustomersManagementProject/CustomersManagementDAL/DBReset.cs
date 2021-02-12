using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersManagementDAL
{
    public class CustomInitializer<T> : DropCreateDatabaseAlways<CustomerContext>
    {
        public override void InitializeDatabase(CustomerContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
                , string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            base.InitializeDatabase(context);
        }

        protected override void Seed(CustomerContext context)
        {
            // Seed code goes here...

            base.Seed(context);
        }
    }
}
