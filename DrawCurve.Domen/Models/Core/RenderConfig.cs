using DrawCurve.Domen.Models.Core.Enums;

namespace DrawCurve.Domen.Models.Core
{
    public class RenderConfig
    {
        public string Title { get; set; }

        public List<TagRender> Tags { get; set; }

        public uint FPS { get; set; }
        public float Time { get; set; }

        public float SpeedRender { get; set; }

        private int indexSmooth;
        public int IndexSmooth
        {
            get => indexSmooth;
            set
            {
                if (value <= 0)
                {
                    indexSmooth = 1;
                    return;
                }
                indexSmooth = value;
            }
        }

        public uint Width { get; set; }
        public uint Height { get; set; }

        public List<ActionConfig> ActionsConfig { get; set; }
        public Dictionary<string, Color> Colors { get; set; }
    }
}
