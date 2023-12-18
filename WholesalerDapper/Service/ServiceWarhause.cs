using CsvHelper.Configuration;
using CsvHelper;
using Dapper;
using System.Data;
using System.Globalization;
using Wholesaler.Models;
using WholesalerDapper.DB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Wholesaler.DB;
using Microsoft.Data.SqlClient;

namespace WholesalerDapper.Service
{
    public class ServiceWarhause : IServiceWarhause
    {
        private readonly DapperContext _context;
        private readonly CSV _cSV;

        public ServiceWarhause(DapperContext context, CSV cSV)
        {
            _context = context;
            _cSV = cSV;
        }

        public async Task<bool> ExtractCsvProductsD()
        {
            var tf = _cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Products.csv");
            if (!tf)
            {
                return false;
            }
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                Delimiter = ";"
            };
            // Read the CSV file and add the products to the database
            using (var reader = new StreamReader("Products.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<ProductsMap>();
                var records = csv.GetRecords<Products>();
                var prod = records.Where(a => !a.Name.ToLower().Contains("kabel") && a.Shipping == "24h");

                var query = "INSERT INTO ProductsDB (Id,Name,SKU,Reference_number,Is_wire,Available,Available_in_parcel_locker,Can_be_returned,Category,Default_image,EAN,Is_vendor,Logistic_height,Logistic_length,Logistic_weight,Logistic_width,Package_size,Producer_name,Shipping)" +
                   "VALUES (@Id,@Name,@SKU,@Reference_number,@Is_wire,@Available,@Available_in_parcel_locker,@Can_be_returned,@Category,@Default_image,@EAN,@Is_vendor,@Logistic_height,@Logistic_length,@Logistic_weight,@Logistic_width,@Package_size,@Producer_name,@Shipping)";

                using (var connection = _context.CreateConnection())
                {
                   await connection.ExecuteAsync(query, prod);
                }
            }
            return true;
        }
        public async Task<bool> ExtractCsvInventoryD()
        {
            // Download the CSV file from the specified URL
            var tf = _cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Inventory.csv");
            if (!tf)
            {
                return false;
            }
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                Delimiter = ","
            };
            // Read the CSV file and add the Inventory to the database
            using (var reader = new StreamReader("Inventory.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Inventory>();
                var prod = (from a in records where a.Shipping == "24h" select a).ToList();
                var query = "INSERT INTO InventoriesDB (Product_id,SKU,Unit,Qty,Manufacturer_name,Manufacturer_ref_num,Shipping,Shipping_cost)" +
                            "VALUES (@Product_id,@SKU,@Unit,@Qty,@Manufacturer_name,@Manufacturer_ref_num,@Shipping,@Shipping_cost)";

                using (var connection = _context.CreateConnection())
                {
                   await connection.ExecuteAsync(query, prod);
                }
            }
            return true;
        }
        public async Task<bool> ExtractCsvPricesD()
        {
            // Download the CSV file from the specified URL
            var tf = _cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Prices.csv");
            if (!tf)
            {
                return false;
            }
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                MissingFieldFound = null,
                Delimiter = ","
            };
            // Read the CSV file and add the prices to the database
            using (var reader = new StreamReader("Prices.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Prices>().ToList();

                var query = "INSERT INTO PricesDB (Id,SKU,Nett_product_price,Nett_product_price_discount,Vat_Tax,Nett_product_price_discount_logistic_unit)" +
                   "VALUES (@Id,@SKU,@Nett_product_price,@Nett_product_price_discount,@Vat_Tax,@Nett_product_price_discount_logistic_unit)";

                using (var connection = _context.CreateConnection())
                {
                   await connection.ExecuteAsync(query, records);
                }
            }
            return true;
        }
        //This is one of the possible solutions.
        //it is not specified what should be returned, consequently it will be an object in json format.
        //Of course, I could have used the option with @sku parameters
        public async Task<object> GetProductsBySKUD(string sku)
        {
            using (var connection = _context.CreateConnection())
            {
                var query = "SELECT ProductsDB.Name,ProductsDB.EAN,InventoriesDB.Manufacturer_name,ProductsDB.Category,ProductsDB.Default_image,ProductsDB.Available,PricesDB.Nett_product_price_discount_logistic_unit,PricesDB.Nett_product_price,InventoriesDB.Shipping_cost FROM ProductsDB inner join PricesDB on ProductsDB.SKU = PricesDB.SKU inner join InventoriesDB on ProductsDB.SKU = InventoriesDB.SKU where ProductsDB.SKU = '" + sku + "'";
                var products = await connection.QuerySingleAsync<object>(query);
                return products;
            }
        }

        public void TruncateTable(string TableName)// Truncate Table start to new 
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Execute($"Truncate Table {TableName}"); 
            }
        }

    }
}
