using System;
using System.Threading;
using System.Windows.Forms;

namespace Habanero.Faces.Test.Win
{
    public class UnhandledExceptionHelperWin: IDisposable
    {
        private readonly AppDomain _currentAppDomain;
        private bool _exceptionHandlingActive;

        public event UnhandledExceptionEventHandler UnhandledException;
        public UnhandledExceptionHelperWin(UnhandledExceptionEventHandler handler) : this(handler, true)
        {
            
        }
        public UnhandledExceptionHelperWin(UnhandledExceptionEventHandler handler, bool addExceptionHandling) : this(addExceptionHandling)
        {
            if (handler != null) UnhandledException += handler;
        }

        public UnhandledExceptionHelperWin(bool addExceptionHandling)
        {
            _currentAppDomain = AppDomain.CurrentDomain;
            if (addExceptionHandling) AddExceptionHandling();
        }
         public UnhandledExceptionHelperWin():this(true)
         {
         }

        ~UnhandledExceptionHelperWin()
        {
            RemoveExceptionHandling();
        }

        public void Dispose()
        {
            RemoveExceptionHandling();
        }

        public bool IsExceptionHandlingActive
        {
            get { return _exceptionHandlingActive; }
        }

        public void AddExceptionHandling()
        {
            if (IsExceptionHandlingActive) return;
            _exceptionHandlingActive = true;
            _currentAppDomain.UnhandledException += OnUnhandledExceptionEventHandler;
            Application.ThreadException += OnThreadExceptionEventHandler;
        }

        public void RemoveExceptionHandling()
        {
            if (!IsExceptionHandlingActive) return;
            _currentAppDomain.UnhandledException -= OnUnhandledExceptionEventHandler;
            Application.ThreadException -= OnThreadExceptionEventHandler;
            _exceptionHandlingActive = false;
        }

        private void OnThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            ProcessUnhandledException("Application.ThreadException", sender, new UnhandledExceptionEventArgs(threadExceptionEventArgs.Exception, true));
        }

        private void OnUnhandledExceptionEventHandler(object sender1, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            ProcessUnhandledException("AppDomain.Current.UnhandledException", sender1, unhandledExceptionEventArgs);
        }

        private void ProcessUnhandledException(string sourceHandler, object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Console.WriteLine(string.Format("Unhandled Exception caught by '{0}' : {1}", sourceHandler, unhandledExceptionEventArgs.ExceptionObject));
            if (UnhandledException != null) UnhandledException(sender, unhandledExceptionEventArgs);
        }
    }
}