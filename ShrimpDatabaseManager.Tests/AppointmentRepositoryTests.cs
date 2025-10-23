using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Shrimp.Domain.Entities; // Referência à sua entidade
using ShrimpDatabaseManager.Factories;
using ShrimpDatabaseManager.Repositories;
using ShrimpDatabaseManager.UnitOfWork;

namespace ShrimpDatabaseManager.Tests
{
    public class AppointmentRepositoryTests
    {
        // ====================================================================
        // Testes de Integração (contra bancos de dados reais via Docker)
        // ====================================================================

        /// <summary>
        /// Este teste de teoria executa o mesmo fluxo contra ambos os bancos de dados.
        /// Justificativa: Garante que a abstração (IDbAdapter, IUnitOfWork) funciona corretamente
        /// e que a lógica de persistência é compatível com MySQL e PostgreSQL sem alteração de código.
        /// </summary>
        [Theory]
        [InlineData("postgres")]
        [InlineData("mysql")]
        public async Task Should_Add_And_Get_Appointment_Successfully_In_RealDatabase(string dbType)
        {
            // --- ARRANGE (Organização) ---
            var newAppointment = new Appointment
            {
                Id = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                ProfessionalId = Guid.NewGuid(),
                ServiceId = Guid.NewGuid(),
                StartAt = DateTime.UtcNow.Date,
                DurationMinutes = 60,
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow
            };

            var connectionString = GetConnectionString(dbType);
            var adapter = DbAdapterFactory.Create(connectionString, dbType);

            // --- ACT (Ação) ---

            // 1. Inserir o dado em uma transação
            await using (var uow = new UnitOfWork.UnitOfWork(adapter))
            {
                await uow.BeginTransactionAsync();
                var repo = new AppointmentRepositoryAdo(uow);
                await repo.AddAsync(newAppointment);
                await uow.CommitAsync();
            }

            // 2. Buscar o dado em uma nova transação para garantir que foi persistido
            Appointment retrievedAppointment;
            await using (var uow = new UnitOfWork.UnitOfWork(adapter))
            {
                await uow.BeginTransactionAsync();
                var repo = new AppointmentRepositoryAdo(uow);
                retrievedAppointment = await repo.GetByIdAsync(newAppointment.Id);
                // O commit aqui não é estritamente necessário para um SELECT, mas é boa prática.
                await uow.CommitAsync();
            }
            
            // --- ASSERT (Verificação) ---
            retrievedAppointment.Should().NotBeNull();
            retrievedAppointment.Id.Should().Be(newAppointment.Id);
            retrievedAppointment.ClientId.Should().Be(newAppointment.ClientId);
            retrievedAppointment.Status.Should().Be("Scheduled");

            // --- CLEANUP (Limpeza) ---
            await CleanUp(adapter, newAppointment.Id);
        }

        // ====================================================================
        // Teste de Unidade/Componente (usando o repositório em memória)
        // ====================================================================

        /// <summary>
        /// Este teste usa a implementação InMemory.
        /// Justificativa: É extremamente rápido e não depende de infraestrutura externa (Docker, rede).
        /// Ideal para ser executado em pipelines de CI/CD para um feedback rápido.
        /// </summary>
        [Fact]
        public async Task Should_Add_And_Get_Appointment_From_InMemory_Repository()
        {
            // --- ARRANGE ---
            var newAppointment = new Appointment { Id = Guid.NewGuid(), Status = "InMemoryTest" };
            var repo = new AppointmentRepositoryInMemory();

            // --- ACT ---
            await repo.AddAsync(newAppointment);
            var retrievedAppointment = await repo.GetByIdAsync(newAppointment.Id);

            // --- ASSERT ---
            retrievedAppointment.Should().NotBeNull();
            retrievedAppointment.Id.Should().Be(newAppointment.Id);
            retrievedAppointment.Status.Should().Be("InMemoryTest");
        }

        #region Métodos de Suporte

        private string GetConnectionString(string dbType)
        {
            return dbType.ToLower() switch
            {
                "postgres" => "Server=localhost;Port=5432;Database=scheduling_db;User Id=user;Password=password;",
                "mysql" => "Server=localhost;Port=3306;Database=scheduling_db;User=user;Password=password;",
                _ => throw new ArgumentOutOfRangeException(nameof(dbType), "Tipo de banco de dados não suportado para teste.")
            };
        }

        private async Task CleanUp(Adapters.IDbAdapter adapter, Guid id)
        {
            await using var uow = new UnitOfWork.UnitOfWork(adapter);
            await uow.BeginTransactionAsync();
            var sql = adapter.GetType().Name.Contains("MySql") 
                ? "DELETE FROM appointments WHERE id = UUID_TO_BIN(@Id);" 
                : "DELETE FROM appointments WHERE id = @Id;";
            
            await uow.Executor.ExecuteAsync(sql, new { Id = id });
            await uow.CommitAsync();
        }

        #endregion
    }
}