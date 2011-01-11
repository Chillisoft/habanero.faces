using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base.Mappers
{
    public abstract class TestRelationshipComboBoxMapper
    {
        protected IControlFactory _controlFactory;
        private IClassDef _cpClassDef;
        private IClassDef _orgClassDef;

        protected virtual IControlFactory GetControlFactory()
        {
            if (_controlFactory == null) CreateControlFactory();
            return _controlFactory;
        }

        protected abstract void CreateControlFactory();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            CreateControlFactory();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerFake();
            ClassDef.ClassDefs.Clear();
            _cpClassDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            _orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
        }

        [Test]
// ReSharper disable InconsistentNaming
        public void Test_Constructor()

        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            //---------------Execute Test ----------------------
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper
                (cmbox, relationshipName, false, GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreSame(cmbox, mapper.Control);
            Assert.AreSame(relationshipName, mapper.RelationshipName);
            Assert.AreEqual(false, mapper.IsReadOnly);
            Assert.AreSame(GetControlFactory(), mapper.ControlFactory);
            Assert.IsTrue(mapper.IncludeBlankItem);
        }

        [Test]
        public void Test_Constructor_WhenComboBoxNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            const string relationshipName = "RelationshipDoesNotExist";
            try
            {
                new RelationshipComboBoxMapper(null, relationshipName, false, GetControlFactory());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("comboBox", ex.ParamName);
            }
        }

        [Test]
        public void Test_Constructor_WhenControlFactoryNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            //---------------Execute Test ----------------------
            const string relationshipName = "RelationshipDoesNotExist";
            try
            {
                new RelationshipComboBoxMapper(cmbox, relationshipName, false, null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_Constructor_WhenRelationshipNameNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            //---------------Execute Test ----------------------
            try
            {
                new RelationshipComboBoxMapper(cmbox, null, false, GetControlFactory());
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
        public void Test_BusinessObject_WhenSet_WhenRelationshipDoesNotExistInClassDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            const string relationshipName = "RelationshipDoesNotExist";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            //---------------Execute Test ----------------------

            try
            {
                mapper.BusinessObject = new ContactPersonTestBO();
                Assert.Fail("expected HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (RelationshipNotFoundException ex)
            {
                StringAssert.Contains
                    (string.Format("The relationship '{0}' on '{1}' cannot be found.", 
                                   relationshipName, _cpClassDef.ClassNameFull), ex.Message);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WhenRelationshipNotSingle_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            const string relationshipName = "ContactPeople";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObject = new OrganisationTestBO();
                Assert.Fail("expected HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    (string.Format("The relationship '{0}' on '{1}' is not a single relationship. ", 
                                   relationshipName, _orgClassDef.ClassNameFull),
                     ex.Message);
                StringAssert.Contains
                    ("The 'RelationshipComboBoxMapper' can only be used for single relationships",
                     ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_Constructor_ShouldHaveNoBusinessObjectSet_ShouldDisableControl()
        {
            //--------------- Set up test pack ------------------
            IComboBox cmbox = CreateComboBox();
            const string relationshipName = "Organisation";
            //--------------- Test Preconditions ----------------
            //--------------- Execute Test ----------------------
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            //--------------- Test Result -----------------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsFalse(mapper.Control.Enabled);
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSetToEmpty_ShouldPopulateItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper
                (cmbox, relationshipName, false, GetControlFactory());
            IBusinessObjectCollection boCol = new BusinessObjectCollection<OrganisationTestBO>();
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = boCol;
            //---------------Test Result -----------------------
            Assert.AreSame(boCol, mapper.BusinessObjectCollection);
            Assert.AreEqual(1, cmbox.Items.Count, "Should have blank Item");
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSetToEmpty_WhenIncludeBlankItemFalse_ShouldNotIncludeBlankItem()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.IncludeBlankItem = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(mapper.IncludeBlankItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cmbox.Items.Count, "Should not have blank item");
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSetWithOneItem_ShouldPopulateItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper
                (cmbox, relationshipName, false, GetControlFactory());
            IBusinessObjectCollection boCol = GetBoColWithOneItem();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, boCol.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = boCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, cmbox.Items.Count, "Should have one object and blank item");
            Assert.IsInstanceOf(typeof (IBusinessObject), cmbox.Items[1]);
            Assert.IsTrue(cmbox.Items.Contains(boCol[0]));
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObjectCollection_WithExistingCollectionSet_WhenSetToNewCollectionWithOneItem_ShouldRePopulateItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, cmbox.Items.Count, "Should have one object and blank item");
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cmbox.Items.Count, "Should have blank item");
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSetToNewCollectionContainingSelectedItem_ShouldPreserveSelection()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            OrganisationTestBO selectedOrganisation = boCol[0];
            person.Organisation = selectedOrganisation;
            mapper.BusinessObject = person;
            BusinessObjectCollection<OrganisationTestBO> newBoCol = new BusinessObjectCollection<OrganisationTestBO>
                    {boCol};
            //---------------Assert Precondition----------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.AreSame(selectedOrganisation, person.Organisation);
            Assert.AreSame(selectedOrganisation, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = newBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.AreSame(selectedOrganisation, person.Organisation);
            Assert.AreSame(selectedOrganisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WhenMappersClassDefIsNull_ShouldSetClassDefFromBo()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.IsNull(mapper.ClassDef);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person.ClassDef, mapper.ClassDef);
            Assert.AreSame(person, mapper.BusinessObject);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_ShouldSelectRelatedItemInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO {Organisation = boCol[0]};
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            Assert.IsFalse(mapper.Control.Enabled);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsTrue(mapper.Control.Enabled);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreSame(person.Organisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WhenRelationshipIsLevelsDeep_ShouldSelectRelatedItemInComboBox()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            OrganisationTestBO.LoadDefaultClassDef();
            AddressTestBO.LoadDefaultClassDef();
            const string relationshipName = "ContactPersonTestBO.Organisation";
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(relationshipName);
            IComboBox cmbox = mapper.Control;
            BusinessObjectCollection<OrganisationTestBO> boCol = (BusinessObjectCollection<OrganisationTestBO>)mapper.BusinessObjectCollection;
            ContactPersonTestBO person = new ContactPersonTestBO {Organisation = boCol[0]};
            AddressTestBO addressTestBO = new AddressTestBO { ContactPersonTestBO = person };
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            Assert.IsFalse(mapper.Control.Enabled);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = addressTestBO;
            //---------------Test Result -----------------------
            Assert.AreSame(addressTestBO, mapper.BusinessObject);
            Assert.IsTrue(mapper.Control.Enabled);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreSame(person.Organisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WithIncorrectType_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            mapper.ClassDef = _cpClassDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObject = new OrganisationTestBO();
                Assert.Fail("expected HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("You cannot set the Business Object to the 'OrganisationTestBO' identified as '", ex.Message);
                StringAssert.Contains
                    ("since it is not of the appropriate type ('ContactPersonTestBO') for this '", ex.Message);
                StringAssert.Contains("RelationshipComboBoxMapper", ex.Message);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WhenIsReadOnly_IsTrue_ShouldNotBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            BusinessObjectCollection<OrganisationTestBO> boCol = GetBoColWithOneItem();
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, true, GetControlFactory());
            mapper.BusinessObjectCollection = boCol;
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            //---------------Assert Preconditions---------------
            Assert.IsTrue(mapper.IsReadOnly);
            Assert.IsTrue(person.Status.IsNew);
            Assert.IsFalse(cmbox.Enabled);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.IsFalse(cmbox.Enabled);
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSet_WithIncorrectType_BeforeBOIsSet_ShouldNotRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //mapper.ClassDef = _cpClassDef;
            BusinessObjectCollection<ContactPersonTestBO> businessObjectCollection = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = businessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(businessObjectCollection, mapper.BusinessObjectCollection);
        }

        [Test]
        public void Test_BusinessObjectCollection_WhenSet_WithIncorrectType_AfterBOIsSet_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //mapper.ClassDef = _cpClassDef;
            mapper.BusinessObject = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObjectCollection = new BusinessObjectCollection<ContactPersonTestBO>();
                Assert.Fail("expected HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("You cannot set the Business Object Collection to the 'ContactPersonTestBO'", ex.Message);
                StringAssert.Contains
                    ("since it is not of the appropriate type ('OrganisationTestBO') for this '", ex.Message);
                StringAssert.Contains("RelationshipComboBoxMapper", ex.Message);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WhenExistsInCollection_ShouldSelectRelatedItemInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, mapper.Control.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(person.Organisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObject_WhenSet_WhenDoesNotExistInCollection_ShouldAddRelatedItemToComboBox()
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
        }
     
        [Test]
        public void Test_BusinessObject_WhenSetWithNullRelatedObject_WhenItemAlreadySelected_ShouldSelectNoItemInList()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            boCol.CreateBusinessObject();
            ContactPersonTestBO origCP = new ContactPersonTestBO();
            origCP.Organisation = boCol[0];
            mapper.BusinessObject = origCP;

            ContactPersonTestBO person = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.IsNull(person.Organisation);
            Assert.AreSame(boCol[0], cmbox.SelectedItem);
            Assert.AreNotEqual("", cmbox.Text);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.AreEqual(null, cmbox.SelectedItem);
            Assert.AreEqual("", cmbox.Text);
        }

        [Test]
        public void Test_RelatedObjectChanged_ShouldUpdate()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO contactPerson = new ContactPersonTestBO {Organisation = boCol[0]};
            mapper.BusinessObject = contactPerson;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreNotSame(newOrganisation, contactPerson.Organisation);
            Assert.AreNotSame(newOrganisation, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = newOrganisation;
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, contactPerson.Organisation);
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            
        }

        [Test]
        public void Test_RelatedObjectChanged_WhenRelatedObjectPathChanged_WhenRelationshipMultipleLevelsDeep_ShouldUpdate()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            OrganisationTestBO.LoadDefaultClassDef();
            AddressTestBO.LoadDefaultClassDef();
            const string relationshipName = "ContactPersonTestBO.Organisation";
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(relationshipName);
            IComboBox cmbox = mapper.Control;
            BusinessObjectCollection<OrganisationTestBO> boCol = (BusinessObjectCollection<OrganisationTestBO>)mapper.BusinessObjectCollection;
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO newPerson = new ContactPersonTestBO{Organisation = newOrganisation};
            AddressTestBO addressTestBO = new AddressTestBO { ContactPersonTestBO = person };
            mapper.BusinessObject = addressTestBO;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreNotSame(newOrganisation, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            addressTestBO.ContactPersonTestBO = newPerson;
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_ShouldReturnBusinessObjectsRelatedObject()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO {Organisation = boCol[0]};
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(person.Organisation);
            //---------------Execute Test ----------------------
            IBusinessObject businessObject = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(person.Organisation, businessObject);
        }

        protected IComboBox CreateComboBox()
        {
            return GetControlFactory().CreateComboBox();
        }

        [Test]
        public void Test_GetRelatedBusinessObject_WhenBODoesNotHaveRelatedBO_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNull(person.Organisation);
            //---------------Execute Test ----------------------
            IBusinessObject businessObject = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsNull(businessObject);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_WhenNullBo_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(mapper.BusinessObject);
            //---------------Execute Test ----------------------
            IBusinessObject relatedBO = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsNull(relatedBO);
        }

        [Test]
        public void Test_GetRelatedBusinessObject_WhenRelationshipIsLevelsDeep_ShouldReturnCorrectBO()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            OrganisationTestBO.LoadDefaultClassDef();
            AddressTestBO.LoadDefaultClassDef();
            const string relationshipName = "ContactPersonTestBO.Organisation";
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(relationshipName);
            BusinessObjectCollection<OrganisationTestBO> boCol = (BusinessObjectCollection<OrganisationTestBO>)mapper.BusinessObjectCollection;
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            AddressTestBO addressTestBO = new AddressTestBO{ContactPersonTestBO = person};
            mapper.BusinessObject = addressTestBO;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            //---------------Execute Test ----------------------
            IBusinessObject relatedBO = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(person.Organisation, relatedBO);
        }

        protected static ContactPersonTestBO CreateCPWithRelatedOrganisation(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            person.Organisation = organisationTestBO;
            return person;
        }

        [Test]
        public virtual void Test_AddBOToCol_ShouldUpdateItems()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Execute Test ----------------------
            boCol.Add(newBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, mapper.Control.Items.Count);
        }

        [Test]
        //TODO Mark 07 Aug 2009: NNB Review this - I don't think that this is the action you want because it could have side effects when creating a new BO
        public virtual void Test_AddBOToCol_WhenNullOrEmptyToString_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            RelationshipComboBoxMapper mapper1 = GetMapper(cmbox);
            BusinessObjectCollection<ContactPersonTestBO> boCol = new BusinessObjectCollection<ContactPersonTestBO> { new ContactPersonTestBO() };
            mapper1.BusinessObjectCollection = boCol;
            RelationshipComboBoxMapper mapper = mapper1;
            ContactPersonTestBO newBO = new ContactPersonTestBO {Surname = ""};
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            Assert.IsNull(newBO.ToString());
            //---------------Execute Test ----------------------
            try
            {
                boCol.Add(newBO);
                Assert.Fail("expected HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string message = string.Format("Cannot add a business object of type '{0}' "
                                               + "to the 'ComboBoxCollectionSelector' if its ToString is null or zero length"
                                               , newBO.ClassDef.ClassName);
                StringAssert.Contains(message, ex.Message);
            }
        }

        [Test]
        public virtual void Test_RemoveBOFromCol_ShouldUpdateItems()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO bo = boCol[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Execute Test ----------------------
            boCol.Remove(bo);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boCol.Count);
            Assert.AreEqual(1, mapper.Control.Items.Count);
        }

        [Test]
        public virtual void Test_BusinessObjectCollection_WhenSetToNull_ShouldNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObjectCollection);
            Assert.AreEqual(1, mapper.Control.Items.Count);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_ShouldNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(cmbox.SelectedItem)));
        }

        [Test]
        // Note: Mark - This test name doesn't match what the test does. Look into renaming or removing.
        public void Test_BusinessObject_WhenSetToBO_AfterSetToNull_ShouldFillList_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(cmbox.SelectedItem)));
            Assert.AreEqual(2, cmbox.Items.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(1 + 1, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void Test_BusinessObject_WhenSetToNull_WhenNullCollectionIsSet_ShouldNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.BusinessObjectCollection = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(mapper.BusinessObjectCollection);
            Assert.AreEqual(1, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(cmbox.SelectedItem);
            Assert.AreEqual(1, cmbox.Items.Count, "Should have only the null item in it.");
        }

        [Test]
        public void Test_ApplyChangesToBusinessObject_WhenAnItemIsSelectedAndRelatedBusnessObjectWasNull_ShouldUpdateBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
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

        [Test]
        public void Test_ApplyChangesToBusinessObject_WhenNewItemIsSelected_ShouldUpdateBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO relatedBo = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO {Organisation = relatedBo};
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
        public void Test_ApplyChangesToBusinessObject_WhenNewItemIsSelected_WhenSet_WhenRelationshipIsLevelsDeep_ShouldUpdateRelatedBusinessObjectWithSelectedValue()
        {
            //---------------Set up test pack-------------------

            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            OrganisationTestBO.LoadDefaultClassDef();
            AddressTestBO.LoadDefaultClassDef();
            const string relationshipName = "ContactPersonTestBO.Organisation";
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(relationshipName);
            IComboBox cmbox = mapper.Control;
            BusinessObjectCollection<OrganisationTestBO> boCol = (BusinessObjectCollection<OrganisationTestBO>)mapper.BusinessObjectCollection;
            ContactPersonTestBO person = new ContactPersonTestBO {Organisation = boCol[0]};
            AddressTestBO addressTestBO = new AddressTestBO { ContactPersonTestBO = person };
            mapper.BusinessObject = addressTestBO;
            OrganisationTestBO newOrganisation = new OrganisationTestBO();
            boCol.Add(newOrganisation);
            mapper.BusinessObject = addressTestBO;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, cmbox.Items.Count);
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
        public void Test_ApplyChangesToBusinessObject_WhenNoItemIsSelected_ShouldUpdateBusinessObjectWithNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO relatedBo = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = relatedBo;
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreNotSame(newOrganisation, person.Organisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedIndex = 0;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(null, cmbox.SelectedItem);
            Assert.IsNull(person.Organisation);
        }

        [Test]
        public void Test_ApplyChangesToBusinessObject_WhenInvalidItemInComboIsSelected_ShouldSetBOPropValueToNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            mapper.BusinessObject = person;
            cmbox.Items.Add("SomeItem");
            //---------------Assert Preconditions---------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual("SomeItem", LastComboBoxItem(cmbox).ToString());
            //---------------Execute Test ----------------------
            cmbox.SelectedIndex = cmbox.Items.Count - 1;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsNull(person.Organisation);
        }

        [Test]
        public void Test_UpdateControlValueFromBusinessObject_WhenSetNewValue_AfterNullCurrentValue_ShouldUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNotNull(mapper.BusinessObjectCollection);
            Assert.IsNull(cmbox.SelectedItem, "There should be no selected item to start with");
            //---------------Execute Test ----------------------

            person.Organisation = organisationTestBO;
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(organisationTestBO, cmbox.SelectedItem, "Value is not set after changing bo prop Value");
        }

        [Test]
        public void Test_UpdateControlValueFromBusinessObject_ShouldUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = organisationTestBO;
            mapper.BusinessObject = person;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(2, boCol.Count);
            Assert.IsNotNull(mapper.BusinessObjectCollection);
            Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
            //---------------Execute Test ----------------------

            person.Organisation = newBO;
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(newBO, cmbox.SelectedItem, "Value is not set after changing bo prop Value");
        }

        [Test]
        public virtual void Test_ChangeComboBoxSelected_ShouldNotUpdatePropValue_VWGOnly()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO relatedBo = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = relatedBo;
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            //---------------Test Result -----------------------
            Assert.AreNotSame(newOrganisation, person.Organisation);
            Assert.AreSame(relatedBo, person.Organisation);
        }

        [Test]
        public void Test_STATE_WhenComposition_AndBusinessObjectNew_ShouldBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsTrue(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsTrue(enabled);
        }

        [Test]
        public void Test_STATE_WhenComposition_AndBusinessObjectNotNew_ShouldNotBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            person.Surname = TestUtil.GetRandomString();
            person.Save();
            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsFalse(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_STATE_WhenComposition_AndBusinessObjectSaved_ShouldNotBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            person.Surname = TestUtil.GetRandomString();

            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsTrue(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            person.Save();
            //---------------Execute Test ----------------------
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(person.Status.IsNew);
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_STATE_WhenIsReadOnly_IsFalse_ShouldBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper1 = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            mapper1.BusinessObject = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.IsFalse(mapper1.IsReadOnly);
            //---------------Execute Test ----------------------
            bool enabled = mapper1.Control.Enabled;
            //---------------Test Result -----------------------
            Assert.IsTrue(enabled);
        }

        [Test]
        public void Test_STATE_WhenIsReadOnly_IsTrue_ShouldNotBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper1 = new RelationshipComboBoxMapper(cmbox,  relationshipName, true, GetControlFactory());
            //---------------Assert Precondition----------------
            Assert.IsTrue(mapper1.IsReadOnly);
            //---------------Execute Test ----------------------
            bool enabled = mapper1.Control.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_STATE_WhenIsReadOnly_IsFalse_AndComposition_AndBusinessObjectSaved_ShouldDisable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            BusinessObjectCollection<OrganisationTestBO> boCol = GetBoColWithOneItem();
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            mapper.BusinessObjectCollection = boCol;
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            person.Surname = TestUtil.GetRandomString();

            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsFalse(mapper.IsReadOnly);
            Assert.IsTrue(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            Assert.IsTrue(cmbox.Enabled);
            //---------------Execute Test ----------------------
            person.Save();
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(person.Status.IsNew);
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_CreateControlMapper()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IControlMapper controlMapper = ControlMapper.Create
                ("RelationshipComboBoxMapper", "Habanero.Faces.Base", cmbox, TestUtil.GetRandomString(), false,
                 GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(RelationshipComboBoxMapper), controlMapper);
        }

        [Test]
        public void Test_CreateAutoLoadingMapper()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IControlMapper controlMapper = ControlMapper.Create
                ("AutoLoadingRelationshipComboBoxMapper", "Habanero.Faces.Base", cmbox, TestUtil.GetRandomString(), false,
                 GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(AutoLoadingRelationshipComboBoxMapper), controlMapper);     
            Assert.IsInstanceOf(typeof(RelationshipComboBoxMapper), controlMapper);     
        }

        [Test]
        public void Test_AutoLoadingMapper()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            IControlMapper controlMapper = ControlMapper.Create
                ("AutoLoadingRelationshipComboBoxMapper", "Habanero.Faces.Base", cmbox, "ContactPersonTestBO", false,
                 GetControlFactory());
            //ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteDoNothing();

            ContactPersonTestBO person1 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO person2 = ContactPersonTestBO.CreateSavedContactPerson();

            AddressTestBO addressTestBo = new AddressTestBO();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controlMapper.BusinessObject = addressTestBo;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.IsTrue(cmbox.Items.Contains(person1));
            Assert.IsTrue(cmbox.Items.Contains(person2));

        }

        protected RelationshipComboBoxMapper GetMapperBoColHasOneItem
            (IComboBox cmbox, out BusinessObjectCollection<OrganisationTestBO> boCol)
        {
            RelationshipComboBoxMapper mapper = GetMapper(cmbox);
            boCol = GetBoColWithOneItem();
            mapper.BusinessObjectCollection = boCol;
            return mapper;
        }

        protected RelationshipComboBoxMapper GetMapperBoColHasOneItem(string relationshipName)
        {
            RelationshipComboBoxMapper mapper = GetMapper(CreateComboBox(), relationshipName);
            mapper.BusinessObjectCollection = GetBoColWithOneItem();
            return mapper;
        }

        protected static BusinessObjectCollection<OrganisationTestBO> GetBoColWithOneItem()
        {
            return new BusinessObjectCollection<OrganisationTestBO> {new OrganisationTestBO()};
        }

        protected RelationshipComboBoxMapper GetMapper(IComboBox cmbox)
        {
            return GetMapper(cmbox, "Organisation");
        }

        private RelationshipComboBoxMapper GetMapper(IComboBox cmbox, string relationshipName)
        {
            return new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
        }

        private static object LastComboBoxItem(IComboBox cmbox)
        {
            return cmbox.Items[cmbox.Items.Count - 1];
        }

        //TODO Mark 11 Aug 2009: Test Multiple Levels: Construct with a BORelationshipMapper, should have this BORelationshipMapper and use it

        //Test Multiple Levels: When the BO is set, it should select the correct item in the list

        //Test Multiple Levels: When the BO's related BO in the path is changed, it should select the new item in the list

        //Test Multiple Levels: When the item is selected, it should update the prop of the BO
    }

    public class BusinessObjectManagerFake : BusinessObjectManager
    {
    }

}