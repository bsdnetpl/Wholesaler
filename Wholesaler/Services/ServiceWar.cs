using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Wholesaler.DB;
using Wholesaler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Wholesaler.Services
{
    public class ServiceWar : IServiceWar
    {
        private readonly MssqlConnect _mssqlConnect;
        private readonly MssqlConnect _mssqlConnect2;
        private readonly MssqlConnect _mssqlConnect3;
        private readonly CSV _cSV;

        public ServiceWar(MssqlConnect mssqlConnect, CSV cSV, MssqlConnect mssqlConnect2, MssqlConnect mssqlConnect3)
        {
            _mssqlConnect = mssqlConnect;
            _cSV = cSV;
            _mssqlConnect2 = mssqlConnect2;
            _mssqlConnect3 = mssqlConnect3;
        }
        // Extracts products from CSV file and adds them to the database
        public async Task<bool> ExtractCsvProducts()
        {
            _mssqlConnect.Database.ExecuteSqlRaw("TRUNCATE TABLE ProductsDB"); // Truncate Table start to new 
            // Download the CSV file from the specified URL
            var tf = _cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Products.csv");

            if (!tf)
            {
                return false;
            }

            // Configure the CSV reader
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
                await _mssqlConnect.ProductsDB.AddRangeAsync(prod);
                await _mssqlConnect.SaveChangesAsync();
                return true;
            }
        }
        // Extracts inventory from CSV file and adds it to the database
        public async Task<bool> ExtractCsvInventory()
        {
            _mssqlConnect2.Database.ExecuteSqlRaw("TRUNCATE TABLE InventoriesDB");// Truncate Table start to new 
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
                await _mssqlConnect2.InventoriesDB.AddRangeAsync(prod);
                await _mssqlConnect2.SaveChangesAsync();
                return true;
            }

        }
        public async Task<bool> ExtractCsvPrices()
        {
            _mssqlConnect3.Database.ExecuteSqlRaw("TRUNCATE TABLE PricesDB");// Truncate Table start to new 
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
            using (var reader = new StreamReader("prices.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Prices>().ToList();
                await _mssqlConnect3.PricesDB.AddRangeAsync(records);
                await _mssqlConnect3.SaveChangesAsync();
                return true;
            }

        }
        // Join Products, Prices and Inventories tables on SKU
        public async Task<object> GetProductsBySKU(string sku)
        {
            //This is one of the possible solutions.
            //it is not specified what should be returned, consequently it will be an object in json format.
            var skuFind = (from ep in _mssqlConnect.ProductsDB
                           join p in _mssqlConnect.PricesDB on ep.SKU equals p.SKU
                           join i in _mssqlConnect.InventoriesDB on p.SKU equals i.SKU
                           where i.SKU == sku
                           select new
                           {
                               ep.Name,
                               ep.EAN,
                               i.Manufacturer_name,
                               ep.Category,
                               ep.Default_image,
                               ep.Available,
                               p.Nett_product_price_discount_logistic_unit,
                               p.Nett_product_price,
                               i.Shipping_cost
                           });
            return skuFind;

        }

        public void Cleaning() //I added function to remove records that are not in the Products table,on table prices and inventories
        {
            _mssqlConnect3.Database.ExecuteSqlRaw("DELETE Price FROM PricesDB Price LEFT JOIN ProductsDB Pro ON Pro.SKU = Price.SKU WHERE Pro.SKU IS NULL");
            _mssqlConnect3.Database.ExecuteSqlRaw("DELETE Inv FROM InventoriesDB Inv LEFT JOIN ProductsDB Pro ON Pro.SKU = Inv.SKU WHERE Inv.SKU IS NULL");
        }
    }
}
