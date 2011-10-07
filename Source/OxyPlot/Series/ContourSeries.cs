//-----------------------------------------------------------------------
// <copyright file="ContourSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a contour series.
    /// </summary>
    /// <remarks>
    /// See <see cref="http://en.wikipedia.org/wiki/Contour_line"/> and <see cref="http://www.mathworks.se/help/techdoc/ref/contour.html"/>.
    /// </remarks>
    public class ContourSeries : DataPointSeries
    {
        #region Constants and Fields

        /// <summary>
        ///   The contour collection.
        /// </summary>
        private List<Contour> contours;

        /// <summary>
        ///   The temporary segment collection.
        /// </summary>
        private List<ContourSegment> segments;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ContourSeries" /> class.
        /// </summary>
        public ContourSeries()
        {
            this.ContourLevelStep = double.NaN;

            this.LabelSpacing = double.NaN;
            this.LabelStep = 1;
            this.LabelFont = null;
            this.LabelFontSize = 12;
            this.LabelFontWeight = FontWeights.Normal;
            this.LabelColor = OxyColors.Black;
            this.LabelBackground = OxyColor.FromAColor(220, OxyColors.White);

            this.Color = null;
            this.StrokeThickness = 1.0;
            this.LineStyle = LineStyle.Solid;

            this.TrackerFormatString = "{1}: {2:0.000}\n{3}: {4:0.000}\nZ: {5:0.000}";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the column coordinates.
        /// </summary>
        /// <value>The column coordinates.</value>
        public double[] ColumnCoordinates { get; set; }

        /// <summary>
        ///   Gets or sets the contour level step size.
        ///   This property is not used if the ContourLevels vector is set.
        /// </summary>
        /// <value>The contour level step size.</value>
        public double ContourLevelStep { get; set; }

        /// <summary>
        ///   Gets or sets the contour levels.
        /// </summary>
        /// <value>The contour levels.</value>
        public double[] ContourLevels { get; set; }

        /// <summary>
        ///   Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public double[,] Data { get; set; }

        /// <summary>
        ///   Gets or sets the text background color.
        /// </summary>
        /// <value>The text background color.</value>
        public OxyColor LabelBackground { get; set; }

        /// <summary>
        ///   Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        ///   Gets or sets the label font.
        /// </summary>
        /// <value>The font.</value>
        public string LabelFont { get; set; }

        /// <summary>
        ///   Gets or sets the size of the font.
        /// </summary>
        public double LabelFontSize { get; set; }

        /// <summary>
        ///   Gets or sets the font weight.
        /// </summary>
        public double LabelFontWeight { get; set; }

        /// <summary>
        ///   Gets or sets the format string for contour values.
        /// </summary>
        /// <value>The format string.</value>
        public string LabelFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the label spacing.
        /// </summary>
        /// <value>The label spacing.</value>
        public double LabelSpacing { get; set; }

        /// <summary>
        ///   Gets or sets the label step (number of contours per label).
        /// </summary>
        /// <value>The label step.</value>
        public int LabelStep { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the row coordinates.
        /// </summary>
        /// <value>The row coordinates.</value>
        public double[] RowCoordinates { get; set; }

        /// <summary>
        ///   Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the contours.
        /// </summary>
        public void CalculateContours()
        {
            if (this.Data == null)
            {
                return;
            }

            this.segments = new List<ContourSegment>();
            Conrec.RendererDelegate renderer =
                (startX, startY, endX, endY, contourLevel) =>
                this.segments.Add(
                    new ContourSegment(new DataPoint(startX, startY), new DataPoint(endX, endY), contourLevel));

            double[] actualContourLevels = this.ContourLevels;

            if (actualContourLevels == null)
            {
                double max = this.Data[0, 0];
                double min = this.Data[0, 0];
                for (int i = 0; i < this.Data.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < this.Data.GetUpperBound(0); j++)
                    {
                        max = Math.Max(max, this.Data[i, j]);
                        min = Math.Min(min, this.Data[i, j]);
                    }
                }

                double actualStep = this.ContourLevelStep;
                if (double.IsNaN(actualStep))
                {
                    double range = max - min;
                    double step = range / 20;
                    actualStep = Math.Pow(10, Math.Floor(step.GetExponent()));
                }

                max = max.ToUpperMultiple(actualStep);
                min = min.ToLowerMultiple(actualStep);
                actualContourLevels = ArrayHelper.CreateVector(min, max, actualStep);
            }

            Conrec.Contour(this.Data, this.ColumnCoordinates, this.RowCoordinates, actualContourLevels, renderer);

            this.JoinContourSegments();
        }

        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// The interpolate.
        /// </param>
        /// <returns>
        /// A hit result object.
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result = null;

            foreach (Contour c in this.contours)
            {
                int index;
                DataPoint dpn;
                ScreenPoint spn;
                if (interpolate)
                {
                    if (this.GetNearestInterpolatedPointInternal(c.Points, point, out dpn, out spn, out index))
                    {
                        if (result == null || result.Position.DistanceToSquared(point) > spn.DistanceToSquared(point))
                        {
                            result = new TrackerHitResult(this, dpn, spn, c.ContourLevel);
                        }
                    }
                }
                else
                {
                    if (this.GetNearestPointInternal(c.Points, point, out dpn, out spn, out index))
                    {
                        if (result == null || result.Position.DistanceToSquared(point) > spn.DistanceToSquared(spn))
                        {
                            result = new TrackerHitResult(this, dpn, spn, c.ContourLevel);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (this.contours == null)
            {
                this.CalculateContours();
            }

            if (this.contours.Count == 0)
            {
                return;
            }

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            OxyRect clippingRect = this.GetClippingRect();

            var contourLabels = new List<ContourLabel>();

            foreach (Contour contour in this.contours)
            {
                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    var pts = new ScreenPoint[contour.Points.Count];
                    {
                        int i = 0;
                        foreach (IDataPoint pt in contour.Points)
                        {
                            pts[i++] = this.XAxis.Transform(pt.X, pt.Y, this.YAxis);
                        }
                    }

                    rc.DrawClippedLine(
                        pts, 
                        clippingRect, 
                        4, 
                        this.Color, 
                        this.StrokeThickness, 
                        this.LineStyle, 
                        OxyPenLineJoin.Miter, 
                        false);

                    // rc.DrawClippedPolygon(pts, clippingRect, 4, model.GetDefaultColor(), OxyColors.Black);
                    if (pts.Length > 10)
                    {
                        this.AddContourLabels(contour, pts, clippingRect, contourLabels);
                    }
                }
            }

            foreach (ContourLabel cl in contourLabels)
            {
                this.RenderLabelBackground(rc, cl);
            }

            foreach (ContourLabel cl in contourLabels)
            {
                this.RenderLabel(rc, cl);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets default values from the plotmodel.
        /// </summary>
        /// <param name="model">
        /// The plot model.
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.Color == null)
            {
                this.LineStyle = model.GetDefaultLineStyle();
                this.Color = model.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the max/min from the datapoints.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            this.MinX = this.ColumnCoordinates.Min();
            this.MaxX = this.ColumnCoordinates.Max();
            this.MinY = this.RowCoordinates.Min();
            this.MaxY = this.RowCoordinates.Max();
        }

        /// <summary>
        /// Determines if two values are close.
        /// </summary>
        /// <param name="x1">
        /// The first value.
        /// </param>
        /// <param name="x2">
        /// The second value.
        /// </param>
        /// <param name="eps">
        /// The squared tolerance.
        /// </param>
        /// <returns>
        /// True if the values are close.
        /// </returns>
        private static bool AreClose(double x1, double x2, double eps = 1e-6)
        {
            double dx = x1 - x2;
            return dx * dx < eps;
        }

        /// <summary>
        /// Determines if two points are close.
        /// </summary>
        /// <param name="p0">
        /// The first point.
        /// </param>
        /// <param name="p1">
        /// The second point.
        /// </param>
        /// <param name="eps">
        /// The squared tolerance.
        /// </param>
        /// <returns>
        /// True if the points are close.
        /// </returns>
        private static bool AreClose(DataPoint p0, DataPoint p1, double eps = 1e-6)
        {
            double dx = p0.X - p1.X;
            double dy = p0.Y - p1.Y;
            return dx * dx + dy * dy < eps;
        }

        /// <summary>
        /// The add contour labels.
        /// </summary>
        /// <param name="contour">
        /// The contour.
        /// </param>
        /// <param name="pts">
        /// The pts.
        /// </param>
        /// <param name="clippingRect">
        /// The clipping rect.
        /// </param>
        /// <param name="contourLabels">
        /// The contour labels.
        /// </param>
        private void AddContourLabels(
            Contour contour, ScreenPoint[] pts, OxyRect clippingRect, List<ContourLabel> contourLabels)
        {
            // todo: support label spacing and label step
            if (pts.Length < 2)
            {
                return;
            }

            // Calculate position and angle of the label
            double i = (pts.Length - 1) * 0.5;
            var i0 = (int)i;
            int i1 = i0 + 1;
            double dx = pts[i1].X - pts[i0].X;
            double dy = pts[i1].Y - pts[i0].Y;
            double x = pts[i0].X + dx * (i - i0);
            double y = pts[i0].Y + dy * (i - i0);
            if (!clippingRect.Contains(x, y))
            {
                return;
            }

            var pos = new ScreenPoint(x, y);
            double angle = Math.Atan2(dy, dx) * 180 / Math.PI;
            if (angle > 90)
            {
                angle -= 180;
            }

            if (angle < -90)
            {
                angle += 180;
            }

            string text = contour.ContourLevel.ToString(this.LabelFormatString, this.ActualCulture);
            contourLabels.Add(new ContourLabel { Position = pos, Angle = angle, Text = text });
        }

        /// <summary>
        /// Finds the connected segment.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="contourLevel">
        /// The contour level.
        /// </param>
        /// <param name="eps">
        /// The eps.
        /// </param>
        /// <param name="reverse">
        /// reverse the segment if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// The connected segment, or null if no segment was found.
        /// </returns>
        private ContourSegment FindConnectedSegment(DataPoint point, double contourLevel, double eps, out bool reverse)
        {
            reverse = false;
            foreach (ContourSegment s in this.segments)
            {
                if (!AreClose(s.ContourLevel, contourLevel, eps))
                {
                    continue;
                }

                if (AreClose(point, s.StartPoint, eps))
                {
                    return s;
                }

                if (AreClose(point, s.EndPoint, eps))
                {
                    reverse = true;
                    return s;
                }
            }

            return null;
        }

        /// <summary>
        /// Joins the contour segments.
        /// </summary>
        /// <param name="eps">
        /// The tolerance for segment ends to connect (squared distance).
        /// </param>
        private void JoinContourSegments(double eps = 1e-10)
        {
            // This is a simple, slow, naïve method - should be improved:
            // http://stackoverflow.com/questions/1436091/joining-unordered-line-segments
            this.contours = new List<Contour>();
            var contourPoints = new List<IDataPoint>();
            int contourPointsCount = 0;

            ContourSegment firstSegment = null;
            int segmentCount = this.segments.Count;
            while (segmentCount > 0)
            {
                ContourSegment segment1 = null, segment2 = null;

                if (firstSegment != null)
                {
                    bool reverse;

                    // Find a segment that is connected to the head of the contour
                    segment1 = this.FindConnectedSegment(
                        (DataPoint)contourPoints[0], firstSegment.ContourLevel, eps, out reverse);
                    if (segment1 != null)
                    {
                        contourPoints.Insert(0, reverse ? segment1.StartPoint : segment1.EndPoint);
                        contourPointsCount++;
                        this.segments.Remove(segment1);
                        segmentCount--;
                    }

                    // Find a segment that is connected to the tail of the contour
                    segment2 = this.FindConnectedSegment(
                        (DataPoint)contourPoints[contourPointsCount - 1], firstSegment.ContourLevel, eps, out reverse);
                    if (segment2 != null)
                    {
                        contourPoints.Add(reverse ? segment2.StartPoint : segment2.EndPoint);
                        contourPointsCount++;
                        this.segments.Remove(segment2);
                        segmentCount--;
                    }
                }

                if ((segment1 == null && segment2 == null) || segmentCount == 0)
                {
                    if (contourPointsCount > 0 && firstSegment != null)
                    {
                        this.contours.Add(new Contour(contourPoints, firstSegment.ContourLevel));
                        contourPoints = new List<IDataPoint>();
                        contourPointsCount = 0;
                    }

                    if (segmentCount > 0)
                    {
                        firstSegment = this.segments.First();
                        contourPoints.Add(firstSegment.StartPoint);
                        contourPoints.Add(firstSegment.EndPoint);
                        contourPointsCount += 2;
                        this.segments.Remove(firstSegment);
                        segmentCount--;
                    }
                }
            }
        }

        /// <summary>
        /// Renders the contour label.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="cl">
        /// The contour label.
        /// </param>
        private void RenderLabel(IRenderContext rc, ContourLabel cl)
        {
            if (this.LabelColor != null)
            {
                string actualLabelFont = this.LabelFont ?? PlotModel.DefaultFont;
                rc.DrawText(
                    cl.Position, 
                    cl.Text, 
                    this.LabelColor, 
                    actualLabelFont, 
                    this.LabelFontSize, 
                    this.LabelFontWeight, 
                    cl.Angle, 
                    HorizontalTextAlign.Center, 
                    VerticalTextAlign.Middle);
            }
        }

        /// <summary>
        /// Renders the contour label background.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="cl">
        /// The contour label.
        /// </param>
        private void RenderLabelBackground(IRenderContext rc, ContourLabel cl)
        {
            if (this.LabelBackground != null)
            {
                // Calculate background polygon
                string actualLabelFont = this.LabelFont ?? PlotModel.DefaultFont;
                OxySize size = rc.MeasureText(cl.Text, actualLabelFont, this.LabelFontSize, this.LabelFontWeight);
                double a = cl.Angle / 180 * Math.PI;
                double dx = Math.Cos(a);
                double dy = Math.Sin(a);

                /*double dl = Math.Sqrt(dx * dx + dy * dy);
                dx /= dl;
                dy /= dl;*/
                double ux = dx * 0.6;
                double uy = dy * 0.6;
                double vx = -dy * 0.5;
                double vy = dx * 0.5;
                double x = cl.Position.X;
                double y = cl.Position.Y;

                var bpts = new[]
                    {
                        new ScreenPoint(x - size.Width * ux - size.Height * vx, y - size.Width * uy - size.Height * vy), 
                        new ScreenPoint(x + size.Width * ux - size.Height * vx, y + size.Width * uy - size.Height * vy), 
                        new ScreenPoint(x + size.Width * ux + size.Height * vx, y + size.Width * uy + size.Height * vy), 
                        new ScreenPoint(x - size.Width * ux + size.Height * vx, y - size.Width * uy + size.Height * vy)
                    };
                rc.DrawPolygon(bpts, this.LabelBackground, null);
            }
        }

        #endregion

        /// <summary>
        /// Represents a contour.
        /// </summary>
        private class Contour
        {
            #region Constants and Fields

            /// <summary>
            ///   Gets or sets the contour level.
            /// </summary>
            /// <value>The contour level.</value>
            internal readonly double ContourLevel;

            /// <summary>
            ///   Gets or sets the points.
            /// </summary>
            /// <value>The points.</value>
            internal readonly IList<IDataPoint> Points;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Contour"/> class.
            /// </summary>
            /// <param name="points">
            /// The points.
            /// </param>
            /// <param name="contourLevel">
            /// The contour level.
            /// </param>
            public Contour(IList<IDataPoint> points, double contourLevel)
            {
                this.Points = points;
                this.ContourLevel = contourLevel;
            }

            #endregion
        }

        /// <summary>
        /// Represents a contour label.
        /// </summary>
        private class ContourLabel
        {
            #region Public Properties

            /// <summary>
            ///   Gets or sets the angle.
            /// </summary>
            /// <value>The angle.</value>
            public double Angle { get; set; }

            /// <summary>
            ///   Gets or sets the position.
            /// </summary>
            /// <value>The position.</value>
            public ScreenPoint Position { get; set; }

            /// <summary>
            ///   Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            public string Text { get; set; }

            #endregion
        }

        /// <summary>
        /// Represents a contour segment.
        /// </summary>
        private class ContourSegment
        {
            #region Constants and Fields

            /// <summary>
            ///   The contour level.
            /// </summary>
            internal readonly double ContourLevel;

            /// <summary>
            ///   The end point.
            /// </summary>
            internal readonly DataPoint EndPoint;

            /// <summary>
            ///   The start point.
            /// </summary>
            internal readonly DataPoint StartPoint;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ContourSegment"/> class.
            /// </summary>
            /// <param name="startPoint">
            /// The start point.
            /// </param>
            /// <param name="endPoint">
            /// The end point.
            /// </param>
            /// <param name="contourLevel">
            /// The contour level.
            /// </param>
            public ContourSegment(DataPoint startPoint, DataPoint endPoint, double contourLevel)
            {
                this.ContourLevel = contourLevel;
                this.StartPoint = startPoint;
                this.EndPoint = endPoint;
            }

            #endregion
        }
    }
}