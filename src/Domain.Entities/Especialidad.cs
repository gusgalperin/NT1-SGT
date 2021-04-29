using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Especialidad : Entity<Guid>
    {
        public string Descripcion { get; private set; }

        public ICollection<ProfesionalEspecialidad> Profesionales { get; private set; }

        public Especialidad(string descripcion)
            : base(Guid.NewGuid())
        {
            if (string.IsNullOrEmpty(descripcion))
            {
                throw new ArgumentException($"'{nameof(descripcion)}' cannot be null or empty.", nameof(descripcion));
            }

            Descripcion = descripcion;
        }
    }
}