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
using Habanero.Faces.Base.CF;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{

    public abstract class TestBOColTabControl : TestMapperBase
    {
        protected abstract IControlFactory GetControlFactory();
        protected abstract IBusinessObjectControl GetBusinessObjectControlStub();
        protected abstract Type ExpectedTypeOfBOControl();
        [SetUp]
        public void TestSetup()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }

            [Test]
        public void TestConstructor()
        {
            //---------------Execute Test ----------------------
            IBOColTabControl iboColTabControl = GetControlFactory().CreateBOColTabControl();
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(iboColTabControl.TabControl);
            Assert.IsInstanceOf(typeof(ITabControl), iboColTabControl.TabControl);
        }

        [Test]
        public void TestSetBusinessObjectControl()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.AreSame(busControl, boColTabControl.BusinessObjectControl);
        }

        [Test]
        public void TestSetCollection()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol, boColTabControl.BusinessObjectCollection);
            Assert.AreEqual(myBoCol.Count, boColTabControl.TabControl.TabPages.Count);
        }

        [Test]
        public void TestSetCollectionTwice()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol, boColTabControl.BusinessObjectCollection);
            Assert.AreEqual(3, boColTabControl.TabControl.TabPages.Count);
        }

//        [Test]
//        public void TestSetCollectionHasNoObjects()
//        {
//            //---------------Set up test pack-------------------
//            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
//            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
//            boColTabControl.IBOEditor = busControl;
//            BusinessObjectCollection<MyBO> myBOS = new BusinessObjectCollection<MyBO>();
//
//            //---------------Execute Test ----------------------
//            boColTabControl.BusinessObjectCollection = myBOS;
//
//            //---------------Test Result -----------------------
//            Assert.AreSame(myBOS, boColTabControl.BusinessObjectCollection);
//            Assert.IsNull(boColTabControl.CurrentBusinessObject);
//        }

        [Test]
        public void TestGetBo()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection= myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol[1], boColTabControl.GetBo(boColTabControl.TabControl.TabPages[1]));
        }

        [Test]
        public void TestGetTabPage()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(boColTabControl.TabControl.TabPages[2], boColTabControl.GetTabPage(myBoCol[2]));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestCurrentBusinessObject_ReturnsNullWhenNoCollectionIsSet()
        {
            //---------------Execute Test ----------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            boColTabControl.BusinessObjectControl = GetBusinessObjectControlStub();
            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_ReturnsNullWhenCollectionIsSetAndThenSetToNull()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Assert Precondition----------------
            Assert.IsNotNull(boColTabControl.BusinessObjectCollection);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = null;

            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.CurrentBusinessObject);
            Assert.IsNull(boColTabControl.BusinessObjectCollection);
        }

        [Test]
        public void TestCurrentBusinessObject_IsSetToFirstObjectInCollection()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(myBoCol[0],boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_ChangesWhenTabIsChanged()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Execute Test ----------------------
            boColTabControl.TabControl.SelectedTab = boColTabControl.TabControl.TabPages[2];

            //---------------Test Result -----------------------
            Assert.AreEqual(myBoCol[2], boColTabControl.CurrentBusinessObject);
        }

        [Test]
        public void TestCurrentBusinessObject_SettingCurrentBusinessObjectChangesSelectedTab()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Assert Precondition----------------
            Assert.AreEqual(myBoCol[0], boColTabControl.CurrentBusinessObject);

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = myBoCol[2];

            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);
        }

        [Test]
        public void TestCurrentBusinessObject_SettingCurrentBusinessObjectToNullHasNoEffect()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;
            boColTabControl.CurrentBusinessObject = myBoCol[2];

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColTabControl.TabControl.SelectedIndex);
        }

        [Test]
        public void TestBusinessObjectControlHasNullBusinessObjectByDefault()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();

            //---------------Assert Precondition----------------
            Assert.IsNull(boColTabControl.BusinessObjectControl);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectControl = busControl;

            //---------------Test Result -----------------------
            Assert.IsNull(boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestBusinessObjectControlIsSetWhenCollectionIsSet()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            
            //---------------Assert Precondition----------------
            Assert.IsNull(boColTabControl.BusinessObjectControl.BusinessObject);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol[0], boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestBusinessObjectControlIsSetWhenCurrentBusinessObjectIsChanged()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectCollection<MyBO> myBoCol = SetupColTabControlWith3ItemCollection(boColTabControl);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = myBoCol[1];

            //---------------Test Result -----------------------
            Assert.AreSame(myBoCol[1], boColTabControl.BusinessObjectControl.BusinessObject);
        }

        [Test]
        public void TestInitialLayout()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);

            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColTabControl.TabControl.TabPages[0].Controls.Count);
            Assert.AreSame(busControl, boColTabControl.TabControl.TabPages[0].Controls[0]);
            Assert.AreEqual(DockStyle.Fill, busControl.Dock);
            Assert.AreEqual(firstBo.ToString(), boColTabControl.TabControl.TabPages[0].Text);
        }

        [Test]
        public void TestLayoutAfterChangingBusinessObject()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);
            MyBO secondBo = new MyBO();
            myBoCol.Add(secondBo);
            myBoCol.Add(new MyBO());
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = secondBo;

            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColTabControl.TabControl.TabPages[1].Controls.Count);
            Assert.AreSame(busControl, boColTabControl.TabControl.TabPages[1].Controls[0]);
        }

        private BusinessObjectCollection<MyBO> SetupColTabControlWith3ItemCollection(IBOColTabControl boColTabControl)
        {
            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            return myBoCol;
        }

        [Test]
        public void Test_SetUpBOTabColManagerWithDelegateForCreating_aBOControl()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            BusinessObjectControlCreatorDelegate creator = GetBusinessObjectControlStub;
            //---------------Assert Precondition----------------
            Assert.IsNull(boColTabControl.BusinessObjectControlCreator);
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectControlCreator = creator;
            //---------------Test Result -----------------------
            Assert.IsNotNull(boColTabControl.BusinessObjectControlCreator);
            Assert.AreEqual(creator, boColTabControl.BusinessObjectControlCreator);
        }

        [Test]
        public void Test_WhenSetBOCol_ShouldCreateTabPageWithControlFromCreator()
        {
            //---------------Set up test pack-------------------
            BusinessObjectControlCreatorDelegate creator;
            IBOColTabControl boColTabControl = CreateBOTabControlWithControlCreator(out creator);

            MyBO expectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { expectedBO };
            //---------------Assert Precondition----------------
            Assert.IsNotNull(boColTabControl.BusinessObjectControlCreator);
            Assert.AreEqual(creator, boColTabControl.BusinessObjectControlCreator);
            Assert.AreEqual(0, boColTabControl.TabControl.TabPages.Count);
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColTabControl.TabControl.TabPages.Count);
            ITabPage page = boColTabControl.TabControl.TabPages[0];
            Assert.AreEqual(1, page.Controls.Count);
            IControlHabanero boControl = page.Controls[0];
            Assert.IsInstanceOf(ExpectedTypeOfBOControl(), boControl);
            IBusinessObjectControl businessObjectControl = (IBusinessObjectControl)boControl;
            Assert.AreSame(expectedBO, businessObjectControl.BusinessObject);
        }


        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctAddedToCollection_ShouldAddTab()
        {
            BusinessObjectControlCreatorDelegate creator;
            IBOColTabControl boColTabControl = CreateBOTabControlWithControlCreator(out creator);

            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { new MyBO(), new MyBO(), new MyBO() };
            boColTabControl.BusinessObjectCollection = myBoCol;
            bool pageAddedEventFired = false;
            TabPageEventArgs ex = null;
            boColTabControl.TabPageAdded += (sender, e) =>
            {
                pageAddedEventFired = true;
                ex = e;
            };
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, boColTabControl.TabControl.TabPages.Count);
            Assert.IsFalse(pageAddedEventFired);
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection.Add(addedBo); 
            //---------------Test Result -----------------------
            Assert.AreEqual(4, boColTabControl.TabControl.TabPages.Count);
            ITabPage tabPage = boColTabControl.TabControl.TabPages[3];
            Assert.AreEqual(addedBo.ToString(), tabPage.Text);
            Assert.AreEqual(addedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1, tabPage.Controls.Count);
            IControlHabanero boControl = tabPage.Controls[0];
            Assert.IsTrue(pageAddedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControl, ex.BOControl);
        }

        [Ignore("This does not work")]
        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctAddedToCollection_ShouldAddTab_CorrectName()
        {
            BusinessObjectControlCreatorDelegate creator;
            IBOColTabControl boColTabControl = CreateBOTabControlWithControlCreator(out creator);

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { new MyBO(), new MyBO(), new MyBO() };
            boColTabControl.BusinessObjectCollection = myBoCol;
            bool pageAddedEventFired = false;
            TabPageEventArgs ex = null;
            boColTabControl.TabPageAdded += (sender, e) =>
            {
                pageAddedEventFired = true;
                ex = e;
            };
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, boColTabControl.TabControl.TabPages.Count);
            Assert.IsFalse(pageAddedEventFired);
            //---------------Execute Test ----------------------
            MyBO addedBo = new MyBO();
            boColTabControl.BusinessObjectCollection.Add(addedBo);
            addedBo.TestProp = TestUtil.GetRandomString(); 
            //---------------Test Result -----------------------
            Assert.AreEqual(4, boColTabControl.TabControl.TabPages.Count);
            ITabPage tabPage = boColTabControl.TabControl.TabPages[3];
            Assert.AreEqual(addedBo.ToString(), tabPage.Text);
            Assert.AreEqual(addedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1, tabPage.Controls.Count);
            IControlHabanero boControl = tabPage.Controls[0];
            Assert.IsTrue(pageAddedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControl, ex.BOControl);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctRemovedToCollection_ShouldRemoveTab()
        {
            BusinessObjectControlCreatorDelegate creator;
            IBOColTabControl boColTabControl = CreateBOTabControlWithControlCreator(out creator);

            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { removedBo, new MyBO(), new MyBO() };
            boColTabControl.BusinessObjectCollection = myBoCol;
            bool pageRemovedEventFired = false;
            TabPageEventArgs ex = null;
            boColTabControl.TabPageRemoved += (sender, e) =>
            {
                pageRemovedEventFired = true;
                ex = e;
            };
            ITabPage tabPage = boColTabControl.TabControl.TabPages[0];
            IControlHabanero boControlToBeRemoved = tabPage.Controls[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, boColTabControl.TabControl.TabPages.Count);
            Assert.IsFalse(pageRemovedEventFired);
            //---------------Execute Test ----------------------
            boColTabControl.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boColTabControl.TabControl.TabPages.Count);

            Assert.AreEqual(removedBo.ToString(), tabPage.Text);
            Assert.AreEqual(removedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1, tabPage.Controls.Count);

            Assert.IsTrue(pageRemovedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControlToBeRemoved, ex.BOControl);
        }

        private IBOColTabControl CreateBOTabControlWithControlCreator(out BusinessObjectControlCreatorDelegate creator)
        {
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();
            creator = GetBusinessObjectControlStub;
            boColTabControl.BusinessObjectControlCreator = creator;
            return boColTabControl;
        }

        [Test]
        public void Test_WhenChangeTabIndex_ShouldNotRecreateTheBOControl()
        {
            //---------------Set up test pack-------------------
            BusinessObjectControlCreatorDelegate creator;
            IBOColTabControl boColTabControl = CreateBOTabControlWithControlCreator(out creator);

            MyBO firstBO = new MyBO();
            MyBO secondBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { firstBO, secondBO };
            boColTabControl.BusinessObjectCollection = myBoCol;

            ITabPage secondTabPage = boColTabControl.TabControl.TabPages[1];
            IBusinessObjectControl secondBOControl = (IBusinessObjectControl)secondTabPage.Controls[0];
            //---------------Assert Precondition----------------
            Assert.AreSame(secondBO, secondBOControl.BusinessObject);
            Assert.AreEqual(2, boColTabControl.TabControl.TabPages.Count);
            Assert.AreEqual(0, boColTabControl.TabControl.SelectedIndex);
            //---------------Execute Test ----------------------
            boColTabControl.CurrentBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boColTabControl.TabControl.SelectedIndex);
            Assert.AreSame(secondBOControl, secondTabPage.Controls[0]);
        }


       

            }



 

}