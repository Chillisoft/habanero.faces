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

// ReSharper disable InconsistentNaming

namespace Habanero.Faces.Test.Base.Mappers
{
    public abstract class TestReadOnlyGridMapper
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
            var readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();
            const string propName = "asdfa";

            //---------------Execute Test ----------------------
            var mapper = CreateReadOnlyGridMapper(readOnlyGrid, propName);

            //---------------Test Result -----------------------
            Assert.AreSame(readOnlyGrid, mapper.Control);
            Assert.AreEqual(propName, mapper.PropertyName);
        }

        [Test]
        public void Test_BusinessObject()
        {
            //---------------Set up test pack-------------------
            var readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();

            const string propName = "Addresses";
            var mapper = CreateReadOnlyGridMapper(readOnlyGrid, propName);
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            //---------------Assert PreConditions---------------        
            Assert.IsNull(mapper.BusinessObject);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(addresses, readOnlyGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObject_WhenNull()
        {
            //---------------Set up test pack-------------------
            var readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();

            const string propName = "Addresses";
            var mapper = CreateReadOnlyGridMapper(readOnlyGrid, propName);
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            mapper.BusinessObject = contactPersonTestBO;
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            //---------------Assert PreConditions---------------        
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(addresses, readOnlyGrid.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.AreSame(null, readOnlyGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObject_WhenNullInitialValue()
        {
            //---------------Set up test pack-------------------
            var readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();

            const string propName = "Addresses";
            var mapper = CreateReadOnlyGridMapper(readOnlyGrid, propName);
            //---------------Assert PreConditions---------------        
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.AreSame(null, readOnlyGrid.BusinessObjectCollection);
        }

        [Test]
        public void Test_EditableGridIsPopulated()
        {
            //---------------Set up test pack-------------------
            var readOnlyGrid = GetControlFactory().CreateReadOnlyGrid();
            const string propName = "Addresses";
            var mapper = CreateReadOnlyGridMapper(readOnlyGrid, propName);
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_DeleteDoNothing(out address);

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //---------------Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //---------------Test Result -----------------------

            Assert.AreSame(contactPersonTestBO.Addresses, readOnlyGrid.BusinessObjectCollection);
        }

        private ReadOnlyGridMapper CreateReadOnlyGridMapper(IReadOnlyGrid readOnlyGrid, string propName)
        {
            return new ReadOnlyGridMapper(readOnlyGrid, propName, false, GetControlFactory());
        }
    }

}
