using DrawCurve.Domen.Models.Core;
using DrawCurve.Domen.Models.Core.Objects;

namespace DrawCurve.API
{
    public class ResponceRenderInfo
    {
        public List<ObjectRender> obejcts {  get; set; }
        public RenderConfig config { get; set; }
    }
}
