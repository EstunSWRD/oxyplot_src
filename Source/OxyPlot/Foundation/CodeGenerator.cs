﻿namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Attribute that controls if code should be generated for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CodeGenerationAttribute : Attribute
    {
        #region Constructors and Destructors

        public CodeGenerationAttribute(bool generateCode)
        {
            this.GenerateCode = generateCode;
        }

        #endregion

        #region Public Properties

        public bool GenerateCode { get; set; }

        #endregion
    }

    /// <summary>
    /// Provides functionality to generate c# code of an object.
    /// </summary>
    public interface ICodeGenerating
    {
        #region Public Methods

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        string ToCode();

        #endregion
    }

    /// <summary>
    /// This class generates c# code for the specified PlotModel.
    /// This is useful for creating examples or unit tests.
    /// Press Ctrl+Alt+C in a plot to copy code to the clipboard.
    /// 
    /// Usage:
    ///   var cg = new CodeGenerator(myPlotModel);
    ///   Clipboard.SetText(cg.ToCode());
    /// </summary>
    public class CodeGenerator
    {
        #region Constants and Fields

        private readonly StringBuilder sb;

        private readonly HashSet<string> variables;

        private string indentString;

        private int indents;

        #endregion

        #region Constructors and Destructors

        public CodeGenerator(PlotModel model)
        {
            this.variables = new HashSet<string>();
            this.sb = new StringBuilder();
            this.Indents = 8;
            this.AppendLine("[Example(\"{0}\")]", model.Title);
            string methodName = this.MakeValidVariableName(model.Title) ?? "Untitled";
            this.AppendLine("public static PlotModel {0}()", methodName);
            this.AppendLine("{");
            this.Indents += 4;
            string modelName = this.Add(model);
            this.AddChildren(modelName, "Axes", model.Axes);
            this.AddChildren(modelName, "Series", model.Series);
            this.AddChildren(modelName, "Annotations", model.Annotations);
            this.AppendLine("return {0};", modelName);
            this.Indents -= 4;
            this.AppendLine("}");
        }

        #endregion

        #region Properties

        private int Indents
        {
            get
            {
                return this.indents;
            }
            set
            {
                this.indents = value;
                this.indentString = new string(' ', value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the c# code for this model.
        /// </summary>
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            return this.sb.ToString();
        }

        #endregion

        #region Methods

        private string Add(object obj)
        {
            Type type = obj.GetType();
            object defaultInstance = Activator.CreateInstance(type);
            string varName = this.GetNewVariableName(type);
            this.variables.Add(varName);
            this.AppendLine("var {0} = new {1}();", varName, type.Name);
            this.SetProperties(obj, varName, defaultInstance);
            return varName;
        }

        private void AddChildren(string name, string collectionName, IEnumerable children)
        {
            foreach (object child in children)
            {
                string childName = this.Add(child);
                this.AppendLine("{0}.{1}.Add({2});", name, collectionName, childName);
            }
        }

        private void AddItems(string name, IList list)
        {
            foreach (object item in list)
            {
                var cgi = item as ICodeGenerating;
                if (cgi != null)
                {
                    this.AppendLine("{0}.Add({1});", name, cgi.ToCode());
                }
            }
        }

        private void AppendLine(string format, params object[] args)
        {
            if (args.Length > 0)
            {
                this.sb.AppendLine(this.indentString + String.Format(CultureInfo.InvariantCulture, format, args));
            }
            else
            {
                this.sb.AppendLine(this.indentString + format);
            }
        }

        private bool AreListsEqual(IList list1, IList list2)
        {
            if (list1 == null || list2 == null)
            {
                return false;
            }
            if (list1.Count != list2.Count)
            {
                return false;
            }
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private T GetFirstAttribute<T>(PropertyInfo pi) where T : class
        {
            foreach (T a in pi.GetCustomAttributes(typeof(CodeGenerationAttribute), true))
            {
                return a;
            }
            return null;
        }

        private string GetNewVariableName(Type type)
        {
            string prefix = type.Name;
            prefix = Char.ToLower(prefix[0]) + prefix.Substring(1);
            int i = 1;
            while (this.variables.Contains(prefix + i))
            {
                i++;
            }
            return prefix + i;
        }

        /// <summary>
        /// Makes a valid variable of a string.
        /// Invalid characters will simply be removed.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        private string MakeValidVariableName(string title)
        {
            if (title == null)
            {
                return null;
            }
            var regex = new Regex("[a-zA-Z_][a-zA-Z0-9_]*");
            var result = new StringBuilder();
            foreach (char c in title)
            {
                string s = c.ToString();
                if (regex.Match(s).Success)
                {
                    result.Append(s);
                }
            }
            return result.ToString();
        }

        private void SetEnumProperty(string name, Enum value)
        {
            this.AppendLine("{0} = {1}.{2};", name, value.GetType(), value);
        }

        private void SetProperties(object o, string varName, object defaultValues)
        {
            Type type = o.GetType();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                // check the [CodeGeneration] attribute
                var cga = this.GetFirstAttribute<CodeGenerationAttribute>(pi);
                if (cga != null && !cga.GenerateCode)
                {
                    continue;
                }

                // only properties with public setters are used
                MethodInfo sm = pi.GetSetMethod();
                if (sm == null || !sm.IsPublic)
                {
                    continue;
                }

                string name = varName + "." + pi.Name;
                object value = pi.GetValue(o, null);
                object defaultValue = pi.GetValue(defaultValues, null);

                // skip default values
                if ((value != null && value.Equals(defaultValue)) || value == defaultValue)
                {
                    continue;
                }
                if (this.AreListsEqual(value as IList, defaultValue as IList))
                {
                    continue;
                }

                // Only List<T>s where T:ICodeGenerating will be added.
                var list = value as IList;
                if (list != null)
                {
                    Type listType = list.GetType();
                    Type[] gargs = listType.GetGenericArguments();
                    if (gargs.Length > 0)
                    {
                        bool isCodeGenerating = gargs[0].GetInterfaces().Any(x => x == typeof(ICodeGenerating));
                        if (!isCodeGenerating)
                        {
                            continue;
                        }
                    }
                    this.AddItems(name, list);
                    continue;
                }

                this.SetProperty(pi.PropertyType, name, value);
            }
        }

        private void SetProperty(Type propertyType, string name, object value)
        {
            if (value == null)
            {
                this.AppendLine("{0} = null;", name);
                return;
            }
            if (propertyType == typeof(string))
            {
                this.SetProperty(name, (string)value);
                return;
            }
            if (value is ICodeGenerating)
            {
                this.SetProperty(name, (ICodeGenerating)value);
                return;
            }
            if (value is Enum)
            {
                this.SetEnumProperty(name, (Enum)value);
                return;
            }
            if (value is Boolean)
            {
                this.SetProperty(name, (bool)value);
            }
            if (value is double)
            {
                this.SetProperty(name, (double)value);
            }
            if (value is int)
            {
                SetProperty(name, value);
            }
            // SetProperty(name, value);
        }

        private void SetProperty(string name, object value)
        {
            this.AppendLine("{0} = {1};", name, value);
        }

        private void SetProperty(string name, ICodeGenerating value)
        {
            this.AppendLine("{0} = {1};", name, value.ToCode());
        }

        private void SetProperty(string name, string value)
        {
            this.AppendLine("{0} = \"{1}\";", name, value);
        }

        private void SetProperty(string name, bool value)
        {
            this.AppendLine("{0} = {1};", name, value.ToString().ToLower());
        }

        private void SetProperty(string name, double value)
        {
            string v = value.ToString(CultureInfo.InvariantCulture);
            if (double.IsNaN(value))
            {
                v = "double.NaN";
            }
            if (double.IsPositiveInfinity(value))
            {
                v = "double.PositiveInfinity";
            }
            if (double.IsNegativeInfinity(value))
            {
                v = "double.NegativeInfinity";
            }
            this.AppendLine("{0} = {1};", name, v);
        }

        #endregion
    }
}