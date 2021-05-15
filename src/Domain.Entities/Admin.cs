namespace Domain.Entities
{
    public class Admin : Usuario
    {
        protected Admin() { }

        public Admin(string nombre, string email, string password)
            : base(nombre, email, password)
        { }
    }
}