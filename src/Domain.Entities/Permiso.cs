namespace Domain.Entities
{
    public class Permiso : Entity<string>
    {
        protected Permiso()
        {
        }

        public Permiso(string descripcion)
            : base(descripcion)
        { }

        //turnos
        public static string CrearTurno => "turno.crear";

        public static string CancerlarTurno => "turno.cancelar";
        public static string LlamarTurno => "turno.llamar";
        public static string CheckinTurno => "turno.checkin";
        public static string FinTurno => "turno.fin";
    }
}