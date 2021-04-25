using System;

namespace Domain.Core.Exceptions
{
    public class ProfesionalNoAtiendeException : UserException
    {
        public ProfesionalNoAtiendeException(string nombreProfesional, DateTimeOffset fecha, TimeSpan horaInicio, TimeSpan horaFin)
            : base($"El profesional {nombreProfesional} no atiende los dias {fecha.DayOfWeek} entre los horarios {horaInicio} y {horaFin}")
        { }
    }
}