using CsvHelper.Configuration;
using CsvHelper;
using Dapper;
using System.Data;
using System.Globalization;
using Wholesaler.Models;
using WholesalerDapper.DB;

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


            Products products = new Products();

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

                var parameters = new DynamicParameters();
                parameters.Add("ID", prod.Select(s => s.Id), DbType.String);
                parameters.Add("Name", prod.Select(s => s.Name), DbType.String);
                parameters.Add("SKU", prod.Select(s => s.SKU), DbType.String);
                parameters.Add("Reference_number", prod.Select(s => s.Reference_number), DbType.String);
                parameters.Add("Is_wire", prod.Select(s => s.Is_wire), DbType.String);
                parameters.Add("Available", prod.Select(s => s.Available), DbType.String);
                parameters.Add("Available_in_parcel_locker", prod.Select(s => s.Available_in_parcel_locker), DbType.String);
                parameters.Add("Can_be_returned", prod.Select(s => s.Can_be_returned), DbType.String);
                parameters.Add("Category", prod.Select(s => s.Category), DbType.String);
                parameters.Add("Default_image", prod.Select(s => s.Default_image), DbType.String);
                parameters.Add("EAN", prod.Select(s => s.EAN), DbType.String);
                parameters.Add("Is_vendor", prod.Select(s => s.Is_vendor), DbType.String);
                parameters.Add("Logistic_height", prod.Select(s => s.Logistic_height), DbType.String);
                parameters.Add("Logistic_length", prod.Select(s => s.Logistic_length), DbType.String);
                parameters.Add("Logistic_weight", prod.Select(s => s.Logistic_weight), DbType.String);
                parameters.Add("Logistic_width", prod.Select(s => s.Logistic_width), DbType.String);
                parameters.Add("Package_size", prod.Select(s => s.Package_size), DbType.String);
                parameters.Add("Producer_name", prod.Select(s => s.Producer_name), DbType.String);
                parameters.Add("Shipping", prod.Select(s => s.Shipping), DbType.String);
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
                return true;
            }
        }
    }
}
