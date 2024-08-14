using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Menedger.Models
{
    public enum TypeStatus
    {
        ProccessRenderFrame,
        ProccessConcatFrame,
        ProccessConcatAudio,
        ProccessEnd,

        Error
    }
}
