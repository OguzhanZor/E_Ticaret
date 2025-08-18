using System.Data;
using Microsoft.Data.SqlClient;

namespace E_Ticaret.Data
{
    public class DatabaseContext
    {
        public string ConnectionString { get; }
        public string MasterConnectionString { get; }

        public DatabaseContext(IConfiguration configuration)
        {
            var baseConnectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                "Server=127.0.0.1;Database=E_Ticaret;User Id=sa;Password=1;TrustServerCertificate=true;";
            
            ConnectionString = baseConnectionString;
            
            // Master veritabanına bağlanmak için connection string
            var builder = new SqlConnectionStringBuilder(baseConnectionString);
            builder.InitialCatalog = "master";
            MasterConnectionString = builder.ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public SqlConnection CreateMasterConnection()
        {
            return new SqlConnection(MasterConnectionString);
        }
    }
}
