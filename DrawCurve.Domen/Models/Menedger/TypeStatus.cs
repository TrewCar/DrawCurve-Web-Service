using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DrawCurve.Domen.Core.Menedger.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeStatus
    {
        [EnumMember(Value = "PROCCESS_RENDER_FRAME")]
        ProccessRenderFrame,
        [EnumMember(Value = "PROCCESS_CONCAT_FRAME")]
        ProccessConcatFrame,
        [EnumMember(Value = "PROCCESS_CONCAT_AUDIO")]
        ProccessConcatAudio,
        [EnumMember(Value = "PROCCESS_END")]
        ProccessEnd,
        [EnumMember(Value = "PROCCESS_ERROR")]
        Error
    }
}
