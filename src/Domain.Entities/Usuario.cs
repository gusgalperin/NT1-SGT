using System;

namespace Domain.Entities
{
    public abstract class Usuario : Entity<Guid>
    {
        public string Nombre { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public string RolId { get; private set; }
        public Rol Rol { get; private set; }

        protected Usuario() { }

        public Usuario(Rol rol, string nombre, string email, string password)
            : base (Guid.NewGuid())
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException($"'{nameof(nombre)}' cannot be null or empty.", nameof(nombre));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"'{nameof(email)}' cannot be null or empty.", nameof(email));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));
            }

            Rol = rol;
            Nombre = nombre;
            Email = email.ToLower();
            Password = password;
        }
    }
}