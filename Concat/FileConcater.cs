#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

#endregion

namespace Concat
{
    /// <summary>
    ///     Save text from files into result file on a worker thread with progress reporting
    /// </summary>
    public class FileConcater : IProgressOperation
    {
        private readonly List<FileInfo> _fileInfos;
        private readonly string _savePath;
        private readonly int _total;
        private readonly string _fileTitle;
        private int _current;
        private bool _isCancelationPending;

        public FileConcater(List<FileInfo> fileInfos, string savePath, int total, string fileTitle)
        {
            _fileInfos = fileInfos;
            _savePath = savePath;
            _total = total;
            _fileTitle = fileTitle;
            _current = 0;
            _isCancelationPending = false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var file = File.AppendText(_savePath))
            {
                foreach (var fileInfo in _fileInfos)
                {
                    if (_isCancelationPending)
                    {
                        return;
                    }
                    using (var sr = File.OpenText(fileInfo.FullName))
                    {
                        file.WriteLine(_fileTitle.Replace("\\n", Environment.NewLine));
                        file.WriteLine(_fileTitle.Replace("\\r", Environment.NewLine));
                        file.WriteLine(fileInfo.FullName);
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            file.WriteLine(s);
                        }
                    }
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(Environment.NewLine);
                    Current++;
                }
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnComplete(EventArgs.Empty);
        }

        private void OnProgressChanged(EventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        private void OnComplete(EventArgs e)
        {
            if (Complete != null)
            {
                Complete(this, e);
            }
        }

        #region IProgressOperation Members

        public int Total
        {
            get { return _total; }
        }

        public int Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                OnProgressChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Starts the background operation
        /// </summary>
        public void Start()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        /// <summary>
        ///     Requests cancelation
        /// </summary>
        public void CancelAsync()
        {
            _isCancelationPending = true;
        }

        public event EventHandler ProgressChanged;
        public event EventHandler ProgressTotalChanged;
        public event EventHandler Complete;

        #endregion
    }
}
