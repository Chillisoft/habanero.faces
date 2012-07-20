using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win.Async
{
    public class BackgroundWorkerWin
    {
        public delegate bool BackgroundWorkerMethodDelegate(ConcurrentDictionary<string, object> data);
        public delegate void UIWorkerMethodDelegate(ConcurrentDictionary<string, object> data);
        public delegate void BackgroundWorkerExceptionHandlerDelegate(Exception ex);

        public IActionDispatcher ActionDispatcher { get; protected set; }
        public BackgroundWorkerMethodDelegate BackgroundWorker { get; set; }
        public UIWorkerMethodDelegate OnSuccess { get; set; }
        public UIWorkerMethodDelegate OnCancelled { get; set; }
        public BackgroundWorkerExceptionHandlerDelegate OnException { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }

        private Thread _thread;

        public void Run()
        {
            this._thread = new Thread(this.RunBackgroundWorker);
            this._thread.IsBackground = true;
            this._thread.Start();
        }

        public void WaitForBackgroundWorkerToComplete()
        {
            if (this._thread == null)
                return;
            if (this._thread.IsAlive)
                this._thread.Join();
        }

        protected void RunBackgroundWorker()
        {
            var success = true;
            if (this.BackgroundWorker != null)
            {
                foreach (var del in this.BackgroundWorker.GetInvocationList())
                {
                    object result;
                    try
                    {
                        result = del.DynamicInvoke(this.Data);
                    }
                    catch (Exception ex)
                    {
                        this.RunExceptionDelegate(ex);
                        success = false;
                        break;
                    }
                    try
                    {
                        success = (bool)result;
                        if (!success)
                            break;
                    }
                    catch (Exception)
                    {
                        this.RunExceptionDelegate(new Exception("Delegate method returns a result which cannot be coalesced into a boolean value"));
                        success = false;
                        break;
                    }
                }
            }
            this.RunUIWorkerDelegate(success);
        }

        protected void RunExceptionDelegate(Exception ex)
        {
            if (this.OnException != null)
            {
                this.ActionDispatcher.Dispatch(() => { this.OnException(ex); });
            }
        }

        protected void RunUIWorkerDelegate(bool success)
        {
            if (success && (this.OnSuccess != null))
            {
                this.ActionDispatcher.Dispatch(() => { this.OnSuccess(this.Data); });
            }
            else if (this.OnCancelled != null)
            {
                this.ActionDispatcher.Dispatch(() => { this.OnCancelled(this.Data); });
            }
        }

        public static BackgroundWorkerWin Run(ActionDispatcher dispatcher, ConcurrentDictionary<string, object> data, BackgroundWorkerMethodDelegate backgroundWorker, UIWorkerMethodDelegate onSuccess, UIWorkerMethodDelegate onCancel, BackgroundWorkerExceptionHandlerDelegate onException)
        {
            var runner = new BackgroundWorkerWin()
            {
                ActionDispatcher = dispatcher,
                BackgroundWorker = backgroundWorker,
                OnSuccess = onSuccess,
                OnCancelled = onCancel,
                OnException =  onException,
                Data = data
            };
            runner.Run();
            return runner;
        }

        public static BackgroundWorkerWin Run(Control uiControl, ConcurrentDictionary<string, object> data, BackgroundWorkerMethodDelegate backgroundWorker, UIWorkerMethodDelegate onSuccess, UIWorkerMethodDelegate onCancel, BackgroundWorkerExceptionHandlerDelegate onException)
        {
            return Run(new ActionDispatcher(uiControl), data, backgroundWorker, onSuccess, onCancel, onException);
        }

    }
}
