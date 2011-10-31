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
using Habanero.Base.Exceptions;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Selectors
{
    /// <summary>
    /// This test class tests the GridSelector class.
    /// </summary>
    [TestFixture]
    public class TestBOSelectorEditableGridControlWin : TestBOSelectorEditableGridControl
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            IEditableGridControl readOnlyGridControl = GetControlFactory().CreateEditableGridControl();
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)readOnlyGridControl);
            return readOnlyGridControl;
        }

        [Test]
        public virtual void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new EditableGridControlWin(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("Cannot create an editable grid control if the control factory is null", ex.Message);
            }
        }
    }
}