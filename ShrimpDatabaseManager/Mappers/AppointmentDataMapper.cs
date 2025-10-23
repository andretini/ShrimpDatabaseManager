using System.Data;
using Shrimp.Domain.Entities;

namespace ShrimpDatabaseManager.Mappers
{
    public class AppointmentDataMapper : IDataMapper<Appointment>
    {
        public Appointment Map(IDataReader reader)
        {
            // Função auxiliar para tratar a conversão de ID, que já estava correta
            Guid GetGuid(string columnName)
            {
                // O indexador reader[columnName] funciona com string, por isso não deu erro aqui.
                var value = reader[columnName];
                return value is byte[] bytes ? new Guid(bytes) : (Guid)value;
            }

            return new Appointment
            {
                Id = GetGuid("id"),
                ClientId = GetGuid("client_id"),
                ProfessionalId = GetGuid("professional_id"),
                ServiceId = GetGuid("service_id"),

                // --- CORREÇÃO APLICADA AQUI ---
                // Usamos GetOrdinal() para encontrar o índice da coluna pelo nome
                StartAt = reader.GetDateTime(reader.GetOrdinal("start_at")),
                DurationMinutes = reader.GetInt32(reader.GetOrdinal("duration_minutes")),
                Status = reader.GetString(reader.GetOrdinal("status")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
            };
        }
    }
}