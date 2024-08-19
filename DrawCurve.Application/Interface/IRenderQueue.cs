using DrawCurve.Domen.Core.Menedger.Models;
using DrawCurve.Domen.Models;

namespace DrawCurve.Application.Interface
{
    public interface IRenderQueue
    {
        public RenderInfo? GetRender(string Key);
        public List<RenderInfo> GetQueue();
        public List<RenderInfo> GetBroken();
        public void UpdateState(RenderInfo render, TypeStatus status);
    }
}
