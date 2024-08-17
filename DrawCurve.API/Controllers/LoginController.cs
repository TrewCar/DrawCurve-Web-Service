using DrawCurve.API.Controllers.Responces;
using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Application.Services;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly MenedgerSession menedgerSession;

        public LoginController(ILoginService loginService, MenedgerSession sessionManager)
        {
            this.loginService = loginService;
            this.menedgerSession = sessionManager;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string username, string password)
        {
            var user = loginService.Login(username, password);

            if (user != null)
            {
                menedgerSession.ClearUserSession();
                menedgerSession.SetUserSession(user);

                return Ok(new { Message = "Login successful" });
            }

            return BadRequest(new { Message = "Invalid username or password" });
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // Удаляем информацию о сессии
            menedgerSession.ClearUserSession();
            return Ok(new { Message = "Logout successful" });
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(ResponceRegistration userResponce)
        {
            User user = new User()
            {
                Name = userResponce.Name,
                Role = Role.User,
                DateCreate = DateTime.Now,
            };

            UserLogin userLogin = new UserLogin()
            {
                Login = userResponce.Login,
                Email = userResponce.Email,
                Password = userResponce.Password,
            };

            var res = loginService.RegIn(ref user ,userLogin);

            if (string.IsNullOrEmpty(res))
            {
                menedgerSession.ClearUserSession();
                menedgerSession.SetUserSession(user);

                return Ok(new { Message = "Регистрация успешна" });
            }

            return BadRequest(new { Message = res });
        }
    }

}
