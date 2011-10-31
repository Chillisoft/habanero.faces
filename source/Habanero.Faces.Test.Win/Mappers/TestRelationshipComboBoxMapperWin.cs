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
using System;
using Habanero.BO;
using Habanero.Test.BO;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestRelationshipComboBoxMapperWin : TestRelationshipComboBoxMapper
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

        [Test]
        public void Test_WhenSetInvalidPropertyValue_ShouldUpdateItemInComboToBlank()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            string origToString = organisationTestBO.ToString();
            Guid newToString = Guid.NewGuid();
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Assert precondition----------------
            Assert.AreEqual(organisationTestBO.ToString(), origToString);
            Assert.AreEqual(origToString.ToString(), cmbox.Text);
            //---------------Execute Test ----------------------
            person.OrganisationID = newToString;
            //---------------Test Result -----------------------
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual("", cmbox.Text);
        }

        [Test]
        public void Test_WhenChangePropValue_ShouldUpdateControlValue_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Assert precondition----------------
            Assert.AreSame(organisationTestBO, person.Organisation);
            Assert.AreSame(organisationTestBO, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            person.Organisation = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(newBO, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        [Test]
        public void Test_MustDeregisterForEvents_WhenSetBOToNull_ThenChangePropValue_ShouldNotUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Assert precondition----------------
            Assert.AreSame(organisationTestBO, person.Organisation);
            Assert.AreSame(organisationTestBO, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            person.Organisation = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(null, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        [Test]
        public void Test_MustDeregisterForEvents_WhenSetBOToAnotherBO_ThenChangePropValue_ShouldNotUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            ContactPersonTestBO newCP = new ContactPersonTestBO();
            //---------------Assert precondition----------------
            Assert.AreSame(organisationTestBO, person.Organisation);
            Assert.AreSame(organisationTestBO, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = newCP;
            person.Organisation = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(null, newCP.Organisation);
            Assert.AreSame(null, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        [Test]
        public override void Test_ChangeComboBoxSelected_ShouldNotUpdatePropValue_VWGOnly()
        {
            //For Windows the value should be changed.
            Assert.IsTrue
                (true,
                 "For Windows the value should be changed. See TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges");
        }

        [Test]
        public virtual void Test_ChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            var mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            var newBO = boCol.CreateBusinessObject();

            var organisationTestBO = boCol[0];
            var person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public virtual void Test_KeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            var cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            var mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            var newBO = boCol.CreateBusinessObject();
            var organisationTestBO = boCol[0];
            var person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.Text = newBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public virtual void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
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
        public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
        {
            //---------------Set up test pack-------------------

            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newBO;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreNotSame(newBO, person.Organisation, "For Windows the value should be changed.");
            Assert.AreSame(organisationTestBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public void Test_SetPerson_WithUnsavedOrg_ShouldNotRemoveOrg()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapper(cmbox);
            boCol = GetBoColWithOneItem();

            mapper.BusinessObject = new ContactPersonTestBO();

            OrganisationTestBO testBO = new OrganisationTestBO();
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(testBO);
            //---------------Assert Precondition----------------
            Assert.False(boCol.Contains(testBO));
            Assert.AreSame(testBO, person.Organisation);
            Assert.IsNotNull(person.Organisation);
            Assert.IsTrue(person.Organisation.Status.IsNew);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.IsNotNull(person.Organisation);
        }

        [Test]
        public void Test_AutoLoading_SetPerson_WithUnsavedOrganisation_ShouldNotRemoveOrganisation_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapperFake mapper = new RelationshipComboBoxMapperFake(cmbox, "Organisation", false,
                                                                                   GlobalUIRegistry.ControlFactory);
            boCol = GetBoColWithOneItem();
            mapper.BusinessObjectCollectionForLoading = boCol;

            OrganisationTestBO testBO = new OrganisationTestBO();
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(testBO);
            //---------------Assert Precondition----------------
            Assert.False(boCol.Contains(testBO));
            Assert.AreSame(testBO, person.Organisation);
            Assert.IsNotNull(person.Organisation);
            Assert.IsTrue(person.Organisation.Status.IsNew);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.IsNotNull(person.Organisation);
            Assert.IsTrue(cmbox.Items.Contains(testBO));
        }

        [Test]
        public void Test_NonAutoLoading_SetPerson_WithUnsavedOrganisation_ShouldNotRemoveOrganisation_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, "Organisation", false,
                                                                                   GlobalUIRegistry.ControlFactory);
            boCol = GetBoColWithOneItem();
            mapper.BusinessObjectCollection = boCol;
            mapper.BusinessObject = new ContactPersonTestBO();

            OrganisationTestBO testBO = new OrganisationTestBO();
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(testBO);
            //---------------Assert Precondition----------------
            Assert.False(boCol.Contains(testBO));
            Assert.AreSame(testBO, person.Organisation);
            Assert.IsNotNull(person.Organisation);
            Assert.IsTrue(person.Organisation.Status.IsNew);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.IsNotNull(person.Organisation);
        }

        [Test]
        public void Test_SetSelectedItem_WhenAnItemIsSelectedAndRelatedBusnessObjectWasNull_ShouldUpdateBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            var newOrganisation = boCol.CreateBusinessObject();
            newOrganisation.Save();
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsNull(person.Organisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            Assert.AreSame(newOrganisation, person.Organisation);
        }

        protected class ComboBoxWinStub : ComboBoxWin
        {
            public void CallSendKeyBob()
            {
                this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
            }
        }

        class RelationshipComboBoxMapperFake : RelationshipComboBoxMapper
        {
            public RelationshipComboBoxMapperFake(IComboBox comboBox, string relationshipName, bool isReadOnly, IControlFactory controlFactory) : base(comboBox, relationshipName, isReadOnly, controlFactory)
            {
            }

            public BusinessObjectCollection<OrganisationTestBO> BusinessObjectCollectionForLoading
            {
                get; set;
            }

            protected override void LoadCollectionForBusinessObject()
            {
                this.BusinessObjectCollection = BusinessObjectCollectionForLoading;
            }
        }
    }
}