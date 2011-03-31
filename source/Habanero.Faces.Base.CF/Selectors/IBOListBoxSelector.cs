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

namespace Habanero.Faces.Base
{
    /// <summary>
    /// Provides an interface that is specialised for showing a collection of 
    /// Business Objects in a <see cref="ListBox"/> and allowing the user to select one.
    /// </summary>
    public interface IBOListBoxSelector : IBOColSelectorControl
    {
        ///<summary>
        /// Returns the Underlying <see cref="ListBox"/> that is used by this selector
        ///</summary>
        ListBox ListBox { get; }
    }

    public class NullBOListBoxSelector : IBOListBoxSelector
    {
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IBusinessObject SelectedBusinessObject
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int NoOfItems
        {
            get { throw new NotImplementedException(); }
        }

        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            throw new NotImplementedException();
        }

        public bool AutoSelectFirstItem
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ControlEnabled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ListBox ListBox
        {
            get { throw new NotImplementedException(); }
        }
    }
}