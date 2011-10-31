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
using Habanero.Faces.Base;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Base
{
    public abstract class TestBOColSelectorLinkerComboBox : TestBOColSelectorLinker
    {

        protected override IBOColSelector CreateControl()
        {
            return CreateComboBoxControl();
        }

        protected abstract IBOComboBoxSelector CreateComboBoxControl();

        [Test]
        public void Test_ChangeParentSelectedIndex_WithEmptyEntry_ShouldClearChildSelector()
        {
            //---------------Set up test pack-------------------
            CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateComboBoxControl();
            IBOComboBoxSelector childSelector = CreateComboBoxControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.NoOfItems, "The Blank Item and child");
            Assert.IsNotNull(childSelector.SelectedBusinessObject);
            Assert.IsNotNull(childSelector.SelectedItem);
            Assert.IsNotNull(childSelector.SelectedValue);
            //---------------Execute Test ----------------------
            parentSelector.SelectedIndex = 0;
            //---------------Test Result -----------------------
            Assert.IsNull(parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.NoOfItems);
            Assert.IsNull(childSelector.SelectedBusinessObject);
            Assert.IsNull(childSelector.SelectedValue);
        }
        [Test]
        public void Test_ChangeParentSelectedIndex_WithNonEmpty_ShouldLoadChildren()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoAddress = CreateFakeContactPersonWithNoFakeAddress();
            CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateComboBoxControl();
            IBOComboBoxSelector childSelector = CreateComboBoxControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = contactPersonNoAddress;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(contactPersonNoAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.NoOfItems, "The Blank Item");
            //---------------Execute Test ----------------------
            parentSelector.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.IsNotNull(parentSelector.SelectedBusinessObject);
            FakeContactPerson contactPerson = parentSelector.SelectedBusinessObject as FakeContactPerson;
            Assert.AreEqual(contactPerson.Addresses.Count + 1, childSelector.NoOfItems);
        }


        [Test]
        public void Test_WhenParentSelectedBOIsNull_ShouldDisableChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateComboBoxControl();
            IBOComboBoxSelector childSelector = CreateComboBoxControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.NoOfItems, "The Blank Item and the address");
            Assert.AreSame(address, childSelector.SelectedBusinessObject);
            Assert.IsTrue(childSelector.ComboBox.Enabled, "Should be enabled");
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsFalse(childSelector.ComboBox.Enabled, "Should be disabled");

            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.IsNull(parentSelector.SelectedBusinessObject, "Should not have a selected parent");
            Assert.AreEqual(1, childSelector.NoOfItems, "The Blank Item as there is no selected parent");
            Assert.IsNull(childSelector.SelectedBusinessObject, "Should not have a selected child");
        }

        [Test]
        public void Test_WhenParentSelectedBOIsNull_AndChangedToNotBO_ShouldEnableChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateComboBoxControl();
            IBOComboBoxSelector childSelector = CreateComboBoxControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            
            parentSelector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.IsFalse(childSelector.ComboBox.Enabled, "Should be disabled");

            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.IsNull(parentSelector.SelectedBusinessObject, "Should not have a selected parent");
            Assert.AreEqual(1, childSelector.NoOfItems, "The Blank Item as there is no selected parent");
            Assert.IsNull(childSelector.SelectedBusinessObject, "Should not have a selected child");

            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Test Result -----------------------
            Assert.IsTrue(childSelector.ComboBox.Enabled, "Should be enabled");
        }
    }
}