﻿using System;
using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Silverlight
{
    internal static class ConverterExtensions
    {
        public static OxyColor ToOxyColor(this System.Windows.Media.Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ToColor(this OxyColor c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static Point ToPoint(this ScreenPoint pt, bool aliased)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
                return new Point(0.5+(int)pt.X,0.5+(int)pt.Y);
            return new Point(pt.X, pt.Y);
        }

        public static ScreenPoint ToScreenPoint(this Point pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }

        public static Brush ToBrush(this OxyColor c)
        {
            return new SolidColorBrush(c.ToColor());
        }

        public static Rect ToRect(this OxyRect r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        public static double DistanceTo(this Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx*dx + dy*dy);
        }
    }
}