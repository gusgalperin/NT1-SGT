using System.Linq;

namespace Domain.Core.Security
{
    public interface IAuthenticatedUser
    {
        UserInfo UserInfo { get; set; }

        bool Puede(string permiso);

        void ValidarPermiso(string permiso);
    }

    public class AuthenticatedUser : IAuthenticatedUser
    {
        public UserInfo UserInfo { get; set; }

        public bool Puede(string permiso)
        {
            return UserInfo.Permisos.Any() && UserInfo.Permisos.Contains(permiso);
        }

        public void ValidarPermiso(string permiso)
        {
            if (!Puede(permiso))
                throw new NoTienePermisosException();
        }
    }
}