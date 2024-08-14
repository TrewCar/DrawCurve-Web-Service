using DrawCurve.Domen.Models.Enums;

namespace DrawCurve.Domen.Models
{
    public class RenderConfig
    {
        public string Title { get; set; }

        public List<TagRender> Tags { get; set; }

        public uint FPS { get; set; }
        public float Time { get; set; }

        public float SpeedRender { get; set; }

        public TypeDeltaTime DeltaTime { get; set; }

        public uint Width { get; set; }
        public uint Height { get; set; }

        public List<ActionConfig> ActionsConfig { get; set; }
        public Dictionary<string, Color> Colors { get; set; }
    }
}
