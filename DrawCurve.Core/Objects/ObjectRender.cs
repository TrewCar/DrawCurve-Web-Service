using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Core.Objects
{
    public abstract class ObjectRender
    {
        public ObjectRender() { }
        public abstract void Update(float deltaTime);
    }
}
