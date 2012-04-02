namespace ExampleLibrary
{
    using System.Diagnostics;
    using System.Linq;

    using OxyPlot;

    /// <summary>
    /// The examples base class.
    /// </summary>
    public abstract class ExamplesBase
    {
        /// <summary>
        /// Creates a plot model using the ExampleAttribute as title.
        /// </summary>
        /// <returns>A new plot model.</returns>
        protected static PlotModel CreatePlotModel()
        {
            return new PlotModel(GetTitle(2));
        }

        /// <summary>
        /// Gets the title from the ExampleAttribute of the calling method.
        /// </summary>
        /// <param name="frameIndex">Index of the stack frame.</param>
        /// <returns>
        /// The title.
        /// </returns>
        protected static string GetTitle(int frameIndex = 1)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(frameIndex);
            var m = sf.GetMethod();
            var ea = m.GetCustomAttributes(typeof(ExampleAttribute), false).First() as ExampleAttribute;
            return ea.Title;
        }
    }
}