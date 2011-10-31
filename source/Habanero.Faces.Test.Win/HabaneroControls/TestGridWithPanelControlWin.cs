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
using Habanero.BO;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
#pragma warning disable 612,618
    //Even though this is obsolete the tests for it should remain in place.
    [TestFixture]
    public class TestGridWithPanelControlWin : TestGridWithPanelControl
    {

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new BusinessObjectControlStubWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        }
        //Even though this is obsolete the tests for it should remain in place.
        [Test]
        public void TestCancelButton_RestoreChangesGridText()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();

            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];

            MyBO currentBO = gridWithPanelControl.CurrentBusinessObject;
            string originalValue = currentBO.TestProp;
            string newValue = BOTestUtils.RandomString;
            currentBO.TestProp = newValue;
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(newValue, currentBO.TestProp);
            Assert.AreEqual(newValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalValue, currentBO.TestProp);
            Assert.AreEqual(originalValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            Assert.AreSame(currentBO, gridWithPanelControl.CurrentBusinessObject);
        }
#pragma warning restore 612,618
    }
}