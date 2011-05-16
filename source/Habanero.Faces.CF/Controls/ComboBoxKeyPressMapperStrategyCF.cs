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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Faces.Base;
using KeyPressEventHandler = System.Windows.Forms.KeyPressEventHandler;

namespace Habanero.Faces.Controls
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a lookup ComboBox
    /// depending on the environment
    /// </summary>
    public class ComboBoxKeyPressMapperStrategyCF : IComboBoxMapperStrategy
    {
        private const int ENTER_KEY_CHAR = 13;
        private IComboBoxMapper _mapper;

        /// <summary>
        /// Removes event handlers previously assigned to the ComboBox
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void RemoveCurrentHandlers(IComboBoxMapper mapper)
        {

            _mapper = mapper;
            var comboBoxWin = mapper.GetControl() as ComboBox;
            if (comboBoxWin != null)
            {
                comboBoxWin.SelectedIndexChanged -= mapper.SelectedIndexChangedHandler;
            }
        }

        /// <summary>
        /// Adds event handlers to the ComboBox that are suitable for the UI environment
        /// </summary>
        /// <param name="mapper">The mapper for the lookup ComboBox</param>
        public void AddHandlers(IComboBoxMapper mapper)
        {
            _mapper = mapper;
            var comboBoxWin = mapper.GetControl();
            if (comboBoxWin != null)
            {
                comboBoxWin.KeyPress += ComboBoxWinOnKeyPressHandler();
            }
        }

        private KeyPressEventHandler ComboBoxWinOnKeyPressHandler()
        {
            return delegate(object sender, System.Windows.Forms.KeyPressEventArgs e)
                       {
                           try
                           {
                               if (e.KeyChar == ENTER_KEY_CHAR)
                               {
                                   _mapper.ApplyChangesToBusinessObject();
                                   _mapper.UpdateControlValueFromBusinessObject();
                               }
                           }
                           catch (Exception ex)
                           {
                               GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
                           }
                       };
        }

    }
}