using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Test.Base.Controllers;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win.HabaneroControls
{
    [TestFixture]
    public class TestListBoxCollectionManagerWin : TestListBoxCollectionManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        //this cannot be tested for VWG.
        [Test]
        public void Test_BusinessObjectEdited_ShouldRefreshTheValueInTheList()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            var listBox = GetControlFactory().CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            var boToBeUpdated = new MyBO();
            var myBoCol = new BusinessObjectCollection<MyBO> {new MyBO(), boToBeUpdated };
            manager.BusinessObjectCollection = myBoCol;

            manager.Control.SelectedItem = boToBeUpdated;
            var initialListBoxDisplayText = manager.Control.Text;
            var initialBOToString = boToBeUpdated.ToString();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, manager.Control.Items.Count);

            Assert.AreSame(boToBeUpdated, manager.Control.Items[1]);
            Assert.AreEqual(initialBOToString, initialListBoxDisplayText);
            //---------------Execute Test ----------------------
            boToBeUpdated.TestProp = GetRandomString();
            boToBeUpdated.Save();
            //---------------Test Result -----------------------
            var updatedListBoxDisplayText = manager.Control.Text;
            var updatedBOToString = boToBeUpdated.ToString();
            Assert.AreNotEqual(initialListBoxDisplayText, updatedListBoxDisplayText);
            Assert.AreNotEqual(initialBOToString, updatedBOToString);

            Assert.AreEqual(updatedBOToString, updatedListBoxDisplayText);
        }
        //this cannot be tested for VWG.
        [Test]
        public void Test_BusinessObjectEdited_WhenSelected_ShouldRemainSelected()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            var listBox = GetControlFactory().CreateListBox();
            var manager = CreateListBoxCollectionManager(listBox);
            var boToBeUpdated = new MyBO();
            var myBoCol = new BusinessObjectCollection<MyBO> {new MyBO(), boToBeUpdated };
            manager.BusinessObjectCollection = myBoCol;

            manager.Control.SelectedItem = boToBeUpdated;
            //---------------Assert Precondition----------------
            Assert.AreSame(boToBeUpdated, manager.Control.SelectedItem);
            //---------------Execute Test ----------------------
            boToBeUpdated.TestProp = GetRandomString();
            boToBeUpdated.Save();
            //---------------Test Result -----------------------
            Assert.AreSame(boToBeUpdated, manager.Control.SelectedItem);

        }


        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
        }
    }
}
// ReSharper restore InconsistentNaming