using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Menedger;

namespace DrawCurve.Application.Interface
{
    public interface IRenderQueue
    {
        public RenderInfo? GetRender(string Key);
        public List<RenderInfo> GetQueue(TypeStatus status);
        public void UpdateState(RenderInfo render, TypeStatus status);
    }
}
