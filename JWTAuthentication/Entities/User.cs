namespace JWTAuthentication.Entities
{
    public class User
    {
        public int Id;

        public string FirstName =string.Empty; 

        public string LastName =string.Empty;

        public string UserName = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty; 
        public string Contact { get; set; } = string.Empty;

        public bool isActive;
    }
}
