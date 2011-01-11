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

using Habanero.Test;

namespace Habanero.Faces.Test.Base
{
    public class TestListViewCollectionManager
    {
        //TODO: Port
    }
//        //IListView _listView;
//        //ListViewCollectionManager controller;
//        //private BusinessObjectCollection<BusinessObject> _collection;
//
//        protected abstract IControlFactory GetControlFactory();
//
//        //[TestFixture]
//        //public class TestListViewCollectionMapperWin : TestListViewCollectionManager
//        //{
//        //    protected override IControlFactory GetControlFactory()
//        //    {
//        //        return new Habanero.Faces.Win.ControlFactoryWin();
//        //    }
//
//           
//        //}
//
//        [TestFixture]
    //        public class TestListViewCollectionMapperVWG : TestListViewCollectionManager
//        {
//            protected override IControlFactory GetControlFactory()
//            {
//                return new Habanero.Faces.WebGUI.ControlFactoryVWG();
//            }
//
//        }
//        [SetUp]
//        public void SetupTest()
//        {
//            ClassDef.ClassDefs.Clear();
//        }
//
//        [TestFixtureSetUp]
//        public void TestFixtureSetup()
//        {
//            //Code that is executed before any test is run in this class. If multiple tests
//            // are executed then it will still only be called once.
//            base.SetupDBConnection();
//        }
//
//        [Test]
//        public void TestCreateListViewCollectionController()
//        {
//            //---------------Set up test pack-------------------
//            ClassDef classDef = MyBO.LoadDefaultClassDef();
//            IListView listView = GetControlFactory().CreateListView();
//            //---------------Execute Test ----------------------
//            ListViewCollectionManager cntrl = new ListViewCollectionManager(listView, classDef);
//            //---------------Test Result -----------------------
//            Assert.IsNotNull(cntrl.ListView);
//            Assert.AreEqual(classDef, cntrl.ClassDef);
//            Assert.AreEqual("default", cntrl.UiDefName);
//            //---------------Tear down -------------------------
//        }
//
//        [Test, Ignore("still to be done")]
//        public void TestSetCollection_EmptyCollection()
//        {
//            //---------------Set up test pack-------------------
//            ListViewCollectionManager cntrl = CreateDefaultListVievController();
//            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
//
//            //---------------Execute Test ----------------------
//            cntrl.SetBusinessObjectCollection(col);
//            //---------------Test Result -----------------------
//            Assert.AreEqual(0, cntrl.ListView.Items.Count);
//            UIDef uiDef = GetDefaultUIDef(cntrl);
//            Assert.AreEqual(uiDef.GetUIGridProperties().Count, cntrl.ListView.Columns.Count);//There are 8 columns in the collection BO
//            //Assert.IsNull(gridBase.SelectedBusinessObject);
//            //---------------Tear Down -------------------------          
//        }
//
//        private UIDef GetDefaultUIDef(ListViewCollectionManager cntrl)
//        {
//            return cntrl.ClassDef.UIDefCol["default"];
//        }
//
//        private ListViewCollectionManager CreateDefaultListVievController()
//        {
//            ClassDef classDef = MyBO.LoadDefaultClassDef();
//            IListView listView = GetControlFactory().CreateListView();
//            ListViewCollectionManager cntrl = new ListViewCollectionManager(listView, classDef);
//            return cntrl;
//        }
//
////        [Test]
////        public void TestSetCollection()
////        {
////            //---------------Set up test pack-------------------
////            IListView listView = GetControlFactory().CreateListView();
    ////            ClassDef def = MyBO.LoadDefaultClassDefVWG();
////            ListViewCollectionManager controller = new ListViewCollectionManager(listView, def);
////            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
////            col.Add(new MyBO());
////            col.Add(new MyBO());
////            col.Add(new MyBO());
////            listView.SetCollection(col);
////            controller.SetBusinessObjectCollection(col);
////            //---------------Execute Test ----------------------
////
////            listView.Items[0].Selected = true;
////            listView.Items[1].Selected = true;
////
////            //---------------Test Result -----------------------
////            Assert.AreEqual(3, listView.Items.Count);
////            //---------------Tear Down -------------------------   
////
////        }
//
//        //[Test]
//        //public void TestSetCollection()
//        //{
//        //    Assert.AreEqual(3, itsListView.Items.Count);
//        //}
//
//        
//        ////TODO: this test works in debug mode, but not in nunit.
//        ////[Test]
//        ////public void TestGetBusinessObject()
//        ////{
//        ////    itsListView.Focus();
//        ////    itsListView.Items[2].Selected = true;
//        ////    itsListView.Items[0].Focused = true;
//        ////    Assert.IsNotNull(mapper.SelectedBusinessObject);
//        ////    Assert.AreSame(_collection[2], mapper.SelectedBusinessObject);
//        ////}
//
//        //[Test]
//        //public void TestAddBoToCollection()
//        //{
//        //    _collection.Add(new Sample());
//        //    Assert.AreEqual(4, itsListView .Items.Count);
//        //}
//
//        //[Test]
//        //public void TestRemoveBoFromCollection()
//        //{
//        //    itsCollection.RemoveAt(0);
//        //    Assert.AreEqual(2, itsListView.Items.Count);
//        //}        
//    }
}
