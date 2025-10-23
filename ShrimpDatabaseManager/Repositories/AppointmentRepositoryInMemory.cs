using Shrimp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShrimpDatabaseManager.Repositories
{
    /// <summary>
    /// (Adapter) Implementação "fake" do repositório para testes de unidade.
    /// Armazena os dados em uma lista na memória, permitindo testes rápidos e isolados
    /// sem a necessidade de um banco de dados real.
    /// </summary>
    public class AppointmentRepositoryInMemory : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = new();

        public Task AddAsync(Appointment appointment)
        {
            _appointments.Add(appointment);
            return Task.CompletedTask;
        }

        public Task<Appointment> GetByIdAsync(Guid id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(appointment);
        }
    }
}