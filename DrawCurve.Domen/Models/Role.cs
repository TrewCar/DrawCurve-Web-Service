using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DrawCurve.Domen.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        [EnumMember(Value = "ADMIN")]
        Admin = 1,
        [EnumMember(Value = "USER")]
        User = 2
    }
}
