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

using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
#pragma warning disable 618,612
    public abstract class TestReadOnlyGridButtonControl //: TestUsingDatabase
    {


        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }
        protected abstract void AddControlToForm(IControlHabanero cntrl);
        protected abstract IControlFactory GetControlFactory();
        //protected abstract IGridBase CreateReadOnlyGridControl();

 

        public void TestCreateReadOnlyGridButtonControl()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGridButtonsControl();
            //---------------Test Result ----------------------
            Assert.IsNotNull(grid);
            Assert.IsTrue(grid is IReadOnlyGridButtonsControl);
        }

        [Test]
        public void TestCreateReadOnlyGridButtonControl_TestButtonsAdded()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGridButtonsControl();
            //---------------Test Result ----------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = (IReadOnlyGridButtonsControl)grid;
            AddControlToForm(readOnlyGridButtonsControl);
            //Delete button should be first (This is right aligned so that means it will be furthest right if visible
            Assert.AreEqual(3, readOnlyGridButtonsControl.Controls.Count);
            IButton btn = (IButton) readOnlyGridButtonsControl.Controls[0];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Delete", btn.Name);

            //Edit button should be second (This is right aligned so that means it will be second from left
            btn = (IButton)readOnlyGridButtonsControl.Controls[1];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Edit", btn.Name);

            //Add button should be last (This is right aligned so that means it will be the first button from the left 
            //(last from the right)
            btn = (IButton)readOnlyGridButtonsControl.Controls[2];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Add", btn.Name);
        }

        [Test]
        public void TestCreateReadOnlyGridButtonControl_GetControlUsingIndexer()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGridButtonsControl();
            //---------------Test Result ----------------------

            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = (IReadOnlyGridButtonsControl)grid;
            Assert.AreEqual(3, readOnlyGridButtonsControl.Controls.Count);

            //Delete Not Visible and Text Correct
            IButton btn = readOnlyGridButtonsControl["Delete"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Delete", btn.Name);
            Assert.IsFalse(btn.Visible);

            //Edit Visible and Text Correct
            btn = readOnlyGridButtonsControl["Edit"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Edit", btn.Text);
            Assert.IsTrue(btn.Visible);

            //Add Visible and Text Correct
            btn = readOnlyGridButtonsControl["Add"];
            Assert.IsNotNull(btn);
            Assert.AreEqual("Add", btn.Text);
            Assert.IsTrue(btn.Visible);
        }

        [Test]
        public void Test_ShowDefaultDeleteButton_MakesTheDeleteButtonVisible()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = GetControlFactory().CreateReadOnlyGridButtonsControl();

            IButton btn = readOnlyGridButtonsControl["Delete"];
            //--------------verify Test pack -------------------
            Assert.IsFalse(btn.Visible);

            //---------------Execute Test ----------------------
            readOnlyGridButtonsControl.ShowDefaultDeleteButton = true;
            //---------------Verify Result ----------------------
            Assert.IsTrue(btn.Visible);
        }
        [Test]
        public void Test_NotShowDefaultDeleteButton_MakesTheDeleteButton_NotVisible()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = GetControlFactory().CreateReadOnlyGridButtonsControl();

            IButton btn = readOnlyGridButtonsControl["Delete"];
            readOnlyGridButtonsControl.ShowDefaultDeleteButton = true;
            //--------------verify PreConditions -------------------
            Assert.IsTrue(btn.Visible);

            //---------------Execute Test ----------------------
            readOnlyGridButtonsControl.ShowDefaultDeleteButton = false;
            //---------------Verify Result ----------------------
            Assert.IsFalse(btn.Visible);
        }
        [Test]
        public void TestDeleteButtonClick()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = GetControlFactory().CreateReadOnlyGridButtonsControl();
            AddControlToForm(readOnlyGridButtonsControl);  
            IButton btn = readOnlyGridButtonsControl["Delete"];
            btn.Visible = true;
            bool deleteClicked = false;

            readOnlyGridButtonsControl.DeleteClicked += delegate { deleteClicked = true; };
            //---------------Execute Test ----------------------
            btn.PerformClick();

            //---------------Test Result ----------------------
            Assert.IsTrue(deleteClicked);
        }
        [Test]
        public void TestAddButtonClick()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = GetControlFactory().CreateReadOnlyGridButtonsControl();
            AddControlToForm(readOnlyGridButtonsControl);
            IButton btn = readOnlyGridButtonsControl["Add"];
            bool addClicked = false;

            readOnlyGridButtonsControl.AddClicked += delegate { addClicked = true; };
            //---------------Execute Test ----------------------
            btn.PerformClick();

            //---------------Test Result ----------------------
            Assert.IsTrue(addClicked);
        }
        [Test]
        public void TestEditButtonClick()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridButtonsControl readOnlyGridButtonsControl = GetControlFactory().CreateReadOnlyGridButtonsControl();
            AddControlToForm(readOnlyGridButtonsControl);
            IButton btn = readOnlyGridButtonsControl["Edit"];
            bool editClicked = false;

            readOnlyGridButtonsControl.EditClicked += delegate { editClicked = true; };
            //---------------Execute Test ----------------------
            btn.PerformClick();

            //---------------Test Result ----------------------
            Assert.IsTrue(editClicked);
        }

        [Test]
        public void TestGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            IReadOnlyGridControl readOnlyGridControl = GetControlFactory().CreateReadOnlyGridControl();
            BusinessObjectCollection<MyBO> myBOS = new BusinessObjectCollection<MyBO>();
            myBOS.Add(new MyBO());
            MyBO bo = new MyBO();
            myBOS.Add(bo);
            myBOS.Add(new MyBO());

            readOnlyGridControl.SetBusinessObjectCollection(myBOS);
            bool gridItemSelected = false;
            readOnlyGridControl.Grid.SelectedBusinessObject = null;
            readOnlyGridControl.Grid.BusinessObjectSelected += (delegate { gridItemSelected = true; });
            
            //---------------Execute Test ----------------------
            readOnlyGridControl.Grid.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }
#pragma warning restore 618,612
    }

}