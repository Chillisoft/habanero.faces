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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Controllers;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.HabaneroControls
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestListViewCollectionSelectorWin
    {
        protected virtual IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        protected virtual ListView CreateListView()
        {
            return new ListView();
        }

        [SetUp]
        public void SetupTest()
        {
            //Code that is executed before each and every test in this class.
            // ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            var classDef = MyBO.LoadDefaultClassDefVWG();
            ClassDef.ClassDefs.Add(classDef);
        }

        [Test]
        public void TestCreateListViewCollectionController()
        {
            //---------------Set up test pack-------------------
//            IClassDef classDef = MyBO.LoadDefaultClassDef();
            ListView listView = CreateListView();
            //---------------Execute Test ----------------------
            ListViewCollectionManager cntrl = new ListViewCollectionManager(listView);
            //---------------Test Result -----------------------
            Assert.IsNotNull(cntrl.ListView);
/*            Assert.AreEqual(classDef, cntrl.ClassDef);
            Assert.AreEqual("default", cntrl.UiDefName);*/
            //---------------Tear down -------------------------
        }


        [Test]
        public void TestSetCollection_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            ListViewCollectionManager cntrl = CreateDefaultListVievController();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();

            //---------------Execute Test ----------------------
            cntrl.SetCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cntrl.ListView.Items.Count);
            //UIDef uiDef = GetDefaultUIDef(cntrl);
            //Assert.AreEqual(uiDef.GetUIGridProperties().Count, cntrl.ListView.Columns.Count);//There are 8 columns in the collection BO
            //Assert.IsNull(gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------          
        }

        private ListViewCollectionManager CreateDefaultListVievController()
        {
            ListView listView = CreateListView();
            ListViewCollectionManager cntrl = new ListViewCollectionManager(listView);
            return cntrl;
        }

        //        private UIDef GetDefaultUIDef(ListViewCollectionManager cntrl)
        //        {
        //            return cntrl.ClassDef.UIDefCol["default"];
        //        }
        //

        [Test]
        public void TestSetCollection()
        {
            //---------------Set up test pack-------------------
            ListView listView = CreateListView();
            ListViewCollectionManager controller = new ListViewCollectionManager(listView);
            BusinessObjectCollection<MyBO> col = GetColWith3Items();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, listView.Items.Count);
            //---------------Execute Test ----------------------
            controller.SetCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, listView.Items.Count);
        }

        [Test]
        public void TestAddBoToCollection()
        {
            ListView listView = CreateListView();
            ListViewCollectionManager controller = new ListViewCollectionManager(listView);
            BusinessObjectCollection<MyBO> col = GetColWith3Items();
            controller.SetCollection(col);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, listView.Items.Count);
            //---------------Execute Test ----------------------
            col.Add(new MyBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(4, listView.Items.Count);
        }

        [Test]
        public void TestRemoveBoFromCollection()
        {
            ListView listView = CreateListView();
            ListViewCollectionManager controller = new ListViewCollectionManager(listView);
            BusinessObjectCollection<MyBO> col = GetColWith3Items();
            controller.SetCollection(col);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, listView.Items.Count);
            //---------------Execute Test ----------------------
            col.RemoveAt(0);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, listView.Items.Count);
        }

        [Ignore("REASON")] //this test works in debug mode, but not in nunit
        [Test]
        public void TestGetBusinessObject()
        {
            ListView listView = CreateListView();
            ListViewCollectionManager controller = new ListViewCollectionManager(listView);
            BusinessObjectCollection<MyBO> col = GetColWith3Items();
            controller.SetCollection(col);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, listView.Items.Count);
            //---------------Execute Test ----------------------
            listView.Items[2].Selected = true;
            listView.Items[0].Focused = true;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, listView.SelectedItems.Count);
            Assert.IsNotNull(controller.SelectedBusinessObject);
            Assert.AreSame(col[2], controller.SelectedBusinessObject);
        }

        private static BusinessObjectCollection<MyBO> GetColWith3Items()
        {
            return new BusinessObjectCollection<MyBO>
                       {
                           new MyBO(),
                           new MyBO(),
                           new MyBO()
                       };
        }
    }

}
// ReSharper restore InconsistentNaming