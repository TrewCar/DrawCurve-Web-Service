using SvgCurveCore = DrawCurve.Core.Objects.SvgCurve;
using SvgCurveModel = DrawCurve.Domen.Models.Core.Objects.SvgCurve;

namespace DrawCurve.Domen.DTO.Models.Objects
{
    public static class SvgCurveDTO
    {
        public static SvgCurveCore Transfer(this SvgCurveModel cnf)
            => new SvgCurveCore()
            {
                SvgValue = cnf.SvgValue,
            };
        public static SvgCurveModel Transfer(this SvgCurveCore cnf)
            => new SvgCurveModel()
            {
                SvgValue = cnf.SvgValue,
            };
    }
}
