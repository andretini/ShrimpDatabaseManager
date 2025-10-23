using System.Data;

namespace ShrimpDatabaseManager.Adapters
{
    /// <summary>
    /// (Porta de Saída) Define o contrato para um adaptador de banco de dados.
    /// Esta abstração permite que o UnitOfWork opere com diferentes bancos de dados (PostgreSQL, MySQL)
    /// sem conhecer os detalhes de implementação de cada um. É a base da intercambialidade.
    /// </summary>
    public interface IDbAdapter
    {
        IDbConnection CreateConnection();
    }
}