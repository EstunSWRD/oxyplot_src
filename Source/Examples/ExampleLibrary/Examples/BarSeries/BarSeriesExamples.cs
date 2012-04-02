// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    using System;
    using System.Collections.ObjectModel;

    [Examples("BarSeries and ColumnSeries")]
    public static class BarSeriesExamples
    {
        [Example("ColumnSeries")]
        public static PlotModel SimpleColumnSeries()
        {
            var model = CreateColumnSeriesModel(false);
            return model;
        }

        [Example("ColumnSeries with labels")]
        public static PlotModel SimpleColumnSeriesWithLabels()
        {
            var model = CreateColumnSeriesModel(false);
            var s0 = model.Series[0] as ColumnSeries;
            var s1 = model.Series[1] as ColumnSeries;
            s0.LabelFormatString = "{0}";
            s1.LabelFormatString = "{0:0.00}";
            s1.LabelPlacement = LabelPlacement.Middle;
            s1.TextColor = OxyColors.White;
            return model;
        }

        [Example("Stacked ColumnSeries")]
        public static PlotModel StakcedColumnSeries()
        {
            var model = CreateColumnSeriesModel(true);
            return model;
        }

        [Example("Stacked ColumnSeries with negative values")]
        public static PlotModel StackedNegativeColumnSeries()
        {
            var model = CreateColumnSeriesModelWithNegativeValues(true);
            return model;
        }

        [Example("ColumnSeries and LineSeries")]
        public static PlotModel SimpleColumnAndLineSeries()
        {
            var model = CreateColumnSeriesModel(false);
            model.Title = "ColumnSeries and LineSeries";
            var s1 = new LineSeries { Title = "LineSeries 1" };
            s1.Points.Add(new DataPoint(0, 25));
            s1.Points.Add(new DataPoint(1, 137));
            s1.Points.Add(new DataPoint(2, 18));
            s1.Points.Add(new DataPoint(3, 40));
            model.Series.Add(s1);
            return model;
        }

        [Example("BarSeries")]
        public static PlotModel SimpleHorizontalBarSeries()
        {
            var model = CreateBarSeriesModel(false);
            return model;
        }

        [Example("BarSeries with labels")]
        public static PlotModel SimpleHorizontalBarSeriesWithLabels()
        {
            var model = CreateBarSeriesModel(false);
            var s0 = model.Series[0] as BarSeries;
            var s1 = model.Series[1] as BarSeries;
            s0.LabelFormatString = "{0}";
            s1.LabelFormatString = "{0:0.00}";
            s1.LabelPlacement = LabelPlacement.Inside;
            s1.TextColor = OxyColors.White;
            return model;
        }

        [Example("Stacked BarSeries")]
        public static PlotModel StackedHorizontalBarSeries()
        {
            var model = CreateBarSeriesModel(true);
            return model;
        }

        [Example("Stacked BarSeries with negative values")]
        public static PlotModel StackedNegativeHorizontalBarSeries()
        {
            var model = CreateBarSeriesModelWithNegativeValues(true);
            return model;
        }

        [Example("Logarithmic axis")]
        public static PlotModel LogAxisBarSeries()
        {
            var model = new PlotModel("ColumnSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };
            var s1 = new ColumnSeries { Title = "ColumnSeries 1", BaseValue = 0.1, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(25);
            s1.Values.Add(37);
            s1.Values.Add(18);
            s1.Values.Add(40);
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            model.Series.Add(s1);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Left) { Minimum = 0.1, MinimumPadding = 0, AbsoluteMinimum = 0 });
            return model;
        }

        private static PlotModel CreateBarSeriesModel(bool stacked)
        {
            var model = new PlotModel("BarSeries")
                          {
                              LegendPlacement = LegendPlacement.Outside,
                              LegendPosition = LegendPosition.BottomCenter,
                              LegendOrientation = LegendOrientation.Horizontal,
                              LegendBorderThickness = 0
                          };
            var s1 = new BarSeries { Title = "BarSeries 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(25);
            s1.Values.Add(137);
            s1.Values.Add(18);
            s1.Values.Add(40);
            var s2 = new BarSeries { Title = "BarSeries 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Values.Add(12);
            s2.Values.Add(14);
            s2.Values.Add(120);
            s2.Values.Add(26);
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis(AxisPosition.Bottom) { MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        private static PlotModel CreateColumnSeriesModel(bool stacked)
        {
            var model = new PlotModel("ColumnSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };
            var s1 = new ColumnSeries { Title = "ColumnSeries 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(25);
            s1.Values.Add(137);
            s1.Values.Add(18);
            s1.Values.Add(40);
            var s2 = new ColumnSeries { Title = "ColumnSeries 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Values.Add(12);
            s2.Values.Add(14);
            s2.Values.Add(120);
            s2.Values.Add(26);
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis(AxisPosition.Left) { MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        private static PlotModel CreateBarSeriesModelWithNegativeValues(bool stacked)
        {
            var model = new PlotModel("BarSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };
            var s1 = new BarSeries { Title = "BarSeries 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(25);
            s1.Values.Add(137);
            s1.Values.Add(18);
            s1.Values.Add(40);
            var s2 = new BarSeries { Title = "BarSeries 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Values.Add(-12);
            s2.Values.Add(-14);
            s2.Values.Add(-120);
            s2.Values.Add(-26);
            var s3 = new BarSeries { Title = "BarSeries 3", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s3.Values.Add(21);
            s3.Values.Add(8);
            s3.Values.Add(48);
            s3.Values.Add(3);
            var s4 = new BarSeries { Title = "BarSeries 4", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s4.Values.Add(-8);
            s4.Values.Add(-21);
            s4.Values.Add(-3);
            s4.Values.Add(-48);

            model.Series.Add(s3);
            model.Series.Add(s4);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis(AxisPosition.Bottom) { MinimumPadding = 0.06, MaximumPadding = 0.06, ExtraGridlines = new[] { 0.0 } };
            valueAxis.ExtraGridlineStyle = LineStyle.Solid;
            valueAxis.ExtraGridlineColor = OxyColors.Black;
            valueAxis.ExtraGridlineThickness = 1;
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        private static PlotModel CreateColumnSeriesModelWithNegativeValues(bool stacked)
        {
            var model = new PlotModel("ColumnSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };
            var s1 = new ColumnSeries { Title = "ColumnSeries 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(25);
            s1.Values.Add(137);
            s1.Values.Add(18);
            s1.Values.Add(40);
            var s2 = new ColumnSeries { Title = "ColumnSeries 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Values.Add(-12);
            s2.Values.Add(-14);
            s2.Values.Add(-120);
            s2.Values.Add(-26);
            var s3 = new ColumnSeries { Title = "ColumnSeries 3", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s3.Values.Add(21);
            s3.Values.Add(8);
            s3.Values.Add(48);
            s3.Values.Add(3);
            var s4 = new ColumnSeries { Title = "ColumnSeries 4", IsStacked = true, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s4.Values.Add(-8);
            s4.Values.Add(-21);
            s4.Values.Add(-3);
            s4.Values.Add(-48);

            model.Series.Add(s3);
            model.Series.Add(s4);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis(AxisPosition.Left) { MinimumPadding = 0.06, MaximumPadding = 0.06, ExtraGridlines = new[] { 0.0 } };
            valueAxis.ExtraGridlineStyle = LineStyle.Solid;
            valueAxis.ExtraGridlineColor = OxyColors.Black;
            valueAxis.ExtraGridlineThickness = 1;
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        public class Item
        {
            public string Label { get; set; }
            public double Value1 { get; set; }
            public double Value2 { get; set; }
            public double Value3 { get; set; }
        }

        [Example("ColumnSeries")]
        public static PlotModel ColumnSeries()
        {
            var items = new Collection<Item>
                            {
                                new Item {Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19},
                                new Item {Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9},
                                new Item {Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29}
                            };
            var plotModel1 = new PlotModel { LegendPlacement = LegendPlacement.Outside, Title = "ColumnSeries" };
            var categoryAxis1 = new CategoryAxis { LabelField = "Label", ItemsSource = items, MajorStep = 1, MinorStep = 1 };
            plotModel1.Axes.Add(categoryAxis1);
            var linearAxis1 = new LinearAxis { AbsoluteMinimum = 0, MinimumPadding = 0 };
            plotModel1.Axes.Add(linearAxis1);
            var barSeries1 = new ColumnSeries
                {
                    FillColor = OxyColor.FromArgb(255, 78, 154, 6),
                    ValueField = "Value1",
                    Title = "2009",
                    ItemsSource = items
                };
            plotModel1.Series.Add(barSeries1);
            var barSeries2 = new ColumnSeries
                {
                    FillColor = OxyColor.FromArgb(255, 200, 141, 0),
                    ValueField = "Value2",
                    Title = "2010",
                    ItemsSource = items
                };
            plotModel1.Series.Add(barSeries2);
            var barSeries3 = new ColumnSeries
                {
                    FillColor = OxyColor.FromArgb(255, 204, 0, 0),
                    ValueField = "Value3",
                    Title = "2011",
                    ItemsSource = items
                };
            plotModel1.Series.Add(barSeries3);
            return plotModel1;
        }

        public class HistogramBin
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }

        [Example("Histogram (random numbers)")]
        public static PlotModel Histogram()
        {
            int n = 1000;
            int binCount = 20;
            var bins = new HistogramBin[binCount];
            for (int i = 0; i < bins.Length; i++) bins[i] = new HistogramBin() { Label = i.ToString() };

            var r = new Random();
            for (int i = 0; i < n; i++)
            {
                int value = r.Next(binCount);
                bins[value].Value++;
            }

            var temp = new PlotModel();

            temp.Series.Add(new ColumnSeries { BarWidth = 1, ItemsSource = bins, ValueField = "Value" });

            temp.Axes.Add(new CategoryAxis { ItemsSource = bins, LabelField = "Label" });
            temp.Axes.Add(new LinearAxis { MinimumPadding = 0, AbsoluteMinimum = 0 });

            return temp;
        }
    }
}