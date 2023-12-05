﻿using CsvHelper.Configuration;
using CsvHelper;
using Dapper;
using System.Data;
using System.Globalization;
using Wholesaler.Models;
using WholesalerDapper.DB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Wholesaler.DB;

namespace WholesalerDapper.Service
{
    public class ServiceWarhause : IServiceWarhause
    {
        private readonly DapperContext _context;
        private readonly CSV cSV;

        public ServiceWarhause(DapperContext context, CSV cSV)
        {
            _context = context;
            this.cSV = cSV;
        }

        public async Task<bool> ExtractCsvProductsD()
        {
            var tf = cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Products.csv");
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
                    connection.Open();
                    foreach (var product in prod)
                    {
                        await connection.ExecuteAsync(query, product);
                    }
                }
            }
            return true;
        }
        public async Task<bool> ExtractCsvInventoryD()
        {
            // Download the CSV file from the specified URL
            var tf = cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Inventory.csv");
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
                    connection.Open();
                    foreach (var prices in prod)
                    {
                        await connection.ExecuteAsync(query, prices);
                    }
                }
            }
            return true;
        }
        public async Task<bool> ExtractCsvPricesD()
        {
            // Download the CSV file from the specified URL
            var tf = cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Prices.csv");
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
                    connection.Open();
                    foreach (var prices in records)
                    {
                        await connection.ExecuteAsync(query, prices);
                    }
                }
            }
            return true;
        }
    }
}
