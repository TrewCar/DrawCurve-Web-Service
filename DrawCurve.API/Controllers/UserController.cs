using DrawCurve.API.Menedgers;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MenedgerSession menedgerSession;

        public UserController(MenedgerSession sessionManager)
        {
            this.menedgerSession = sessionManager;
        }
        [HttpGet]
        [Route("Info")]
        public ActionResult<User> GetInfo()
        {
            var res = menedgerSession.GetUserSession();

            if (res == null)
            {
                return BadRequest();
            }

            return Ok(res);
        }
    }
}
