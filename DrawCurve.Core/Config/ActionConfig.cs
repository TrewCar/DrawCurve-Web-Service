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
        public string Key { get; set; }

        public string Name { get; set; }
        public string Description;

        public float Step { get; set; }

        public float Start { get; set; }
        public float End { get; set; }

        public float MaxValue { get; set; }
        public float MinValue { get; set; }
    }
}
