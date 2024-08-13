using DrawCurve.Core.Objects;
using DrawCurve.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFML.Graphics;
using SFML.Window;
using System.Runtime.Serialization;

namespace DrawCurve.Core.Config
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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeDeltaTime
    {
        [EnumMember(Value = "FIXED")]
        Fixed = 0,
        [EnumMember(Value = "DYMANIC")]
        Dynamic = 1,
    }
}
