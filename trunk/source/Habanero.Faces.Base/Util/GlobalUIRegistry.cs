// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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

using System.Threading;
using Habanero.Faces.Base.UIHints;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Maintains an application-wide store of UI-related settings
    /// </summary>
    public static class GlobalUIRegistry
    {
        /// <summary>
        /// Gets and sets the store of general user interface settings
        /// </summary>
        public static IUISettings UISettings { get; set; }

        /// <summary>
        /// Gets and sets the store of date display settings
        /// </summary>
        public static DateDisplaySettings DateDisplaySettings { get; set; }

        /// <summary>
        /// Gets and sets the store of date display settings
        /// </summary>
        public static CurrencyDisplaySettings CurrencyDisplaySettings { get; set; }

        /// <summary>
        /// Gets and sets the store of global ui hints
        /// </summary>
        public static UIStyleHints UIStyleHints { get; set; }

        /// <summary>
        /// gets and sets a store of global async settings
        /// </summary>
        public static AsyncSettings AsyncSettings { 
            get 
            {
                if (_asyncSettings == null)
                {
                    var mutex = new Mutex(false, "Habanero.Faces.Base.GlobalUIRegistry.AsyncSettings");
                    if (mutex.WaitOne(0))
                    {
                        if (_asyncSettings == null)
                            _asyncSettings = new AsyncSettings();
                    }
                    mutex.ReleaseMutex();
                }
                return _asyncSettings; 
            }
            set
            {
                _asyncSettings = value;
            }
        }
        private static AsyncSettings _asyncSettings;

        /// <summary>
        /// Gets and sets the control factory to use in the application
        /// </summary>
        public static IControlFactory ControlFactory { get; set; }
    }
}
