using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DrawCurve.Tags
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TagRender
    {
        [EnumMember(Value = "MILLION")]
        Million,
        [EnumMember(Value = "ZOOM")]
        Zoom,
        [EnumMember(Value = "FOLLOW")]
        Follow,
        [EnumMember(Value = "SPEED")]
        Speed,
        [EnumMember(Value = "UNFOLLOW")]
        Unfollow,
        [EnumMember(Value = "RENDER_HAND")]
        RenderHand,
        [EnumMember(Value = "RENDER_CURVE")]
        RenderCurve
    }
}
