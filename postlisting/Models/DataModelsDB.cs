using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postlisting.Models
{
    public class DataModelsDB : DbContext
    {
        static DataModelsDB()
        {
            //do not try to create a database 
            Database.SetInitializer<DataModelsDB>(null);
        }

        public DataModelsDB()
            : base("name=OPWContext")
        {
        }

        public DbSet<SellerOrderHistory> EbayOrders { get; set; }

        public void StoreOrder(SellerOrderHistory order)
        {
            this.EbayOrders.Add(order);
            this.SaveChanges();
        }
        public void RemoveOrder(string ebayItemId)
        {
            Database.ExecuteSqlCommand("delete from SellerOrderHistory where itemId = '" + ebayItemId + "'");
        }
    }
}
