#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
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

        public UnhandledExceptionHelperWin(UnhandledExceptionEventHandler handler, bool addExceptionHandling = true) : this(addExceptionHandling)
        {
            if (handler != null) UnhandledException += handler;
        }

        public UnhandledExceptionHelperWin(bool addExceptionHandling = true)
        {
            _currentAppDomain = AppDomain.CurrentDomain;
            if (addExceptionHandling) AddExceptionHandling();
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