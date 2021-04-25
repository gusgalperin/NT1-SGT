using System;

namespace Domain.Core.Exceptions
{
    public class TurnoOcupadoException : UserException
    {
        public TurnoOcupadoException(DateTimeOffset fecha, TimeSpan horaInicio, TimeSpan horaFin)
            : base($"El turno {fecha} - {horaFin} / {horaFin} se encuentra ocupado")
        { }
    }
}