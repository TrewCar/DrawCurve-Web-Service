using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Требует авторизацию для всех действий в этом контроллере
    public class UserController : ControllerBase
    {
        private readonly JwtManager menedgerSession;
        private readonly IUserService userService;

        public UserController(JwtManager sessionManager, IUserService userService)
        {
            this.menedgerSession = sessionManager;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<User> Get()
        {
            var res = menedgerSession.GetUserSession(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (res == null)
            {
                return Unauthorized(); // Возвращает 401 Unauthorized если пользователь не авторизован
            }

            return Ok(userService.GetUser(res.Id));
        }
    }
}
