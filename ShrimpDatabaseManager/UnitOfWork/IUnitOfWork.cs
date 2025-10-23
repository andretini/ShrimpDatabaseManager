using ShrimpDatabaseManager.Adapters;

namespace ShrimpDatabaseManager.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IDbExecutor Executor { get; }
        
        // --- ADIÇÃO ---
        // Expõe o adaptador para que o repositório possa saber com qual banco está falando.
        IDbAdapter Adapter { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}