using System.Collections.Generic;
using System.Linq;

namespace TotpAuthenticationExample.Data
{
    public class UserRepository
    {
        private List<User> _users;

        public UserRepository()
        {
            _users = new List<User>();
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public User Get(string username)
        {
            return _users.FirstOrDefault(x => x.Name == username);
        }
    }
}