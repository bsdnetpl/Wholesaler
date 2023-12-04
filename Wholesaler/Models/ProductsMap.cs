using CsvHelper.Configuration;

namespace Wholesaler.Models
{
    public class ProductsMap : ClassMap<Products>
    {
        public ProductsMap()
        {
            Map(m => m.Id).Name("ID");
            Map(m => m.SKU).Name("SKU");
            Map(m => m.Is_wire).Name("is_wire");
            Map(m => m.Available).Name("available");
            Map(m => m.Available_in_parcel_locker).Name("available_in_parcel_locker");
            Map(m => m.Can_be_returned).Name("can_be_returned");
            Map(m => m.Category).Name("category");
            Map(m => m.Default_image).Name("default_image");
            Map(m => m.Name).Name("name");
            Map(m => m.EAN).Name("EAN");
            Map(m => m.Is_vendor).Name("is_vendor");
            Map(m => m.Logistic_height).Name("logistic_height");
            Map(m => m.Logistic_length).Name("logistic_length");
            Map(m => m.Logistic_weight).Name("logistic_weight");
            Map(m => m.Logistic_width).Name("logistic_width");
            Map(m => m.Package_size).Name("package_size");
            Map(m => m.Producer_name).Name("producer_name");
            Map(m => m.Shipping).Name("shipping");
            Map(m => m.Reference_number).Name("reference_number");
        }
    }
}
