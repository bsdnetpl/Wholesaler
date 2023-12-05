using Microsoft.Data.SqlClient;
using System.Data;

namespace WholesalerDapper.DB
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("CS");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
