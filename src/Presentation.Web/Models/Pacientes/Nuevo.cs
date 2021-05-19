using System;

namespace Presentation.Web.Models.Pacientes
{
    public class Nuevo
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacmiento { get; set; }

        public Guid Id { get; private set; }

        public Nuevo SetId(Guid id)
        {
            Id = id;
            return this;
        }
    }
}