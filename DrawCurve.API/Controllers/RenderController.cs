using DrawCurve.API.Controllers.Responces;
using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenderController : ControllerBase
    {
        private MenedgerSession Session { get; set; }
        private IRenderService Queue { get; set; }

        public RenderController(IRenderService queue, MenedgerSession session)
        {
            this.Session = session;
            this.Queue = queue;
        }

        [HttpPost]
        [Route("{RenderType}/{RenderName}")]
        public string StartRender(RenderType type, string RenderName, ResponceRenderInfo render)
        {
            string key = Guid.NewGuid().ToString();
            RenderInfo info = new RenderInfo()
            {
                KEY = key,
                AuthorId = this.Session.GetUserSession().Id,
                Type = type,
                Status = TypeStatus.ProccessInQueue,
                Name = RenderName,
                Objects = render.obejcts,
                RenderConfig = render.config,
                DateCreate = DateTime.Now
            };
            Queue.Queue(info);

            return key;
        }

        [HttpGet]
        [Route("{RenderKey}")]
        public RenderInfo GetRender(string RenderKey)
        {
            return Queue.GetRender(RenderKey);
        }
    }
}
