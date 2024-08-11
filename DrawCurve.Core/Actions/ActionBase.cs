using DrawCurve.Core.Config;
using DrawCurve.Core.Window;
using SFML.Graphics;
using SFML.System;

namespace DrawCurve.Core.Actions
{
    public abstract class ActionBase
    {
        protected ActionConfig config;
        public abstract Vertex[] Action(Vector2f center, Vertex[] array, float deltaTime, Render context);
        public virtual void SetConfig(RenderConfig cnf)
        {
            foreach (var item in cnf.ActionsConfig)
            {
                if (GetDefaultConfig().Key == item.Key)
                {
                    config = item;
                    break;
                }
            }
        }
        public abstract ActionConfig GetDefaultConfig();

        
    }
}
