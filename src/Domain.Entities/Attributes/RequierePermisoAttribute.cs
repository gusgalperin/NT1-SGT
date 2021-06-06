using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class RequierePermisoAttribute : Attribute
    {
        public string Permiso { get; }

        public RequierePermisoAttribute(string permiso)
        {
            Permiso = permiso;
        }
    }

    public static class RequierePermisoAttributeExtensions
    {
        public static bool EstaHabilitado(this TurnoAccion turnoAccion, IEnumerable<string> permisosDelUsuario)
        {
            var permisoRequerido = turnoAccion.GetAttribute<RequierePermisoAttribute>()?.Permiso ?? string.Empty;

            return string.IsNullOrEmpty(permisoRequerido) || permisosDelUsuario.Contains(permisoRequerido);
        }

        public static string Permiso(this TurnoAccion turnoAccion)
        {
            return turnoAccion.GetAttribute<RequierePermisoAttribute>()?.Permiso;
        }
    }
}