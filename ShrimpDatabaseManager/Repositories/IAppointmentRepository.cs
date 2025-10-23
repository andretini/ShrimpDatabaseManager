using Shrimp.Domain.Entities;

namespace ShrimpDatabaseManager.Repositories
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task<Appointment> GetByIdAsync(Guid id);
    }
}