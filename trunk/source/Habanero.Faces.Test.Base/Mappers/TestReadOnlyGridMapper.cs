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
