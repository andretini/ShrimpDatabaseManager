using System.Data;
using Moq; // Apenas para simular a interface IDbConnection

namespace ShrimpDatabaseManager.Adapters
{
    /// <summary>
    /// Implementação "fake" para testes. Não se conecta a um banco de dados real.
    /// Justificativa: Permite que os testes de unidade e integração (sem DB)
    /// rodem de forma rápida e isolada, sem a necessidade de um banco de dados real.
    /// </summary>
    public class InMemoryAdapter : IDbAdapter
    {
        // Usamos Moq para criar um objeto fake que satisfaça a interface IDbConnection.
        // Em um cenário real, poderíamos ter uma implementação mais robusta se necessário.
        public IDbConnection CreateConnection()
        {
            return new Mock<IDbConnection>().Object;
        }
    }
}