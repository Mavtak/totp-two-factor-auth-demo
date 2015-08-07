namespace TotpAuthenticationExample.Data
{
    public class User
    {
        public string Name { get; private set; }
        public string Password { get; set; }
        public string TotpSecret { get; set; }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}