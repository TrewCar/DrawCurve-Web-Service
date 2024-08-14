using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DrawCurve.Domen.Models.Core.Objects
{
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    [JsonDerivedType(typeof(ObjectRender), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(LineCurve), typeDiscriminator: "LineCurve")]
    public class ObjectRender
    {
    }
}
