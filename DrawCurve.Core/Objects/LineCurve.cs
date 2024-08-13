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

        public float radian;

        public float Length;
        /// <summary>
        /// Radian Pes Second
        /// </summary>
        public float RPS;

        /// <param name="Lenght">Длина отрезка</param>
        /// <param name="Angle">Стартовый угл</param>
        /// <param name="RPS">Скорость врашения в радианах в секунду</param>
        public LineCurve(float Lenght, float Angle = 0, float RPS = 3)
        {
            radian = Angle * MathF.PI / 180f;

            Length = Lenght;
            this.Angle = Angle;
            this.RPS = RPS;
        }

        public LineCurve(double Lenght, double Ranidan, double RPS)
        {
            radian = (float)Ranidan;


            Length = (float)Lenght;
            this.RPS = (float)(RPS * 360.0f * Math.PI / 180d);
        }

        public LineCurve() { }

        public override void Update(float deltaTime)
        {
            radian += RPS * deltaTime;
        }

        public Vector2f GetPoint(Vector2f pos)
        {
            return new Vector2f(
                Length * MathF.Cos(radian) + pos.X,
                Length * MathF.Sin(radian) + pos.Y
                );
        }
    }
}
