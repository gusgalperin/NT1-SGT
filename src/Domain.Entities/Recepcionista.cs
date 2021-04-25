namespace Domain.Entities
{
    public class Recepcionista : Usuario
    {
        public Recepcionista(string nombre, string email, string password) 
            : base(nombre, email, password)
        { }
    }
}