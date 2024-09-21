using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Mvc;
using DrawCurve.Domen.Responces;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IConfiguration configuration;
        private readonly JwtManager jwtManager;

        public LoginController(ILoginService loginService, IConfiguration configuration, JwtManager jwtManager)
        {
            this.loginService = loginService;
            this.configuration = configuration;
            this.jwtManager = jwtManager;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserResource res)
        {
            var user = loginService.Login(res.Login, res.Password);

            if (user != null)
            {
                var token = jwtManager.GenerateJwtToken(user);
                return Ok(new LoginResponse { Token = token });
            }

            return BadRequest(new LoginResponse { Message = "Invalid username or password" });
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // С удалением сессии можно больше не заниматься, т.к. мы теперь используем JWT
            return Ok(new LoginResponse { Message = "Logout successful" });
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(ResponceRegistration userResponse)
        {
            if (userResponse == null || string.IsNullOrWhiteSpace(userResponse.Login) || string.IsNullOrWhiteSpace(userResponse.Password))
            {
                return BadRequest(new { Message = "Invalid registration data" });
            }
            User user = new User()
            {
                Name = userResponse.Name,
                Role = Role.User,
                DateCreate = DateTime.Now,
            };

            UserLogin userLogin = new UserLogin()
            {
                Login = userResponse.Login,
                Email = userResponse.Email,
                Password = userResponse.Password,
            };

            var res = loginService.RegIn(ref user, userLogin);

            if (string.IsNullOrEmpty(res))
            {
                // Регистрация успешна, возвращаем сообщение
                return Ok(new LoginResponse { Message = "Registration successful" });
            }

            return BadRequest(new LoginResponse { Message = res });
        }
    }
}
