using DrawCurve.Core.Actions;
using DrawCurve.Core.Config;
using DrawCurve.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Window
{
    public class LissajousRender : Render
    {
        public override List<ActionBase> active { get; set; } = new();
        public LissajousRender() : base() { }

        public LissajousRender(RenderConfig config, List<ObjectRender> Objects) : base(config, Objects)
        {
        }
        public override bool TickRender(float deltaTime)
        {
            throw new NotImplementedException();
        }
        public override RenderConfig GetDefaultRenderConfig()
        {
            throw new NotImplementedException();
        }
    }
}
