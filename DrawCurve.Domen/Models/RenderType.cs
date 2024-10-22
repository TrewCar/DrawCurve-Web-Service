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
        [EnumMember(Value = "LISSAJOUS_FIGURES")] LissajousFigures = 1,
        [EnumMember(Value = "SVG_RENDER_CURVE")] SvgRenderCurve = 2,
    }
}
