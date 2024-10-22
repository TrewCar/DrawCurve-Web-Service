using DrawCurve.Core.Objects;
using System.Numerics;

namespace DrawCurve.Core.Helpers
{
    public class FourierTransform
    {
        const double TAU = Math.PI * 2;

        public static List<ObjectRender> CalculateArrows(List<Complex> path, int components, double err)
        {
            int N = path.Count;
            List<ObjectRender> arrows = new List<ObjectRender>();

            for (int f = -components / 2; f <= components / 2; f++)
            {
                Complex Cn = NumericIntegrateC(t =>
                {
                    int i = (int)Math.Round(t * (N - 1));
                    return path[i] * Complex.FromPolarCoordinates(1, -f * TAU * t);
                }, 0, 1, err);

                arrows.Add(new LineCurve(Cn.Magnitude, Cn.Phase, f));
            }

            return arrows;
        }

        private static Complex NumericIntegrateC(Func<double, Complex> func, double a, double b, double err)
        {
            Complex sum = Complex.Zero;

            for (double i = 0; i <= 1; i += err)
            {
                double t = a + i;
                sum += func(t) * err;
            }

            return sum;
        }
    }
}
