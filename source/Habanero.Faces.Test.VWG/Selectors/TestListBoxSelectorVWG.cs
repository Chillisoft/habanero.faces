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
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Selectors
{
    [TestFixture]
    public class TestListBoxSelectorVWG : TestListBoxSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_Constructor_nullControlFactory_RaisesError()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new ListBoxSelectorVWG(null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Ignore(" Not working in VWG")]
        [Test]
        public override void Test_SelectedBusinessObject_ReturnsNullIfNoItemSelected()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            colSelector.BusinessObjectCollection = collection;
            colSelector.SelectedBusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNull(selectedBusinessObject);
        }

        [Ignore(" Not working in VWG")]
        [Test]
        public override void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }

        [Ignore(" Not working in VWG")]
        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO, myBO2 };
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual(collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(-1, SelectedIndex(colSelector));
        }


        [Ignore(" Not working in VWG")]
        [Test]
        public override void Test_SelectorFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectCollection col;
            IBOColSelectorControl boColSelector = GetSelectorWith_4_Rows(out col);
            bool itemSelected = false;
            boColSelector.SelectedBusinessObject = null;
            boColSelector.BusinessObjectSelected += (delegate { itemSelected = true; });
            //---------------Execute Test ----------------------
            boColSelector.SelectedBusinessObject = col[1];
            //---------------Test Result -----------------------
            Assert.IsTrue(itemSelected);
        }

      
    }
}