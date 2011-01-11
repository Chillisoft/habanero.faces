//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Base.Controllers
{

    public abstract class TestComboBoxCollectionSelector
    {
        protected abstract IControlFactory GetControlFactory();

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        private ComboBoxCollectionSelector CreateComboBoxCollectionSelector()
        {
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            return new ComboBoxCollectionSelector(cmbox, controlFactory);
        }

        private static void SetupSelectorWithTestPackCollection(ComboBoxCollectionSelector selectorManager, bool includeBlankItems)
        {
            MyBO.LoadDefaultClassDef();
            selectorManager.IncludeBlankItem = includeBlankItems;
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManager.SetCollection(myBOs);
        }

        [Test]
        public void Test_CreateTestComboBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithBoolean();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> {{new MyBO(), new MyBO()}};
            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionSelector selector = new ComboBoxCollectionSelector(cmb,GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selector.SetCollection(myBOs);
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, selector.BusinessObjectCollection);
            Assert.AreSame(cmb,selector.Control);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void Test_SetCollectionNull()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadClassDefWithBoolean();

            IComboBox cmb = GetControlFactory().CreateComboBox();
            ComboBoxCollectionSelector selector = new ComboBoxCollectionSelector(cmb, GetControlFactory());
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selector.SetCollection(null);
            //---------------Verify Result -----------------------
            Assert.IsNull(selector.BusinessObjectCollection);
            Assert.AreSame(cmb,selector.Control);
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            ComboBoxCollectionSelector selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectorManager);
            Assert.AreSame(cmbox, selectorManager.Control);
            Assert.AreSame(controlFactory, selectorManager.ControlFactory);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_Constructor_NullControlFactoryRaisesError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxCollectionSelector(cmbox, null);
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
        public void Test_Constructor_NullComboBoxRaisesError()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            try
            {
                new ComboBoxCollectionSelector(null, controlFactory);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("listControl", ex.ParamName);
            }
        }

        [Test]
        public void Test_SetComboBoxCollection()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory) {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            var myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            selectorManager.SetCollection(myBoCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManager.BusinessObjectCollection.Count);
            Assert.AreEqual(3, selectorManager.Control.Items.Count);
        }

        [Test]
        public void Test_SetComboBoxCollection_AddNullItemTrue()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            var myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            selectorManager.SetCollection(myBoCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManager.BusinessObjectCollection.Count);
            Assert.AreEqual(4, selectorManager.Control.Items.Count);
        }

        [Test]
        public void Test_SetComboBoxCollection_IncludeBlankFalse_SetsFirstItem()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            var firstBo = new MyBO();
            var myBoCol = new BusinessObjectCollection<MyBO> 
                                                         { firstBo, new MyBO(), new MyBO() };
            //---------------Execute Test ----------------------
            selectorManager.SetCollection(myBoCol);
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManager.SelectedBusinessObject);
        }

        [Test]
        public void Test_SetComboBoxCollection_IncludeBlank_True()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory);
            MyBO.LoadDefaultClassDef();
            var firstBo = new MyBO();
            var myBoCol = new BusinessObjectCollection<MyBO>
                                                         {firstBo, new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            selectorManager.SetCollection(myBoCol);
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManager.SelectedBusinessObject);
            Assert.AreEqual(4, cmbox.Items.Count);
        }

        [Test]
        public void Test_SelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory)
                                                             {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            var selectedBO = new MyBO();
            var myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), selectedBO, new MyBO()};
            //---------------Execute Test ----------------------
            selectorManager.SetCollection(myBoCol);
            selectorManager.Control.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, selectorManager.SelectedBusinessObject);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_BusinessObjectAddedToCollection()
        {
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory)
                                                             {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            var addedBo = new MyBO();
            var myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManager.SetCollection(myBoCol);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManager.Control.Items.Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_BusinessObjectRemovedFromCollection()
        {
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory)
                                                             {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), removedBo, new MyBO()};
            selectorManager.SetCollection(myBoCol);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectorManager.Control.Items.Count);
        }

        [Test]
        public void Test_EditItemFromCollection_UpdatesItemInCombo()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory) {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            var myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManager.SetCollection(myBOs);
            var myBO = myBOs[0];
            var origToString = myBO.ToString();
            var newValue = Guid.NewGuid();
            var index = cmbox.Items.IndexOf(myBO);
            cmbox.SelectedIndex = index;
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreEqual(origToString, cmbox.Text);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            //Assert.AreNotEqual(origToString, cmbox.Text);
            //Assert.AreEqual(newToString, cmbox.Text);
        }

        [Test]
        public void Test_EditSecondItemFromCollection_UpdatesItemInCombo()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory) {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            var myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManager.SetCollection(myBOs);
            var myBO = myBOs[1];
            string origToString = myBO.ToString();
            var newValue = Guid.NewGuid();
            var index = cmbox.Items.IndexOf(myBO);
            cmbox.SelectedIndex = index;
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(1, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreEqual(origToString, cmbox.Text);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            //Assert.AreNotEqual(origToString, cmbox.Text);
            //Assert.AreEqual(newToString, cmbox.Text);
        }

        [Test]
        public void Test_EditUnselectedItemFromCollection_UpdatesItemInCombo_DoesNotSelectItem()
        {
            //---------------Set up test pack-------------------
            var cmbox = GetControlFactory().CreateComboBox();
            var controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory) {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            var myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManager.SetCollection(myBOs);
            var myBO = myBOs[1];
            var origToString = myBO.ToString();
            var newValue = Guid.NewGuid();
            var index = cmbox.Items.IndexOf(myBO);
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreNotEqual(index, cmbox.SelectedIndex);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(0, cmbox.SelectedIndex);
        }

        [Test]
        public void Test_EditUnselectedItemFromCollection_UpdatesItemInCombo_DoesNotSelectItem_WithBlank()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory, false);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManager.SetCollection(myBOs);
            MyBO myBO = myBOs[1];
            string origToString = myBO.ToString();
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            //---------------Assert precondition----------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual(-1, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(origToString, cmbox.Items[index].ToString());
            Assert.AreNotEqual(index, cmbox.SelectedIndex);
            //---------------Execute Test ----------------------
            myBO.MyBoID = newValue;
            //---------------Test Result -----------------------
            string newToString = myBO.ToString();
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual(-1, cmbox.SelectedIndex);
//            Assert.AreNotEqual(origToString, cmbox.Text);
//            Assert.AreEqual(newToString, cmbox.Text);
        }

        [Test]
        public void Test_CancelEditsItemFromCollection_UpdatesItemInCombo()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            ComboBoxCollectionSelector selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory)
                                                             {IncludeBlankItem = false};
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            selectorManager.SetCollection(myBOs);
            MyBO myBO = myBOs[0];
            Guid newValue = Guid.NewGuid();
            int index = cmbox.Items.IndexOf(myBO);
            cmbox.SelectedIndex = index;
            myBO.MyBoID = newValue;
            myBO.Save();
            //---------------Assert precondition----------------
            Assert.AreEqual(2, cmbox.Items.Count);
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            //---------------Execute Test ----------------------
            myBO.MyBoID = Guid.NewGuid();
            myBO.CancelEdits();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cmbox.SelectedIndex);
            Assert.AreEqual(index, cmbox.SelectedIndex);
            Assert.AreSame(myBO, cmbox.Items[index]);
            string newToString = myBO.ToString();
            Assert.AreEqual(newToString, cmbox.Items[index].ToString());

        }

        [Test]
        public void Test_ShouldSupportIBOColSelector()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ComboBoxCollectionSelector selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory, false);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IBOColSelector>(selectorManager);
        }

        [Test]
        public void Test_NoOfItems_WhenNoBlankItem_ShouldReturnColCount()
        {
            //---------------Set up test pack-------------------
            ComboBoxCollectionSelector selectorManager = CreateComboBoxCollectionSelector();
            SetupSelectorWithTestPackCollection(selectorManager, false);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            int noOfItems = selectorManager.NoOfItems;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectorManager.BusinessObjectCollection.Count, noOfItems);
        }

        [Test]
        public void Test_NoOfItems_WhenHasBlankItem_ShouldReturnColCountPlusOne()
        {
            //---------------Set up test pack-------------------
            ComboBoxCollectionSelector selectorManager = CreateComboBoxCollectionSelector();
            SetupSelectorWithTestPackCollection(selectorManager, true);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            int noOfItems = selectorManager.NoOfItems;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectorManager.BusinessObjectCollection.Count + 1, noOfItems);
        }

        [Test]
        public void Test_BusinessObjectCollection_Get_ShouldReturnSameAsCollection()
        {
            //---------------Set up test pack-------------------
            ComboBoxCollectionSelector selectorManager = CreateComboBoxCollectionSelector();
            SetupSelectorWithTestPackCollection(selectorManager, true);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IBusinessObjectCollection businessObjectCollection = selectorManager.BusinessObjectCollection;
            //---------------Test Result -----------------------
            Assert.AreSame(selectorManager.BusinessObjectCollection, businessObjectCollection);
        }

        [Test]
        public void Test_BusinessObjectCollection_Set_ShouldSetCollection()
        {
            //---------------Set up test pack-------------------
            ComboBoxCollectionSelector selectorManager = CreateComboBoxCollectionSelector();
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> { { new MyBO(), new MyBO() } };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBOs;
            //---------------Test Result -----------------------
            Assert.AreSame(myBOs, selectorManager.BusinessObjectCollection);
        }
        
        [Test]
        public void Test_SelectedBusinessObject_EdgeCase_SelectCustomStringItem_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            IControlFactory controlFactory = GetControlFactory();
            var selectorManager = new ComboBoxCollectionSelector(cmbox, controlFactory);
            selectorManager.IncludeBlankItem = false;
            MyBO.LoadDefaultClassDef();
            MyBO selectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { new MyBO(), selectedBO, new MyBO() };
            selectorManager.IncludeBlankItem = false;
            selectorManager.SetCollection(myBoCol);
            selectorManager.Control.SelectedIndex = 1;
            cmbox.Items.Add("SomeItem");
            //---------------Assert Preconditions---------------
            Assert.AreEqual(selectedBO, selectorManager.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            cmbox.SelectedIndex = cmbox.Items.Count - 1;
            IBusinessObject selectedBusinessObject = selectorManager.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }
    }
}
// ReSharper restore InconsistentNaming
