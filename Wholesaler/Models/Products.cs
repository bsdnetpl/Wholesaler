using CsvHelper.Configuration.Attributes;

namespace Wholesaler.Models
{
    public class Products
    {
        //[Name("ID")]
        [Index(0)]
        public string Id { get; set; }
        [Index(1)]
        //[Name("SKU")]
        public string SKU { get; set; }
        [Index(2)]
        //[Name("name")]
        public string Name { get; set; }
        [Index(3)]
        //[Name("reference_number")]
        public string Reference_number { get; set; }
        [Index(4)]
        ///[Name("EAN")]
        public string EAN { get; set; }
        [Index(5)]
        //[Name("can_be_returned")]
        public string Can_be_returned { get; set; } 
        //[Name("producer_name")]
         [Index(6)]
        public string Producer_name { get; set; }
        //[Name("category")]
        [Index(7)]
        public string Category { get; set; }
        [Index(8)]
        //[Name("is_wire")]
        public string Is_wire { get; set; } 
        [Index(9)]
        //[Name("shipping")]
        public string Shipping { get; set; }
        [Index(10)]
        //[Name("package_size")]
        public string Package_size { get;set; }
        [Index(11)]
        //[Name("available")]
        public string Available { get; set;}
        [Index(12)]
        //[Name("logistic_height")]
        public string Logistic_height { get; set; } 
        [Index(13)]
        //[Name("logistic_width")]
        public string Logistic_width { get; set; } 
        [Index(14)]
        //[Name("logistic_length")]
        public string Logistic_length { get; set; } 
        [Index(15)]
        //[Name("logistic_weight")]
        public string Logistic_weight { get; set; }
        [Index(16)]
        //[Name("is_vendor")]
        public string Is_vendor { get; set; } 
        [Index(17)]
        //[Name("available_in_parcel_locker")]
        public string Available_in_parcel_locker { get; set; } 
        [Index(18)]
        //[Name("default_image")]
        public string Default_image {  get; set; } 
    }
}
