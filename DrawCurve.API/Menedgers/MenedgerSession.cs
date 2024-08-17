using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace DrawCurve.API.Menedgers
{
    public class MenedgerSession
    {
        private readonly ISession _session;

        public MenedgerSession(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void SetUserSession(User user)
        {
            _session.SetString("UserId", user.Id.ToString());
            _session.SetString("Username", user.Name);
            _session.SetInt32("UserRole", (int)user.Role);
        }

        public User? GetUserSession()
        {
            if (_session.TryGetValue("UserId", out var userIdBytes) &&
                _session.TryGetValue("Username", out var usernameBytes))
            {
                int userId = int.Parse(Encoding.UTF8.GetString(userIdBytes));
                string username = Encoding.UTF8.GetString(usernameBytes);
                Role role = (Role)_session.GetInt32("UserRole");
                return new User()
                {
                    Id = userId,
                    Name = username,
                    Role = role,
                };
            }
            return null;
        }

        public void ClearUserSession()
        {
            _session.Clear();
        }
    }
}
