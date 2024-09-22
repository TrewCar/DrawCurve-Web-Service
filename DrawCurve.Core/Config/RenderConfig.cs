using DrawCurve.Core.Objects;
using DrawCurve.Core.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFML.Graphics;
using SFML.Window;
using System.Runtime.Serialization;

namespace DrawCurve.Core.Config
{
    public class RenderConfig
    {
        public string Title;

        public List<TagRender> Tags;

        public uint FPS;
        public float Time;

        public string PathMusic = string.Empty;

        private int indexSmooth;
        public int IndexSmooth { 
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

        public float SpeedRender;

        public uint Width;
        public uint Height;

        public List<ActionConfig> ActionsConfig;
        public Dictionary<string, Color> Colors;


    }
}
