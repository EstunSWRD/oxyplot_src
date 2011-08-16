﻿namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    public abstract class AxisBase : FrameworkElement, IAxis
    {
        #region Constants and Fields
        
        public static readonly DependencyProperty AbsoluteMaximumProperty =
            DependencyProperty.Register(
                "AbsoluteMaximum",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(double.MaxValue, VisualChanged));

        public static readonly DependencyProperty AbsoluteMinimumProperty =
            DependencyProperty.Register(
                "AbsoluteMinimum",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(double.MinValue, VisualChanged));

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.0, VisualChanged));

        public static readonly DependencyProperty EndPositionProperty = DependencyProperty.Register(
            "EndPosition", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(1.0, VisualChanged));

        public static readonly DependencyProperty ExtraGridlineColorProperty =
            DependencyProperty.Register(
                "ExtraGridlineColor",
                typeof(Color),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        public static readonly DependencyProperty ExtraGridlineStyleProperty =
            DependencyProperty.Register(
                "ExtraGridlineStyle",
                typeof(LineStyle),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(LineStyle.Solid, VisualChanged));

        public static readonly DependencyProperty ExtraGridlineThicknessProperty =
            DependencyProperty.Register(
                "ExtraGridlineThickness",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(1.0, VisualChanged));

        public static readonly DependencyProperty ExtraGridlinesProperty = DependencyProperty.Register(
            "ExtraGridLines", typeof(double[]), typeof(AxisBase), new FrameworkPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty FilterMaxValueProperty = DependencyProperty.Register(
            "FilterMaxValue",
            typeof(double),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(double.MaxValue, DataChanged));

        public static readonly DependencyProperty FilterMinValueProperty = DependencyProperty.Register(
            "FilterMinValue",
            typeof(double),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(double.MinValue, DataChanged));

        public static readonly DependencyProperty FontProperty = DependencyProperty.Register(
            "Font", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(12.0, VisualChanged));

        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight",
            typeof(FontWeight),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(FontWeights.Normal, VisualChanged));

        public static readonly DependencyProperty IsAxisVisibleProperty = DependencyProperty.Register(
            "IsAxisVisible", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true, VisualChanged));

        public static readonly DependencyProperty IsPanEnabledProperty = DependencyProperty.Register(
            "IsPanEnabled", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register(
            "IsZoomEnabled", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty MajorGridlineColorProperty =
            DependencyProperty.Register(
                "MajorGridlineColor",
                typeof(Color),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(Color.FromArgb(0x40, 0, 0, 0), VisualChanged));

        public static readonly DependencyProperty MajorGridlineStyleProperty =
            DependencyProperty.Register(
                "MajorGridlineStyle",
                typeof(LineStyle),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(LineStyle.None, VisualChanged));

        public static readonly DependencyProperty MajorGridlineThicknessProperty =
            DependencyProperty.Register(
                "MajorGridlineThickness",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(1.0, VisualChanged));

        public static readonly DependencyProperty MajorStepProperty = DependencyProperty.Register(
            "MajorStep", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        public static readonly DependencyProperty MajorTickSizeProperty = DependencyProperty.Register(
            "MajorTickSize", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(7.0, VisualChanged));

        public static readonly DependencyProperty MaximumPaddingProperty = DependencyProperty.Register(
            "MaximumPadding", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.01, VisualChanged));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        public static readonly DependencyProperty MinimumPaddingProperty = DependencyProperty.Register(
            "MinimumPadding", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.01, VisualChanged));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        public static readonly DependencyProperty MinimumRangeProperty = DependencyProperty.Register(
            "MinimumRange", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.0, VisualChanged));

        public static readonly DependencyProperty MinorGridlineColorProperty =
            DependencyProperty.Register(
                "MinorGridlineColor",
                typeof(Color),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(Color.FromArgb(0x20, 0, 0, 0), VisualChanged));

        public static readonly DependencyProperty MinorGridlineStyleProperty =
            DependencyProperty.Register(
                "MinorGridlineStyle",
                typeof(LineStyle),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(LineStyle.None, VisualChanged));

        public static readonly DependencyProperty MinorGridlineThicknessProperty =
            DependencyProperty.Register(
                "MinorGridlineThickness",
                typeof(double),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(1.0, VisualChanged));

        public static readonly DependencyProperty MinorStepProperty = DependencyProperty.Register(
            "MinorStep", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(double.NaN, VisualChanged));

        public static readonly DependencyProperty MinorTickSizeProperty = DependencyProperty.Register(
            "MinorTickSize", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(4.0, VisualChanged));

        public static readonly DependencyProperty PositionAtZeroCrossingProperty =
            DependencyProperty.Register(
                "PositionAtZeroCrossing",
                typeof(bool),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(false, VisualChanged));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(AxisPosition),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(AxisPosition.Left, VisualChanged));

        public static readonly DependencyProperty ShowMinorTicksProperty = DependencyProperty.Register(
            "ShowMinorTicks", typeof(bool), typeof(AxisBase), new FrameworkPropertyMetadata(true, VisualChanged));

        public static readonly DependencyProperty StartPositionProperty = DependencyProperty.Register(
            "StartPosition", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.0, VisualChanged));

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TickStyleProperty = DependencyProperty.Register(
            "TickStyle",
            typeof(TickStyle),
            typeof(AxisBase),
            new FrameworkPropertyMetadata(TickStyle.Inside, VisualChanged));

        public static readonly DependencyProperty TicklineColorProperty = DependencyProperty.Register(
            "TicklineColor", typeof(Color), typeof(AxisBase), new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        public static readonly DependencyProperty TitleFormatStringProperty =
            DependencyProperty.Register(
                "TitleFormatString",
                typeof(string),
                typeof(AxisBase),
                new FrameworkPropertyMetadata("{0} [{1}]", VisualChanged));

        public static readonly DependencyProperty TitlePositionProperty = DependencyProperty.Register(
            "TitlePosition", typeof(double), typeof(AxisBase), new FrameworkPropertyMetadata(0.5, VisualChanged));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(
            "Unit", typeof(string), typeof(AxisBase), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty UseSuperExponentialFormatProperty =
            DependencyProperty.Register(
                "UseSuperExponentialFormat",
                typeof(bool),
                typeof(AxisBase),
                new FrameworkPropertyMetadata(false, VisualChanged));

        /// <summary>
        /// Internal axis
        /// </summary>
        protected OxyPlot.IAxis Axis;

        #endregion

        #region Public Properties

        public double AbsoluteMaximum
        {
            get
            {
                return (double)this.GetValue(AbsoluteMaximumProperty);
            }
            set
            {
                this.SetValue(AbsoluteMaximumProperty, value);
            }
        }

        public double AbsoluteMinimum
        {
            get
            {
                return (double)this.GetValue(AbsoluteMinimumProperty);
            }
            set
            {
                this.SetValue(AbsoluteMinimumProperty, value);
            }
        }

        public double Angle
        {
            get
            {
                return (double)this.GetValue(AngleProperty);
            }
            set
            {
                this.SetValue(AngleProperty, value);
            }
        }

        public double EndPosition
        {
            get
            {
                return (double)this.GetValue(EndPositionProperty);
            }
            set
            {
                this.SetValue(EndPositionProperty, value);
            }
        }

        public double[] ExtraGridLines
        {
            get
            {
                return (double[])this.GetValue(ExtraGridlinesProperty);
            }
            set
            {
                this.SetValue(ExtraGridlinesProperty, value);
            }
        }

        public Color ExtraGridlineColor
        {
            get
            {
                return (Color)this.GetValue(ExtraGridlineColorProperty);
            }
            set
            {
                this.SetValue(ExtraGridlineColorProperty, value);
            }
        }

        public LineStyle ExtraGridlineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(ExtraGridlineStyleProperty);
            }
            set
            {
                this.SetValue(ExtraGridlineStyleProperty, value);
            }
        }

        public double ExtraGridlineThickness
        {
            get
            {
                return (double)this.GetValue(ExtraGridlineThicknessProperty);
            }
            set
            {
                this.SetValue(ExtraGridlineThicknessProperty, value);
            }
        }

        public double FilterMaxValue
        {
            get
            {
                return (double)this.GetValue(FilterMaxValueProperty);
            }
            set
            {
                this.SetValue(FilterMaxValueProperty, value);
            }
        }

        public double FilterMinValue
        {
            get
            {
                return (double)this.GetValue(FilterMinValueProperty);
            }
            set
            {
                this.SetValue(FilterMinValueProperty, value);
            }
        }

        public string Font
        {
            get
            {
                return (string)this.GetValue(FontProperty);
            }
            set
            {
                this.SetValue(FontProperty, value);
            }
        }

        public double FontSize
        {
            get
            {
                return (double)this.GetValue(FontSizeProperty);
            }
            set
            {
                this.SetValue(FontSizeProperty, value);
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(FontWeightProperty);
            }
            set
            {
                this.SetValue(FontWeightProperty, value);
            }
        }

        public bool IsAxisVisible
        {
            get
            {
                return (bool)this.GetValue(IsAxisVisibleProperty);
            }
            set
            {
                this.SetValue(IsAxisVisibleProperty, value);
            }
        }

        public bool IsPanEnabled
        {
            get
            {
                return (bool)this.GetValue(IsPanEnabledProperty);
            }
            set
            {
                this.SetValue(IsPanEnabledProperty, value);
            }
        }

        public bool IsZoomEnabled
        {
            get
            {
                return (bool)this.GetValue(IsZoomEnabledProperty);
            }
            set
            {
                this.SetValue(IsZoomEnabledProperty, value);
            }
        }

        public string Key
        {
            get
            {
                return (string)this.GetValue(KeyProperty);
            }
            set
            {
                this.SetValue(KeyProperty, value);
            }
        }

        public Color MajorGridlineColor
        {
            get
            {
                return (Color)this.GetValue(MajorGridlineColorProperty);
            }
            set
            {
                this.SetValue(MajorGridlineColorProperty, value);
            }
        }

        public LineStyle MajorGridlineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(MajorGridlineStyleProperty);
            }
            set
            {
                this.SetValue(MajorGridlineStyleProperty, value);
            }
        }

        public double MajorGridlineThickness
        {
            get
            {
                return (double)this.GetValue(MajorGridlineThicknessProperty);
            }
            set
            {
                this.SetValue(MajorGridlineThicknessProperty, value);
            }
        }

        public double MajorStep
        {
            get
            {
                return (double)this.GetValue(MajorStepProperty);
            }
            set
            {
                this.SetValue(MajorStepProperty, value);
            }
        }

        public double MajorTickSize
        {
            get
            {
                return (double)this.GetValue(MajorTickSizeProperty);
            }
            set
            {
                this.SetValue(MajorTickSizeProperty, value);
            }
        }

        public double Maximum
        {
            get
            {
                return (double)this.GetValue(MaximumProperty);
            }
            set
            {
                this.SetValue(MaximumProperty, value);
            }
        }

        public double MaximumPadding
        {
            get
            {
                return (double)this.GetValue(MaximumPaddingProperty);
            }
            set
            {
                this.SetValue(MaximumPaddingProperty, value);
            }
        }

        public double Minimum
        {
            get
            {
                return (double)this.GetValue(MinimumProperty);
            }
            set
            {
                this.SetValue(MinimumProperty, value);
            }
        }

        public double MinimumPadding
        {
            get
            {
                return (double)this.GetValue(MinimumPaddingProperty);
            }
            set
            {
                this.SetValue(MinimumPaddingProperty, value);
            }
        }

        public double MinimumRange
        {
            get
            {
                return (double)this.GetValue(MinimumRangeProperty);
            }
            set
            {
                this.SetValue(MinimumRangeProperty, value);
            }
        }

        public Color MinorGridlineColor
        {
            get
            {
                return (Color)this.GetValue(MinorGridlineColorProperty);
            }
            set
            {
                this.SetValue(MinorGridlineColorProperty, value);
            }
        }

        public LineStyle MinorGridlineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(MinorGridlineStyleProperty);
            }
            set
            {
                this.SetValue(MinorGridlineStyleProperty, value);
            }
        }

        public double MinorGridlineThickness
        {
            get
            {
                return (double)this.GetValue(MinorGridlineThicknessProperty);
            }
            set
            {
                this.SetValue(MinorGridlineThicknessProperty, value);
            }
        }

        public double MinorStep
        {
            get
            {
                return (double)this.GetValue(MinorStepProperty);
            }
            set
            {
                this.SetValue(MinorStepProperty, value);
            }
        }

        public double MinorTickSize
        {
            get
            {
                return (double)this.GetValue(MinorTickSizeProperty);
            }
            set
            {
                this.SetValue(MinorTickSizeProperty, value);
            }
        }

        public AxisPosition Position
        {
            get
            {
                return (AxisPosition)this.GetValue(PositionProperty);
            }
            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        public bool PositionAtZeroCrossing
        {
            get
            {
                return (bool)this.GetValue(PositionAtZeroCrossingProperty);
            }
            set
            {
                this.SetValue(PositionAtZeroCrossingProperty, value);
            }
        }

        public bool ShowMinorTicks
        {
            get
            {
                return (bool)this.GetValue(ShowMinorTicksProperty);
            }
            set
            {
                this.SetValue(ShowMinorTicksProperty, value);
            }
        }

        public double StartPosition
        {
            get
            {
                return (double)this.GetValue(StartPositionProperty);
            }
            set
            {
                this.SetValue(StartPositionProperty, value);
            }
        }

        public string StringFormat
        {
            get
            {
                return (string)this.GetValue(StringFormatProperty);
            }
            set
            {
                this.SetValue(StringFormatProperty, value);
            }
        }

        public TickStyle TickStyle
        {
            get
            {
                return (TickStyle)this.GetValue(TickStyleProperty);
            }
            set
            {
                this.SetValue(TickStyleProperty, value);
            }
        }

        public Color TicklineColor
        {
            get
            {
                return (Color)this.GetValue(TicklineColorProperty);
            }
            set
            {
                this.SetValue(TicklineColorProperty, value);
            }
        }

        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }
            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        public string TitleFormatString
        {
            get
            {
                return (string)this.GetValue(TitleFormatStringProperty);
            }
            set
            {
                this.SetValue(TitleFormatStringProperty, value);
            }
        }

        public double TitlePosition
        {
            get
            {
                return (double)this.GetValue(TitlePositionProperty);
            }
            set
            {
                this.SetValue(TitlePositionProperty, value);
            }
        }

        public string Unit
        {
            get
            {
                return (string)this.GetValue(UnitProperty);
            }
            set
            {
                this.SetValue(UnitProperty, value);
            }
        }

        public bool UseSuperExponentialFormat
        {
            get
            {
                return (bool)this.GetValue(UseSuperExponentialFormatProperty);
            }
            set
            {
                this.SetValue(UseSuperExponentialFormatProperty, value);
            }
        }

        #endregion

        #region Public Methods

        public abstract OxyPlot.IAxis CreateModel();

        public virtual void SynchronizeProperties()
        {
            var a = this.Axis as OxyPlot.AxisBase;
            a.AbsoluteMaximum = this.AbsoluteMaximum;
            a.AbsoluteMinimum = this.AbsoluteMinimum;
            a.Angle = this.Angle;
            a.EndPosition = this.EndPosition;
            a.ExtraGridlineColor = this.ExtraGridlineColor.ToOxyColor();
            a.ExtraGridlineStyle = this.ExtraGridlineStyle;
            a.ExtraGridlineThickness = this.ExtraGridlineThickness;
            a.ExtraGridlines = this.ExtraGridLines;
            a.FilterMaxValue = this.FilterMaxValue;
            a.FilterMinValue = this.FilterMinValue;
            a.Font = this.Font;
            a.FontSize = this.FontSize;
            a.FontWeight = this.FontWeight.ToOpenTypeWeight();
            a.IsPanEnabled = this.IsPanEnabled;
            a.IsAxisVisible = this.IsAxisVisible;
            a.IsZoomEnabled = this.IsZoomEnabled;
            a.Key = this.Key;
            a.MajorGridlineColor = this.MajorGridlineColor.ToOxyColor();
            a.MinorGridlineColor = this.MinorGridlineColor.ToOxyColor();
            a.MajorGridlineStyle = this.MajorGridlineStyle;
            a.MinorGridlineStyle = this.MinorGridlineStyle;
            a.MajorGridlineThickness = this.MajorGridlineThickness;
            a.MinorGridlineThickness = this.MinorGridlineThickness;
            a.MajorStep = this.MajorStep;
            a.MajorTickSize = this.MajorTickSize;
            a.MinorStep = this.MinorStep;
            a.MinorTickSize = this.MinorTickSize;
            a.Minimum = this.Minimum;
            a.Maximum = this.Maximum;
            a.MinimumPadding = this.MinimumPadding;
            a.MaximumPadding = this.MaximumPadding;
            a.Position = this.Position;
            a.PositionAtZeroCrossing = this.PositionAtZeroCrossing;
            a.ShowMinorTicks = this.ShowMinorTicks;
            a.StartPosition = this.StartPosition;
            a.StringFormat = this.StringFormat;
            a.TicklineColor = this.TicklineColor.ToOxyColor();
            a.TitleFormatString = this.TitleFormatString;
            a.Title = this.Title;
            a.TickStyle = this.TickStyle;
            a.TitlePosition = this.TitlePosition;
            a.Unit = this.Unit;
            a.UseSuperExponentialFormat = this.UseSuperExponentialFormat;
        }

        #endregion

        #region Methods

        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AxisBase)d).OnDataChanged();
        }

        protected static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AxisBase)d).OnVisualChanged();
        }

        protected void OnDataChanged()
        {
            // post event to  parent
            this.OnVisualChanged();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.OwnerType == this.GetType())
            {
                var fpm = e.Property.GetMetadata(e.Property.OwnerType) as FrameworkPropertyMetadata;
                if (fpm != null && fpm.AffectsRender)
                {
                    var plot = this.Parent as Plot;
                    plot.InvalidatePlot();
                }
            }
        }

        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        #endregion
    }
}