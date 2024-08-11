using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using DrawCurve.Core.Window;
using DrawCurve.Tags;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Actions
{
    internal class ZoomAction : ActionBase
    {
        private float scale = 1.0f;
        private bool zoomIn = true;

        public override Vertex[] Action(Vector2f center, Vertex[] array, float deltaTime, Render context)
        {
            if (!context.RenderConfig.Tags.Contains(TagRender.Zoom))
                return array;


            // Рассчитываем проценты от общего времени
            float progress = MathHelper.Progress(context.CountFrame, context.RenderConfig.FPS, context.RenderConfig.Time);

            // Если время больше 30% и меньше 60% - зумим внутрь
            if (progress >= config.Start && progress < config.End)
            {
                zoomIn = true;
                scale += deltaTime * config.Step;
            }
            // Если время больше 60% - возвращаемся к исходному положению
            else if (progress >= config.End)
            {
                zoomIn = false;
                scale -= deltaTime * config.Step;
            }

            // Устанавливаем пределы масштаба
            if (zoomIn && scale > config.MaxValue) // Макс. зум
            {
                scale = config.MaxValue;
            }
            else if (!zoomIn && scale < config.MinValue) // Возврат к исходному состоянию
            {
                scale = config.MinValue;
            }

            // Применяем масштаб к точкам
            for (int i = 0; i < array.Length; i++)
            {
                var direction = array[i].Position - center;
                direction *= scale;
                array[i].Position = center + direction;
            }

            return array;
        }

        public override ActionConfig GetDefaultConfig()
        {
            return new ActionConfig("ZoomAction", "Приближение", "Приближение к N точке")
            {
                Step = 0.3f,

                Start = 0.2f,
                End = 0.55f,

                MaxValue = 15.0f,
                MinValue = 1.0f,
            };
        }
    }
}
