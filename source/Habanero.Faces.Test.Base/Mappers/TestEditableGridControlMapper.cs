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
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.Mappers
{
    public abstract class TestEditableGridControlMapper
    {
        protected abstract IControlFactory GetControlFactory();

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();
            const string propName = "asdfa";

            //---------------Execute Test ----------------------
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());

            //---------------Test Result -----------------------
            Assert.AreSame(editableGrid, mapper.Control);
            Assert.AreEqual(propName, mapper.PropertyName);
            Assert.IsFalse(editableGrid.Buttons.Visible);
        }

        [Test]
        public void Test_BusinessObject()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();

            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            //---------------Assert PreConditions---------------        
            Assert.IsNull(mapper.BusinessObject);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(addresses, editableGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObject_WhenNull()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();

            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            mapper.BusinessObject = contactPersonTestBO;
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            //---------------Assert PreConditions---------------        
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(addresses, editableGrid.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.AreSame(null, editableGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObject_WhenNullInitialValue()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();

            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            //---------------Assert PreConditions---------------        
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.AreSame(null, editableGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_EditableGridIsPopulated()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl editableGrid = GetControlFactory().CreateEditableGridControl();
            const string propName = "Addresses";
            EditableGridControlMapper mapper = new EditableGridControlMapper(editableGrid, propName, false, GetControlFactory());
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------

            Assert.AreSame(contactPersonTestBO.Addresses, editableGrid.Grid.BusinessObjectCollection);
        }
    }


}