using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DrawCurve.Domen.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RenderType
    {
        [EnumMember(Value = "RENDER_CURVE")] RenderCurve = 0,
    }
}
