using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Objects
{
    public class LineCurve : ObjectRender
    {
        public float Angle;

        public float Radian;

        public float Length;
        /// <summary>
        /// Radian Pes Second
        /// </summary>
        public float RPS;

        /// <param name="Lenght">Длина отрезка</param>
        /// <param name="Angle">Стартовый угл</param>
        /// <param name="RPS">Скорость врашения в радианах в секунду</param>
        public LineCurve(float Lenght, float Angle, float RPS)
        {
            Radian = Angle * MathF.PI / 180f;

            Length = Lenght;
            this.Angle = Angle;
            this.RPS = RPS;
        }

        public LineCurve() { }

        public override void Update(float deltaTime)
        {
            Radian += RPS * deltaTime;
        }

        public Vector2f GetPoint(Vector2f pos)
        {
            return new Vector2f(
                Length * MathF.Cos(Radian) + pos.X,
                Length * MathF.Sin(Radian) + pos.Y
                );
        }
    }
}
