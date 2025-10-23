using System;
using System.Data;
using System.Threading.Tasks;
using ShrimpDatabaseManager.Adapters;
using Shrimp.Domain.Entities; // Referência à entidade
using System.Data.Common;

namespace ShrimpDatabaseManager.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbAdapter Adapter { get; } 
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public IDbExecutor Executor { get; private set; }

        public UnitOfWork(IDbAdapter dbAdapter)
        {
            Adapter = dbAdapter; // Atribui o adaptador à nova propriedade pública
            _connection = (DbConnection)Adapter.CreateConnection();
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                await ((dynamic)_connection).OpenAsync(); // ADO.NET Core usa OpenAsync
            }
            _transaction = _connection.BeginTransaction();

            // O Executor é instanciado com a transação ativa
            Executor = new AdoExecutor(
                _transaction,
                new Dictionary<Type, object> {
                    { typeof(Appointment), new Mappers.AppointmentDataMapper() }
                }
            );
        }

        public async Task CommitAsync()
        {
            try
            {
                await ((dynamic)_transaction).CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                await ((dynamic)_transaction).RollbackAsync();
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is System.Data.Common.DbTransaction dbTransaction)
            {
                await dbTransaction.DisposeAsync();
            }
            if (_connection is System.Data.Common.DbConnection dbConnection)
            {
                await dbConnection.DisposeAsync();
            }
        }
    }
}