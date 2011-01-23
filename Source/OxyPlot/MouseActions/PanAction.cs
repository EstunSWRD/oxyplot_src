﻿namespace OxyPlot
{
    public class PanAction : MouseAction
    {
        public PanAction(IPlot pc)
            : base(pc)
        {
        }

        private IAxis xaxis;
        private IAxis yaxis;
        private ScreenPoint ppt;
        private bool isPanning;

        public override void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift)
        {
            pc.GetAxesFromPoint(pt, out xaxis, out yaxis);

            //if (button == OxyMouseButton.XButton1 || button == OxyMouseButton.XButton2)
            //{
            //    double dx = xaxis != null ? (xaxis.ActualMaximum - xaxis.ActualMinimum) * 0.05 : 0;
            //    double dy = yaxis != null ? (yaxis.ActualMaximum - yaxis.ActualMinimum) * 0.05 : 0;
            //    if (button == OxyMouseButton.XButton1)
            //    {
            //        dx *= -1;
            //        dy *= -1;
            //    }
            //    if (xaxis != null)
            //        pc.Pan(xaxis, -dx);
            //    if (yaxis != null)
            //        pc.Pan(yaxis, -dy);
            //    pc.Refresh();
            //    return;
            //}

            if (button != OxyMouseButton.Right || control)
                return;

            // Right button double click resets the axis
            if (clickCount == 2)
            {
                if (xaxis != null) xaxis.Reset();
                if (yaxis != null) yaxis.Reset();
                pc.Refresh();
                return;
            }

            ppt = pt;
            //   pc.Cursor = Cursors.SizeAll;
            isPanning = true;
        }

        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift)
        {
            if (!isPanning)
                return;
            var previousPoint = AxisBase.InverseTransform(ppt, xaxis, yaxis);
            var currentPoint = AxisBase.InverseTransform(pt, xaxis, yaxis);
            double dx = currentPoint.X - previousPoint.X;
            double dy = currentPoint.Y - previousPoint.Y;
            if (xaxis != null)
                pc.Pan(xaxis, -dx);
            if (yaxis != null)
                pc.Pan(yaxis, -dy);
            pc.Refresh();
            ppt = pt;
        }

        public override void OnMouseUp()
        {
            if (!isPanning)
                return;

            //   pc.Cursor = Cursors.Arrow;
            isPanning = false;
        }
    }
}