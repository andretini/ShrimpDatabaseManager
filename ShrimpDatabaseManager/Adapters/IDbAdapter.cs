using System.Data;
using ShrimpDatabaseManager.Mappers;

namespace ShrimpDatabaseManager.Adapters
{
    /// <summary>
    /// (Porta de Saída) Define o contrato para um adaptador de banco de dados.
    /// Esta abstração permite que o UnitOfWork opere com diferentes bancos de dados (PostgreSQL, MySQL)
    /// sem conhecer os detalhes de implementação de cada um. É a base da intercambialidade.
    /// </summary>
    public abstract class IDbAdapter
    {
        public abstract IDbConnection CreateConnection();
        private Dictionary<Type, object> Mappers { get; set; }

        protected IDbAdapter()
        {
            Mappers = new Dictionary<Type, object>();
        }

        public Dictionary<Type, object> get_mappers()
        {
            return Mappers;
        }

        public void RegisterMapper<T>(IDataMapper<T> mapper) where T : class
        {
            Mappers[typeof(T)] = mapper;
        }
    }
}