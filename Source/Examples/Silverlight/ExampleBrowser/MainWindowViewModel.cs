﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using ExampleLibrary;
using OxyPlot;

namespace ExampleBrowser
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IEnumerable<ExampleInfo> examples;
        public IEnumerable<ExampleInfo> Examples
        {
            get { return examples; }
            set { examples = value; RaisePropertyChanged("Examples"); }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value;
                var cc = value as ContentControl;
                SelectedExample = cc.Content as ExampleInfo;
            }
        }

        private ExampleInfo selectedExample;
        public ExampleInfo SelectedExample
        {
            get { return selectedExample; }
            set { selectedExample = value; RaisePropertyChanged("SelectedExample"); }
        }

        public MainWindowViewModel()
        {
            Examples = ExampleLibrary.Examples.GetList();
            // SelectedExample = Examples.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }


    }

    // Simple IGroupingItemsControlConverterSelector implementation for grouping by an Animal's species
    public class ExampleInfoGroupSelector : IGroupingItemsControlConverterSelector
    {
        public Func<object, IComparable> GetGroupSelector()
        {
            return (o) => ((ExampleInfo)o).Category;
        }
    }
}