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
using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides a store of application-wide user interface settings
    /// </summary>
    public class UISettings : IUISettings
    {
        private IDictionary<string, object> _settings = new Dictionary<string, object>();

        public UISettings()
        {
            _settings.Add("PauseBeforeClosingForm", false);
        }

        /// <summary>
        /// Assign a method to this delegate that returns a boolean
        /// to indicate whether the user has permission to right-click
        /// on the ComboBox that represents the given
        /// BusinessObject type.  This applies to all ComboBoxes in the
        /// application that are mapped using a Habanero IComboBoxMapper,
        /// but the individual XML class definition parameter settings for
        /// a field take precedence.
        /// </summary>
        public PermitComboBoxRightClickDelegate PermitComboBoxRightClick { get; set; }

        public object this[string settingName]
        {
            get
            {
                if (_settings.ContainsKey(settingName)) return _settings[settingName];
                throw new HabaneroDeveloperException("Setting not found in UISettings: " +settingName);
            }

            set
            {
                _settings[settingName] = value;
            }
            
        }

    }
}
