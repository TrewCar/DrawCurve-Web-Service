using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using DrawCurve.Core.Window;
using DrawCurve.Core.Tags;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Actions
{
    internal class SpeedAction : ActionBase
    {
        public override Vertex[] Action(Vector2f center, Vertex[] array, float deltaTime, Render context)
        {
            if (!context.RenderConfig.Tags.Contains(TagRender.Speed))
                return array;

            float progress = MathHelper.Progress(context.CountFrame, context.RenderConfig.FPS, context.RenderConfig.Time);

            // Если время больше 30% и меньше 60% - зумим внутрь
            if (progress >= config.Start && progress < config.End)
            {
                context.SpeedRender -= deltaTime * config.Step;
            }
            // Если время больше 60% - возвращаемся к исходному положению
            else if (progress >= config.End)
            {
                context.SpeedRender += deltaTime * config.Step;
            }

            // Устанавливаем пределы масштаба
            if (context.SpeedRender > config.MaxValue) // Макс. зум
            {
                context.SpeedRender = config.MaxValue;
            }
            else if (context.SpeedRender < config.MinValue) // Возврат к исходному состоянию
            {
                context.SpeedRender = config.MinValue;
            }

            return array;
        }

        public override ActionConfig GetDefaultConfig()
        {
            return new ActionConfig()
            {
                Key = "SpeedAction",
                Name = "Замедление",
                Description = "Замедление рендера",

                Step = 0.1f,

                Start = 0.2f,
                End = 0.6f,

                MaxValue = 0.5f,
                MinValue = 0.01f,
            };
        }
    }
}
