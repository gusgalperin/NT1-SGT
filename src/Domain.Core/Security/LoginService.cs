using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Domain.Core.Security
{
    public interface ILoginService
    {
        Task<UserInfo> LoginAsync(string email, string password);
    }

    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserInfo> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.LoginAsync(email, password);
                return new UserInfo { Id = user.Id, Nombre = user.Nombre, Rol = user.Rol() };
            }
            catch (EntityNotFoundException)
            {
                throw new IncorrectUserOrPasswordException();
            }
        }
    }
}