﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Silverlight
{
    /// <summary>
    /// Rendering Silverlight shapes to a Canvas
    /// </summary>
    public class SilverlightRenderContext : IRenderContext
    {
        private readonly Canvas canvas;

        public SilverlightRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            Width = canvas.ActualWidth;
            Height = canvas.ActualHeight;
        }

        #region IRenderContext Members

        public double Width { get; private set; }

        public double Height { get; private set; }

        readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            var path = new Path();
            SetStroke(path, stroke, thickness, dashArray, lineJoin, aliased);
            var pg = new PathGeometry();
            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                var figure = new PathFigure();
                figure.StartPoint = points[i].ToPoint();
                figure.IsClosed = false;
                figure.Segments.Add(new LineSegment() { Point = points[i + 1].ToPoint() });
                pg.Figures.Add(figure);
            }
            path.Data = pg;
            Add(path);

            //for (int i = 0; i + 1 < points.Count; i += 2)
            //    DrawLine(new[] { points[i], points[i + 1] }, stroke, thickness, dashArray, lineJoin, aliased);

        }

        private void Add(Shape shape)
        {
            canvas.Children.Add(shape);
        }

        public void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             OxyPenLineJoin lineJoin, bool aliased)
        {
            var e = new Polyline();
            SetStroke(e, stroke, thickness, dashArray, lineJoin, aliased);

            // TODO
            // if (aliased) pl.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            var pc = new PointCollection();
            foreach (var p in points)
                pc.Add(ToPoint(p));
            e.Points = pc;

            canvas.Children.Add(e);
        }


        private Brush GetCachedBrush(OxyColor stroke)
        {
            Brush brush;
            if (!brushCache.TryGetValue(stroke, out brush))
            {
                brush = new SolidColorBrush(stroke.ToColor());
                brushCache.Add(stroke, brush);
            }
            return brush;
        }

        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            var po = new Polygon();
            SetStroke(po, stroke, thickness, dashArray, lineJoin, aliased);

            if (fill != null)
                po.Fill = GetCachedBrush(fill);

            var pc = new PointCollection();
            foreach (var p in points)
                pc.Add(ToPoint(p));
            po.Points = pc;

            canvas.Children.Add(po);
        }

        private void SetStroke(Shape shape, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            if (stroke != null && thickness > 0)
            {
                shape.Stroke = GetCachedBrush(stroke);

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        shape.StrokeLineJoin = PenLineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        shape.StrokeLineJoin = PenLineJoin.Bevel;
                        break;
                    //  The default StrokeLineJoin is Miter
                }

                if (thickness != 1) // default values is 1
                    shape.StrokeThickness = thickness;
                if (dashArray != null)
                {
                    shape.StrokeDashArray = CreateDashArrayCollection(dashArray);
                }
            }
            if (aliased)
            {
                // todo: does not work?
                shape.UseLayoutRounding = true;
            }
        }

        private static DoubleCollection CreateDashArrayCollection(double[] dashArray)
        {
            var dac = new DoubleCollection();
            foreach (var v in dashArray)
                dac.Add(v);
            return dac;
        }

        ///<summary>
        /// Draws a rectangle.
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="width"></param>
        ///<param name="height"></param>
        ///<param name="fill"></param>
        ///<param name="stroke"></param>
        ///<param name="thickness"></param>
        public void DrawRectangle(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            var el = new Rectangle();
            if (stroke != null)
            {
                el.Stroke = new SolidColorBrush(stroke.ToColor());
                el.StrokeThickness = thickness;
            }
            if (fill != null)
            {
                el.Fill = new SolidColorBrush(fill.ToColor());
            }

            el.Width = width;
            el.Height = height;
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canvas.Children.Add(el);
        }

        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            var el = new Ellipse();
            if (stroke != null)
            {
                el.Stroke = new SolidColorBrush(stroke.ToColor());
                el.StrokeThickness = thickness;
            }
            if (fill != null)
            {
                el.Fill = new SolidColorBrush(fill.ToColor());
            }

            el.Width = width;
            el.Height = height;
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canvas.Children.Add(el);
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            var tb = new TextBlock
                         {
                             Text = text,
                             Foreground = new SolidColorBrush(fill.ToColor())
                         };
            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 500)
                tb.FontWeight = FontWeights.Bold;

            tb.Measure(new Size(1000, 1000));

            double dx = 0;
            if (halign == HorizontalTextAlign.Center)
                dx = -tb.ActualWidth / 2;
            if (halign == HorizontalTextAlign.Right)
                dx = -tb.ActualWidth;

            double dy = 0;
            if (valign == VerticalTextAlign.Middle)
                dy = -tb.ActualHeight / 2;
            if (valign == VerticalTextAlign.Bottom)
                dy = -tb.ActualHeight;


            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform { X = dx, Y = dy });
            if (rotate != 0)
                transform.Children.Add(new RotateTransform { Angle = rotate });
            transform.Children.Add(new TranslateTransform { X = p.X, Y = p.Y });
            tb.RenderTransform = transform;

            // tb.SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            canvas.Children.Add(tb);
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            var tb = new TextBlock
                         {
                             Text = text
                         };

            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight >= 0)
                tb.FontWeight = FontWeights.Bold; // FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.ActualWidth, tb.ActualHeight);
        }

        #endregion

        private static Point ToPoint(ScreenPoint point)
        {
            return new Point(point.X, point.Y);
            // return new Point((int)point.X, (int)point.Y);
        }
    }
}