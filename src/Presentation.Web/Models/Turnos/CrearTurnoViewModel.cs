using Domain.Entities;
using System.Collections.Generic;

namespace Presentation.Web.Models.Turnos
{
    public class CrearTurnoViewModel
    {
        public IEnumerable<Profesional> Profesionales { get; set; }

        public CrearTurnoModel Model => new CrearTurnoModel();
    }
}