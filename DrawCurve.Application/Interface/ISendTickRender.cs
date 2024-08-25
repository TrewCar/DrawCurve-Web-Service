using DrawCurve.Domen.Core.Menedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Interface
{
    public interface ISendTickRender
    {
        public Task SendTick(int AuthorId, RenderTick tick);
    }
}
