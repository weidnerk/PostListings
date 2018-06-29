using System;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SellerOrderHistory")]
public class SellerOrderHistory
{
    public int ID { get; set; }
    public int SourceID { get; set; }
    public string Title { get; set; }
    public decimal EbaySellerPrice { get; set; }
    public string Qty { get; set; }
    //public string DateOfPurchaseStr { get; set; }
    public DateTime? DateOfPurchase { get; set; }
    public string EbayUrl { get; set; }

    public string ImageUrl { get; set; }
    public bool ListingEnded { get; set; }
    public int PageNumber { get; set; }
    public string ItemId { get; set; }
    public string SupplierItemId { get; set; }
    public string PrimaryCategoryID { get; set; }
    public string PrimaryCategoryName { get; set; }
    public string PictureUrl { get; set; }
    public string Description { get; set; }
    public int BatchID { get; set; }
}
