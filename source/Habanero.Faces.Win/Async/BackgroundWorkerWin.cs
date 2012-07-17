using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Habanero.Faces.Win.Async
{
    public class BackgroundWorkerWin
    {
        public delegate bool BackgroundWorkerMethodDelegate(ConcurrentDictionary<string, object> data);
        public delegate void UIWorkerMethodDelegate(ConcurrentDictionary<string, object> data);
        public delegate void BackgroundWorkerExceptionHandlerDelegate(Exception ex);

        public BackgroundWorkerMethodDelegate BackgroundWorker { get; set; }
        public UIWorkerMethodDelegate OnSuccess { get; set; }
        public UIWorkerMethodDelegate OnCancelled { get; set; }
        public BackgroundWorkerExceptionHandlerDelegate OnException { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }
        public MethodDispatcher MethodDispatcher { get; set; }

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
                    catch (Exception ex)
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
                this.MethodDispatcher.Dispatch(() => { this.OnException(ex); });
            }
        }

        protected void RunUIWorkerDelegate(bool success)
        {
            if (success && (this.OnSuccess != null))
            {
                this.MethodDispatcher.Dispatch(() => { this.OnSuccess(this.Data); });
            }
            else if (this.OnCancelled != null)
            {
                this.MethodDispatcher.Dispatch(() => { this.OnCancelled(this.Data); });
            }
        }

        public static BackgroundWorkerWin Run(MethodDispatcher dispatcher, BackgroundWorkerMethodDelegate backgroundWorker, 
            UIWorkerMethodDelegate onSuccess, UIWorkerMethodDelegate onCancel, BackgroundWorkerExceptionHandlerDelegate onException,
            ConcurrentDictionary<string, object> data)
        {
            var runner = new BackgroundWorkerWin()
            {
                MethodDispatcher = dispatcher,
                BackgroundWorker = backgroundWorker,
                OnSuccess = onSuccess,
                OnCancelled = onCancel,
                OnException =  onException,
                Data = data
            };
            runner.Run();
            return runner;
        }

        public static BackgroundWorkerWin Run(Control uiControl, BackgroundWorkerMethodDelegate backgroundWorker, 
            UIWorkerMethodDelegate onSuccess, UIWorkerMethodDelegate onCancel, BackgroundWorkerExceptionHandlerDelegate onException,
            ConcurrentDictionary<string, object> data)
        {
            return Run(new MethodDispatcher(uiControl), backgroundWorker, onSuccess, onCancel, onException, data);
        }

    }
}
