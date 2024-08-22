using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DrawCurve.Domen.Core.Menedger.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeStatus
    {
        [EnumMember(Value = "PROCCESS_RENDER_FRAME")]       ProccessRenderFrame = 0,
        [EnumMember(Value = "PROCCESS_RENDER_FRAME_END")]   ProccessRenderFrameEnd = 1,
        [EnumMember(Value = "PROCCESS_CONCAT_FRAME")]       ProccessConcatFrame = 2,
        [EnumMember(Value = "PROCCESS_CONCAT_FRAME_END")]   ProccessConcatFrameEnd = 3,
        [EnumMember(Value = "PROCCESS_CONCAT_AUDIO")]       ProccessConcatAudio = 4,
        [EnumMember(Value = "PROCCESS_END")]                ProccessEnd         = 5,
        [EnumMember(Value = "PROCCESS_ERROR")]              Error               = 6,
        [EnumMember(Value = "PROCCESS_IN_QUEUE")]           ProccessInQueue     = 7,
    }
}
