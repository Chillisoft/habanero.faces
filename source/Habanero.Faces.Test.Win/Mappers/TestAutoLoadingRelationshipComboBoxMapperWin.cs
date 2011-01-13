using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Win;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestAutoLoadingRelationshipComboBoxMapperWin : TestRelationshipComboBoxMapperWin
    {
        protected override IControlFactory GetControlFactory()
        {
            if (_controlFactory == null) CreateControlFactory();
            return _controlFactory;
        }

        protected override void CreateControlFactory()
        {
            _controlFactory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = _controlFactory;
        }

        protected override RelationshipComboBoxMapper GetMapper(IComboBox cmbox, string relationshipName, bool isReadOnly)
        {
            return new AutoLoadingRelationshipComboBoxMapper(cmbox, relationshipName, isReadOnly, GetControlFactory());
        }

        protected override RelationshipComboBoxMapper GetMapper(IComboBox cmbox, string relationshipName)
        {
            return new AutoLoadingRelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
        }

        protected override BusinessObjectCollection<OrganisationTestBO> GetBoColWithOneItem()
        {
            new OrganisationTestBO().Save();
            return Broker.GetBusinessObjectCollection<OrganisationTestBO>("", "");
        }
/*        [Test]
        public override void Test_BusinessObject_WhenSet_WhenDoesNotExistInCollection_ShouldAddRelatedItemToComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, mapper.Control.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(4, mapper.Control.Items.Count);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(person.Organisation, cmbox.SelectedItem);
        }*/

        [Test]
        public void Test_AutoLoadingMapper_WhenCreateFromControlMapper_ShouldSetupCollection()
        {
            //---------------Set up test pack-------------------
            var cmbox = _controlFactory.CreateComboBox();
            var controlMapper = ControlMapper.Create
                ("AutoLoadingRelationshipComboBoxMapper", "Habanero.Faces.Base", cmbox, "ContactPersonTestBO", false,
                 GetControlFactory());

            var person1 = ContactPersonTestBO.CreateSavedContactPerson();
            var person2 = ContactPersonTestBO.CreateSavedContactPerson();

            var addressTestBo = new AddressTestBO();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controlMapper.BusinessObject = addressTestBo;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.IsTrue(cmbox.Items.Contains(person1));
            Assert.IsTrue(cmbox.Items.Contains(person2));
        }

        [Test]
        public void Test_AutoLoadingMapper_WhenConstructNormally_ShouldSetupCollection()
        {
            //---------------Set up test pack-------------------
            var cmbox = _controlFactory.CreateComboBox();
            var controlMapper = GetMapper(cmbox, "ContactPersonTestBO");

            var person1 = ContactPersonTestBO.CreateSavedContactPerson();
            var person2 = ContactPersonTestBO.CreateSavedContactPerson();

            var addressTestBo = new AddressTestBO();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controlMapper.BusinessObject = addressTestBo;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.IsTrue(cmbox.Items.Contains(person1));
            Assert.IsTrue(cmbox.Items.Contains(person2));
        }

        [Test]
        public override void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            newBO.Save();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            cmbox.Text = newBO.ToString();
            //---------------Assert Precondition----------------
            Assert.AreNotSame(newBO, person.Organisation, "For Windows the value should be changed.");
            //---------------Execute Test ----------------------
            cmbox.CallSendKeyBob();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public override void Test_KeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            newBO.Save();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.Text = newBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public override void Test_ChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            var mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            var newBO = boCol.CreateBusinessObject();
            newBO.Save();
            var organisationTestBO = boCol[0];
            var person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }
/*
        [Test]
        public override void Test_BusinessObject_WhenSet_WhenDoesNotExistInCollection_ShouldAddRelatedItemToComboBox()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            var mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            var person = new ContactPersonTestBO();
            person.Organisation = new OrganisationTestBO();
            //---------------Assert Precondition----------------

            Assert.AreEqual(2, mapper.Control.Items.Count);
            Assert.IsFalse(mapper.Control.Items.Contains(person.Organisation));
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, mapper.Control.Items.Count, "Should Add BO");
            Assert.AreEqual(person.Organisation, cmbox.SelectedItem);
        }*/

        [Test]
        public override void Test_ApplyChangesToBusinessObject_WhenNewItemIsSelected_WhenSet_WhenRelationshipIsLevelsDeep_ShouldUpdateRelatedBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            OrganisationTestBO.LoadDefaultClassDef();
            AddressTestBO.LoadDefaultClassDef();

            const string relationshipName = "ContactPersonTestBO.Organisation";
            var mapper = GetMapperBoColHasOneItem(relationshipName);
            var cmbox = mapper.Control;
            var boCol = (BusinessObjectCollection<OrganisationTestBO>)mapper.BusinessObjectCollection;
            var person = new ContactPersonTestBO { Organisation = boCol[0] };
            var addressTestBO = new AddressTestBO { ContactPersonTestBO = person };
            
            var newOrganisation = new OrganisationTestBO();
            newOrganisation.Save();
            mapper.BusinessObject = addressTestBO;
            //---------------Assert Precondition----------------
            Assert.AreSame(addressTestBO, mapper.BusinessObject);
            Assert.AreSame(person.Organisation, cmbox.SelectedItem);
            Assert.AreNotSame(person.Organisation, newOrganisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            Assert.AreSame(newOrganisation, cmbox.SelectedItem, "Selected Item should be set.");
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            Assert.AreSame(newOrganisation, person.Organisation);
        }

        [Test]
        public override void Test_ApplyChangesToBusinessObject_WhenNewItemIsSelected_ShouldUpdateBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            var mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            var relatedBo = boCol[0];
            var newOrganisation = boCol.CreateBusinessObject();
            newOrganisation.Save();
            var person = new ContactPersonTestBO { Organisation = relatedBo };
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreNotSame(newOrganisation, person.Organisation);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.AreNotSame(newOrganisation, cmbox.SelectedItem, "Selected Item should not be set.");
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            Assert.AreSame(newOrganisation, cmbox.SelectedItem, "Selected Item should be set.");
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            Assert.AreSame(newOrganisation, person.Organisation);
        }
        [Test]
        public override void Test_ApplyChangesToBusinessObject_WhenAnItemIsSelectedAndRelatedBusnessObjectWasNull_ShouldUpdateBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            newOrganisation.Save();
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsNull(person.Organisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            Assert.AreSame(newOrganisation, person.Organisation);
        }
    }
}