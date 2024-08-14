using ColorCore = SFML.Graphics.Color;
using ColorModel = DrawCurve.Domen.Models.Core.Color;

namespace DrawCurve.Domen.DTO.Models
{
    public static class ColorDTO
    {
        public static ColorCore Transfer(this ColorModel cnf)
            => new ColorCore()
            {
                R = cnf.R,
                G = cnf.G,
                B = cnf.B,
                A = cnf.A,
            };
        public static ColorModel Transfer(this ColorCore cnf)
            => new ColorModel()
            {
                R = cnf.R,
                G = cnf.G,
                B = cnf.B,
                A = cnf.A,
            };
    }
}
