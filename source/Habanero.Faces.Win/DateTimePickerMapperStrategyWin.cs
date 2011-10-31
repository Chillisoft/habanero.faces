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
using System.Windows.Forms;
using Habanero.Faces.Base;

namespace Habanero.Faces.Win
{
    /// <summary>
    /// Provides a set of behaviour strategies that can be applied to a TextBox
    /// depending on the environment
    /// </summary>
    internal class DateTimePickerMapperStrategyWin : IDateTimePickerMapperStrategy
    {
        // Assumes that one strategy is created for each control.
        // These fields exist so that the IsValidCharacter method knows
        //   which prop and textbox it is dealing with

        public void AddUpdateBoPropOnValueChangedHandler(DateTimePickerMapper mapper)
        {
			var control = mapper.GetControl();
			EventHandler eventHandler = (sender, args) => mapper.ApplyChangesToBusinessObject();
			
			if (!ConvertAndExecuteAs<IDateTimePicker>(control, picker => picker.ValueChanged += eventHandler))
				ConvertAndExecuteAs<DateTimePicker>(control, picker => picker.ValueChanged += eventHandler);
        }

		private static bool ConvertAndExecuteAs<T>(object obj, Action<T> action) 
			where T : class
		{
			var convertedValue = obj as T;
			if (convertedValue == null) return false;
			if (action != null) action(convertedValue);
			return true;
		}
    }
}