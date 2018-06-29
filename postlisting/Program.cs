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
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        static void Process()
        {
            foreach(SellerOrderHistory order in db.EbayOrders)
            {
                List<string> pictureURLs = Util.DelimitedToList(order.PictureUrl, ';');
                ebayItem.VerifyAddItemRequest(order.Title,
                    order.Description,                  // want the sam's description
                    order.PrimaryCategoryID,
                    (double)order.EbaySellerPrice,
                    pictureURLs);

            }
        }
    }
}
