using System;

namespace Domain.Core.Exceptions
{
    public class ProfesionalNoAtiendeException : UserException
    {
        public ProfesionalNoAtiendeException(string nombreProfesional, DateTimeOffset fecha, TimeSpan horaInicio)
            : base($"El profesional {nombreProfesional} no atiende los dias {fecha.DayOfWeek} en el horario {horaInicio}")
        { }
    }
}