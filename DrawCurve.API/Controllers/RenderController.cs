using DrawCurve.Domen.Responces;
using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using DrawCurve.Application.Utils;
using Microsoft.AspNetCore.Authorization;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RenderController : ControllerBase
    {
        private IRenderService Queue { get; set; }

        public RenderController(IRenderService queue)
        {
            this.Queue = queue;
        }

        [HttpGet("all")]
        public List<RenderInfo> GetRender()
        {
            var user = ControllerContext.HttpContext.User;

            return Queue.GetRenderList(int.Parse(user.FindFirst("Id").Value)) ?? new List<RenderInfo>();
        }

        [HttpGet("{RenderKey}")]
        public RenderInfo GetRender(string RenderKey)
        {
            return Queue.GetRender(RenderKey) ?? new RenderInfo();
        }

        [HttpPost("{RenderType}/Generate")]
        public string StartRender(RenderType RenderType, ResponceRenderInfo render)
        {
            var user = ControllerContext.HttpContext.User;

            string key = Guid.NewGuid().ToString();
            RenderInfo info = new RenderInfo()
            {
                KEY = key,
                AuthorId = int.Parse(user.FindFirst("Id").Value),
                Type = RenderType,
                Status = TypeStatus.ProccessInQueue,
                Name = render.Name,
                Objects = render.obejcts,
                RenderConfig = render.config,
                DateCreate = DateTime.Now
            };
            Queue.Queue(info);

            return key;
        }
    }
}
