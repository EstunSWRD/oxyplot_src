﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenIssues.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Z1 Issues (open)")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    // ReSharper disable InconsistentNaming
    public class OpenIssues : ExamplesBase
    {
        [Example("#10018: Sub/superscript in vertical axis title")]
        public static PlotModel SubSuperScriptInAxisTitles()
        {
            var plotModel1 = new PlotModel { Title = "x_{i}^{j}", Subtitle = "x_{i}^{j}" };
            var leftAxis = new LinearAxis { Position = AxisPosition.Left, Title = "x_{i}^{j}" };
            plotModel1.Axes.Add(leftAxis);
            var bottomAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "x_{i}^{j}" };
            plotModel1.Axes.Add(bottomAxis);
            plotModel1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 100, "x_{i}^{j}"));
            return plotModel1;
        }

        [Example("#10018: Sub/superscript in rotated annotations")]
        public static PlotModel RotatedSubSuperScript()
        {
            var s = "x_{A}^{B}";
            var plotModel1 = new PlotModel { Title = s, Subtitle = s };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = s, Minimum = -1, Maximum = 1 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = s, Minimum = -1, Maximum = 11 });
            for (int rotation = 0; rotation < 360; rotation += 45)
            {
                plotModel1.Annotations.Add(new TextAnnotation { Text = s, Position = new DataPoint(rotation / 360d * 10, 0), Rotation = rotation });
            }

            return plotModel1;
        }

        [Example("#10045: DateTimeAxis with IntervalType = Minutes")]
        public static PlotModel DateTimeAxisWithIntervalTypeMinutes()
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);

            var dateTimeAxis1 = new DateTimeAxis
                                    {
                                        IntervalType = DateTimeIntervalType.Minutes,
                                        EndPosition = 0,
                                        StartPosition = 1,
                                        StringFormat = "hh:mm:ss"
                                    };
            plotModel1.Axes.Add(dateTimeAxis1);
            var time0 = new DateTime(2013, 5, 6, 3, 24, 0);
            var time1 = new DateTime(2013, 5, 6, 3, 28, 0);
            var lineSeries1 = new LineSeries();
            lineSeries1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time0), 36));
            lineSeries1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time1), 26));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("#10055: Hit testing LineSeries with smoothing")]
        public static PlotModel MouseDownEvent()
        {
            var model = new PlotModel { Title = "LineSeries with smoothing", Subtitle = "Tracker uses wrong points" };
            var logarithmicAxis1 = new LogarithmicAxis { Position = AxisPosition.Bottom };
            model.Axes.Add(logarithmicAxis1);

            // Add a line series
            var s1 = new LineSeries
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5,
                Smooth = true
            };
            s1.Points.Add(new DataPoint(100, 100));
            s1.Points.Add(new DataPoint(400, 200));
            s1.Points.Add(new DataPoint(600, -300));
            s1.Points.Add(new DataPoint(1000, 400));
            s1.Points.Add(new DataPoint(1500, 500));
            s1.Points.Add(new DataPoint(2500, 600));
            s1.Points.Add(new DataPoint(3000, 700));
            model.Series.Add(s1);

            return model;
        }

        [Example("#10056: Tracker wrong for logarithmic y-axis")]
        public static PlotModel ValueTime()
        {
            var plotModel1 = new PlotModel
            {
                LegendBackground = OxyColor.FromArgb(200, 255, 255, 255),
                LegendBorder = OxyColors.Black,
                LegendPlacement = LegendPlacement.Outside,
                PlotAreaBackground = OxyColors.Gray,
                PlotAreaBorderColor = OxyColors.Gainsboro,
                PlotAreaBorderThickness = 2,
                Title = "Value / Time"
            };
            var linearAxis1 = new LinearAxis
            {
                AbsoluteMaximum = 45,
                AbsoluteMinimum = 0,
                Key = "X-Axis",
                Maximum = 46,
                Minimum = -1,
                Position = AxisPosition.Bottom,
                Title = "Years",
                Unit = "yr"
            };
            plotModel1.Axes.Add(linearAxis1);
            var logarithmicAxis1 = new LogarithmicAxis { Key = "Y-Axis", Title = "Value for section" };
            plotModel1.Axes.Add(logarithmicAxis1);
            var lineSeries1 = new LineSeries
            {
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                MarkerFill = OxyColors.Black,
                MarkerSize = 2,
                MarkerStroke = OxyColors.Black,
                MarkerType = MarkerType.Circle,
                DataFieldX = "X",
                DataFieldY = "Y",
                XAxisKey = "X-Axis",
                YAxisKey = "Y-Axis",
                Background = OxyColors.White,
                Title = "Section Value",
                TrackerKey = "ValueVersusTimeTracker"
            };
            lineSeries1.Points.Add(new DataPoint(0, 0));
            lineSeries1.Points.Add(new DataPoint(5, 0));
            lineSeries1.Points.Add(new DataPoint(10, 0));
            lineSeries1.Points.Add(new DataPoint(15, 0));
            lineSeries1.Points.Add(new DataPoint(20, 1));
            lineSeries1.Points.Add(new DataPoint(25, 1));
            lineSeries1.Points.Add(new DataPoint(30, 1));
            lineSeries1.Points.Add(new DataPoint(35, 1));
            lineSeries1.Points.Add(new DataPoint(40, 1));
            lineSeries1.Points.Add(new DataPoint(45, 1));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("#10080: LegendItemAlignment = Center")]
        public static PlotModel LegendItemAlignmentCenter()
        {
            var plotModel1 = new PlotModel { Title = "LegendItemAlignment = Center" };
            plotModel1.LegendItemAlignment = HorizontalAlignment.Center;
            plotModel1.LegendBorder = OxyColors.Black;
            plotModel1.LegendBorderThickness = 1;
            plotModel1.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, 0, 10, 100, "sin(x)/x"));
            plotModel1.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 100, "cos(x)"));
            return plotModel1;
        }

        [Example("#10115: GetNearestPoint return DataPoint even when custom IDataPoint used")]
        public static PlotModel GetNearestPointReturnsDataPoint()
        {
            var plotModel1 = new PlotModel { Title = "Issue 10115" };
            return plotModel1;
        }

        [Example("#10117: Selecting points changes the legend colours")]
        public static PlotModel SelectingPointsChangesTheLegendColors()
        {
            var plotModel1 = new PlotModel { Title = "Issue 10117" };
            return plotModel1;
        }

        [Example("#10148: Data points remain visible outside of bounds on panning")]
        public static PlotModel DataPointsRemainVisibleOutsideBoundsOnPanning()
        {
            var plotModel1 = new PlotModel();

            var masterAxis = new DateTimeAxis { Key = "MasterDateTimeAxis", Position = AxisPosition.Bottom };
            plotModel1.Axes.Add(masterAxis);

            var verticalAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Measurement",
                Key = "Measurement",
                AbsoluteMinimum = -100,
                Minimum = -100,
                AbsoluteMaximum = 100,
                Maximum = 100,
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            plotModel1.Axes.Add(verticalAxis);

            var line = new LineSeries { Title = "Measurement", XAxisKey = masterAxis.Key, YAxisKey = verticalAxis.Key };
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now), 10));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(1)), 10));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(2)), 45));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(3)), 17));

            line.Points.Add(DataPoint.Undefined);

            // this point should be visible
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(4)), 10));
            //// line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(4)), 10));

            line.Points.Add(DataPoint.Undefined);

            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(5)), 45));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(6)), 17));

            plotModel1.Series.Add(line);

            return plotModel1;
        }
    }
}