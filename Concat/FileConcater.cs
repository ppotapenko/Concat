#region using

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

namespace SimpleWPFProgressWindow
{
    /// <summary>
    ///     Save text from files into result file on a worker thread with progress reporting
    /// </summary>
    public class FileConcater : IProgressOperation
    {
        private int _total;
        private int _current;
        private bool _isCancelationPending;
        private string _dirPath;
        private string _filterExt;
        private string _ignoreFolders;

        public FileConcater(string dirPath, string filterExt, string ignoreFolders)
        {
            _ignoreFolders = ignoreFolders;
            _filterExt = filterExt;
            _dirPath = dirPath;
            _total = 0;
            _current = 0;
            _isCancelationPending = false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var eventLogs = EventLog.GetEventLogs();
            
            //get the total number of lines in the event logs
            //so a progress bar maximum can be updated
            var numRows = 0;
            foreach (var eventLog in eventLogs)
            {
                numRows += eventLog.Entries.Count;
            }
            Total = numRows;

            foreach (var eventLog in eventLogs)
            {
                //only write the log if entries exist
                if (eventLog.Entries.Count > 0)
                {
                    using (var memStream = new MemoryStream())
                    {
                        var now = DateTime.Now;
                        var fileName = String.Format("{0}-{1:00}-{2:00}_{3:00}{4:00}{5:00}_EventLog_{6}.log",
                            new object[]
                            {now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, eventLog.LogDisplayName});

                        foreach (EventLogEntry eventLogEntry in eventLog.Entries)
                        {
                            //exit if the user cancels
                            if (_isCancelationPending)
                            {
                                return;
                            }

                            var row = String.Format("{0} {1} {2} {3}", eventLogEntry.EntryType,
                                eventLogEntry.TimeWritten, eventLogEntry.Source, eventLogEntry.Message);

                            row = row.Replace("\r", String.Empty);
                            row = row.Replace("\n", String.Empty);
                            row += "\r\n";
                            var asciiEncoding = new ASCIIEncoding();
                            var rowData = asciiEncoding.GetBytes(row);
                            memStream.Write(rowData, 0, rowData.Length);

                            //notify that the current event log line has changed
                            //(Updates progress bar)
                            Current++;
                        }

                        //write the log
                        using (var fs = File.Create(fileName))
                        {
                            memStream.WriteTo(fs);
                        }
                    }
                }
            }
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
