namespace Domain.Entities
{
    public class Recepcionista : Usuario
    {
        protected Recepcionista() { }

        public Recepcionista(string nombre, string email, string password) 
            : base(Rol.Recepcionista(), nombre, email, password)
        { }
    }
}