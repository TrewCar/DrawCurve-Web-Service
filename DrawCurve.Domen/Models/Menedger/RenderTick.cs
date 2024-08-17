using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Domen.Core.Menedger.Models
{
    public class RenderTick
    {
        public int IdRender {  get; set; }
        public float FPS { get; set; }
        public float CountFPS { get; set; }
        public float MaxCountFPS { get; set; }
        public TypeStatus Status { get; set; }
        public string Error { get; set; }

    }
}
