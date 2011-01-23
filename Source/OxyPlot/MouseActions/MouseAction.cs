﻿namespace OxyPlot
{
    public abstract class MouseAction : IMouseAction
    {
        protected IPlot pc;

        protected MouseAction(IPlot pc)
        {
            this.pc = pc;
        }

        public virtual void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift)
        {
        }

        public virtual void OnMouseMove(ScreenPoint pt, bool control, bool shift)
        {
        }

        public virtual void OnMouseUp()
        {
        }

        public virtual void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift)
        {
        }
    }
}