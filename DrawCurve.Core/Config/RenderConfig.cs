using DrawCurve.Core.Objects;
using DrawCurve.Tags;
using SFML.Graphics;
using SFML.Window;
using System.Runtime.Serialization;

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

        public VideoMode VideoMode;

        public List<ActionConfig> ActionsConfig;
        public Dictionary<string, Color> Colors;

        public List<ObjectRender> Objects;
    }

    public enum TypeDeltaTime
    {
        [EnumMember(Value = "FIXED")]
        Fixed = 0,
        [EnumMember(Value = "DYMANIC")]
        Dynamic = 1,
    }
}
