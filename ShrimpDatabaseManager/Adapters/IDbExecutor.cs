using System.Data;
using System.Threading.Tasks;

namespace ShrimpDatabaseManager.Adapters
{
    /// <summary>
    /// (Porta de Saída) Abstrai a execução de comandos SQL.
    /// O repositório usará esta interface para executar queries e DML,
    /// garantindo que ele não dependa diretamente de classes como NpgsqlCommand ou MySqlCommand.
    /// </summary>
    public interface IDbExecutor
    {
        Task<int> ExecuteAsync(string sql, object param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null) where T : class;
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null) where T : class;
    }
}