// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAnnotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a text object annotation.
    /// </summary>
    public class TextAnnotation : Annotation
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TextAnnotation" /> class.
        /// </summary>
        public TextAnnotation()
        {
            this.Color = OxyColors.Blue;
            this.Stroke = OxyColors.Black;
            this.Background = null;
            this.StrokeThickness = 1;
            this.Rotation = 0;
            this.HorizontalAlignment = HorizontalTextAlign.Center;
            this.VerticalAlignment = VerticalTextAlign.Bottom;
            this.Padding = new OxyThickness(4);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the fill color of the background rectangle.
        /// </summary>
        /// <value> The background. </value>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets or sets the color of the text.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the horizontal alignment.
        /// </summary>
        /// <value> The horizontal alignment. </value>
        public HorizontalTextAlign HorizontalAlignment { get; set; }

        /// <summary>
        ///   Gets or sets the position offset (screen coordinates).
        /// </summary>
        /// <value> The offset. </value>
        public ScreenVector Offset { get; set; }

        /// <summary>
        ///   Gets or sets the padding of the background rectangle.
        /// </summary>
        /// <value> The padding. </value>
        public OxyThickness Padding { get; set; }

        /// <summary>
        ///   Gets or sets the position of the text.
        /// </summary>
        public DataPoint Position { get; set; }

        /// <summary>
        ///   Gets or sets the rotation angle (degrees).
        /// </summary>
        /// <value> The rotation. </value>
        public double Rotation { get; set; }

        /// <summary>
        ///   Gets or sets the stroke color of the background rectangle.
        /// </summary>
        /// <value> The stroke color. </value>
        public OxyColor Stroke { get; set; }

        /// <summary>
        ///   Gets or sets the stroke thickness of the background rectangle.
        /// </summary>
        /// <value> The stroke thickness. </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the vertical alignment.
        /// </summary>
        /// <value> The vertical alignment. </value>
        public VerticalTextAlign VerticalAlignment { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the arrow annotation.
        /// </summary>
        /// <param name="rc">
        /// The render context. 
        /// </param>
        /// <param name="model">
        /// The plot model. 
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            var position = this.XAxis.Transform(this.Position.X, this.Position.Y, this.YAxis);
            position.X += this.Offset.X;
            position.Y += this.Offset.Y;

            var clippingRect = this.GetClippingRect();

            var textSize = rc.MeasureText(
                this.Text, model.ActualAnnotationFont, model.AnnotationFontSize, FontWeights.Normal);

            const double MinDistSquared = 4;

            var textBounds = GetTextBounds(
                position, textSize, this.Padding, this.Rotation, this.HorizontalAlignment, this.VerticalAlignment);
            rc.DrawClippedPolygon(
                textBounds, clippingRect, MinDistSquared, this.Background, this.Stroke, this.StrokeThickness);

            if (clippingRect.Contains(position))
            {
                rc.DrawText(
                    position, 
                    this.Text, 
                    model.TextColor, 
                    model.ActualAnnotationFont, 
                    model.AnnotationFontSize, 
                    FontWeights.Normal, 
                    this.Rotation, 
                    this.HorizontalAlignment, 
                    this.VerticalAlignment);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the coordinates of the (rotated) background rectangle.
        /// </summary>
        /// <param name="position">
        /// The position. 
        /// </param>
        /// <param name="size">
        /// The size. 
        /// </param>
        /// <param name="padding">
        /// The padding. 
        /// </param>
        /// <param name="rotation">
        /// The rotation. 
        /// </param>
        /// <param name="horizontalAlignment">
        /// The horizontal alignment. 
        /// </param>
        /// <param name="verticalAlignment">
        /// The vertical alignment. 
        /// </param>
        /// <returns>
        /// The background rectangle coordinates.
        /// </returns>
        private static IList<ScreenPoint> GetTextBounds(
            ScreenPoint position, 
            OxySize size, 
            OxyThickness padding, 
            double rotation, 
            HorizontalTextAlign horizontalAlignment, 
            VerticalTextAlign verticalAlignment)
        {
            double left, right, top, bottom;
            switch (horizontalAlignment)
            {
                case HorizontalTextAlign.Center:
                    left = -size.Width * 0.5;
                    right = -left;
                    break;
                case HorizontalTextAlign.Right:
                    left = -size.Width;
                    right = 0;
                    break;
                default:
                    left = 0;
                    right = size.Width;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalTextAlign.Middle:
                    top = -size.Height * 0.5;
                    bottom = -top;
                    break;
                case VerticalTextAlign.Bottom:
                    top = -size.Height;
                    bottom = 0;
                    break;
                default:
                    top = 0;
                    bottom = size.Height;
                    break;
            }

            double cost = Math.Cos(rotation / 180 * Math.PI);
            double sint = Math.Sin(rotation / 180 * Math.PI);
            var u = new ScreenVector(cost, sint);
            var v = new ScreenVector(-sint, cost);
            var polygon = new ScreenPoint[4];
            polygon[0] = position + u * (left - padding.Left) + v * (top - padding.Top);
            polygon[1] = position + u * (right + padding.Right) + v * (top - padding.Top);
            polygon[2] = position + u * (right + padding.Right) + v * (bottom + padding.Bottom);
            polygon[3] = position + u * (left - padding.Left) + v * (bottom + padding.Bottom);
            return polygon;
        }

        #endregion
    }
}