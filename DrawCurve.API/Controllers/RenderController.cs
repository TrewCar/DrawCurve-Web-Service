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

        [HttpGet]
        [Route("get/all/self")]
        public List<RenderInfo> GetRender()
        {
            if(Session.GetUserSession() != null)
                return Queue.GetRenderList(Session.GetUserSession());

            this.Response.StatusCode = 401;
            return new List<RenderInfo>();
        }

        [HttpGet]
        [Route("get/{RenderKey}")]
        public RenderInfo GetRender(string RenderKey)
        {
            if (Session.GetUserSession() != null)
                return Queue.GetRender(RenderKey);

            this.Response.StatusCode = 401;
            return new RenderInfo();
        }

        [HttpPost]
        [Route("generate/{RenderType}")]
        public string StartRender(RenderType type, ResponceRenderInfo render)
        {
            if (Session.GetUserSession() == null)
            {
                this.Response.StatusCode = 401;
                return "Not faund session";
            }

            string key = Guid.NewGuid().ToString();
            RenderInfo info = new RenderInfo()
            {
                KEY = key,
                AuthorId = this.Session.GetUserSession().Id,
                Type = type,
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
