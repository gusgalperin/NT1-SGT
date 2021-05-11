using Domain.Entities;
using System.Collections.Generic;

namespace Tests.Utils.Entities
{
    public static class ProfesionalExtensions
    {
        public static Profesional ProfesionalDefault()
        {
            return new Profesional("nombre", "email", "password", new List<Especialidad> { new Especialidad("sarasa") }, DiaHorario.DefaultTodaLaSemana());
        }
    }
}