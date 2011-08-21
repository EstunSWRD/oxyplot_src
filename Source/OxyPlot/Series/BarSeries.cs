﻿namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The BarSeries is used to create clustered or stacked bar charts.
    /// A bar chart or bar graph is a chart with rectangular bars with lengths proportional to the values that they represent. 
    /// The bars can be plotted vertically or horizontally.
    /// http://en.wikipedia.org/wiki/Bar_chart
    /// 
    /// The BarSeries requires a CategoryAxis. 
    /// The Values collection must contain the same number of elements as the number of categories in the CategoryAxis.
    /// You can define a ItemsSource and ValueField, or add the Values manually.
    /// 
    /// Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </summary>
    public class BarSeries : PlotSeriesBase
    {
        #region Constants and Fields

        /// <summary>
        /// The actual rectangles for the bars.
        /// </summary>
        internal IList<OxyRect> ActualBarRectangles;

        /// <summary>
        /// The values for the bars of this BarSeries.
        /// </summary>
        internal IList<double> InternalValues;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeries"/> class.
        /// </summary>
        public BarSeries()
        {
            this.InternalValues = new List<double>();
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 0;
            this.BarWidth = 0.5;
            this.TrackerFormatString = "{0}, {1}: {2}";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the width of the bars (as a fraction of the available width).
        /// The default value is 0.5 (50%)
        /// </summary>
        /// <value>The width of the bars.</value>
        public double BarWidth { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars when the value is negative.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor NegativeFillColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this bar series is stacked.
        /// </summary>
        public bool IsStacked { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        ///   Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IList<double> Values
        {
            get
            {
                return this.InternalValues;
            }
            set
            {
                this.InternalValues = value;
            }
        }

        #endregion

        #region Public Methods

        public CategoryAxis GetCategoryAxis()
        {
            return this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
        }

        /// <summary>
        ///   Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            int i = 0;
            foreach (OxyRect r in this.ActualBarRectangles)
            {
                if (point.X >= r.Left && point.X <= r.Right && point.Y >= r.Top && point.Y <= r.Bottom)
                {
                    ScreenPoint sp = point; // new ScreenPoint((r.Left + r.Right) / 2, r.Top);
                    var dp = new DataPoint(i, this.InternalValues[i]);
                    CategoryAxis categoryAxis = this.GetCategoryAxis();
                    string text = String.Format(
                        this.TrackerFormatString,
                        this.Title,
                        categoryAxis.FormatValueForTracker(i),
                        this.InternalValues[i]);
                    return new TrackerHitResult(this, dp, sp, this.GetItem(i), text);
                }
                i++;
            }
            return null;
        }

        public IAxis GetValueAxis()
        {
            return this.XAxis is CategoryAxis ? this.YAxis : this.XAxis;
        }

        /// <summary>
        ///   Gets the value from the specified X.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns></returns>
        public double? GetValueFromX(double x)
        {
            return null;
        }

        public virtual bool IsValidPoint(double v, IAxis yAxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

        public bool IsVertical()
        {
            return this.GetCategoryAxis() == this.XAxis;
        }

        /// <summary>
        ///   Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.Values.Count == 0)
            {
                return;
            }

            OxyRect clippingRect = this.GetClippingRect();

            CategoryAxis categoryAxis = this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category axis defined.");
            }

            bool isVertical = categoryAxis == this.XAxis;
            IAxis valueAxis = isVertical ? this.YAxis : this.XAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value axis defined.");
            }

            double dx = categoryAxis.BarOffset - this.BarWidth * 0.5;

            int i = 0;
            this.ActualBarRectangles = new List<OxyRect>();

            foreach (double v in this.Values)
            {
                if (!this.IsValidPoint(v, valueAxis))
                {
                    continue;
                }

                double baseValue = this.IsStacked ? categoryAxis.BaseValue[i] : double.NaN;
                if (double.IsNaN(baseValue))
                {
                    baseValue = 0;
                }

                double topValue = this.IsStacked ? baseValue + v : v;
                int nSeries = this.IsStacked ? 1 : categoryAxis.AttachedSeriesCount;
                OxyRect rect;
                if (isVertical)
                {
                    ScreenPoint p0 = this.XAxis.Transform(i + dx, baseValue, this.YAxis);
                    ScreenPoint p1 = this.XAxis.Transform(i + dx + this.BarWidth / nSeries, topValue, this.YAxis);

                    p0.X = (int)p0.X;
                    p0.Y = (int)p0.Y;
                    p1.X = (int)p1.X;
                    p1.Y = (int)p1.Y;

                    if (!double.IsNaN(categoryAxis.BaseValueScreen[i]))
                    {
                        if (this.IsStacked)
                        {
                            p0.Y = categoryAxis.BaseValueScreen[i];
                        }
                        else
                        {
                            p0.X = categoryAxis.BaseValueScreen[i];
                        }
                    }

                    rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                    if (this.IsStacked)
                    {
                        categoryAxis.BaseValue[i] = topValue;
                        categoryAxis.BaseValueScreen[i] = p1.Y;
                    }
                    else
                    {
                        categoryAxis.BaseValueScreen[i] = p1.X;
                    }
                }
                else
                {
                    ScreenPoint p0 = this.XAxis.Transform(baseValue, i + dx, this.YAxis);
                    ScreenPoint p1 = this.XAxis.Transform(topValue, i + dx + this.BarWidth / nSeries, this.YAxis);

                    p0.X = (int)p0.X;
                    p0.Y = (int)p0.Y;
                    p1.X = (int)p1.X;
                    p1.Y = (int)p1.Y;

                    if (!double.IsNaN(categoryAxis.BaseValueScreen[i]))
                    {
                        if (this.IsStacked)
                        {
                            p0.X = categoryAxis.BaseValueScreen[i];
                        }
                        else
                        {
                            p0.Y = categoryAxis.BaseValueScreen[i];
                        }
                    }

                    rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                    if (this.IsStacked)
                    {
                        categoryAxis.BaseValue[i] = topValue;
                        categoryAxis.BaseValueScreen[i] = p1.X;
                    }
                    else
                    {
                        categoryAxis.BaseValueScreen[i] = p1.Y;
                    }
                }
                this.ActualBarRectangles.Add(rect);

                var actualFillColor = FillColor;
                if (v<0 && NegativeFillColor!=null) actualFillColor = NegativeFillColor;

                // rc.DrawClippedRectangle(rect, clippingRect, actualFillColor, StrokeColor, StrokeThickness);
                rc.DrawClippedRectangleAsPolygon(
                    rect, clippingRect, actualFillColor, this.StrokeColor, this.StrokeThickness);

                i++;
            }
            if (!this.IsStacked)
            {
                categoryAxis.BarOffset += this.BarWidth / categoryAxis.AttachedSeriesCount;
            }
        }

        /// <summary>
        ///   Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The legend rectangle.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double height = legendBox.Bottom - legendBox.Top;
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - 0.25 * height, legendBox.Top + height * 0.1, 0.5 * height, height * 0.8),
                this.FillColor,
                this.StrokeColor,
                this.StrokeThickness);
        }

        public override void SetDefaultValues(PlotModel model)
        {
            if (this.FillColor == null)
            {
                this.FillColor = model.GetDefaultColor();
            }
        }

        public override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }
            this.InternalValues.Clear();
            ReflectionHelper.FillValues(this.ItemsSource, this.ValueField, this.InternalValues);
        }

        /// <summary>
        ///   Updates the maximum/minimum value on the value axis from the bar values.
        /// </summary>
        public override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (this.InternalValues == null || this.InternalValues.Count == 0)
            {
                return;
            }

            CategoryAxis ca = this.XAxis as CategoryAxis ?? this.YAxis as CategoryAxis;
            if (ca == null)
            {
                throw new Exception("CategoryAxis not defined.");
            }

            bool isVertical = ca == this.XAxis;

            IAxis valueAxis = isVertical ? this.YAxis : this.XAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value axis defined.");
            }

            double minValue = this.InternalValues[0];
            double maxValue = minValue;

            int i = 0;
            foreach (double v in this.Values)
            {
                double baseValue = 0;
                if (ca.BaseValue != null)
                {
                    baseValue = ca.BaseValue[i];
                }
                if (this.IsStacked)
                {
                    // Add to the max/min value on the category axis for stacked bars
                    ca.MaxValue[i] = Math.Max(ca.MaxValue[i], ca.MaxValue[i] + v);
                    ca.MinValue[i] = Math.Min(ca.MinValue[i], ca.MinValue[i] + v);
                    minValue = Math.Min(minValue, ca.MinValue[i]);
                    maxValue = Math.Max(maxValue, ca.MaxValue[i]);
                }

                minValue = Math.Min(minValue, baseValue);
                maxValue = Math.Max(maxValue, baseValue);

                minValue = Math.Min(minValue, v);
                maxValue = Math.Max(maxValue, v);
                i++;
            }

            if (isVertical)
            {
                valueAxis.Include(minValue);
                valueAxis.Include(maxValue);
                this.MinY = minValue;
                this.MaxY = maxValue;
            }
            else
            {
                valueAxis.Include(minValue);
                valueAxis.Include(maxValue);
                this.MinX = minValue;
                this.MaxX = maxValue;
            }
        }

        #endregion

        #region Methods

        private object GetItem(int i)
        {
            return null;
        }

        #endregion
    }
}