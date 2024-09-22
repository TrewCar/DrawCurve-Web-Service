using DrawCurve.Domen.Models.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Markup;

namespace DrawCurve.Domen.Models.Core
{
    public class RenderConfig
    {
        public string Title { get; set; }

        public List<TagRender> Tags { get; set; }
        public int FPS { get; set; }
        public float Time { get; set; }
        public string PathMusic { get; set; } = string.Empty;

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
        public int Width { get; set; }
        public int Height { get; set; }

        public List<ActionConfig> ActionsConfig { get; set; }

        public Dictionary<string, Color> Colors { get; set; }
    }

}
