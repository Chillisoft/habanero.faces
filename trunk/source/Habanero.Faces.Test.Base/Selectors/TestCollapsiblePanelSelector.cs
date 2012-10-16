using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
   

    public abstract class TestCollapsiblePanelSelector : TestBOColSelector
    {

        protected override IBOColSelectorControl CreateSelector()
        {
            return GetControlFactory().CreateCollapsiblePanelSelector();
        }

        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            ICollapsiblePanelGroupControl groupControl = ((ICollapsiblePanelGroupControl) colSelector);
            groupControl.AllCollapsed = true;
            ((ICollapsiblePanelGroupControl) colSelector).PanelsList[index].Collapsed = false;
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            ICollapsiblePanelGroupControl groupControl = ((ICollapsiblePanelGroupControl) colSelector);
            int count = 0;
            foreach (ICollapsiblePanel panel in groupControl.PanelsList)
            {
                if (!panel.Collapsed)
                {
                    return count;
                }
                count++;
            }
            return -1;
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        protected override int NumberOfTrailingBlankRows()
        {
            return 0;
        }

        [Test]
        public void Test_Constructor_CollapsiblePanelSet()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOColSelectorControl colSelector = CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(colSelector);
            Assert.IsInstanceOf(typeof (IBOCollapsiblePanelSelector), colSelector);
            Assert.IsInstanceOf(typeof (ICollapsiblePanelGroupControl), colSelector);
        }

        [Ignore(" Not Yet implemented : Brett  01 Mar 2009:")]
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
            Assert.Fail("Not hyet implemented");
        }

        [Test]
        public override void Test_RemoveBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO newMyBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, newMyBO};
            colSelector.BusinessObjectCollection = collection;
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(myBO, colSelector.GetBusinessObjectAtRow(ActualIndex(0)));
            Assert.AreSame(newMyBO, colSelector.GetBusinessObjectAtRow(ActualIndex(1)));
            //---------------Execute Test ----------------------
            collection.Remove(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualNumberOfRows(1), colSelector.NoOfItems, "The blank item and one other");
            Assert.AreSame(newMyBO, colSelector.GetBusinessObjectAtRow(ActualIndex(0)));
        }

        [Ignore(" Not Implemented : Brett 02 Mar 2009")]
        [Test]
        public override void Test_SelectedBusinessObject_SecondItemSelected_ReturnsItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(1));
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = colSelector.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO2, selectedBusinessObject);
        }

        [Test]
        public override void Test_Set_SelectedBusinessObject_SetsItem()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            colSelector.BusinessObjectCollection = collection;
            SetSelectedIndex(colSelector, ActualIndex(0));
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
//            Assert.AreEqual(ActualIndex(1), SelectedIndex(selector));
            Assert.AreSame(myBO, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = myBO2;
            //---------------Test Result -----------------------
            Assert.AreSame(myBO2, colSelector.SelectedBusinessObject);
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
        }

        [Test]
        public override void Test_Set_SelectedBusinessObject_Null_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            colSelector.BusinessObjectCollection = collection;
//            SetSelectedIndex(selector, ActualIndex(1));
            colSelector.SelectedBusinessObject = myBO2;
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual(1, SelectedIndex(colSelector), "This does not make sense with a collapsible panel similar to a boTabcontrol");
        }

        [Test]
        public override void Test_Set_SelectedBusinessObject_ItemNotInList_SetsItemNull()
        {
            //---------------Set up test pack-------------------
            IBOColSelectorControl colSelector = CreateSelector();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> {myBO, myBO2};
            colSelector.BusinessObjectCollection = collection;
//            SetSelectedIndex(selector, ActualIndex(1));
            colSelector.SelectedBusinessObject = myBO2;
            //---------------Assert Precondition----------------
            Assert.AreEqual
                (collection.Count + NumberOfLeadingBlankRows(), colSelector.NoOfItems, "The blank item and others");
            Assert.AreEqual(ActualIndex(1), SelectedIndex(colSelector));
            Assert.AreEqual(myBO2, colSelector.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            colSelector.SelectedBusinessObject = new MyBO();
            //---------------Test Result -----------------------
            Assert.AreEqual(ActualIndex(2), colSelector.NoOfItems, "The blank item");
            Assert.IsNull(colSelector.SelectedBusinessObject);
            Assert.AreEqual
                (1, SelectedIndex(colSelector),
                 "This does not make sense with a collapsible panel similar to a boTabcontrol");
        }
    }

}