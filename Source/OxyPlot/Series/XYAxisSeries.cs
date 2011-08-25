﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Abstract base class for Series that contains an X-axis and Y-axis
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Abstract base class for Series that contains an X-axis and Y-axis
    /// </summary>
    public abstract class XYAxisSeries : Series
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the maximum x-coordinate of the dataset.
        /// </summary>
        /// <value>The maximum x-coordinate.</value>
        public double MaxX { get; protected set; }

        /// <summary>
        ///   Gets or sets the maximum y-coordinate of the dataset.
        /// </summary>
        /// <value>The maximum y-coordinate.</value>
        public double MaxY { get; protected set; }

        /// <summary>
        ///   Gets or sets the minimum x-coordinate of the dataset.
        /// </summary>
        /// <value>The minimum x-coordinate.</value>
        public double MinX { get; protected set; }

        /// <summary>
        ///   Gets or sets the minimum y-coordinate of the dataset.
        /// </summary>
        /// <value>The minimum y-coordinate.</value>
        public double MinY { get; protected set; }

        /// <summary>
        ///   Gets or sets the x-axis.
        /// </summary>
        /// <value>The x-axis.</value>
        public Axis XAxis { get; private set; }

        /// <summary>
        ///   Gets or sets the x-axis key.
        /// </summary>
        /// <value>The x-axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the y-axis.
        /// </summary>
        /// <value>The y-axis.</value>
        public Axis YAxis { get; private set; }

        /// <summary>
        ///   Gets or sets the y-axis key.
        /// </summary>
        /// <value>The y-axis key.</value>
        public string YAxisKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the rectangle the series uses on the screen (screen coordinates).
        /// </summary>
        /// <returns>
        /// The rectangle.
        /// </returns>
        public OxyRect GetScreenRectangle()
        {
            return this.GetClippingRect();
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
        }

        /// <summary>
        /// Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The legend rectangle.
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if this data series requires X/Y axes.
        /// (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns>
        /// The are axes required.
        /// </returns>
        protected internal override bool AreAxesRequired()
        {
            return true;
        }

        /// <summary>
        /// Ensures that the series has axes.
        /// </summary>
        /// <param name="axes">
        /// The axes collection of the parent PlotModel.
        /// </param>
        /// <param name="defaultXAxis">
        /// The default X axis of the parent PlotModel.
        /// </param>
        /// <param name="defaultYAxis">
        /// The default Y axis of the parent PlotModel.
        /// </param>
        protected internal override void EnsureAxes(Collection<Axis> axes, Axis defaultXAxis, Axis defaultYAxis)
        {
            // reset
            this.XAxis = null;
            this.YAxis = null;

            if (this.XAxisKey != null)
            {
                this.XAxis = axes.FirstOrDefault(a => a.Key == this.XAxisKey);
            }

            if (this.YAxisKey != null)
            {
                this.YAxis = axes.FirstOrDefault(a => a.Key == this.YAxisKey);
            }

            // If axes are not found, use the default axes
            if (this.XAxis == null)
            {
                this.XAxis = defaultXAxis;
            }

            if (this.YAxis == null)
            {
                this.YAxis = defaultYAxis;
            }
        }

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">
        /// </param>
        /// <returns>
        /// The is using.
        /// </returns>
        protected internal override bool IsUsing(IAxis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <summary>
        /// Sets default values from the plotmodel.
        /// </summary>
        /// <param name="model">
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
        }

        /// <summary>
        /// Updates the max/minimum values.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            this.MinX = this.MinY = this.MaxX = this.MaxY = double.NaN;
        }

        /// <summary>
        /// Gets the clipping rect.
        /// </summary>
        /// <returns>
        /// </returns>
        protected OxyRect GetClippingRect()
        {
            double minX = Math.Min(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double minY = Math.Min(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);
            double maxX = Math.Max(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double maxY = Math.Max(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets the point on the curve that is nearest the specified point.
        /// </summary>
        /// <param name="points">
        /// The point list.
        /// </param>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="dpn">
        /// The nearest point (data coordinates).
        /// </param>
        /// <param name="spn">
        /// The nearest point (screen coordinates).
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// True if a point was found.
        /// </returns>
        protected bool GetNearestInterpolatedPointInternal(
            IList<IDataPoint> points, ScreenPoint point, out DataPoint dpn, out ScreenPoint spn, out int index)
        {
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            index = -1;

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double minimumDistance = double.MaxValue;

            for (int i = 0; i + 1 < points.Count; i++)
            {
                IDataPoint p1 = points[i];
                IDataPoint p2 = points[i + 1];
                ScreenPoint sp1 = AxisBase.Transform(p1, this.XAxis, this.YAxis);
                ScreenPoint sp2 = AxisBase.Transform(p2, this.XAxis, this.YAxis);

                double sp21X = sp2.x - sp1.x;
                double sp21Y = sp2.y - sp1.y;
                double u1 = (point.x - sp1.x) * sp21X + (point.y - sp1.y) * sp21Y;
                double u2 = sp21X * sp21X + sp21Y * sp21Y;
                double ds = sp21X * sp21X + sp21Y * sp21Y;

                if (ds < 4)
                {
                    // if the points are very close, we can get numerical problems, just use the first point...
                    u1 = 0;
                    u2 = 1;
                }

                if (u2 < double.Epsilon && u2 > -double.Epsilon)
                {
                    continue; // P1 && P2 coincident
                }

                double u = u1 / u2;
                if (u < 0)
                {
                    u = 0;
                }

                if (u > 1)
                {
                    u = 1;
                }

                double sx = sp1.x + u * sp21X;
                double sy = sp1.y + u * sp21Y;

                double dx = point.x - sx;
                double dy = point.y - sy;
                double distance = dx * dx + dy * dy;

                if (distance < minimumDistance)
                {
                    double px = p1.X + u * (p2.X - p1.X);
                    double py = p1.Y + u * (p2.Y - p1.Y);
                    dpn = new DataPoint(px, py);
                    spn = new ScreenPoint(sx, sy);
                    minimumDistance = distance;
                    index = i;
                }
            }

            return minimumDistance < double.MaxValue;
        }

        /// <summary>
        /// The get nearest point internal.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="dpn">
        /// The dpn.
        /// </param>
        /// <param name="spn">
        /// The spn.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The get nearest point internal.
        /// </returns>
        protected bool GetNearestPointInternal(
            IEnumerable<IDataPoint> points, ScreenPoint point, out DataPoint dpn, out ScreenPoint spn, out int index)
        {
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            index = -1;

            double minimumDistance = double.MaxValue;
            int i = 0;
            foreach (DataPoint p in points)
            {
                ScreenPoint sp = AxisBase.Transform(p, this.XAxis, this.YAxis);
                double dx = sp.x - point.x;
                double dy = sp.y - point.y;
                double d2 = dx * dx + dy * dy;

                if (d2 < minimumDistance)
                {
                    dpn = p;
                    spn = sp;
                    minimumDistance = d2;
                    index = i;
                }

                i++;
            }

            return minimumDistance < double.MaxValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a double precision floating point number.
        /// DateTime objects are converted using DateTimeAxis.ToDouble
        /// TimeSpan objects are converted using TimeSpanAxis.ToDouble
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The to double.
        /// </returns>
        protected virtual double ToDouble(object value)
        {
            if (value is DateTime)
            {
                return DateTimeAxis.ToDouble((DateTime)value);
            }

            if (value is TimeSpan)
            {
                return ((TimeSpan)value).TotalSeconds;
            }

            return Convert.ToDouble(value);
        }

        #endregion
    }
}