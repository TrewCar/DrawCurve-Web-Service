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
        [MinLength(1)]
        public int FPS { get; set; }
        [MinLength(1)]
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
        [MinLength(100)]
        public int Width { get; set; }
        [MinLength(100)]
        public int Height { get; set; }

        public List<ActionConfig> ActionsConfig { get; set; }

        public Dictionary<string, Color> Colors { get; set; }
    }

}
