using Habanero.Faces.Base;
using NUnit.Framework;

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
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
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
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
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
    }
}