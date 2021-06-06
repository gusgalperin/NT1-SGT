using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Rol : Entity<string>
    {
        protected Rol() { }

        public Rol(Options descripcion, IEnumerable<Permiso> permisos = null) 
            : base(descripcion.ToString())
        {
            Descripcion = descripcion;
            Permisos = permisos != null
                ? permisos.Select(x => new RolPermiso(x.Id)).ToList()
                : new List<RolPermiso>();
        }

        public Options Descripcion { get; private set; }

        public ICollection<RolPermiso> Permisos { get; private set; }

        public static Rol Admin(IEnumerable<Permiso> permisos = null) 
            => new Rol(Options.Admin, permisos);
        public static Rol Profesional(IEnumerable<Permiso> permisos = null) 
            => new Rol(Options.Profesional, permisos);
        public static Rol Recepcionista(IEnumerable<Permiso> permisos = null) 
            => new Rol(Options.Recepcionista, permisos);

        public enum Options
        {
            Admin,
            Profesional,
            Recepcionista
        }
    }
}