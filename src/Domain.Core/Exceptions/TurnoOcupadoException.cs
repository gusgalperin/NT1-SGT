using System;

namespace Domain.Core.Exceptions
{
    public class TurnoOcupadoException : UserException
    {
        public TurnoOcupadoException(DateTimeOffset fecha, TimeSpan horaInicio)
            : base($"El turno {fecha} - {horaInicio} se encuentra ocupado")
        { }
    }
}