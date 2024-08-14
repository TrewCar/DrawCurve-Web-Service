using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Config
{
    public struct ActionConfig
    {
        public string Key;

        public string Name;
        public string Description;

        public float Step;

        public float Start;
        public float End;

        public float MaxValue;
        public float MinValue;
    }
}
