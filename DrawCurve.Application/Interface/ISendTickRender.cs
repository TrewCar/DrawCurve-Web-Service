﻿using DrawCurve.Domen.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Interface
{
    public interface ISendTickRender
    {
        public void SendTick(int AuthorId, RenderTick tick);
    }
}
