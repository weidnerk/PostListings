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

        public DbSet<ReadyToList> ItemsToList { get; set; }

        public void StoreOrder(ReadyToList order)
        {
            this.ItemsToList.Add(order);
            this.SaveChanges();
        }

        public bool UpdateListedItemID(int sourceID, string supplierItemID, string listedItemID)
        {
            bool ret = false;
            var rec = this.ItemsToList.FirstOrDefault(r => r.SourceID == sourceID && r.SupplierItemId == supplierItemID);
            if (rec != null)
            {
                ret = true;
                rec.ListedItemID = listedItemID;

                using (var context = new DataModelsDB())
                {
                    // Pass the entity to Entity Framework and mark it as modified
                    context.Entry(rec).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return ret;
        }
    }
}
