using DrawCurve.API.Controllers.Responces;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IConfiguration configuration;

        public LoginController(ILoginService loginService, IConfiguration configuration)
        {
            this.loginService = loginService;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserResource res)
        {
            var user = loginService.Login(res.Login, res.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return BadRequest(new { Message = "Invalid username or password" });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // С удалением сессии можно больше не заниматься, т.к. мы теперь используем JWT
            return Ok(new { Message = "Logout successful" });
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(ResponceRegistration userResponse)
        {
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
                return Ok(new { Message = "Registration successful" });
            }

            return BadRequest(new { Message = res });
        }
    }

    public class UserResource
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
