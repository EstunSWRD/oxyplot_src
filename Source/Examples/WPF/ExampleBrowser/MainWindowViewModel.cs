﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
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

        public ICollectionView ExamplesView { get; set; }

        private ExampleInfo selectedExample;
        public ExampleInfo SelectedExample
        {
            get { return selectedExample; }
            set { selectedExample = value; RaisePropertyChanged("SelectedExample"); }
        }

        public MainWindowViewModel()
        {
            Examples = ExampleLibrary.Examples.GetList();
            ExamplesView = CollectionViewSource.GetDefaultView(Examples.OrderBy(e => e.Category));
            ExamplesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
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
}