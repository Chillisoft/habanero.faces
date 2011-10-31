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
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;

namespace Habanero.Faces.Test.Win
{
    internal class BusinessObjectControlStub : UserControlWin, IBOEditorControl
    {
        public bool DisplayErrorsCalled { get; private set; }

        public bool ClearErrorsCalled { get; private set; }

        public IBusinessObject BusinessObject { get; set; }

        public void DisplayErrors()
        {
            DisplayErrorsCalled = true;
        }

        public void ClearErrors()
        {
            ClearErrorsCalled = true;
        }

        #region Implementation of IBOEditorControl

        /// <summary>
        /// Applies any changes that have occured in any of the Controls on this control's to their related
        /// Properties on the Business Object.
        /// </summary>
        public void ApplyChangesToBusinessObject()
        {

        }

        /// <summary>
        /// Does the business object controlled by this control or any of its Aggregate or Composite children have and Errors.
        /// </summary>
        public bool HasErrors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the Business Object controlled by this control or any of its Aggregate or Composite children have and warnings.
        /// </summary>
        public bool HasWarning
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        ///  Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        public ErrorList Errors
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Does the business object being managed by this control have any edits that have not been persisted.
        /// </summary>
        /// <returns></returns>
        public bool IsDirty
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns a list of all warnings for the business object controlled by this control or any of its children.
        /// </summary>
        /// <returns></returns>
        public ErrorList Warnings
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region Implementation of IBusinessObjectPanel

        /// <summary>
        /// Gets and sets the PanelInfo object created by the control
        /// </summary>
        public IPanelInfo PanelInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}