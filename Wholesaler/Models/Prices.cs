using CsvHelper.Configuration.Attributes;
using System.Diagnostics.Metrics;

namespace Wholesaler.Models
{
    public class Prices
    {
        public string Id { get; set; }
        public string SKU { get; set; }
        public string Nett_product_price { get; set; }
        public string Nett_product_price_discount { get; set; }
        public string Vat_Tax {  get; set; }
        public string Nett_product_price_discount_logistic_unit { get; set; }

    }
}
