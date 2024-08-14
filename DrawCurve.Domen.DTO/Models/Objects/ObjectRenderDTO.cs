using ObjectRenderCore = DrawCurve.Core.Objects.ObjectRender;
using ObjectRenderModel = DrawCurve.Domen.Models.Core.Objects.ObjectRender;

using LineCurveCore = DrawCurve.Core.Objects.LineCurve;
using LineCurveModel = DrawCurve.Domen.Models.Core.Objects.LineCurve;

namespace DrawCurve.Domen.DTO.Models.Objects
{
    public static class ObjectRenderDTO
    {
        public static ObjectRenderCore Transfer(this ObjectRenderModel cnf)
        {
            if(cnf is LineCurveModel obj)
            {
                return obj.Transfer();
            } 
            else
            {
                throw new NotSupportedException($"Type {cnf.GetType().ToString()} note suported in DTO Model to Core");
            }
        }
        public static ObjectRenderModel Transfer(this ObjectRenderCore cnf)
        {
            if (cnf is LineCurveCore obj)
            {
                return obj.Transfer();
            }
            else
            {
                throw new NotSupportedException($"Type {cnf.GetType().ToString()} note suported in DTO Core to Model");
            }
        }
    }
}
