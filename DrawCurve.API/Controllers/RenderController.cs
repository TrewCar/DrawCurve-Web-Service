using DrawCurve.Domen.Responces;
using DrawCurve.API.Menedgers;
using DrawCurve.Application.Interface;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DrawCurve.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenderController : ControllerBase
    {
        private JwtManager Session { get; set; }
        private IRenderService Queue { get; set; }

        public RenderController(IRenderService queue, JwtManager session)
        {
            this.Session = session;
            this.Queue = queue;
        }

        [HttpGet]
        [Route("get/all/self")]
        public List<RenderInfo> GetRender()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (Session.GetUserSession(token) != null)
                return Queue.GetRenderList(Session.GetUserSession(token));

            this.Response.StatusCode = 401;
            return new List<RenderInfo>();
        }

        [HttpGet]
        [Route("get/{RenderKey}")]
        public RenderInfo GetRender(string RenderKey)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (Session.GetUserSession(token) != null)
                return Queue.GetRender(RenderKey);

            this.Response.StatusCode = 401;
            return new RenderInfo();
        }

        [HttpPost]
        [Route("generate/{RenderType}")]
        public string StartRender(RenderType RenderType, ResponceRenderInfo render)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (Session.GetUserSession(token) == null)
            {
                this.Response.StatusCode = 401;
                return "Not faund session";
            }

            string key = Guid.NewGuid().ToString();
            RenderInfo info = new RenderInfo()
            {
                KEY = key,
                AuthorId = this.Session.GetUserSession(token).Id,
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
