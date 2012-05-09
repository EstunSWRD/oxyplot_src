// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using OxyPlot.Reporting;

    /// <summary>
    /// Plot coordinate system type
    /// </summary>
    public enum PlotType
    {
        /// <summary>
        ///   XY coordinate system - two perpendicular axes
        /// </summary>
        // ReSharper disable InconsistentNaming
        XY,
        // ReSharper restore InconsistentNaming

        /// <summary>
        ///   Cartesian coordinate system - perpendicular axes with the same scaling http://en.wikipedia.org/wiki/Cartesian_coordinate_system
        /// </summary>
        Cartesian,

        /// <summary>
        ///   Polar coordinate system - with radial and angular axes http://en.wikipedia.org/wiki/Polar_coordinate_system
        /// </summary>
        Polar
    }

    /// <summary>
    /// Legend placement enumeration.
    /// </summary>
    public enum LegendPlacement
    {
        /// <summary>
        ///   The inside.
        /// </summary>
        Inside,

        /// <summary>
        ///   The outside.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Legend position enumeration.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        ///   The top left.
        /// </summary>
        TopLeft,

        /// <summary>
        ///   The top center.
        /// </summary>
        TopCenter,

        /// <summary>
        ///   The top right.
        /// </summary>
        TopRight,

        /// <summary>
        ///   The bottom left.
        /// </summary>
        BottomLeft,

        /// <summary>
        ///   The bottom center.
        /// </summary>
        BottomCenter,

        /// <summary>
        ///   The bottom right.
        /// </summary>
        BottomRight,

        /// <summary>
        ///   The left top.
        /// </summary>
        LeftTop,

        /// <summary>
        ///   The left middle.
        /// </summary>
        LeftMiddle,

        /// <summary>
        ///   The left bottom.
        /// </summary>
        LeftBottom,

        /// <summary>
        ///   The right top.
        /// </summary>
        RightTop,

        /// <summary>
        ///   The right middle.
        /// </summary>
        RightMiddle,

        /// <summary>
        ///   The right bottom.
        /// </summary>
        RightBottom
    }

    /// <summary>
    /// Legend orientation enumeration.
    /// </summary>
    public enum LegendOrientation
    {
        /// <summary>
        ///   The horizontal.
        /// </summary>
        Horizontal,

        /// <summary>
        ///   The vertical.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Legend item order enumeration.
    /// </summary>
    public enum LegendItemOrder
    {
        /// <summary>
        ///   The normal.
        /// </summary>
        Normal,

        /// <summary>
        ///   The reverse.
        /// </summary>
        Reverse
    }

    /// <summary>
    /// Legend symbol placement enumeration.
    /// </summary>
    public enum LegendSymbolPlacement
    {
        /// <summary>
        ///   The left.
        /// </summary>
        Left,

        /// <summary>
        ///   The right.
        /// </summary>
        Right
    }

    /// <summary>
    /// The PlotModel represents all the content of the plot (titles, axes, series).
    /// </summary>
    public partial class PlotModel
    {
        #region Constants and Fields

        /// <summary>
        /// The default selection color.
        /// </summary>
        internal static readonly OxyColor DefaultSelectionColor = OxyColors.Yellow;

        /// <summary>
        ///   The default font.
        /// </summary>
        private const string PrivateDefaultFont = "Segoe UI";

        /// <summary>
        ///   The current color index.
        /// </summary>
        private int currentColorIndex;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="PlotModel" /> class.
        /// </summary>
        public PlotModel()
        {
            this.Axes = new Collection<Axis>();
            this.Series = new Collection<Series>();
            this.Annotations = new Collection<Annotation>();

            this.PlotType = PlotType.XY;

            this.PlotMargins = new OxyThickness(60, 4, 4, 40);
            this.Padding = new OxyThickness(8, 8, 16, 8);
            this.AutoAdjustPlotMargins = true;

            this.DefaultFont = PrivateDefaultFont;
            this.DefaultFontSize = 12;

            this.TitleFont = null;
            this.TitleFontSize = 18;
            this.TitleFontWeight = FontWeights.Bold;
            this.SubtitleFont = null;
            this.SubtitleFontSize = 14;
            this.SubtitleFontWeight = FontWeights.Normal;
            this.TitlePadding = 6;

            this.TextColor = OxyColors.Black;
            this.PlotAreaBorderColor = OxyColors.Black;
            this.PlotAreaBorderThickness = 1;

            this.IsLegendVisible = true;
            this.LegendTitleFont = null;
            this.LegendTitleFontSize = 12;
            this.LegendTitleFontWeight = FontWeights.Bold;
            this.LegendFont = null;
            this.LegendFontSize = 12;
            this.LegendFontWeight = FontWeights.Normal;
            this.LegendSymbolLength = 16;
            this.LegendSymbolMargin = 4;
            this.LegendPadding = 8;
            this.LegendItemSpacing = 24;
            this.LegendMargin = 8;

            this.LegendBackground = null;
            this.LegendBorder = null;
            this.LegendBorderThickness = 1;

            this.LegendPlacement = LegendPlacement.Inside;
            this.LegendPosition = LegendPosition.RightTop;
            this.LegendOrientation = LegendOrientation.Vertical;
            this.LegendItemOrder = LegendItemOrder.Normal;
            this.LegendItemAlignment = HorizontalTextAlign.Left;
            this.LegendSymbolPlacement = LegendSymbolPlacement.Left;

            this.DefaultColors = new List<OxyColor> 
            {
                    OxyColor.FromRGB(0x4E, 0x9A, 0x06), 
                    OxyColor.FromRGB(0xC8, 0x8D, 0x00), 
                    OxyColor.FromRGB(0xCC, 0x00, 0x00), 
                    OxyColor.FromRGB(0x20, 0x4A, 0x87), 
                    OxyColors.Red, 
                    OxyColors.Orange, 
                    OxyColors.Yellow, 
                    OxyColors.Green, 
                    OxyColors.Blue, 
                    OxyColors.Indigo, 
                    OxyColors.Violet
                };

            this.AxisTierDistance = 4.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotModel"/> class.
        /// </summary>
        /// <param name="title">
        /// The title. 
        /// </param>
        /// <param name="subtitle">
        /// The subtitle. 
        /// </param>
        public PlotModel(string title, string subtitle = null)
            : this()
        {
            this.Title = title;
            this.Subtitle = subtitle;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when the plot has been updated.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        ///   Occurs when the plot is about to be updated.
        /// </summary>
        public event EventHandler Updating;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the default font. 
        /// </summary>
        /// <value> The default font. </value>
        /// <remarks>
        /// This font is used for text on axes, series, legends and plot titles unless other fonts are specified.
        /// </remarks>
        public string DefaultFont { get; set; }

        /// <summary>
        /// Gets or sets the default size of the fonts.
        /// </summary>
        /// <value>
        /// The default size of the font.
        /// </value>
        public double DefaultFontSize { get; set; }

        /// <summary>
        ///   Gets the actual culture.
        /// </summary>
        public CultureInfo ActualCulture
        {
            get
            {
                return this.Culture ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        ///   Gets the actual plot margins.
        /// </summary>
        /// <value> The actual plot margins. </value>
        public OxyThickness ActualPlotMargins { get; private set; }

        /// <summary>
        /// Gets the plot control that renders this plot.
        /// </summary>
        /// <remarks>
        /// Only one PlotControl can render the plot at the same time.
        /// </remarks>
        /// <value>The plot control.</value>
        public IPlotControl PlotControl { get; private set; }

        /// <summary>
        ///   Gets or sets the annotations.
        /// </summary>
        /// <value> The annotations. </value>
        public Collection<Annotation> Annotations { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to auto adjust plot margins.
        /// </summary>
        public bool AutoAdjustPlotMargins { get; set; }

        /// <summary>
        ///   Gets or sets the axes.
        /// </summary>
        /// <value> The axes. </value>
        public Collection<Axis> Axes { get; set; }

        /// <summary>
        ///   Gets or sets the color of the background of the plot.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets or sets the culture.
        /// </summary>
        /// <value> The culture. </value>
        public CultureInfo Culture { get; set; }

        /// <summary>
        ///   Gets or sets the default colors.
        /// </summary>
        /// <value> The default colors. </value>
        public IList<OxyColor> DefaultColors { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the legend is visible. The titles of the series must be set to use the legend.
        /// </summary>
        public bool IsLegendVisible { get; set; }

        /// <summary>
        ///   Gets the legend area.
        /// </summary>
        /// <value> The legend area. </value>
        public OxyRect LegendArea { get; private set; }

        /// <summary>
        ///   Gets or sets the background color of the legend. Use null for no background.
        /// </summary>
        /// <value> The legend background. </value>
        public OxyColor LegendBackground { get; set; }

        /// <summary>
        ///   Gets or sets the border color of the legend.
        /// </summary>
        /// <value> The legend border. </value>
        public OxyColor LegendBorder { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the legend border. Use 0 for no border.
        /// </summary>
        /// <value> The legend border thickness. </value>
        public double LegendBorderThickness { get; set; }

        /// <summary>
        ///   Gets or sets the legend column spacing.
        /// </summary>
        /// <value> The legend column spacing. </value>
        public double LegendColumnSpacing { get; set; }

        /// <summary>
        ///   Gets or sets the legend font.
        /// </summary>
        /// <value> The legend font. </value>
        public string LegendFont { get; set; }

        /// <summary>
        ///   Gets or sets the size of the legend font.
        /// </summary>
        /// <value> The size of the legend font. </value>
        public double LegendFontSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the legend text.
        /// </summary>
        /// <value>
        /// The color of the legend text.
        /// </value>
        /// <remarks>
        /// If this value is null, the TextColor will be used.
        /// </remarks>
        public OxyColor LegendTextColor { get; set; }

        /// <summary>
        ///   Gets or sets the legend font weight.
        /// </summary>
        /// <value> The legend font weight. </value>
        public double LegendFontWeight { get; set; }

        /// <summary>
        ///   Gets or sets the legend item alignment.
        /// </summary>
        /// <value> The legend item alignment. </value>
        public HorizontalTextAlign LegendItemAlignment { get; set; }

        /// <summary>
        ///   Gets or sets the legend item order.
        /// </summary>
        /// <value> The legend item order. </value>
        public LegendItemOrder LegendItemOrder { get; set; }

        /// <summary>
        ///   Gets or sets the legend spacing.
        /// </summary>
        /// <value> The legend spacing. </value>
        public double LegendItemSpacing { get; set; }

        /// <summary>
        ///   Gets or sets the legend margin.
        /// </summary>
        /// <value> The legend margin. </value>
        public double LegendMargin { get; set; }

        /// <summary>
        ///   Gets or sets the legend orientation.
        /// </summary>
        /// <value> The legend orientation. </value>
        public LegendOrientation LegendOrientation { get; set; }

        /// <summary>
        ///   Gets or sets the legend padding.
        /// </summary>
        /// <value> The legend padding. </value>
        public double LegendPadding { get; set; }

        /// <summary>
        ///   Gets or sets the legend placement.
        /// </summary>
        /// <value> The legend placement. </value>
        public LegendPlacement LegendPlacement { get; set; }

        /// <summary>
        ///   Gets or sets the legend position.
        /// </summary>
        /// <value> The legend position. </value>
        public LegendPosition LegendPosition { get; set; }

        /// <summary>
        ///   Gets or sets the length of the legend symbols (the default value is 16).
        /// </summary>
        public double LegendSymbolLength { get; set; }

        /// <summary>
        ///   Gets or sets the legend symbol margins (distance between the symbol and the text).
        /// </summary>
        /// <value> The legend symbol margin. </value>
        public double LegendSymbolMargin { get; set; }

        /// <summary>
        ///   Gets or sets the legend symbol placement.
        /// </summary>
        /// <value> The legend symbol placement. </value>
        public LegendSymbolPlacement LegendSymbolPlacement { get; set; }

        /// <summary>
        ///   Gets or sets the legend title.
        /// </summary>
        /// <value> The legend title. </value>
        public string LegendTitle { get; set; }

        /// <summary>
        /// Gets or sets the color of the legend title.
        /// </summary>
        /// <value>
        /// The color of the legend title.
        /// </value>
        /// <remarks>
        /// If this value is null, the TextColor will be used.
        /// </remarks>
        public OxyColor LegendTitleColor { get; set; }

        /// <summary>
        ///   Gets or sets the legend title font.
        /// </summary>
        /// <value> The legend title font. </value>
        public string LegendTitleFont { get; set; }

        /// <summary>
        ///   Gets or sets the size of the legend title font.
        /// </summary>
        /// <value> The size of the legend title font. </value>
        public double LegendTitleFontSize { get; set; }

        /// <summary>
        ///   Gets or sets the legend title font weight.
        /// </summary>
        /// <value> The legend title font weight. </value>
        public double LegendTitleFontWeight { get; set; }

        /// <summary>
        ///   Gets or sets the padding around the plot.
        /// </summary>
        /// <value> The padding. </value>
        public OxyThickness Padding { get; set; }

        /// <summary>
        ///   Gets the area including both the plot and the axes. Outside legends are rendered outside this rectangle.
        /// </summary>
        /// <value> The plot and axis area. </value>
        public OxyRect PlotAndAxisArea { get; private set; }

        /// <summary>
        ///   Gets the plot area. This area is used to draw the series (not including axes or legends).
        /// </summary>
        /// <value> The plot area. </value>
        public OxyRect PlotArea { get; private set; }

        /// <summary>
        ///   Gets or sets the distance between two neighbourhood tiers of the same AxisPosition.
        /// </summary>
        public double AxisTierDistance { get; set; }

        /// <summary>
        ///   Gets or sets the color of the background of the plot area.
        /// </summary>
        public OxyColor PlotAreaBackground { get; set; }

        /// <summary>
        ///   Gets or sets the color of the border around the plot area.
        /// </summary>
        /// <value> The color of the box. </value>
        public OxyColor PlotAreaBorderColor { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the border around the plot area.
        /// </summary>
        /// <value> The box thickness. </value>
        public double PlotAreaBorderThickness { get; set; }

        /// <summary>
        ///   Gets or sets the minimum margins around the plot (this should be large enough to fit the axes). The default value is (60, 4, 4, 40). Set AutoAdjustPlotMargins if you want the margins to be adjusted when the axes require more space.
        /// </summary>
        public OxyThickness PlotMargins { get; set; }

        /// <summary>
        ///   Gets or sets the type of the coordinate system.
        /// </summary>
        /// <value> The type of the plot. </value>
        public PlotType PlotType { get; set; }

        /// <summary>
        /// Gets or sets the color of the selection.
        /// </summary>
        /// <value>
        /// The color of the selection.
        /// </value>
        public OxyColor SelectionColor { get; set; }

        /// <summary>
        ///   Gets or sets the series.
        /// </summary>
        /// <value> The series. </value>
        public Collection<Series> Series { get; set; }

        /// <summary>
        ///   Gets or sets the subtitle.
        /// </summary>
        /// <value> The subtitle. </value>
        public string Subtitle { get; set; }

        /// <summary>
        ///   Gets or sets the subtitle font. If this property is null, the Title font will be used.
        /// </summary>
        /// <value> The subtitle font. </value>
        public string SubtitleFont { get; set; }

        /// <summary>
        ///   Gets or sets the size of the subtitle font.
        /// </summary>
        /// <value> The size of the subtitle font. </value>
        public double SubtitleFontSize { get; set; }

        /// <summary>
        ///   Gets or sets the subtitle font weight.
        /// </summary>
        /// <value> The subtitle font weight. </value>
        public double SubtitleFontWeight { get; set; }

        /// <summary>
        ///   Gets or sets the default color of the text in the plot (titles, legends, annotations, axes).
        /// </summary>
        /// <value> The color of the text. </value>
        public OxyColor TextColor { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the color of the title.
        /// </summary>
        /// <value>
        /// The color of the title.
        /// </value>
        /// <remarks>
        /// If the value is null, the TextColor will be used.
        /// </remarks>
        public OxyColor TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the subtitle.
        /// </summary>
        /// <value>
        /// The color of the subtitle.
        /// </value>
        public OxyColor SubtitleColor { get; set; }

        /// <summary>
        ///   Gets the title area.
        /// </summary>
        /// <value> The title area. </value>
        public OxyRect TitleArea { get; private set; }

        /// <summary>
        ///   Gets or sets the title font.
        /// </summary>
        /// <value> The title font. </value>
        public string TitleFont { get; set; }

        /// <summary>
        ///   Gets or sets the size of the title font.
        /// </summary>
        /// <value> The size of the title font. </value>
        public double TitleFontSize { get; set; }

        /// <summary>
        ///   Gets or sets the title font weight.
        /// </summary>
        /// <value> The title font weight. </value>
        public double TitleFontWeight { get; set; }

        /// <summary>
        ///   Gets or sets the padding around the title.
        /// </summary>
        /// <value> The title padding. </value>
        public double TitlePadding { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the default angle axis.
        /// </summary>
        /// <value> The default angle axis. </value>
        internal AngleAxis DefaultAngleAxis { get; private set; }

        /// <summary>
        ///   Gets the default magnitude axis.
        /// </summary>
        /// <value> The default magnitude axis. </value>
        internal MagnitudeAxis DefaultMagnitudeAxis { get; private set; }

        /// <summary>
        ///   Gets the default X axis.
        /// </summary>
        /// <value> The default X axis. </value>
        internal Axis DefaultXAxis { get; private set; }

        /// <summary>
        ///   Gets the default Y axis.
        /// </summary>
        /// <value> The default Y axis. </value>
        internal Axis DefaultYAxis { get; private set; }

        /// <summary>
        ///   Gets the default color axis.
        /// </summary>
        /// <value> The default color axis. </value>
        internal ColorAxis DefaultColorAxis { get; private set; }

        /// <summary>
        ///   Gets the actual title font.
        /// </summary>
        protected string ActualTitleFont
        {
            get
            {
                return this.TitleFont ?? this.DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual subtitle font.
        /// </summary>
        protected string ActualSubtitleFont
        {
            get
            {
                return this.SubtitleFont ?? this.DefaultFont;
            }
        }

        /// <summary>
        ///   Gets the visible series.
        /// </summary>
        /// <value> The visible series. </value>
        private IEnumerable<Series> VisibleSeries
        {
            get
            {
                return this.Series.Where(s => s.IsVisible);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Attaches this model to the specified plot control.
        /// </summary>
        /// <param name="plotControl">The plot control.</param>
        /// <remarks>
        /// Only one plot control can be attached to the plot model.
        /// The plot model contains data (e.g. axis scaling) that is only relevant to the current plot control.
        /// </remarks>
        public void AttachPlotControl(IPlotControl plotControl)
        {
            this.PlotControl = plotControl;
        }

        /// <summary>
        /// Creates a report for the plot.
        /// </summary>
        /// <returns>
        /// A report. 
        /// </returns>
        public Report CreateReport()
        {
            var r = new Report { Culture = CultureInfo.InvariantCulture };

            r.AddHeader(1, "P L O T   R E P O R T");
            r.AddHeader(2, "=== PlotModel ===");
            r.AddPropertyTable("PlotModel", this);

            r.AddHeader(2, "=== Axes ===");
            foreach (Axis a in this.Axes)
            {
                r.AddPropertyTable(TypeHelper.GetTypeName(a.GetType()), a);
            }

            r.AddHeader(2, "=== Annotations ===");
            foreach (var a in this.Annotations)
            {
                r.AddPropertyTable(TypeHelper.GetTypeName(a.GetType()), a);
            }

            r.AddHeader(2, "=== Series ===");
            foreach (var s in this.Series)
            {
                r.AddPropertyTable(TypeHelper.GetTypeName(s.GetType()), s);
                var ds = s as DataPointSeries;
                if (ds != null)
                {
                    var fields = new List<ItemsTableField> { new ItemsTableField("X", "X"), new ItemsTableField("Y", "Y") };
                    r.AddItemsTable("Data", ds.Points, fields);
                }
            }

#if !METRO
            var assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            r.AddParagraph(string.Format("Report generated by OxyPlot {0}", assemblyName.Version.ToString(3)));
#endif

            return r;
        }

        /// <summary>
        /// Creates a text report for the plot.
        /// </summary>
        /// <returns>
        /// The create text report. 
        /// </returns>
        public string CreateTextReport()
        {
            using (var ms = new MemoryStream())
            {
                using (var trw = new TextReportWriter(ms))
                {
                    Report report = this.CreateReport();
                    report.Write(trw);
                    trw.Flush();
                    ms.Position = 0;
                    var r = new StreamReader(ms);
                    return r.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Refreshes the plot.
        /// </summary>
        /// <param name="updateData">Updates all data sources if set to <c>true</c>.</param>
        public void RefreshPlot(bool updateData)
        {
            if (this.PlotControl == null)
            {
                throw new InvalidOperationException("RefreshPlot: The plot model is not connected to a plot control.");
            }

            this.PlotControl.RefreshPlot(updateData);
        }

        /// <summary>
        /// Invalidates the plot.
        /// </summary>
        /// <param name="updateData">Updates all data sources if set to <c>true</c>.</param>
        public void InvalidatePlot(bool updateData)
        {
            if (this.PlotControl == null)
            {
                throw new InvalidOperationException("InvalidatePlot: The plot model is not connected to a plot control.");
            }

            this.PlotControl.InvalidatePlot(updateData);
        }

        /// <summary>
        /// Gets the first axes that covers the area of the specified point.
        /// </summary>
        /// <param name="pt">
        /// The point. 
        /// </param>
        /// <param name="xaxis">
        /// The xaxis. 
        /// </param>
        /// <param name="yaxis">
        /// The yaxis. 
        /// </param>
        public void GetAxesFromPoint(ScreenPoint pt, out Axis xaxis, out Axis yaxis)
        {
            xaxis = yaxis = null;

            // Get the axis position of the given point. Using null if the point is inside the plot area.
            AxisPosition? position = null;
            double plotAreaValue = 0;
            if (pt.X < this.PlotArea.Left)
            {
                position = AxisPosition.Left;
                plotAreaValue = this.PlotArea.Left;
            }

            if (pt.X > this.PlotArea.Right)
            {
                position = AxisPosition.Right;
                plotAreaValue = this.PlotArea.Right;
            }

            if (pt.Y < this.PlotArea.Top)
            {
                position = AxisPosition.Top;
                plotAreaValue = this.PlotArea.Top;
            }

            if (pt.Y > this.PlotArea.Bottom)
            {
                position = AxisPosition.Bottom;
                plotAreaValue = this.PlotArea.Bottom;
            }

            foreach (var axis in this.Axes)
            {
                if (axis is ColorAxis)
                {
                    continue;
                }

                if (axis is MagnitudeAxis)
                {
                    xaxis = axis;
                    continue;
                }

                if (axis is AngleAxis)
                {
                    yaxis = axis;
                    continue;
                }

                double x = double.NaN;
                if (axis.IsHorizontal())
                {
                    x = axis.InverseTransform(pt.X);
                }

                if (axis.IsVertical())
                {
                    x = axis.InverseTransform(pt.Y);
                }

                if (x >= axis.ActualMinimum && x <= axis.ActualMaximum)
                {
                    if (position == null)
                    {
                        if (axis.IsHorizontal())
                        {
                            if (xaxis == null)
                            {
                                xaxis = axis;
                            }
                        }
                        else if (axis.IsVertical())
                        {
                            if (yaxis == null)
                            {
                                yaxis = axis;
                            }
                        }
                    }
                    else if (position == axis.Position)
                    {
                        // Choose right tier
                        double positionTierMinShift = axis.PositionTierMinShift;
                        double positionTierMaxShift = axis.PositionTierMaxShift;

                        double posValue = axis.IsHorizontal() ? pt.Y : pt.X;
                        bool isLeftOrTop = position == AxisPosition.Top || position == AxisPosition.Left;
                        if ((posValue >= plotAreaValue + positionTierMinShift
                             && posValue < plotAreaValue + positionTierMaxShift && !isLeftOrTop)
                            ||
                            (posValue <= plotAreaValue - positionTierMinShift
                             && posValue > plotAreaValue - positionTierMaxShift && isLeftOrTop))
                        {
                            if (axis.IsHorizontal())
                            {
                                if (xaxis == null)
                                {
                                    xaxis = axis;
                                }
                            }
                            else if (axis.IsVertical())
                            {
                                if (yaxis == null)
                                {
                                    yaxis = axis;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default color from the DefaultColors palette.
        /// </summary>
        /// <returns>
        /// The next default color. 
        /// </returns>
        public OxyColor GetDefaultColor()
        {
            return this.DefaultColors[this.currentColorIndex++ % this.DefaultColors.Count];
        }

        /// <summary>
        /// Gets the default line style.
        /// </summary>
        /// <returns>
        /// The next default line style. 
        /// </returns>
        public LineStyle GetDefaultLineStyle()
        {
            return (LineStyle)((this.currentColorIndex / this.DefaultColors.Count) % (int)LineStyle.None);
        }

        /// <summary>
        /// Gets a series from the specified point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <param name="limit">
        /// The limit. 
        /// </param>
        /// <returns>
        /// The nearest series. 
        /// </returns>
        public Series GetSeriesFromPoint(ScreenPoint point, double limit)
        {
            double mindist = double.MaxValue;
            Series closest = null;
            foreach (var s in this.VisibleSeries.Reverse())
            {
                var ts = s as ITrackableSeries;
                if (ts == null)
                {
                    continue;
                }

                var thr = ts.GetNearestPoint(point, true) ?? ts.GetNearestPoint(point, false);

                if (thr == null)
                {
                    continue;
                }

                // find distance to this point on the screen
                double dist = point.DistanceTo(thr.Position);
                if (dist < mindist)
                {
                    closest = s;
                    mindist = dist;
                }
            }

            if (mindist < limit)
            {
                return closest;
            }

            return null;
        }

#if !METRO

        /// <summary>
        /// Saves the plot to a svg file.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file. 
        /// </param>
        /// <param name="width">
        /// The width (points). 
        /// </param>
        /// <param name="height">
        /// The height (points). 
        /// </param>
        public void SaveSvg(string fileName, double width, double height)
        {
            SvgExporter.Export(this, fileName, width, height);
        }

#endif

        /// <summary>
        /// Generates C# code of the model.
        /// </summary>
        /// <returns>
        /// C# code. 
        /// </returns>
        public string ToCode()
        {
            var cg = new CodeGenerator(this);
            return cg.ToCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// Create an svg model and return it as a string.
        /// </summary>
        /// <param name="width">
        /// The width (points). 
        /// </param>
        /// <param name="height">
        /// The height (points). 
        /// </param>
        /// <param name="isDocument">if set to <c>true</c>, the xml headers will be included (?xml and !DOCTYPE).</param>
        /// <returns>
        /// The to svg. 
        /// </returns>
        public string ToSvg(double width, double height, bool isDocument = false)
        {
            return SvgExporter.ExportToString(this, width, height, isDocument);
        }

        /// <summary>
        /// Gets all elements of the plot model.
        /// </summary>
        /// <returns>An enumerator of the plot elements.</returns>
        public IEnumerable<PlotElement> GetElements()
        {
            foreach (var axis in this.Axes)
            {
                yield return axis;
            }

            foreach (var annotation in this.Annotations)
            {
                yield return annotation;
            }

            foreach (var s in this.Series)
            {
                yield return s;
            }
        }

        /// <summary>
        /// Updates all axes and series. 0. Updates the owner PlotModel of all plot items (axes, series and annotations) 
        /// 1. Updates the data of each Series (only if updateData==true). 
        /// 2. Ensure that all series have axes assigned. 
        /// 3. Updates the max and min of the axes.
        /// </summary>
        /// <param name="updateData">
        /// if set to <c>true</c> , all data collections will be updated. 
        /// </param>
        public void Update(bool updateData = true)
        {
            this.OnUpdating();

            // update the owner PlotModel
            foreach (var s in this.VisibleSeries)
            {
                s.PlotModel = this;
            }

            foreach (var a in this.Annotations)
            {
                a.PlotModel = this;
            }

            // Updates the default axes
            this.EnsureDefaultAxes();

            // Update data of the series
            if (updateData)
            {
                foreach (var s in this.VisibleSeries)
                {
                    s.UpdateData();
                }
            }

            foreach (var a in this.Axes)
            {
                a.PlotModel = this;
            }

            foreach (var c in this.Axes.OfType<CategoryAxis>())
            {
                c.UpdateLabels(this.VisibleSeries);
            }

            // Update valid data of the series
            if (updateData)
            {
                foreach (var s in this.VisibleSeries)
                {
                    s.UpdateValidData();
                }
            }

            // Updates axes with information from the series
            // This is used by the category axis that need to know the number of series using the axis.
            foreach (var a in this.Axes)
            {
                a.UpdateFromSeries(this.VisibleSeries);
            }

            // Update the max and min of the axes
            this.UpdateMaxMin(updateData);
            this.OnUpdated();
        }

        /// <summary>
        /// Updates the axis transforms.
        /// </summary>
        public void UpdateAxisTransforms()
        {
            // Update the axis transforms
            foreach (var a in this.Axes)
            {
                a.UpdateTransform(this.PlotArea);
            }
        }

        /// <summary>
        /// Gets the axis for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultAxis">The default axis.</param>
        /// <returns>The axis, or the defaultAxis if the key is not found.</returns>
        internal Axis GetAxisOrDefault(string key, Axis defaultAxis)
        {
            if (key != null)
            {
                return this.Axes.FirstOrDefault(a => a.Key == key) ?? defaultAxis;
            }

            return defaultAxis;
        }

        /// <summary>
        /// Raises the Updated event.
        /// </summary>
        protected virtual void OnUpdated()
        {
            var handler = this.Updated;
            if (handler != null)
            {
                var args = new EventArgs();
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        protected virtual void OnUpdating()
        {
            var handler = this.Updating;
            if (handler != null)
            {
                var args = new EventArgs();
                handler(this, args);
            }
        }

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Trace(string message)
        {
#if !SILVERLIGHT
            System.Diagnostics.Trace.WriteLine(string.Format("{0}: {1}", this.GetType().Name, message));
#endif
        }

        /// <summary>
        /// Enforces the same scale on all axes.
        /// </summary>
        private void EnforceCartesianTransforms()
        {
            // Set the same scaling on all axes
            double sharedScale = this.Axes.Min(a => Math.Abs(a.Scale));
            foreach (var a in this.Axes)
            {
                a.Zoom(sharedScale);
            }

            sharedScale = this.Axes.Max(a => Math.Abs(a.Scale));
            foreach (var a in this.Axes)
            {
                a.Zoom(sharedScale);
            }

            foreach (var a in this.Axes)
            {
                a.UpdateTransform(this.PlotArea);
            }
        }

        /// <summary>
        /// Updates the intervals (major and minor step values).
        /// </summary>
        private void UpdateIntervals()
        {
            // Update the intervals for all axes
            foreach (var a in this.Axes)
            {
                a.UpdateIntervals(this.PlotArea);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds and sets the default horizontal and vertical axes (the first horizontal/vertical axes in the Axes collection).
        /// </summary>
        private void EnsureDefaultAxes()
        {
            this.DefaultXAxis = this.Axes.FirstOrDefault(a => a.IsHorizontal() && a.IsXyAxis());
            this.DefaultYAxis = this.Axes.FirstOrDefault(a => a.IsVertical() && a.IsXyAxis());
            this.DefaultMagnitudeAxis = this.Axes.FirstOrDefault(a => a is MagnitudeAxis) as MagnitudeAxis;
            this.DefaultAngleAxis = this.Axes.FirstOrDefault(a => a is AngleAxis) as AngleAxis;
            this.DefaultColorAxis = this.Axes.FirstOrDefault(a => a is ColorAxis) as ColorAxis;

            if (this.DefaultXAxis == null)
            {
                this.DefaultXAxis = this.DefaultMagnitudeAxis;
            }

            if (this.DefaultYAxis == null)
            {
                this.DefaultYAxis = this.DefaultAngleAxis;
            }

            if (this.PlotType == PlotType.Polar)
            {
                if (this.DefaultXAxis == null)
                {
                    this.DefaultXAxis = this.DefaultMagnitudeAxis = new MagnitudeAxis();
                }

                if (this.DefaultYAxis == null)
                {
                    this.DefaultYAxis = this.DefaultAngleAxis = new AngleAxis();
                }
            }
            else
            {
                bool createdlinearxaxis = false;
                bool createdlinearyaxis = false;
                if (this.DefaultXAxis == null)
                {
                    if (this.Series.Any(series => series is ColumnSeries))
                    {
                        this.DefaultXAxis = new CategoryAxis { Position = AxisPosition.Bottom };
                    }
                    else
                    {
                        this.DefaultXAxis = new LinearAxis { Position = AxisPosition.Bottom };
                        createdlinearxaxis = true;
                    }
                }

                if (this.DefaultYAxis == null)
                {
                    if (this.Series.Any(series => series is BarSeries))
                    {
                        this.DefaultYAxis = new CategoryAxis { Position = AxisPosition.Left };
                    }
                    else
                    {
                        this.DefaultYAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0 };
                        createdlinearyaxis = true;
                    }
                }

                if (createdlinearxaxis && this.DefaultYAxis is CategoryAxis)
                {
                    this.DefaultXAxis.MinimumPadding = 0;
                }

                if (createdlinearyaxis && this.DefaultXAxis is CategoryAxis)
                {
                    this.DefaultYAxis.MinimumPadding = 0;
                }
            }

            bool areAxesRequired = false;
            foreach (var s in this.VisibleSeries)
            {
                if (s.AreAxesRequired())
                {
                    areAxesRequired = true;
                }
            }

            if (areAxesRequired)
            {
                if (!this.Axes.Contains(this.DefaultXAxis))
                {
                    Debug.Assert(this.DefaultXAxis != null, "Default x-axis not created.");
                    if (this.DefaultXAxis != null)
                    {
                        this.Axes.Add(this.DefaultXAxis);
                    }
                }

                if (!this.Axes.Contains(this.DefaultYAxis))
                {
                    Debug.Assert(this.DefaultYAxis != null, "Default y-axis not created.");
                    if (this.DefaultYAxis != null)
                    {
                        this.Axes.Add(this.DefaultYAxis);
                    }
                }
            }

            // Update the x/index axes of series without axes defined
            foreach (var s in this.VisibleSeries)
            {
                if (s.AreAxesRequired())
                {
                    s.EnsureAxes();
                }
            }

            // Update the x/index axes of annotations without axes defined
            foreach (var a in this.Annotations)
            {
                a.EnsureAxes();
            }
        }

        /// <summary>
        /// Resets the default color index.
        /// </summary>
        private void ResetDefaultColor()
        {
            this.currentColorIndex = 0;
        }

        /// <summary>
        /// Updates maximum and minimum values of the axes from values of all data series.
        /// </summary>
        /// <param name="isDataUpdated">
        /// if set to <c>true</c> , the data has been updated. 
        /// </param>
        private void UpdateMaxMin(bool isDataUpdated)
        {
            foreach (var a in this.Axes)
            {
                a.ResetActualMaxMin();
            }

            if (isDataUpdated)
            {
                // data has been updated, so we need to calculate the max/min of the series again
                foreach (var s in this.VisibleSeries)
                {
                    s.UpdateMaxMin();
                }
            }

            foreach (var s in this.VisibleSeries)
            {
                s.UpdateAxisMaxMin();
            }

            foreach (var a in this.Axes)
            {
                a.UpdateActualMaxMin();
            }
        }

        #endregion

#if SERIALIZATION_ENABLED
        /// <summary>
        /// Xml serialize the plotmodel to a stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        public void XmlSerialize(Stream s)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(PlotModel));
            serializer.Serialize(s, this);
        }

        /// <summary>
        /// Xml serialize the plotmodel to a file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void XmlSerialize(string filename)
        {
            using (var s = File.OpenWrite(filename))
            {
                this.XmlSerialize(s);
            }
        }
#endif
    }
}