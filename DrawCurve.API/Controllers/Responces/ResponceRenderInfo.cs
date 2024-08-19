using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;

namespace DrawCurve.API.Controllers.Responces
{
    public class ResponceRenderInfo
    {
        public string Name { get; set; }
        public List<ObjectRender> obejcts { get; set; }
        public RenderConfig config { get; set; }
    }
}
