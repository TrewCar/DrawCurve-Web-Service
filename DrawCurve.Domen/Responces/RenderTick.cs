using DrawCurve.Domen.Models.Menedger;

namespace DrawCurve.Domen.Responces
{
    public class RenderTick
    {
        public string KeyRender { get; set; }
        public float FPS { get; set; }
        public float CountFPS { get; set; }
        public float MaxCountFPS { get; set; }
        public TypeStatus Status { get; set; }
    }
}
