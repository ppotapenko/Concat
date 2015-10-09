using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace SimpleWPFProgressWindow
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window, INotifyPropertyChanged
    {
        private IProgressOperation _operation;

        public ProgressWindow(IProgressOperation operation)
        {
            _operation = operation;
            _operation.ProgressChanged += _operation_ProgressChanged;
            _operation.ProgressTotalChanged += _operation_TotalChanged;
            _operation.Complete += _operation_Complete;

            InitializeComponent();

            Loaded += ProgressWindow_Loaded;
        }

        void ProgressWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _operation.Start();
        }

        void _operation_Complete(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof (FileCounter))
            {
                _operation.ProgressChanged += _operation_ProgressChanged;
                _operation.ProgressTotalChanged += _operation_TotalChanged;
                _operation.Complete += _operation_Complete;
            }
            Close();
        }

        void _operation_ProgressChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Current");
        }

        void _operation_TotalChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Total");
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this._operation.CancelAsync();
        }

        public int Current
        {
            get
            {
                return this._operation.Current;
            }
        }

        public int Total
        {
            get
            {
                return this._operation.Total;
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion       
    }
}
