using System;

namespace TotpAuthenticationExample.Data
{
    public class Session
    {
        public string Id { get; private set; }
        public User User { get; private set; }
        public bool TotpAuthenticated { get; set; }
        public bool Expired { get; private set; }

        public Session(User user)
        {
            Id = Guid.NewGuid().ToString();
            User = user;
        }

        public bool NeedsToBeTotpAuthenticated
        {
            get
            {
                return !TotpAuthenticated && User.TotpSecret != null;
            }
        }

        public void Expire()
        {
            Expired = true;
        }
    }
}