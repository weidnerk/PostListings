/*
 * The process is mostly automated up to populating SellerOrderHistory.
 * Then use stored proc to populate ReadyToList.
 * You will (manullay) apply final edits to this table (such as price) to stage actual listings.
 * When done with edits, run this program to perform actual eBay posting and then copy the
 * records to PostedListings as an ongoing record of what has been posted.
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using postlisting.Models;

namespace postlisting
{
    class Program
    {
        static DataModelsDB db = new DataModelsDB();
        static void Main(string[] args)
        {
            Process();
            //Console.ReadKey();
        }

        // What to do first in table, ReadyToList
        //
        // Look for lower priced items.
        // Look for items priced higher than source.
        // Are the source and ebay listing the same thing?
        // Set to Listed to True those records to list.
        // Update Price.
        // Update Title.
        // Check availability.
        // Check limits.
        //
        static void Process()
        {
            foreach(ReadyToList item in db.ItemsToList.Where(r => r.ToList && !r.Transferred).ToList())
            {
                List<string> pictureURLs = Util.DelimitedToList(item.Pictures, ';');
                string verifyItemID = ebayItem.VerifyAddItemRequest(
                    item.Title,
                    item.Description,                  // want the sam's description
                    item.PrimaryCategoryID,
                    (double)item.Price,
                    pictureURLs);
                if (string.IsNullOrEmpty(verifyItemID))
                {
                    Console.WriteLine("Listing {0} could not be verified.", item.Title);
                }
                else
                {
                    db.UpdateListedItemID(item.SourceID, item.SupplierItemId, verifyItemID);
                    Console.WriteLine("New listed item id is {0}", verifyItemID);
                }
            }
        }
    }
}
