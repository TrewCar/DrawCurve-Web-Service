using LineCurveCore = DrawCurve.Core.Objects.LineCurve;
using LineCurveModel = DrawCurve.Domen.Models.Core.Objects.LineCurve;

namespace DrawCurve.Domen.DTO.Models.Objects
{
    public static class LineCurveDTO
    {
        public static LineCurveCore Transfer(this LineCurveModel cnf)
            => new LineCurveCore(cnf.Length, cnf.Angle, cnf.RPS);
        public static LineCurveModel Transfer(this LineCurveCore cnf)
            => new LineCurveModel(cnf.Length, cnf.Angle, cnf.RPS);
    }
}
