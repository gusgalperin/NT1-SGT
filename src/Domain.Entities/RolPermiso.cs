namespace Domain.Entities
{
    public class RolPermiso
    {
        public Rol Rol { get; private set; }
        public string RolId { get; private set; }

        public Permiso Permiso { get; private set; }
        public string PermisoId { get; private set; }

        public RolPermiso(string permisoId)
        {
            PermisoId = permisoId;
        }
    }
}