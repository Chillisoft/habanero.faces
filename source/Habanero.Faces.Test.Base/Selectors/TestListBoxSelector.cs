using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Base;


using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestListBoxSelector : TestBOColSelector
    {
        protected override void SetSelectedIndex(IBOColSelectorControl colSelector, int index)
        {
            ((IBOListBoxSelector)colSelector).ListBox.SelectedIndex = index;
        }

        protected override int SelectedIndex(IBOColSelectorControl colSelector)
        {
            return ((IBOListBoxSelector)colSelector).ListBox.SelectedIndex;
        }

        protected override int NumberOfLeadingBlankRows()
        {
            return 0;
        }

        protected override int NumberOfTrailingBlankRows()
        {
            return 0;
        }

        protected override IBOColSelectorControl CreateSelector()
        {
            return GetControlledLifetimeFor(GetControlFactory().CreateListBoxSelector());
        }

        [Test]
        public void Test_Constructor_ListBoxSet()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBOListBoxSelector selector = (IBOListBoxSelector) CreateSelector();
            //---------------Test Result -----------------------
            Assert.IsNotNull(selector.ListBox);
            Assert.IsInstanceOf(typeof(IListBox), selector.ListBox);
        }

        [Ignore(" Not Yet implemented")] //TODO  01 Mar 2009:
        [Test]
        public void TestEditItemFromCollectionUpdatesItemInSelector()
        {
        }
    }

}