namespace Domain.Core.Security
{
    public interface IAuthenticatedUser
    {
        UserInfo UserInfo { get; set; }
    }

    public class AuthenticatedUser : IAuthenticatedUser
    {
        public UserInfo UserInfo { get; set; }
    }
}