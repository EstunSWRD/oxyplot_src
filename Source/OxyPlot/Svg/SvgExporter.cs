// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.IO;

    /// <summary>
    /// Exports plot models to svg.
    /// </summary>
    public static class SvgExporter
    {
        #region Public Methods

        /// <summary>
        /// Exports the specified model to a file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        public static void Export(PlotModel model, string fileName, double width, double height)
        {
            using (var svgrc = new SvgRenderContext(fileName, width, height))
            {
                model.Update();
                model.Render(svgrc);
            }
        }

        /// <summary>
        /// Exports to string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="isDocument">if set to <c>true</c>, the xml headers will be included (?xml and !DOCTYPE).</param>
        /// <returns>The plot as a svg string.</returns>
        public static string ExportToString(PlotModel model, double width, double height, bool isDocument = false)
        {
            string svg;
            using (var ms = new MemoryStream())
            {
                var svgrc = new SvgRenderContext(ms, width, height, isDocument);
                model.Update();
                model.Render(svgrc);
                svgrc.Complete();
                svgrc.Flush();
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                svg = sr.ReadToEnd();
                svgrc.Close();
            }

            return svg;
        }

        #endregion
    }
}