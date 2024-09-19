using DrawCurve.Core.Config;
using DrawCurve.Core.Tags;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Window
{
    public class LisajuFormsRender : Render
    {
        public override bool TickRender(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override RenderConfig GetDefaultRenderConfig()
        {
            return new RenderConfig()
            {
                Title = "Title name",
                Tags = new List<TagRender>()
                {

                },
                FPS = 144,
                Time = 20,

                IndexSmooth = 1,

                SpeedRender = 1,

                Width = 1000,
                Height = 1000,

                ActionsConfig = new(),

                Colors = new()
                {
                    { "background", Color.Black },
                    { "line", Color.Green },
                },
            };
        }
    }
}
