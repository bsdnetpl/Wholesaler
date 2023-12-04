using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Wholesaler.Models
{
    public class Inventory
    {
        [Index(0)]
        public string Product_id { get; set; }
        [Index(1)]
        public string SKU { get; set;}
        [Index(2)]
        public string Unit { get; set;}
        [Index(3)]
        public double Qty { get; set;}
        [Index(4)]
        public string Manufacturer_name { get; set;}
        [Index(5)]
        public string Manufacturer_ref_num { get; set;}
        [Index(6)]
        public string Shipping { get; set; }
        [Index(7)]
        public string Shipping_cost{ get; set; }

    }
}
