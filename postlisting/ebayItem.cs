/*
 * Needs a reference to ebay.Service (not web reference) (presumes the .NET SDK has been installed)
 * 
 */
using eBay.Service.Core.Soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postlisting
{
    public class ebayItem
    {
        /// <summary>
        /// Verify whether item is ready to be added to eBay.
        /// 
        /// My presets are: 
        ///     NEW condition 
        ///     BuyItNow fixed price
        ///     30 day duration
        ///     14-day returns w/ 20% restocking fee
        ///     payment method=PayPal
        ///     FREE shipping
        ///     buyer pays for return shipping
        /// </summary>
        public static string VerifyAddItemRequest(string title, string description, string categoryID, double price, List<string> pictureURLs)
        {
            string listedItemID = null;
            try
            {
                //pictureURLs = new List<string>(){
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/ZJcAAOSwzE9a0Qpl/$_57.JPG",
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/6eQAAOSwER1a0Qpn/$_57.JPG",
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/YPIAAOSwb~Ja0Qpq/$_57.JPG",
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/zhsAAOSwLjRa0Qps/$_57.JPG",
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/f-8AAOSwF-Ja0Qpv/$_57.JPG",
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/kokAAOSwbLZa0Qpy/$_57.JPG",
                //    "https://i.ebayimg.com/00/s/MTUwMFgxNTAw/z/aZkAAOSwlila0Qp0/$_57.JPG"
                //};
                eBayAPIInterfaceService service = EbayCalls.eBayServiceCall("VerifyAddItem");

                VerifyAddItemRequestType request = new VerifyAddItemRequestType();
                request.Version = "949";
                request.ErrorLanguage = "en_US";
                request.WarningLevel = WarningLevelCodeType.High;

                var item = new ItemType();

                item.Title = title;
                item.Description = description;
                item.PrimaryCategory = new CategoryType
                {
                    CategoryID = categoryID
                };
                item.StartPrice = new AmountType
                {
                    Value = price,
                    currencyID = CurrencyCodeType.USD
                };

                // To view ConditionIDs follow the URL
                // http://developer.ebay.com/devzone/guides/ebayfeatures/Development/Desc-ItemCondition.html#HelpingSellersChoosetheRightCondition
                item.ConditionID = 1000;    // new
                item.Country = CountryCodeType.US;
                item.Currency = CurrencyCodeType.USD;
                item.DispatchTimeMax = 3;
                item.ListingDuration = "Days_30";
                // Buy It Now fixed price
                item.ListingType = ListingTypeCodeType.FixedPriceItem;
                // Auction
                //item.ListingType = ListingTypeCodeType.Chinese; 
                item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection
            {
                BuyerPaymentMethodCodeType.PayPal
            };
                item.AutoPay = true;    // require immediate payment
                                        // Default testing paypal email address
                item.PayPalEmailAddress = "weidnerk@gmail.com";

                item.PictureDetails = new PictureDetailsType();
                item.PictureDetails.PictureURL = new StringCollection();
                item.PictureDetails.PictureURL.AddRange(pictureURLs.ToArray());
                item.PostalCode = "33772";
                item.Quantity = 1; // 1 If Auction
                item.ReturnPolicy = new ReturnPolicyType
                {
                    ReturnsAcceptedOption = "ReturnsAccepted",
                    ReturnsWithinOption = "Days_30",
                    //RefundOption = "MoneyBack",
                    Description = "Please return if unstatisfied.",
                    ShippingCostPaidByOption = "Buyer",
                    RestockingFeeValue = "Percent_20",
                    RestockingFeeValueOption = "Percent_20"
                };
                item.ShippingDetails = GetShippingDetail();
                item.Site = SiteCodeType.US;

                request.Item = item;

                VerifyAddItemResponseType response = service.VerifyAddItem(request);
                Console.WriteLine("ItemID: {0}", response.ItemID);

                // If item is verified, the item will be added.
                if (response.ItemID == "0")
                {
                    Console.WriteLine("=====================================");
                    Console.WriteLine("Add Item Verified");
                    Console.WriteLine("=====================================");
                    listedItemID = AddItemRequest(item);
                }
                return listedItemID;
            }
            catch (Exception exc)
            {
                string s = exc.Message;
                return null;
            }
        }
        protected static ShippingDetailsType GetShippingDetail()
        {
            ShippingDetailsType sd = new ShippingDetailsType();

            //sd.ApplyShippingDiscount = true;
            //sd.PaymentInstructions = "eBay .Net SDK test instruction.";
            //sd.ShippingRateType = ShippingRateTypeCodeType.StandardList;

            //adding domestic shipping

            ShippingServiceOptionsType domesticShipping1 = new ShippingServiceOptionsType();
            domesticShipping1.ShippingService = ShippingServiceCodeType.ShippingMethodStandard.ToString();
            domesticShipping1.ShippingServiceCost = new AmountType { Value = 0, currencyID = CurrencyCodeType.USD };
            domesticShipping1.ShippingInsuranceCost = new AmountType { Value = 0, currencyID = CurrencyCodeType.USD };
            domesticShipping1.ShippingServicePriority = 4;
            domesticShipping1.LocalPickup = false;
            domesticShipping1.FreeShipping = true;
            sd.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection(new[] { domesticShipping1 });
            sd.ShippingType = ShippingTypeCodeType.Flat;

            return sd;
        }

        /// <summary>
        /// Add item to eBay. Once verified.
        /// </summary>
        /// <param name="item">Accepts ItemType object from VerifyAddItem method.</param>
        public static string AddItemRequest(ItemType item)
        {
            eBayAPIInterfaceService service = EbayCalls.eBayServiceCall("AddItem");

            AddItemRequestType request = new AddItemRequestType();
            request.Version = "949";
            request.ErrorLanguage = "en_US";
            request.WarningLevel = WarningLevelCodeType.High;
            request.Item = item;

            AddItemResponseType response = service.AddItem(request);

            Console.WriteLine("Item Added");
            Console.WriteLine("ItemID: {0}", response.ItemID); // Item ID
            return response.ItemID;
        }

    }
}
