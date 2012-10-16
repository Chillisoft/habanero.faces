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
// ReSharper disable InconsistentNaming

namespace Habanero.Faces.Test.Base
{
    public abstract class TestBOColSelectorLinker
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
        protected abstract IBOColSelector CreateControl();

        protected static FakeAddress CreateFakeAddressWithFakeContactPerson()
        {
            return FakeAddress.CreateSavedAddress(FakeContactPerson.CreateSavedContactPerson());
        }
        protected static FakeContactPerson CreateFakeContactPersonWithManyAddresses()
        {
            var savedContactPerson = FakeContactPerson.CreateSavedContactPerson();
            FakeAddress.CreateSavedAddress(savedContactPerson);
            FakeAddress.CreateSavedAddress(savedContactPerson);
            FakeAddress.CreateSavedAddress(savedContactPerson);
            return savedContactPerson;
        }


        protected static FakeContactPerson CreateFakeContactPersonWithNoFakeAddress()
        {
            return FakeContactPerson.CreateSavedContactPerson();
        }

        protected static IBusinessObjectCollection GetFakeContactPeople()
        {
            BusinessObjectCollection<FakeContactPerson> col = new BusinessObjectCollection<FakeContactPerson>();
            col.LoadAll();
            return col;
        }

        [Test]
        public void Test_Construct_ShouldSetProps()
        {
            //---------------Set up test pack-------------------
            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var linker = new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            //---------------Test Result -----------------------
            Assert.AreSame(parentSelector, linker.ParentSelector);
            Assert.AreSame(childSelector, linker.ChildSelector);
            Assert.AreSame(relationshipName, linker.RelationshipName);
        }
        [Test]
        public void Test_Construct_ShouldSetLinkerEnabled()
        {
            //---------------Set up test pack-------------------
            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var linker = new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            //---------------Test Result -----------------------
            Assert.IsTrue(linker.Enabled);
        }

        [Test]
        public void Test_Construct_WithNullParentSelector_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IBOColSelector parentSelector = null;
            var childSelector = CreateControl();
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------
            Assert.IsNull(parentSelector);
            //---------------Execute Test ----------------------
            try
            {
                new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
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
            var parentSelector = CreateControl();
            IBOColSelector childSelector = null;
            const string relationshipName = "ChildLocations";
            //---------------Assert Precondition----------------
            Assert.IsNull(childSelector);
            //---------------Execute Test ----------------------
            try
            {
                new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
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
            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(relationshipName);
            //---------------Execute Test ----------------------
            try
            {
                new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
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
            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "";
            //---------------Assert Precondition----------------
            Assert.IsEmpty(relationshipName);
            //---------------Execute Test ----------------------
            try
            {
                new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
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
        public void Test_ChangeParentSelectedBO_WithSomething_ShouldUpdateChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoFakeAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = contactPersonNoFakeAddress;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(contactPersonNoFakeAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.NoOfItems, "The Blank Item");
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Test Result -----------------------
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.NoOfItems, "The Blank Item and the address");
            Assert.AreSame(address, childSelector.SelectedBusinessObject);
        }
        [Test]
        public void Test_ChangeParentSelectedBO_WithSomething_WhenLinkerDisabled_ShouldNotUpdateChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoFakeAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "Addresses";
            var linker = new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = contactPersonNoFakeAddress;
            linker.Enabled = false;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(contactPersonNoFakeAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.NoOfItems, "The Blank Item");
            Assert.IsFalse(linker.Enabled);
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Test Result -----------------------
            Assert.IsFalse(linker.Enabled);
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreNotSame(address, childSelector.SelectedBusinessObject);
        }

        [Test]
        public void Test_UpdateChildSelectorCollection_WhenSomethingIsSelectedBeforeHand_ShouldUpdateChildSelector()
        {
            //---------------Set up test pack-------------------
            CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "Addresses";
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            BOColSelectorLinker<FakeContactPerson, FakeAddress> selectorLinker = new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selectorLinker.UpdateChildSelectorCollection();
            //---------------Test Result -----------------------
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.NoOfItems, "The Blank Item and the address");
            Assert.AreSame(address, childSelector.SelectedBusinessObject);
        }
        [Test]
        public void Test_UpdateChildSelectorCollection_WhenSpecificChildWasSelectedBeforeHand_ShouldRetainSelectedChild()
        {
            //---------------Set up test pack-------------------
            CreateFakeContactPersonWithNoFakeAddress();
            var contactPerson = CreateFakeContactPersonWithManyAddresses();

            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "Addresses";
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            var originallySelectedAddress = contactPerson.Addresses[1];
            BOColSelectorLinker<FakeContactPerson, FakeAddress> selectorLinker = new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.SelectedBusinessObject = contactPerson;
            selectorLinker.UpdateChildSelectorCollection();
            childSelector.SelectedBusinessObject = originallySelectedAddress;
            //---------------Assert Precondition----------------
            Assert.AreSame(originallySelectedAddress, childSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            selectorLinker.UpdateChildSelectorCollection();
            //---------------Test Result -----------------------
            Assert.AreSame(originallySelectedAddress, childSelector.SelectedBusinessObject);
        }

        [Test]
        public void Test_ChangeParentSelectedBO_WithNull_ShouldClearChildSelector()
        {
            //---------------Set up test pack-------------------
            CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.NoOfItems, "The Blank Item and childAssert;");
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.NoOfItems);
            Assert.IsNull(childSelector.SelectedBusinessObject);
        }
        [Test]
        public void Test_ChangeParentSelectedBO_WithParentThatHasNoChildren_ShouldClearChildSelector()
        {
            //---------------Set up test pack-------------------
            var contactPersonNoAddress = CreateFakeContactPersonWithNoFakeAddress();
            var address = CreateFakeAddressWithFakeContactPerson();

            var parentSelector = CreateControl();
            var childSelector = CreateControl();
            const string relationshipName = "Addresses";
            new BOColSelectorLinker<FakeContactPerson, FakeAddress>(parentSelector, childSelector, relationshipName);
            parentSelector.BusinessObjectCollection = GetFakeContactPeople();
            parentSelector.SelectedBusinessObject = address.ContactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, parentSelector.NoOfItems, "Two departments and Blank Field");
            Assert.AreSame(address.ContactPerson, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(2, childSelector.NoOfItems, "The Blank Item and the address");
            Assert.AreSame(address, childSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            parentSelector.SelectedBusinessObject = contactPersonNoAddress;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonNoAddress, parentSelector.SelectedBusinessObject);
            Assert.AreEqual(1, childSelector.NoOfItems, "The Blank Item");
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