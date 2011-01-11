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
using Habanero.Smooth;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestComboBoxLinker
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();

            AllClassesAutoMapper.ClassDefCol = ClassDef.ClassDefs;
            ClassDef.ClassDefs.Add(typeof(FakeAddress).MapClass());
            ClassDef.ClassDefs.Add(typeof(FakeContactPerson).MapClass());
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }
        [SetUp]
        public void SetUpTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }
        protected abstract IBOComboBoxSelector CreateControl();

        private static FakeAddress CreateFakeAddressWithFakeContactPerson()
        {
            return FakeAddress.CreateSavedAddress(FakeContactPerson.CreateSavedContactPerson());
        }

        private static FakeContactPerson CreateFakeContactPersonWithNoFakeAddress()
        {
            return FakeContactPerson.CreateSavedContactPerson();
        }

        private static IBusinessObjectCollection GetFakeContactPersons()
        {
            BusinessObjectCollection<FakeContactPerson> col = new BusinessObjectCollection<FakeContactPerson>();
            col.LoadAll();
            return col;
        }
        [Test]
        public void Test_CreateComboBoxLinker__ShouldSetProps()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "Addresses";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ComboBoxLinker<FakeContactPerson, FakeAddress> linker = new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            //---------------Test Result -----------------------
            Assert.AreSame(parentSelector, linker.ParentSelector);
            Assert.AreSame(childSelector, linker.ChildSelector);
            Assert.AreSame(relationshipName, linker.RelationshipName);
        }

        [Test]
        public void Test_CreateComboBoxLinker_ShouldSetProps()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ComboBoxLinker<FakeContactPerson, FakeAddress> linker = new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            //---------------Test Result -----------------------
            Assert.AreSame(parentSelector, linker.ParentSelector);
            Assert.AreSame(childSelector, linker.ChildSelector);
            Assert.AreSame(relationshipName, linker.RelationshipName);
        }

        [Test]
        public void Test_Construct_WithNullParentSelector_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector parentSelector = null;
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------
            Assert.IsNull(parentSelector);
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("parentSelector", ex.ParamName);
            }
        }
        [Test]
        public void Test_Construct_WithNullChildSelector_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = null;
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------
            Assert.IsNull(childSelector);
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("childSelector", ex.ParamName);
            }
        }
        [Test]
        public void Test_Construct_WithNullRelationship_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(relationshipName);
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("relationshipName", ex.ParamName);
            }
        }
        [Test]
        public void Test_Construct_WithEmptyRelationship_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "";
            //---------------Assert Precondition----------------
            Assert.IsEmpty(relationshipName);
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("relationshipName", ex.ParamName);
            }
        }

        [Test]
        public void Test_ChangeParentSelectedIndex_ShouldUpdateChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoFakeAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
            parentSelector.SelectedBusinessObject = contactPersonNoFakeAddress;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.Items.Count, "Two departments and Blank Field");
            Assert.AreSame(contactPersonNoFakeAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.Items.Count, "The Blank Item");
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Test Result -----------------------
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.Items.Count, "The Blank Item and the address");
            Assert.AreSame(address, childSelector.SelectedBusinessObject);
        }

        [Test]
        public void Test_ChangeParentSelectedBO_WithNull_ShouldClearChildSelector()
        {
            //---------------Set up test pack-------------------
            CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.Items.Count, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.Items.Count, "The Blank Item and childAssert;");
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.Items.Count);
            Assert.IsNull(childSelector.SelectedBusinessObject);
            Assert.IsNull(childSelector.SelectedValue);
        }
        [Test]
        public void Test_ChangeParentSelectedIndex_WithEmptyEntry_ShouldClearChildSelector()
        {
            //---------------Set up test pack-------------------
            CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.Items.Count, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.Items.Count, "The Blank Item and child");
            Assert.IsNotNull(childSelector.SelectedItem);
            Assert.IsNotNull(childSelector.SelectedValue);
            //---------------Execute Test ----------------------
            parentSelector.SelectedIndex = 0;
            //---------------Test Result -----------------------
            Assert.IsNull(parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.Items.Count);
            Assert.IsNull(childSelector.SelectedBusinessObject);
            Assert.IsNull(childSelector.SelectedValue);
        }
        [Test]
        public void Test_ChangeParentSelectedIndex_WithNonEmpty_ShouldLoadChildren()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoAddress = CreateFakeContactPersonWithNoFakeAddress();
            CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
            parentSelector.SelectedBusinessObject = contactPersonNoAddress;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.Items.Count, "Two departments and Blank Field");
            Assert.AreSame(contactPersonNoAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.Items.Count, "The Blank Item");
            //---------------Execute Test ----------------------
            parentSelector.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.IsNotNull(parentSelector.SelectedBusinessObject);
            FakeContactPerson contactPerson = parentSelector.SelectedBusinessObject as FakeContactPerson;
            Assert.AreEqual(contactPerson.Addresses.Count + 1, childSelector.Items.Count);
        }
        [Test]
        public void Test_ChangeParentSelectedBO_WithParentThatHasNoChildren_ShouldClearChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            IBOComboBoxSelector parentSelector = CreateControl();
            IBOComboBoxSelector childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new ComboBoxLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPersons();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.Items.Count, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.Items.Count, "The Blank Item and the address");
            Assert.AreSame(address, childSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = contactPersonNoAddress;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonNoAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.Items.Count, "The Blank Item");
            Assert.IsNull(childSelector.SelectedBusinessObject);
        }

    }
    public class FakeContactPerson : BusinessObject
    {
        protected override IClassDef ConstructClassDef()
        {
            AllClassesAutoMapper.ClassDefCol = ClassDef.ClassDefs;
            return base.GetType().MapClass();
        }

        public virtual BusinessObjectCollection<FakeAddress> Addresses
        {
            get
            {
                return Relationships.GetRelatedCollection<FakeAddress>("Addresses");
            }
        }

        public static FakeContactPerson CreateSavedContactPerson()
        {
            return (FakeContactPerson) new FakeContactPerson().Save();
        }
    }
    public class FakeAddress : BusinessObject
    {
        protected override IClassDef ConstructClassDef()
        {
            AllClassesAutoMapper.ClassDefCol = ClassDef.ClassDefs;
            return base.GetType().MapClass();
        }
        public virtual FakeContactPerson ContactPerson
        {
            get
            {
                return Relationships.GetRelatedObject<FakeContactPerson>("ContactPerson");
            }
            set
            {
                Relationships.SetRelatedObject("ContactPerson", value);
            }
        }

        public static FakeAddress CreateSavedAddress(FakeContactPerson contactPerson)
        {
            var fakeAddress = new FakeAddress {ContactPerson = contactPerson};
            fakeAddress.Save();
            return fakeAddress;
        }
    }
}