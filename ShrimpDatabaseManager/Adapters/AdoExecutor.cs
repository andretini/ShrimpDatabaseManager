using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using ShrimpDatabaseManager.Mappers;

namespace ShrimpDatabaseManager.Adapters
{
    public class AdoExecutor : IDbExecutor
    {
        private readonly IDbTransaction _transaction;
        private readonly Dictionary<Type, object> _mappers;

        public AdoExecutor(IDbTransaction transaction, Dictionary<Type, object> mappers)
        {
            _transaction = transaction;
            _mappers = mappers;
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var command = CreateCommand(sql, param);
            return await ((dynamic)command).ExecuteNonQueryAsync();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null) where T : class
        {
            using var command = CreateCommand(sql, param);
            using var reader = await ((dynamic)command).ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                var mapper = (IDataMapper<T>)_mappers[typeof(T)];
                return mapper.Map(reader);
            }
            return null;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null) where T : class
        {
            var list = new List<T>();
            using var command = CreateCommand(sql, param);
            using var reader = await ((dynamic)command).ExecuteReaderAsync();
            var mapper = (IDataMapper<T>)_mappers[typeof(T)];
            
            while (await reader.ReadAsync())
            {
                list.Add(mapper.Map(reader));
            }
            return list;
        }
        
        private IDbCommand CreateCommand(string sql, object param)
        {
            var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;
            command.CommandText = sql;

            if (param != null)
            {
                foreach (PropertyInfo prop in param.GetType().GetProperties())
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@{prop.Name}";
                    parameter.Value = prop.GetValue(param) ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
    }
}