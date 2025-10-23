using Shrimp.Domain.Entities;
using ShrimpDatabaseManager.Adapters;
using ShrimpDatabaseManager.UnitOfWork;

namespace ShrimpDatabaseManager.Repositories
{
    public class AppointmentRepositoryAdo : IAppointmentRepository
    {
        private readonly IUnitOfWork _uow;
        // Adicionamos um campo para saber o tipo do adaptador
        private readonly bool _isMySql;

        public AppointmentRepositoryAdo(IUnitOfWork uow)
        {
            _uow = uow;
            // Verifica se estamos usando o adaptador do MySQL
            _isMySql = _uow.Adapter is MySqlAdapter;
        }

        public async Task AddAsync(Appointment appointment)
        {
            var sql = @"
                INSERT INTO appointments (id, client_id, professional_id, service_id, start_at, duration_minutes, status, created_at)
                VALUES (@Id, @ClientId, @ProfessionalId, @ServiceId, @StartAt, @DurationMinutes, @Status, @CreatedAt);";

            // Se for MySQL, converte Guid para byte[]. Senão, envia o Guid.
            object param = _isMySql ? new 
            {
                Id = appointment.Id.ToByteArray(),
                ClientId = appointment.ClientId.ToByteArray(),
                ProfessionalId = appointment.ProfessionalId.ToByteArray(),
                ServiceId = appointment.ServiceId.ToByteArray(),
                appointment.StartAt,
                appointment.DurationMinutes,
                appointment.Status,
                appointment.CreatedAt
            } : (object)appointment;

            await _uow.Executor.ExecuteAsync(sql, param);
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM appointments WHERE id = @Id;";
            
            // Converte o parâmetro 'id' se for MySQL
            object param = _isMySql ? new { Id = id.ToByteArray() } : new { Id = id };

            var result = await _uow.Executor.QueryFirstOrDefaultAsync<Appointment>(sql, param);

            return result;
        }
    }
}