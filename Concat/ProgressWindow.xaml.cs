﻿#region using

using System;
using System.ComponentModel;
using System.Windows;

#endregion

namespace Concat
{
    /// <summary>
    ///     Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : INotifyPropertyChanged
    {
        private IProgressOperation _operation;
        private readonly string _fileName;
        private readonly string _fileTitle;

        public ProgressWindow(IProgressOperation operation, string fileName, string fileTitle)
        {
            _operation = operation;
            _fileName = fileName;
            _fileTitle = fileTitle;
            _operation.ProgressChanged += _operation_ProgressChanged;
            _operation.ProgressTotalChanged += _operation_TotalChanged;
            _operation.Complete += _operation_Complete;

            InitializeComponent();

            Loaded += ProgressWindow_Loaded;
        }

        private void ProgressWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _operation.Start();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            _operation = new FileConcater(((FileCounter) _operation).Files, _fileName, Total, _fileTitle);


            _operation.ProgressChanged += _operation_ProgressChanged;
            _operation.ProgressTotalChanged += _operation_TotalChanged;
            _operation.Complete += _operation_Complete;

            _operation.Start();
            TextBlockProcessing.Text = "Обработано файлов:";
            TextBlockOf.Visibility = Visibility.Visible;
            TextBlockCurrent.Visibility = Visibility.Visible;
            ButtonStart.Visibility = Visibility.Hidden;
        }

        private void _operation_Complete(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof (FileCounter))
            {
                ButtonStart.Visibility = Visibility.Visible;
            }
            else
            {
                Close();
                MessageBox.Show("Файл успешно сформирован.");
            }
        }

        private void _operation_ProgressChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Current");
        }

        private void _operation_TotalChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Total");
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            _operation.Complete -= _operation_Complete;
            _operation.CancelAsync();
            Close();
        }

        public int Current
        {
            get { return _operation.Current; }
        }

        public int Total
        {
            get { return _operation.Total; }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        ///     Notify property changed
        /// </summary>
        /// <param name="propertyName">Property name</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
