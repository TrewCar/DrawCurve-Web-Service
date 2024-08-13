using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using DrawCurve.Core.Window;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DrawCurve.Web.Server.Controllers.Render
{
    [Route("api/render/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpGet("all")]
        public IEnumerable<string> GetListRenders()
        {
            return new List<string>()
            {
                "RenderCurve"
            };
        }
        // GET api/<Config>/5
        [HttpGet("{name}")]
        public RenderConfig? Get(string name)
        {
            if (name != "RenderCurve")
            {
                return null;
            }

            var ren = new CurveRender();
            var cnf = ren.GetDefaultRenderConfig();
            ren.Dispose();

            return cnf;
        }
        public static Thread thProcess;
        [HttpPost("start/{name}")]
        public string Start(string name, RenderConfig cnf)
        {
            if (thProcess != null)
            {
                return "Not. Collection is not empty";
            }

            thProcess = new Thread(() =>
            {
                var ren = new CurveRender(cnf, new List<Core.Objects.ObjectRender> {
                    new LineCurve(100, 90, 2*MathF.PI),
                    new LineCurve(100, 90, -MathF.PI/15), });

                ren.Start();
                
            });
            thProcess.Start();
            return "Start";
        }
    }
}
