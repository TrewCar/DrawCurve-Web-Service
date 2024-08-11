using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Helpers
{
    internal class MathHelper
    {
        public static float Dot(Vector2f v1, Vector2f v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        public static float Progress(float countFrame, float FPS, float Seconds)
        {
            return (countFrame / FPS) / Seconds;
        }
        public static Vector2f Lerp(Vector2f start, Vector2f end, float t)
        {
            t = Math.Clamp(t, 0.0f, 1.0f); // Ограничиваем t от 0 до 1
            return new Vector2f
            {
                X = start.X + (end.X - start.X) * t,
                Y = start.Y + (end.Y - start.Y) * t
            };
        }
        /// <summary>
        /// Ограничивает значение в пределах заданного диапазона.
        /// </summary>
        /// <param name="value">Значение для ограничения.</param>
        /// <param name="min">Минимальное значение диапазона.</param>
        /// <param name="max">Максимальное значение диапазона.</param>
        /// <returns>Ограниченное значение.</returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
