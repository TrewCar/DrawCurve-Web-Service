using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Config
{
    public struct ActionConfig
    {
        public ActionConfig(string Key, string Name, string Description) 
        { 
            this.Key = Key;
            this.Name = Name;
            this.Description = Description;
        }
        public readonly string Key;

        public readonly string Name;
        public readonly string Description;

        public float Step;

        public float Start;
        public float End;

        public float MaxValue;
        public float MinValue;
    }
}
