using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DrawCurve.Domen.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeDeltaTime
    {
        [EnumMember(Value = "FIXED")]
        Fixed = 0,
        [EnumMember(Value = "DYMANIC")]
        Dynamic = 1,
    }
}
