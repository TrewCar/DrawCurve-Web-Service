using DrawCurve.API.Menedgers;
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

        public UserController(JwtManager sessionManager)
        {
            this.menedgerSession = sessionManager;
        }

        [HttpGet]
        [Route("Info")]
        public ActionResult<User> GetInfo()
        {
            var res = menedgerSession.GetUserSession(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());

            if (res == null)
            {
                return Unauthorized(); // Возвращает 401 Unauthorized если пользователь не авторизован
            }

            return Ok(res);
        }
    }
}
