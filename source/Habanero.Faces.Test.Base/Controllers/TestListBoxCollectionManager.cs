using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using Habanero.Test.Structure;
using NUnit.Framework;

#pragma warning disable 612,618
//-- obsolete methods should still be tested untill they are removed.
// ReSharper disable InconsistentNaming

namespace Habanero.Faces.Test.Base.Controllers
{
    public abstract class TestListBoxCollectionManager:TestBaseWithDisposing
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        protected abstract IControlFactory GetControlFactory();
        protected IListBox CreateListBox()
        {
            return GetControlledLifetimeFor(GetControlFactory().CreateListBox());
        }

        [Test]
        public void TestCreateTestListBoxCollectionController()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>();
            MyBO myBO1 = new MyBO();
            MyBO myBO2 = new MyBO();
            myBOs.Add(myBO1, myBO2);
            IListBox cmb = CreateListBox();
            var selector = CreateListBoxCollectionManager(cmb);
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------

            selector.SetCollection(myBOs);
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, selector.Collection);
            Assert.AreSame(cmb, selector.Control);
            //---------------Tear Down -------------------------   
        }


        [Test]
        public void TestSetCollectionNull()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();

            IListBox cmb = CreateListBox();
            var selector = CreateListBoxCollectionManager(cmb);
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selector.SetCollection(null);
            //---------------Verify Result -----------------------
            Assert.IsNull(selector.Collection);
            Assert.AreSame(cmb, selector.Control);
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IListBox listBox = CreateListBox();
            //---------------Execute Test ----------------------
            var manager = CreateListBoxCollectionManager(listBox);
            //---------------Test Result -----------------------
            Assert.IsNotNull(manager);
            Assert.AreSame(listBox, manager.Control);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestConstructor_NullListBoxRaisesError()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            try
            {
                new ListBoxCollectionManager(null);
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
        public void TestSetListBoxCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IListBox listBox = CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            //---------------Execute Test ----------------------
            manager.SetCollection(myBoCol);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, manager.Collection.Count);
            //---------------Tear down -------------------------
        }

        protected static ListBoxCollectionManager CreateListBoxCollectionManager(IListBox listBox)
        {
            return new ListBoxCollectionManager(listBox);
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IListBox listBox = CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO selectedBO = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(selectedBO);
            myBoCol.Add(new MyBO());
            //---------------Execute Test ----------------------
            manager.SetCollection(myBoCol);
            manager.Control.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, manager.SelectedBusinessObject);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBusinessObejctAddedToCollection_ShouldAddToItems()
        {
            ClassDef.ClassDefs.Clear();
            IListBox listBox = CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO addedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            myBoCol.Add(new MyBO());
            manager.SetCollection(myBoCol);
            //---------------Execute Test ----------------------
            manager.Collection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, manager.Control.Items.Count);
        }

        [Test]
        public void TestBusinessObjectRemovedFromCollection_ShouldRemoveFromItems()
        {
            ClassDef.ClassDefs.Clear();
            var listBox = CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            MyBO.LoadDefaultClassDef();
            var myBoCol = new BusinessObjectCollection<MyBO>();
            var removedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(removedBo);
            myBoCol.Add(new MyBO());
            manager.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            manager.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, manager.Control.Items.Count);
        }

        [Test]
        public void Test_RemoveFromBOCol_WhenIsSelected_ShouldFireBusinessObjectSelectedEventWithNullBo()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            var listBox = CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            MyBO.LoadDefaultClassDef();
            var myBoCol = new BusinessObjectCollection<MyBO>();
            var toBeRemovedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(toBeRemovedBo);
            myBoCol.Add(new MyBO());
            manager.BusinessObjectCollection = myBoCol;
            manager.SelectedBusinessObject = toBeRemovedBo;

            IBusinessObject boFromSelectedEvent = null;
            bool boSelectedEventFired = false;
            manager.BusinessObjectSelected += (sender, args) => 
                {
                    boFromSelectedEvent = args.BusinessObject;
                    boSelectedEventFired = true;
                };
            //---------------Assert Precondition----------------
            Assert.AreSame(toBeRemovedBo, manager.SelectedBusinessObject);
            Assert.IsNull(boFromSelectedEvent);
            Assert.IsFalse(boSelectedEventFired);
            //---------------Execute Test ----------------------
            manager.BusinessObjectCollection.Remove(toBeRemovedBo);
            //---------------Test Result -----------------------
            Assert.IsTrue(boSelectedEventFired, "Selected event should be fired");
            Assert.IsNull(boFromSelectedEvent);
        }

        [Test]
        public void Test_RemoveFromBOCol_WhenNotIsSelected_ShouldNotFireBusinessObjectSelectedEvent()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            var listBox = CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            MyBO.LoadDefaultClassDef();
            var myBoCol = new BusinessObjectCollection<MyBO>();
            var toBeRemovedBo = new MyBO();
            myBoCol.Add(new MyBO());
            myBoCol.Add(toBeRemovedBo);
            myBoCol.Add(new MyBO());
            manager.BusinessObjectCollection = myBoCol;
            IBusinessObject boFromSelectedEvent = null;
            bool boSelectedEventFired = false;
            manager.BusinessObjectSelected += (sender, args) =>
            {
                boFromSelectedEvent = args.BusinessObject;
                boSelectedEventFired = true;
            };
            //---------------Assert Precondition----------------
            Assert.AreNotSame(toBeRemovedBo, manager.SelectedBusinessObject);
            Assert.IsNull(boFromSelectedEvent);
            Assert.IsFalse(boSelectedEventFired);
            //---------------Execute Test ----------------------
            manager.BusinessObjectCollection.Remove(toBeRemovedBo);
            //---------------Test Result -----------------------
            Assert.IsFalse(boSelectedEventFired);
        }
    }
}

#pragma warning restore 612,618
// ReSharper restore InconsistentNaming