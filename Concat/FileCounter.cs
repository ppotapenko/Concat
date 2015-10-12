#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace SimpleWPFProgressWindow
{
    /// <summary>
    ///     Save text from files into result file on a worker thread with progress reporting
    /// </summary>
    public class FileCounter : IProgressOperation
    {
        private int _total;
        private int _current;
        private bool _isCancelationPending;
        private readonly string _dirPath;
        private readonly Regex _filterExt;
        private readonly List<string> _ignoreFolders;
        private readonly List<FileInfo> _files;

        public FileCounter(string dirPath, string filterExt, List<string> ignoreFolders)
        {
            _ignoreFolders = ignoreFolders;
            filterExt = string.IsNullOrEmpty(filterExt) ? @"\w*" : filterExt;
            _filterExt = new Regex(string.Format(@"$(?<=\.({0}))", filterExt.Replace(',', '|')), RegexOptions.IgnoreCase);
            _dirPath = dirPath;
            _total = 0;
            _current = 0;
            _isCancelationPending = false;
            _files = new List<FileInfo>();
        }

        private void ReadDirectory(string path)
        {
            //exit if the user cancels
            if (_isCancelationPending)
            {
                return;
            }
            if (_ignoreFolders.Find(p => _dirPath + p == path) == null)
            {
                var dir = new DirectoryInfo(path);
                var dirs = dir.GetDirectories("*", SearchOption.TopDirectoryOnly).OrderBy(d => d.Name);
                if (dirs.Any())
                {
                    foreach (var childDir in dirs)
                    {
                        ReadDirectory(childDir.FullName);
                    }
                }
                var files = dir.GetFiles().Where(f => FilterExt.IsMatch(f.Name)).OrderBy(f => f.Name).ToList();
                if (files.Any())
                {
                    Files.AddRange(files);
                    Total += files.Count();
                }
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadDirectory(_dirPath);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnComplete(EventArgs.Empty);
        }

        protected virtual void OnProgressChanged(EventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        protected virtual void OnProgressTotalChanged(EventArgs e)
        {
            if (ProgressTotalChanged != null)
            {
                ProgressTotalChanged(this, e);
            }
        }

        protected virtual void OnComplete(EventArgs e)
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
            private set
            {
                _total = value;
                OnProgressTotalChanged(EventArgs.Empty);
            }
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

        public Regex FilterExt
        {
            get { return _filterExt; }
        }

        public List<FileInfo> Files
        {
            get { return _files; }
        }

        /// <summary>
        ///     Starts the background operation that will export the event logs
        /// </summary>
        public void Start()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        /// <summary>
        ///     Requests cancelation of the event log exporting
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
