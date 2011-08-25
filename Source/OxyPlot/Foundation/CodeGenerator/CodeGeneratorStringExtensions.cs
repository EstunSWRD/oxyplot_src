﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGeneratorStringExtensions.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The code generator string extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The code generator string extensions.
    /// </summary>
    public static class CodeGeneratorStringExtensions
    {
        #region Public Methods

        /// <summary>
        /// Converts the value of this instance to c# code.
        /// </summary>
        /// <param name="value">
        /// The instance.
        /// </param>
        /// <returns>
        /// C# code.
        /// </returns>
        public static string ToCode(this string value)
        {
            value = value.Replace("\"", "\\\"");
            value = value.Replace("\n", "\\n");
            value = value.Replace("\t", "\\t");
            return "\"" + value + "\"";
        }

        /// <summary>
        /// Converts the value of this instance to c# code.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// C# code.
        /// </returns>
        public static string ToCode(this bool value)
        {
            return value.ToString().ToLower();
        }

        /// <summary>
        /// Converts the value of this instance to c# code.
        /// </summary>
        /// <param name="value">
        /// The instance.
        /// </param>
        /// <returns>
        /// C# code.
        /// </returns>
        public static string ToCode(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of this instance to c# code.
        /// </summary>
        /// <param name="value">
        /// The instance.
        /// </param>
        /// <returns>
        /// C# code.
        /// </returns>
        public static string ToCode(this Enum value)
        {
            return string.Format("{0}.{1}", value.GetType().Name, value);
        }

        /// <summary>
        /// Converts the value of this instance to c# code.
        /// </summary>
        /// <param name="value">
        /// The instance.
        /// </param>
        /// <returns>
        /// C# code.
        /// </returns>
        public static string ToCode(this double value)
        {
            if (double.IsNaN(value))
            {
                return "double.NaN";
            }

            if (double.IsPositiveInfinity(value))
            {
                return "double.PositiveInfinity";
            }

            if (double.IsNegativeInfinity(value))
            {
                return "double.NegativeInfinity";
            }

            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of this instance to c# code.
        /// </summary>
        /// <param name="value">
        /// The instance.
        /// </param>
        /// <returns>
        /// C# code.
        /// </returns>
        public static string ToCode(this object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is int)
            {
                return ((int)value).ToCode();
            }

            if (value is double)
            {
                return ((double)value).ToCode();
            }

            if (value is string)
            {
                return ((string)value).ToCode();
            }

            if (value is bool)
            {
                return ((bool)value).ToCode();
            }

            if (value is Enum)
            {
                return ((Enum)value).ToCode();
            }

            if (value is ICodeGenerating)
            {
                return ((ICodeGenerating)value).ToCode();
            }

            return null;
        }

        #endregion
    }
}