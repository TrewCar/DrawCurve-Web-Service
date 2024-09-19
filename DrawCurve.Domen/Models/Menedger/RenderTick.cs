namespace DrawCurve.Domen.Models.Menedger
{
    public class RenderTick
    {
        public string KeyRender { get; set; }
        public float FPS { get; set; }
        public float CountFPS { get; set; }
        public float MaxCountFPS { get; set; }
        public TypeStatus Status { get; set; }
        public string Error { get; set; }
    }
}
