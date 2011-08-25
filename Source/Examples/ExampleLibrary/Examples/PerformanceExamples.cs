﻿
namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;

    [Examples("Performance")]
    public class PerformanceExamples : ExamplesBase
    {
        [Example("LineSeries, 100k points")]
        public static PlotModel LineSeries1()
        {
            var model = CreatePlotModel();
            var s1 = new LineSeries();
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points, ItemsSource, List<IDataPoint>")]
        public static PlotModel LineSeries10()
        {
            var model = CreatePlotModel();
            var s1 = new LineSeries();
            var points = new List<IDataPoint>();
            AddPoints(points, 100000);
            s1.ItemsSource = points;
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points, ItemsSource, List<OxyRect>")]
        public static PlotModel LineSeries10b()
        {
            var model = CreatePlotModel();
            var s1 = new LineSeries();
            var points = new List<IDataPoint>();
            AddPoints(points, 100000);
            var rects = points.Select(pt => new OxyRect(pt.X, pt.Y, 0, 0)).ToList();
            s1.ItemsSource = rects;
            s1.DataFieldX = "Left";
            s1.DataFieldY = "Top";
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points (thick)")]
        public static PlotModel LineSeries1b()
        {
            var model = CreatePlotModel();
            var s1 = new LineSeries();
            s1.StrokeThickness = 10;
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points (by ItemsSource)")]
        public static PlotModel LineSeries2()
        {
            var model = CreatePlotModel();
            model.Series.Add(new LineSeries { ItemsSource = GetPoints(100000) });
            return model;
        }

        [Example("ScatterSeries (squares)")]
        public static PlotModel ScatterSeries1()
        {
            var model = CreatePlotModel();
            var s1 = new ScatterSeries();
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares with outline)")]
        public static PlotModel ScatterSeries1b()
        {
            var model = CreatePlotModel();
            var s1 = new ScatterSeries();
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares without fill color)")]
        public static PlotModel ScatterSeries1c()
        {
            var model = CreatePlotModel();
            var s1 = new ScatterSeries();
            s1.MarkerFill = OxyColors.Transparent;
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (by ItemsSource)")]
        public static PlotModel ScatterSeries2()
        {
            var model = CreatePlotModel();
            model.Series.Add(new ScatterSeries { ItemsSource = GetPoints(2000) });
            return model;
        }

        [Example("ScatterSeries (circles)")]
        public static PlotModel ScatterSeries3()
        {
            var model = CreatePlotModel();
            var s1 = new ScatterSeries();
            s1.MarkerType = MarkerType.Circle;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (circles with outline)")]
        public static PlotModel ScatterSeries4()
        {
            var model = CreatePlotModel();
            var s1 = new ScatterSeries();
            s1.MarkerType = MarkerType.Circle;
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (cross)")]
        public static PlotModel ScatterSeries5()
        {
            var model = CreatePlotModel();
            var s1 = new ScatterSeries();
            s1.MarkerType = MarkerType.Cross;
            s1.MarkerFill = null;
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LinearAxis (no gridlines)")]
        public static PlotModel LinearAxis1()
        {
            var model = CreatePlotModel();
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1));
            return model;
        }

        [Example("LinearAxis (solid gridlines) ")]
        public static PlotModel LinearAxis2()
        {
            var model = CreatePlotModel();
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Solid });
            return model;
        }

        [Example("LinearAxis (dashed gridlines)")]
        public static PlotModel LinearAxis3()
        {
            var model = CreatePlotModel();
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dash });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dash });
            return model;
        }

        [Example("LinearAxis (dotted gridlines)")]
        public static PlotModel LinearAxis4()
        {
            var model = CreatePlotModel();
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dot });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dot });
            return model;
        }

        private static IList<IDataPoint> GetPoints(int n)
        {
            var points = new List<IDataPoint>();
            AddPoints(points, n);
            return points;
        }

        private static void AddPoints(IList<IDataPoint> points, int n)
        {
            for (int i = 0; i < n; i++)
            {
                double x = Math.PI * 10 * i / (n - 1);
                points.Add(new DataPoint(x * Math.Cos(x), x * Math.Sin(x)));
            }
        }
    }
}