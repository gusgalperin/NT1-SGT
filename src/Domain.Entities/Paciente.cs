using System;

namespace Domain.Entities
{
    public class Paciente : Entity<Guid>
    {
        public Paciente(string nombre, string dni, DateTime fechaNacimiento, DateTime fechaAlta)
            : base(Guid.NewGuid())
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException($"'{nameof(nombre)}' cannot be null or empty.", nameof(nombre));
            }

            if (string.IsNullOrEmpty(dni))
            {
                throw new ArgumentException($"'{nameof(dni)}' cannot be null or empty.", nameof(dni));
            }

            Nombre = nombre;
            Dni = dni;
            FechaNacimiento = fechaNacimiento;
            FechaAlta = fechaAlta;
        }

        public string Nombre { get; private set; }
        public string Dni { get; private set; }
        public DateTime FechaNacimiento { get; private set; }
        public DateTime FechaAlta { get; private set; }
    }
}