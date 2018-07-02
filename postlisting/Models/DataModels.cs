using System;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ReadyToList")]
public class ReadyToList
{
    public int ID { get; set; }
    public int SourceID { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string EbayUrl { get; set; }

    public string EbayItemId { get; set; }
    public string SupplierItemId { get; set; }
    public string PrimaryCategoryID { get; set; }
    public string PrimaryCategoryName { get; set; }
    public string Pictures { get; set; }
    public string Description { get; set; }
    public int CategoryID { get; set; }
    public bool ToList { get; set; }
    public bool Transferred { get; set; }
    public string ListedItemID { get; set; }
}
