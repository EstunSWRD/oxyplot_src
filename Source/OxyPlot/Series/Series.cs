﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Abstract base class for all series.
//   This class contains internal methods that should be called only from the PlotModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Abstract base class for all series.
    /// This class contains internal methods that should be called only from the PlotModel.
    /// </summary>
    [Serializable]
    public abstract class Series : ITrackableSeries
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the background color of the series.
        ///   The background area is defined by the x and y axes.
        /// </summary>
        /// <value>The background color.</value>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets or sets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a format string used for the tracker.
        /// </summary>
        public string TrackerFormatString { get; set; }

        /// <summary>
        /// Gets or sets the key for the tracker to use on this series.
        /// </summary>
        public string TrackerKey { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// interpolate if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        public abstract TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        /// <summary>
        /// Renders the Series on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public abstract void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The legend rectangle.
        /// </param>
        public abstract void RenderLegend(IRenderContext rc, OxyRect legendBox);

        #endregion

        #region Methods

        /// <summary>
        /// Check if this data series requires X/Y axes.
        /// (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns>
        /// True if no axes are required.
        /// </returns>
        protected internal abstract bool AreAxesRequired();

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
        protected internal abstract void EnsureAxes(Collection<Axis> axes, Axis defaultXAxis, Axis defaultYAxis);

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">
        /// An axis.
        /// </param>
        /// <returns>
        /// True if the axis is in use.
        /// </returns>
        protected internal abstract bool IsUsing(IAxis axis);

        /// <summary>
        /// Sets default values (colors, line style etc) from the plotmodel.
        /// </summary>
        /// <param name="model">
        /// A plot model.
        /// </param>
        protected internal abstract void SetDefaultValues(PlotModel model);

        /// <summary>
        /// Updates the axis maximum and minimum values.
        /// </summary>
        protected internal abstract void UpdateAxisMaxMin();

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal abstract void UpdateData();

        /// <summary>
        /// Updates the maximum and minimum of the series.
        /// </summary>
        protected internal abstract void UpdateMaxMin();

        #endregion
    }
}