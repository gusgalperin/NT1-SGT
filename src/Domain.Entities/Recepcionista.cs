namespace Domain.Entities
{
    public class Recepcionista : Usuario
    {
        protected Recepcionista() { }

        public Recepcionista(string nombre, string email, string password) 
            : base(nombre, email, password)
        { }
    }
}