using System;

namespace Domain.Entities
{
    public class TurnoHistorial : Entity<Guid>
    {
        public TurnoHistorial(TurnoEstado? estadoDesde, TurnoAccion accion, TurnoEstado estadoHasta, Guid usuarioId)
        {
            EstadoDesde = estadoDesde;
            Accion = accion;
            EstadoHasta = estadoHasta;
            UsuarioId = usuarioId;
            Fecha = DateTime.Now;
        }

        public Guid TurnoId { get; private set; }
        public Turno Turno { get; private set; }
        public TurnoEstado? EstadoDesde { get; private set; }
        public TurnoAccion Accion { get; private set; }
        public TurnoEstado EstadoHasta { get; private set; }
        public DateTime Fecha { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }
    }
}