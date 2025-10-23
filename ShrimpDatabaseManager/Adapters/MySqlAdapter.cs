using System.Data;
using MySqlConnector; // Dependência NuGet

namespace ShrimpDatabaseManager.Adapters
{
    public class MySqlAdapter : IDbAdapter
    {
        private readonly string _connectionString;

        public MySqlAdapter(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}