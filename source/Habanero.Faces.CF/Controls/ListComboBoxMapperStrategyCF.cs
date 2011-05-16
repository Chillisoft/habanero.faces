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

namespace Habanero.Faces.Controls
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a list ComboBox
    /// depending on the environment
    /// </summary>
    public class ListComboBoxMapperStrategyCF : IListComboBoxMapperStrategy
    {
        /// <summary>
        /// Adds an ItemSelected event handler.
        /// For Windows Forms you may want the business object to be updated immediately, however
        /// for a web environment with low bandwidth you may choose to only update when the user saves.
        ///</summary>
        public void AddItemSelectedEventHandler(ListComboBoxMapper mapper)
        {

            var comboBoxWin = mapper.GetControl() as ComboBox;
            if (comboBoxWin == null) return;
            comboBoxWin.SelectedIndexChanged += delegate
            {
                    try
                    {
                        mapper.ApplyChangesToBusinessObject();
                        mapper.UpdateControlValueFromBusinessObject();
                    }
                    catch (Exception ex)
                    {
                        GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
                    }
                };
        }
    }
}