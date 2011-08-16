﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterPoint.cs" company="OxyPlot">
//   See http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// ScatterPoint - used in ScatterSeries.
    /// </summary>
    public struct ScatterPoint : IDataPoint
    {
        #region Constants and Fields

        /// <summary>
        /// The size.
        /// </summary>
        internal double size;

        /// <summary>
        /// The tag.
        /// </summary>
        internal object tag;

        /// <summary>
        /// The value.
        /// </summary>
        internal double value;

        /// <summary>
        /// The x.
        /// </summary>
        internal double x;

        /// <summary>
        /// The y.
        /// </summary>
        internal double y;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterPoint"/> struct.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="tag">
        /// The tag.
        /// </param>
        public ScatterPoint(double x, double y, double size = double.NaN, double value = double.NaN, object tag = null)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.value = value;
            this.tag = tag;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public double Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag
        {
            get
            {
                return this.tag;
            }

            set
            {
                this.tag = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.y = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.x + " " + this.y;
        }

        #endregion
    }
}