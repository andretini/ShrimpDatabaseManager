using System.Data;
using Npgsql; // Dependência NuGet

namespace ShrimpDatabaseManager.Adapters
{
    public class PostgresAdapter : IDbAdapter
    {
        private readonly string _connectionString;

        public PostgresAdapter(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public override IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}