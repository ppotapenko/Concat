#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace Concat
{
    /// <summary>
    ///     Save text from files into result file on a worker thread with progress reporting
    /// </summary>
    public class FileCounter : IProgressOperation
    {
        private int _total;
        private bool _isCancelationPending;
        private readonly string _dirPath;
        private readonly Regex _filterExt;
        private readonly List<string> _ignoreFolders;
        private readonly List<string> _globalIgnoreFolders;
        private readonly List<FileInfo> _files;

        public FileCounter(string dirPath, string filterExt, List<string> ignoreFolders,
            List<string> globalIgnorefolders)
        {
            _ignoreFolders = ignoreFolders;
            _globalIgnoreFolders = globalIgnorefolders;
            filterExt = string.IsNullOrEmpty(filterExt) ? @"\w*" : filterExt;
            _filterExt = new Regex(string.Format(@"$(?<=\.({0}))", filterExt.Replace(',', '|')), RegexOptions.IgnoreCase);
            _dirPath = dirPath;
            _total = 0;
            Current = 0;
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
            var dir = new DirectoryInfo(path);
            var dirs = dir.GetDirectories("*", SearchOption.TopDirectoryOnly)
                .Where(d => !_globalIgnoreFolders.Contains(Path.DirectorySeparatorChar + d.Name))
                .Where(d => !_ignoreFolders.Contains(d.FullName.Substring(_dirPath.Length)))
                .OrderBy(d => d.Name);
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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadDirectory(_dirPath);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnComplete(EventArgs.Empty);
        }

        private void OnProgressTotalChanged(EventArgs e)
        {
            if (ProgressTotalChanged != null)
            {
                ProgressTotalChanged(this, e);
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
            private set
            {
                _total = value;
                OnProgressTotalChanged(EventArgs.Empty);
            }
        }

        public int Current { get; private set; }

        private Regex FilterExt
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
