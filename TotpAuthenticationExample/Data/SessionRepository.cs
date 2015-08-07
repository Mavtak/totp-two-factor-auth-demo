using System.Collections.Generic;
using System.Linq;

namespace TotpAuthenticationExample.Data
{
    public class SessionRepository
    {
        private readonly List<Session> _sessions;

        public SessionRepository()
        {
            _sessions = new List<Session>();
        }

        public void Add(Session session)
        {
            _sessions.Add(session);
        }

        public Session Get(string id)
        {
            return _sessions.FirstOrDefault(x => x.Id == id && !x.Expired);
        }
    }
}