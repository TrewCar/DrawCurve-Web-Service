using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using DrawCurve.Infrastructure;

namespace DrawCurve.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly DrawCurveDbContext context;

        public LoginService(DrawCurveDbContext context)
        {
            this.context = context;
        }

        public User? Login(string username, string password)
        {
            var result = context.UsersLogin.Where(x => x.Login == username);

            if (!result.Any())
                return null;

            UserLogin user = result.First();

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return context.Users.Where(x => x.Id == user.UserId).First();
            }

            return null;
        }

        public string RegIn(ref User user, UserLogin userLogin)
        {
            var result = context.UsersLogin.Where(x => x.Email == userLogin.Email || x.Login == userLogin.Login);

            if (result.Any())
                return "Данный логин или почта уже занята";

            userLogin.Password = BCrypt.Net.BCrypt.HashPassword(userLogin.Password);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Users.Add(user);
                    context.SaveChanges();

                    userLogin.UserId = user.Id;

                    context.UsersLogin.Add(userLogin);
                    context.SaveChanges();

                    transaction.Commit();

                    return "Регистрация прошла успешно";
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
