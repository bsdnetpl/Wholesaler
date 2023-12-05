using CsvHelper.Configuration;
using CsvHelper;
using Dapper;
using System.Data;
using System.Globalization;
using Wholesaler.Models;
using WholesalerDapper.DB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            //var tf = cSV.DownloadFile("https://rekturacjazadanie.blob.core.windows.net/zadanie/Products.csv");

            //if (!tf)
            //{
            //    return false;
            //}


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
                   " VALUES (@Id,@Name,@SKU,@Reference_number,@Is_wire,@Available,@Available_in_parcel_locker,@Can_be_returned,@Category,@Default_image,@EAN,@Is_vendor,@Logistic_height,@Logistic_length,@Logistic_weight,@Logistic_width,@Package_size,@Producer_name,@Shipping)";
                
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
    }
}
