using DrawCurve.Core.Config;
using DrawCurve.Core.Helpers;
using DrawCurve.Core.Window;
using DrawCurve.Tags;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Actions
{
    internal class FollowAction : ActionBase
    {
        private Vector2f? posToPos = null;

        public override Vertex[] Action(Vector2f pos, Vertex[] array, float deltaTime, Render context)
        {
            if (!context.RenderConfig.Tags.Contains(TagRender.Follow))
                return array;

            float progress = MathHelper.Progress(context.CountFrame, context.RenderConfig.FPS, context.RenderConfig.Time);

            var center = new Vector2f();

            if (progress <= config.Start)
            {
                pos = new Vector2f(context.window.Size.X / 2, context.window.Size.Y / 2);
            }
            else if (progress > config.Start && progress < config.End)
            {
                if (posToPos == null)
                    posToPos = new Vector2f(context.window.Size.X / 2, context.window.Size.Y / 2);

                center = pos;
            }
            else if (context.RenderConfig.Tags.Contains(TagRender.Unfollow) && progress >= config.End)
            {
                if (posToPos == null)
                    posToPos = pos;

                center = new Vector2f(context.window.Size.X / 2, context.window.Size.Y / 2);
            }

            if (posToPos != null)
            {
                Vector2f direction = center - (Vector2f)posToPos;
                Vector2f newC = (Vector2f)posToPos + direction * config.Step * deltaTime;
                // Проверка, не прошли ли мы точку A
                if (MathHelper.Dot(center - (Vector2f)posToPos, center - newC) <= 0)
                {
                    newC = center; // Устанавливаем точку C точно в точку A
                }

                // Обновляем положение точки C
                posToPos = newC;
                pos = (Vector2f)posToPos;
            }

            Vector2f offset = new Vector2f();

            offset.X = context.window.Size.X / 2 - pos.X;
            offset.Y = context.window.Size.Y / 2 - pos.Y;

            for (int i = 0; i < array.Length; i++)
            {
                array[i].Position.X += offset.X;
                array[i].Position.Y += offset.Y;
            }

            return array;
        }

        public override ActionConfig GetDefaultConfig()
        {
            return new ActionConfig("FollowAction", "Слежение за обьектом", "Центрирование массива точек относительно N точки")
            {
                Step = 0.3f,

                Start = 0.1f,
                End = 0.7f
            };
        }
    }
}
