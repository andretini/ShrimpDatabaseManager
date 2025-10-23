using System;

namespace Shrimp.Domain.Entities
{
    // Usando 'record' para imutabilidade, o que é uma boa prática para entidades.
    public record Appointment
    {
        public Guid Id { get; init; }
        public Guid ClientId { get; init; }
        public Guid ProfessionalId { get; init; }
        public Guid ServiceId { get; init; }
        public DateTime StartAt { get; init; }
        public int DurationMinutes { get; init; }
        public string Status { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}