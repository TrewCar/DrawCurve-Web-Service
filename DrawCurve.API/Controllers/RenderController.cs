using DrawCurve.API.Controllers.Responces;
using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenderController : ControllerBase
    {
        [HttpPost]
        [Route("{RenderName}")]
        public void StartRender(string RenderName, ResponceRenderInfo render)
        {

        }
    }
}
