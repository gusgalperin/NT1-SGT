using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Core.Security
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public Rol.Options Rol { get; set; }
        public IEnumerable<string> Permisos { get; set; }
    }

    public enum RolType
    {
        Admin,
        Profesional,
        Recepcionista
    }

    public static class UsuarioExtension
    {
        public static RolType Rol(this Usuario usuario)
        {
            return usuario switch
            {
                Admin _ => RolType.Admin,
                Profesional _ => RolType.Profesional,
                Recepcionista _ => RolType.Recepcionista,
                _ => throw new NotImplementedException("falta declarar roltype")
            };
        }
    }
}