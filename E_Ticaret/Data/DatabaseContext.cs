using System.Data;
using System.Data.SqlClient;

namespace E_Ticaret.Data
{
    public class DatabaseContext
    {
        public string ConnectionString { get; }

        public DatabaseContext(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                "Server=127.0.0.1;Database=E_Ticaret;User Id=sa;Password=1;TrustServerCertificate=true;";
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
