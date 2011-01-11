using System;
using System.ComponentModel;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Base.ControlMappers;
using Habanero.Faces.Test.Base;
using Habanero.Faces.VWG;
using Habanero.Faces.VWG.Grid;
using Habanero.Test;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestExtendedTextBoxMapperVWG
    {
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //   ContactPersonTestBO.CreateSampleData();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
        }

        private static IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_Construct()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            string propName = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxVWG, propName, false, controlFactory);
            //--------------- Test Result -----------------------
            Assert.IsInstanceOf(typeof(IControlMapper), mapper);
            Assert.AreSame(extendedTextBoxVWG, mapper.Control);
            Assert.AreEqual(propName, mapper.PropertyName);
            Assert.AreEqual(false, mapper.IsReadOnly);
            Assert.AreEqual(controlFactory, mapper.ControlFactory);
            ExtendedTextBoxMapper lookupComboBoxMapper = mapper;
            Assert.IsNotNull(lookupComboBoxMapper);
            Assert.AreSame(extendedTextBoxVWG, lookupComboBoxMapper.Control);
            Assert.AreEqual(propName, lookupComboBoxMapper.PropertyName);
            Assert.AreEqual(false, lookupComboBoxMapper.IsReadOnly);
            Assert.AreEqual(controlFactory, lookupComboBoxMapper.ControlFactory);
            Assert.AreEqual(lookupComboBoxMapper.ErrorProvider, mapper.ErrorProvider);
        }

        [Test]
        public void Test_Construct_ReadOnly()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            string propName = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxVWG, propName, true, controlFactory);
            //--------------- Test Result -----------------------
            Assert.IsInstanceOf(typeof(IControlMapper), mapper);
            Assert.AreEqual(true, mapper.IsReadOnly);
            ExtendedTextBoxMapper lookupComboBoxMapper = mapper;
            Assert.IsNotNull(lookupComboBoxMapper);
            Assert.AreEqual(true, lookupComboBoxMapper.IsReadOnly);
        }

        [Test]
        public void Test_SetBusinessObject()
        {
            //--------------- Set up test pack ------------------
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("TestProp");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            MyBO.LoadClassDefWithBOLookup();
            MyBO myBO = new MyBO();
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = myBO;
            //--------------- Test Result -----------------------
            Assert.AreSame(myBO, mapper.BusinessObject);
            Assert.AreSame(myBO, mapper.BusinessObject);
        }

        [Test]
        public void Test_ItemsShowingInComboBox()
        {
            //--------------- Set up test pack ------------------

            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();
            contactPersonTestBO.FirstName = TestUtil.GetRandomString();
            OrganisationTestBO.LoadDefaultClassDef();
            OrganisationTestBO.CreateSavedOrganisation();
            OrganisationTestBO.CreateSavedOrganisation();

            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxVWG, propName, true, controlFactory);
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);

            //--------------- Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //--------------- Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            //            Assert.AreEqual(2, mapper.LookupList.Count);
        }

        [Test]
        public void Test_SetBusinessObject_OnInternalLookupComboBoxMapper()
        {
            //--------------- Set up test pack ------------------
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            ContactPersonTestBO businessObjectInfo = new ContactPersonTestBO();
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = businessObjectInfo;
            //--------------- Test Result -----------------------
            Assert.AreSame(businessObjectInfo, mapper.BusinessObject);
        }
        [Test]
        public void Test_SetBusinessObject_ToNull_OnInternalLookupComboBoxMapper()
        {
            //--------------- Set up test pack ------------------
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = null;
            //--------------- Test Result -----------------------
            Assert.AreSame(null, mapper.BusinessObject);
        }

        private static ExtendedTextBoxMapper CreateExtendedLookupComboBoxMapper(string propertyName)
        {
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxVWG, propertyName, true, controlFactory);
            return mapper;
        }

        [Test]
        public void Test_ShowGridAndBOEditorControlVWGOnClick()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOs = CreateSavedOrganisationTestBOsCollection();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapperSpy mapper = new ExtendedTextBoxMapperSpy(
                extendedTextBoxVWG, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
            // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedTextBoxVWG.Button.PerformClick();
            mapper.CallSetupPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
            IFormHabanero form = mapper.PopupForm;
            Assert.AreEqual(800, form.Width);
            Assert.AreEqual(600, form.Height);
            Assert.AreEqual(2, form.Controls.Count);
            Assert.AreEqual(DockStyle.Fill, form.Controls[0].Dock);

            Assert.IsInstanceOf(typeof(IBOGridAndEditorControl), form.Controls[0]);
            Assert.IsInstanceOf(typeof(BOGridAndEditorControlVWG), form.Controls[0]);
            Assert.IsInstanceOf(typeof(IButtonGroupControl), form.Controls[1]);
            BOGridAndEditorControlVWG andBOGridAndEditorControlVWG = (BOGridAndEditorControlVWG)form.Controls[0];
            //Assert.AreSame(mapper.BusinessObject, BOGridAndEditorControlVWG.BOEditorControlVWG.BusinessObject);
            Assert.IsTrue(andBOGridAndEditorControlVWG.GridControl.IsInitialised);
            IBusinessObjectCollection collection = andBOGridAndEditorControlVWG.GridControl.Grid.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(organisationTestBOs.Count, collection.Count);
            //            Assert.AreEqual(organisationTestBOs.Count, mapper.LookupList.Count);
        }

        [Test]
        public void Test_SetupPopupForm_WithSuperClassDef_ShouldSetUpThePopUpForm()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOs = GetClassDefs();

            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapperSpy mapper = new ExtendedTextBoxMapperSpy(
                extendedTextBoxVWG, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
            // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedTextBoxVWG.Button.PerformClick();
            mapper.CallSetupPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
            IFormHabanero form = mapper.PopupForm;
            Assert.AreEqual(800, form.Width);
            Assert.AreEqual(600, form.Height);
            Assert.AreEqual(2, form.Controls.Count);
            Assert.AreEqual(DockStyle.Fill, form.Controls[0].Dock);

            Assert.IsInstanceOf(typeof(IBOGridAndEditorControl), form.Controls[0]);
            Assert.IsInstanceOf(typeof(BOGridAndEditorControlVWG), form.Controls[0]);
            Assert.IsInstanceOf<IButtonGroupControl>(form.Controls[1]);

            IButtonGroupControl control = (IButtonGroupControl)form.Controls[1];
            Assert.AreEqual("Cancel", control.Controls[0].Text);
            Assert.AreEqual("Select", control.Controls[1].Text);

            BOGridAndEditorControlVWG andBOGridAndEditorControlVWG = (BOGridAndEditorControlVWG)form.Controls[0];
            //Assert.AreSame(mapper.BusinessObject, BOGridAndEditorControlVWG.BOEditorControlVWG.BusinessObject);
            Assert.IsTrue(andBOGridAndEditorControlVWG.GridControl.IsInitialised);
            IBusinessObjectCollection collection = andBOGridAndEditorControlVWG.GridControl.Grid.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(organisationTestBOs.Count, collection.Count);
            //            Assert.AreEqual(organisationTestBOs.Count, mapper.LookupList.Count);
        }

        private static BusinessObjectCollection<OrganisationTestBO> GetClassDefs()
        {
            ClassDef.ClassDefs.Clear();
            PersonTestBO.LoadDefaultClassDefWithTestOrganisationBOLookup();
            ContactPersonTestBO.LoadDefaultClassDefWithPersonTestBOSuperClass();
            return CreateSavedOrganisationTestBOsCollection();
        }

        [Test]
        public void Test_SetBusinessObject_ShouldSetTextOnExtendedTextBox()
        {
            //--------------- Set up test pack ------------------
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            ContactPersonTestBO businessObjectInfo = new ContactPersonTestBO();
            var expectedTextBoxValue = TestUtil.GetRandomString();
            businessObjectInfo.Surname = expectedTextBoxValue;
            //--------------- Test Preconditions ----------------
            Assert.IsNotNullOrEmpty(businessObjectInfo.Surname);
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = businessObjectInfo;
            //--------------- Test Result -----------------------
            Assert.AreSame(businessObjectInfo, mapper.BusinessObject);
            IExtendedTextBox extendedTextBox = (IExtendedTextBox)mapper.Control;
            Assert.AreEqual(expectedTextBoxValue, extendedTextBox.Text, "Text should be set on TextBox");
        }
        [Test]
        public void Test_SetBusinessObject_WhenPropValueNull_ShouldSetTextOnExtendedTextBoxToEmpty()
        {
            //--------------- Set up test pack ------------------
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            ContactPersonTestBO businessObjectInfo = new ContactPersonTestBO { Surname = null };
            //--------------- Test Preconditions ----------------
            Assert.IsNull(businessObjectInfo.Surname);
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = businessObjectInfo;
            //--------------- Test Result -----------------------
            Assert.AreSame(businessObjectInfo, mapper.BusinessObject);
            IExtendedTextBox extendedTextBox = (IExtendedTextBox)mapper.Control;
            Assert.AreEqual("", extendedTextBox.Text, "Text on TextBox should be set to EmptyString");
        }

        [Test]
        public void Test_SelectButtonWhenClicked_ShouldApplyBusinessObjectChanges()
        {
            //---------------Set up test pack-------------------
            GetClassDefs();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapperSpy mapperSpy = new ExtendedTextBoxMapperSpy(
                extendedTextBoxVWG, propName, true, controlFactory);
            ContactPersonTestBO contactPersonTestBo = new ContactPersonTestBO();
            mapperSpy.BusinessObject = contactPersonTestBo;
            var expectedSelectedBO = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.AreSame(contactPersonTestBo, mapperSpy.BusinessObject);
            Assert.IsNull(contactPersonTestBo.OrganisationID);
            //---------------Execute Test ----------------------
            mapperSpy.CallSetupPopupForm();
            mapperSpy.SetSelectedBusinessObject(expectedSelectedBO);
            mapperSpy.CallSelectClick();
            //---------------Test Result -----------------------
            Assert.IsNotNull(contactPersonTestBo.OrganisationID);
            Assert.AreSame(expectedSelectedBO, contactPersonTestBo.Organisation);
        }

        [Test]
        public void Test_SelectButtonWhenClicked_ShouldClosePopupForm()
        {
            GetClassDefs();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapperSpy mapperSpy = new ExtendedTextBoxMapperSpy(
                extendedTextBoxVWG, propName, true, controlFactory);
            ContactPersonTestBO contactPersonTestBo = new ContactPersonTestBO();
            mapperSpy.BusinessObject = contactPersonTestBo;
            //---------------Assert Precondition----------------
            Assert.IsFalse(mapperSpy.FormClosed);
            //---------------Execute Test ----------------------
            mapperSpy.CallSetupPopupForm();
            mapperSpy.CallSelectClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(mapperSpy.FormClosed);
        }
        [Test]
        public void Test_SelectButtonWhenClicked_AndContactPersonNotValid_ShouldCloseWithoutError_FixBugBug541()
        {
            GetClassDefs();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapperSpy mapperSpy = new ExtendedTextBoxMapperSpy(
                extendedTextBoxVWG, propName, true, controlFactory);
            ContactPersonTestBO contactPersonTestBo = new ContactPersonTestBO();
            mapperSpy.BusinessObject = contactPersonTestBo;
            mapperSpy.SelectedBO = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.IsFalse(mapperSpy.FormClosed);
            Assert.IsFalse(contactPersonTestBo.Status.IsValid());
            Assert.IsTrue(mapperSpy.SelectedBO.Status.IsValid());
            Assert.IsNotNull(mapperSpy.SelectedBO);
            //---------------Execute Test ----------------------
            mapperSpy.CallSetupPopupForm();
            mapperSpy.CallSelectClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBo.Status.IsValid());
            Assert.IsTrue(mapperSpy.FormClosed);
        }

        [Test]
        public void Test_CancelButton_WhenClicked_ShouldCancelEditsAndCloseForm()
        {
            //---------------Set up test pack-------------------
            GetClassDefs();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxVWG extendedTextBoxVWG = new ExtendedTextBoxVWG(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapperSpy mapper = new ExtendedTextBoxMapperSpy(
                extendedTextBoxVWG, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
            mapper.CallSetupPopupForm();
            //---------------Assert Precondition----------------
            Assert.IsFalse(mapper.FormClosed);
            //---------------Execute Test ----------------------
            mapper.CallCancelClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(mapper.FormClosed);
        }


        private static BusinessObjectCollection<OrganisationTestBO> CreateSavedOrganisationTestBOsCollection()
        {
            OrganisationTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOs = new BusinessObjectCollection<OrganisationTestBO>
               {
                   OrganisationTestBO.CreateSavedOrganisation(),
                   OrganisationTestBO.CreateSavedOrganisation(),
                   OrganisationTestBO.CreateSavedOrganisation(),
                   OrganisationTestBO.CreateSavedOrganisation()
               };
            return organisationTestBOs;
        }

    }

    public class ExtendedTextBoxMapperSpy : ExtendedTextBoxMapper
    {
        public bool FormClosed { get; private set; }

        public ExtendedTextBoxMapperSpy(IExtendedTextBox ctl, string propName,
            bool isReadOnly, IControlFactory controlFactory)
            : base(ctl, propName, isReadOnly, controlFactory)
        {
            FormClosed = false;
        }
        public void CallCancelClick()
        {
            base.CancelClickHandler(new object(), new EventArgs());
        }
        public void CallSelectClick()
        {
            base.SelectClickHandler(new object(), new EventArgs());
        }

        public void SetSelectedBusinessObject(IBusinessObject bo)
        {
            SelectedBO = bo;
        }

        public IBusinessObject SelectedBO { get; set; }

        protected override IBusinessObject GetSelectedBusinessObject()
        {
            return SelectedBO;
        }

        public void CallSetupPopupForm()
        {
            base.SetupPopupForm();
        }

        protected override void CloseForm()
        {
            FormClosed = true;
            HandlePopUpFormClosedEvent(new object(), new CancelEventArgs());
        }
    }
}