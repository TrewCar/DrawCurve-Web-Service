using DrawCurve.Tags;
using SFML.Graphics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DrawCurve.Core.Config
{
    public struct RenderConfig
    {
        public string Title;

        public List<TagRender> Tags;

        public uint FPS;
        public float Time;

        public float SpeedRender;

        public TypeDeltaTime DeltaTime;

        public uint Width;
        public uint Height;


        public List<ActionConfig> ActionsConfig;
        public Dictionary<string, Color> Colors;
    }
   
    public enum TypeDeltaTime
    {
        [EnumMember(Value = "FIXED")]
        Fixed = 0,
        [EnumMember(Value = "DYMANIC")]
        Dynamic = 1,
    }
}
