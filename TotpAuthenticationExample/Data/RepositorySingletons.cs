namespace TotpAuthenticationExample.Data
{
    public static class RepositorySingletons
    {
        public static UserRepository Users { get; private set; }
        public static SessionRepository Sessions { get; private set; }

        public static void Initialize()
        {
            Users = new UserRepository();
            Sessions = new SessionRepository();
        }
    }
}