using System;
using ShrimpDatabaseManager.Adapters;

namespace ShrimpDatabaseManager.Factories
{
    /// <summary>
    /// (Pattern: Factory) Encapsula a lógica de criação dos adaptadores de banco de dados.
    /// Justificativa: Centraliza a decisão de qual banco usar, permitindo a troca
    /// via configuração (variáveis de ambiente) sem alterar o código que consome o adaptador.
    /// Remove a responsabilidade da camada de aplicação de saber como instanciar um DB específico.
    /// </summary>
    public static class DbAdapterFactory
    {
        public static IDbAdapter Create(string connectionString, string dbType)
        {
            return dbType.ToLowerInvariant() switch
            {
                "postgres" => new PostgresAdapter(connectionString),
                "mysql" => new MySqlAdapter(connectionString),
                "inmemory" => new InMemoryAdapter(),
                _ => throw new ArgumentException($"Tipo de banco de dados não suportado: '{dbType}'")
            };
        }
    }
}