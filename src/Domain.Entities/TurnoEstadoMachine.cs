using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    internal static class TurnoEstadoMachine
    {
        private class Transicion
        {
            public Transicion(TurnoEstado estadoInicial, TurnoAccion estadoAccion, TurnoEstado estadoFinal)
            {
                EstadoInicial = estadoInicial;
                EstadoAccion = estadoAccion;
                EstadoFinal = estadoFinal;
            }

            public TurnoEstado EstadoInicial { get; set; }
            public TurnoAccion EstadoAccion { get; set; }
            public TurnoEstado EstadoFinal { get; set; }


        }

        private static IList<Transicion> Transiciones => new List<Transicion> 
        {
            new Transicion(TurnoEstado.Pendiente, TurnoAccion.CheckIn, TurnoEstado.Encolado),
            new Transicion(TurnoEstado.Encolado, TurnoAccion.Llamar, TurnoEstado.EnAtencion),
            new Transicion(TurnoEstado.EnAtencion, TurnoAccion.Fin, TurnoEstado.Finalizado),
            new Transicion(TurnoEstado.Pendiente, TurnoAccion.Cancelar, TurnoEstado.Cancelado),
            new Transicion(TurnoEstado.Encolado, TurnoAccion.Cancelar, TurnoEstado.Cancelado)
        };

        internal static TurnoEstado ObtenerProximoEstado(TurnoEstado estadoActual, TurnoAccion accion)
        {
            return Transiciones
                .Where(x => x.EstadoInicial == estadoActual)
                .Where(x => x.EstadoAccion == accion)
                .FirstOrDefault()?.EstadoFinal ?? throw new InvalidOperationException();
        }

        internal static IEnumerable<TurnoAccion> ObtenerPosiblesAcciones(TurnoEstado estadoActual)
        {
            return Transiciones
                .Where(x => x.EstadoInicial == estadoActual)
                .Select(x => x.EstadoAccion)
                .ToList();
        }

        internal static bool TransicionPosible(TurnoEstado estadoActual, TurnoAccion accion)
        {
            try
            {
                ObtenerProximoEstado(estadoActual, accion);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}