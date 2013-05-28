using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.Controllers
{
    public abstract class TestBOColTabControlManager:TestBaseWithDisposing
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract IBusinessObjectControl GetBusinessObjectControl();
		protected abstract IBusinessObjectControl GetBusinessObjectControlSpy(Action<IBusinessObject> onBusinessObjectSet);

        protected IBusinessObjectControl BusinessObjectControlCreator()
        {
            return GetBusinessObjectControl();
        }

        protected ITabControl CreateTabControl()
        {
            return GetControlledLifetimeFor(GetControlFactory().CreateTabControl());
        }
        [Test]
        public void Test_Create_tabControlNull_ExpectException()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOColTabControlManager(null, GetControlFactory());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("tabControl", ex.ParamName);
            }
        }

        [Test]
        public void Test_Create_controlFactoryNull_ExpectException()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new BOColTabControlManager(tabControl, null);
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
        public void TestCreateTestTabControlCollectionController()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO> {{new MyBO(), new MyBO()}};
            ITabControl tabControl = CreateTabControl();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, GetControlFactory());
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Verify test pack-------------------
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBOs;
            //---------------Verify Result -----------------------
            Assert.AreEqual(myBOs, selectorManager.BusinessObjectCollection);
            Assert.AreSame(tabControl, selectorManager.TabControl);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            //---------------Execute Test ----------------------
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectorManager);
            Assert.AreSame(tabControl, selectorManager.TabControl);
//            Assert.AreSame(controlFactory, selectorManager.ControlFactory);

        }

		[Test]
		public void Test_Set_Collection_WithNull()
		{
			//---------------Set up test pack-------------------

			ITabControl tabControl = CreateTabControl();
			BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, GetControlFactory());
			selectorManager.BusinessObjectControl = GetBusinessObjectControl();
			//---------------Verify test pack-------------------
			//---------------Execute Test ----------------------
			selectorManager.BusinessObjectCollection = null;
			//---------------Verify Result -----------------------
			Assert.IsNull(selectorManager.BusinessObjectCollection);
			Assert.AreSame(tabControl, selectorManager.TabControl);
		}

    	[Test]
    	public void Test_Set_Collection_WhenBOControlNotSet_ShouldRaiseError()
    	{
    		//---------------Set up test pack-------------------
    		ITabControl tabControl = CreateTabControl();
    		IControlFactory controlFactory = GetControlFactory();
    		BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
    		BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { new MyBO(), new MyBO(), new MyBO() };
    		//---------------Assert Precondition----------------
    		Assert.IsNull(selectorManager.BusinessObjectCollection);
    		Assert.IsNull(selectorManager.BusinessObjectControl);
    		//---------------Execute Test ----------------------
    		try
    		{
    			selectorManager.BusinessObjectCollection = myBoCol;
    			Assert.Fail("expected Err");
    		}
    			//---------------Test Result -----------------------
    		catch (HabaneroDeveloperException ex)
    		{
    			const string expectedMessage = "You cannot set the 'BusinessObjectCollection' for BOColTabControlManager since the BusinessObjectControl has not been set";
    			StringAssert.Contains(expectedMessage, ex.Message);
    		}
    	}

    	[Test]
		public void Test_Set_Collection_ShouldPopulateTabs()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectorManager.BusinessObjectCollection.Count);
            Assert.AreEqual(3, selectorManager.TabControl.TabPages.Count);
        }

        [Test]
		public void Test_Set_Collection_WithAutoSelectFirstItemAsTrue_ShouldSelectFirstItem()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
			BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
			MyBO firstBo;
			BusinessObjectCollection<MyBO> myBoCol = GetMyBoCol_3Items(out firstBo);
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
			selectorManager.AutoSelectFirstItem = true;
			//---------------Assert Precondition----------------
			Assert.IsTrue(selectorManager.AutoSelectFirstItem);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreSame(firstBo, selectorManager.CurrentBusinessObject);
        }

        [Test]
		public void Test_Set_Collection_WhenUsingABusinessObjectControl_ShouldSetToCurrentBO()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
			BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
			MyBO firstBo;
			BusinessObjectCollection<MyBO> myBoCol = GetMyBoCol_3Items(out firstBo);
        	var businessObjectControl = GetBusinessObjectControl();
        	selectorManager.BusinessObjectControl = businessObjectControl;
			selectorManager.AutoSelectFirstItem = true;
			//---------------Assert Precondition----------------
			Assert.IsTrue(selectorManager.AutoSelectFirstItem);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
			Assert.AreSame(selectorManager.CurrentBusinessObject, businessObjectControl.BusinessObject);
        }

        [Test]
		public void Test_Set_Collection_WhenUsingABusinessObjectControl_ShouldOnlySetBOOnce()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
			BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
			MyBO firstBo;
			BusinessObjectCollection<MyBO> myBoCol = GetMyBoCol_3Items(out firstBo);
        	var list = new List<IBusinessObject>();
        	var businessObjectControl = GetBusinessObjectControlSpy(list.Add);
        	selectorManager.BusinessObjectControl = businessObjectControl;
			selectorManager.AutoSelectFirstItem = true;
			//---------------Assert Precondition----------------
			Assert.IsTrue(selectorManager.AutoSelectFirstItem);
			Assert.AreEqual(0, list.Count);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(firstBo, list[0]);
        }

    	private static BusinessObjectCollection<MyBO> GetMyBoCol_3Items(out MyBO firstBo)
    	{
    		firstBo = new MyBO();
    		return new BusinessObjectCollection<MyBO>
    			{firstBo, new MyBO(), new MyBO()};
    	}

    	[Test]
		public void Test_Set_CurrentBusinessObject_WhenUsingABusinessObjectControl_ShouldOnlySetBOOnce()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
			BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
			MyBO firstBo;
			BusinessObjectCollection<MyBO> myBoCol = GetMyBoCol_3Items(out firstBo);
        	var list = new List<IBusinessObject>();
        	var businessObjectControl = GetBusinessObjectControlSpy(list.Add);
        	selectorManager.BusinessObjectControl = businessObjectControl;
            selectorManager.BusinessObjectCollection = myBoCol;
			list.Clear();
			//---------------Assert Precondition----------------
			Assert.IsTrue(selectorManager.AutoSelectFirstItem);
			Assert.AreEqual(0, list.Count);
            //---------------Execute Test ----------------------
        	var newSelectedBo = myBoCol[1];
        	selectorManager.CurrentBusinessObject = newSelectedBo;
            //---------------Test Result -----------------------
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(newSelectedBo, list[0]);
        }

    	[Test]
        public void TestSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            MyBO selectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), selectedBO, new MyBO()};
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            selectorManager.TabControl.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, selectorManager.CurrentBusinessObject);
        }

        [Test]
        public void Test_Set_SelectedBusinessObject_BOColNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            MyBO selectedBO = new MyBO();
            selectorManager.BusinessObjectControl = this.GetBusinessObjectControl();
            selectorManager.BusinessObjectCollection = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(selectorManager.BusinessObjectCollection);
            Assert.IsNotNull(selectorManager.BusinessObjectControl);
            //---------------Execute Test ----------------------
            try
            {
                selectorManager.CurrentBusinessObject = selectedBO;
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                const string expectedMessage = "You cannot set the 'CurrentBusinessObject' for BOColTabControlManager since the BusinessObjectCollection has not been set";
                StringAssert.Contains(expectedMessage, ex.Message);
            }
        }

        [Test]
        public void Test_Set_SelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            MyBO selectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), selectedBO, new MyBO()};
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            selectorManager.CurrentBusinessObject = selectedBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(selectedBO, selectorManager.CurrentBusinessObject);
        }

        [Test]
        public void Test_BusinessObjectAddedToCollection()
        {
            ITabControl tabControl = CreateTabControl();
            BOColTabControlManager selectorManager = CreateBOTabControlManager(tabControl);
            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), new MyBO(), new MyBO()};
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectorManager.TabControl.TabPages.Count);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManager.TabControl.TabPages.Count);
            Assert.AreEqual(addedBo.ToString(), selectorManager.TabControl.TabPages[3].Text);
            Assert.AreEqual(addedBo.ToString(), selectorManager.TabControl.TabPages[3].Name);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctAddedToCollection_ShouldAddTab()
        {
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectControlCreatorDelegate creator = BusinessObjectControlCreator;
            selectorManager.BusinessObjectControlCreator = creator;

            MyBO addedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { new MyBO(), new MyBO(), new MyBO() };
            selectorManager.BusinessObjectCollection = myBoCol;
            bool pageAddedEventFired = false;
            TabPageEventArgs ex = null;
            selectorManager.TabPageAdded += (sender,e) =>
                                               {
                                                   pageAddedEventFired = true;
                                                   ex = e;
                                               };
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectorManager.TabControl.TabPages.Count);
            Assert.IsFalse(pageAddedEventFired);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection.Add(addedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, selectorManager.TabControl.TabPages.Count);
            ITabPage tabPage = selectorManager.TabControl.TabPages[3];
            Assert.AreEqual(addedBo.ToString(), tabPage.Text);
            Assert.AreEqual(addedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1,tabPage.Controls.Count);
            IControlHabanero boControl = tabPage.Controls[0];
            Assert.IsTrue(pageAddedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControl, ex.BOControl);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenBusinessObejctRemovedToCollection_ShouldRemoveTab()
        {
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectControlCreatorDelegate creator = BusinessObjectControlCreator;
            selectorManager.BusinessObjectControlCreator = creator;

            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { removedBo, new MyBO(), new MyBO() };
            selectorManager.BusinessObjectCollection = myBoCol;
            bool pageRemovedEventFired = false;
            TabPageEventArgs ex = null;
            selectorManager.TabPageRemoved += (sender, e) =>
                                                 {
                                                     pageRemovedEventFired = true;
                                                     ex = e;
                                                 };
            ITabPage tabPage = selectorManager.TabControl.TabPages[0];
            IControlHabanero boControlToBeRemoved = tabPage.Controls[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, selectorManager.TabControl.TabPages.Count);
            Assert.IsFalse(pageRemovedEventFired);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectorManager.TabControl.TabPages.Count);
            
            Assert.AreEqual(removedBo.ToString(), tabPage.Text);
            Assert.AreEqual(removedBo.ToString(), tabPage.Name);
            Assert.AreEqual(1, tabPage.Controls.Count);
            
            Assert.IsTrue(pageRemovedEventFired);
            Assert.IsNotNull(ex);
            Assert.AreSame(tabPage, ex.TabPage);
            Assert.AreSame(boControlToBeRemoved, ex.BOControl);
        }

        [Test]
        public void Test_WhenUsingCreator_SetUpBOTabColManagerWithDelegateForCreating_aBOControl()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            BusinessObjectControlCreatorDelegate creator = BusinessObjectControlCreator;
            //---------------Assert Precondition----------------
            Assert.IsNull(selectorManager.BusinessObjectControlCreator);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectControlCreator = creator;
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectorManager.BusinessObjectControlCreator);
            Assert.AreEqual(creator, selectorManager.BusinessObjectControlCreator);
        }

        [Test]
        public void Test_WhenUsingCreator_WhenSetBOCol_ShouldCreateTabPageWithControlFromCreator()
        {
            //---------------Set up test pack-------------------
            BusinessObjectControlCreatorDelegate creator;
            BOColTabControlManager selectorManager = GetselectorManager(out creator);
            MyBO expectedBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {expectedBO};
            //---------------Assert Precondition----------------
            Assert.IsNotNull(selectorManager.BusinessObjectControlCreator);
            Assert.AreEqual(creator, selectorManager.BusinessObjectControlCreator);
            Assert.AreEqual(0, selectorManager.TabControl.TabPages.Count);
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectorManager.TabControl.TabPages.Count);
            ITabPage page = selectorManager.TabControl.TabPages[0];
            Assert.AreEqual(1, page.Controls.Count);
            IControlHabanero boControl = page.Controls[0];
            Assert.IsInstanceOf(TypeOfBusinessObjectControl(), boControl);
            IBusinessObjectControl businessObjectControl = (IBusinessObjectControl) boControl;
            Assert.AreSame(expectedBO, businessObjectControl.BusinessObject);
            Assert.AreSame(boControl, selectorManager.BusinessObjectControl);
        }

        protected abstract Type TypeOfBusinessObjectControl();

        [Test]
        public void Test_WhenChangeTabIndex_ShouldNotRecreateTheBOControl()
        {
            //---------------Set up test pack-------------------
            BusinessObjectControlCreatorDelegate creator;
            BOColTabControlManager selectorManager = GetselectorManager(out creator);

            MyBO firstBO = new MyBO();
            MyBO secondBO = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO> { firstBO, secondBO };
            selectorManager.BusinessObjectCollection = myBoCol;
            ITabPage firstTabPage = selectorManager.TabControl.TabPages[0];
            IBusinessObjectControl firstBOControl = (IBusinessObjectControl)firstTabPage.Controls[0];
            ITabPage secondTabPage = selectorManager.TabControl.TabPages[1];
            IBusinessObjectControl secondBOControl = (IBusinessObjectControl)secondTabPage.Controls[0];
            //---------------Assert Precondition----------------
            Assert.AreSame(secondBO, secondBOControl.BusinessObject);
            Assert.AreSame(firstBOControl, selectorManager.BusinessObjectControl);
            Assert.AreEqual(2, selectorManager.TabControl.TabPages.Count);
            Assert.AreEqual(0, selectorManager.TabControl.SelectedIndex);
            //---------------Execute Test ----------------------
            selectorManager.CurrentBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, selectorManager.TabControl.SelectedIndex);
            Assert.AreSame(secondBOControl, secondTabPage.Controls[0]);
            Assert.AreSame(secondBOControl, selectorManager.BusinessObjectControl);
        }


        [Test]
        public void TestBusinessObejctRemovedFromCollection()
        {
            ITabControl tabControl = CreateTabControl();
            BOColTabControlManager selectorManager = CreateBOTabControlManager(tabControl);
            MyBO removedBo = new MyBO();
            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>
                                                         {new MyBO(), removedBo, new MyBO()};
            selectorManager.BusinessObjectCollection = myBoCol;
            //---------------Execute Test ----------------------
            selectorManager.BusinessObjectCollection.Remove(removedBo);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectorManager.TabControl.TabPages.Count);
        }

        [Test]
        public virtual void Test_SelectedBusinessObject_Null_DoesNothing()
        {
            //---------------Set up test pack-------------------
            //The control is being swapped out 
            // onto each tab i.e. all the tabs use the Same BusinessObjectControl
            // setting the selected Bo to null is therefore not a particularly 
            // sensible action on a BOTabControl.t up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            BOColTabControlManager selectorManager = CreateBOTabControlManager(tabControl);
            MyBO myBO = new MyBO();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO> { myBO };
            selectorManager.BusinessObjectCollection = collection;
            selectorManager.CurrentBusinessObject = null;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IBusinessObject selectedBusinessObject = selectorManager.CurrentBusinessObject;
            //---------------Test Result -----------------------
            Assert.IsNotNull(selectedBusinessObject);
        }

        [Test]
        public void Test_Selector_Clear_ClearsItems()
        {
            //---------------Set up test pack-------------------
            ITabControl tabControl = CreateTabControl();
            BOColTabControlManager selectorManager = CreateBOTabControlManager(tabControl);
            MyBO bo;
            BusinessObjectCollection<MyBO> col= GetMyBoCol_3Items(out bo);
            selectorManager.BusinessObjectCollection = col;
            //---------------Assert Preconditions --------------
            Assert.IsNotNull(selectorManager.CurrentBusinessObject);
            Assert.IsNotNull(selectorManager.BusinessObjectCollection);
            //---------------Execute Test ----------------------
            selectorManager.Clear();
            //---------------Test Result -----------------------
            Assert.IsNull(selectorManager.BusinessObjectCollection);
            Assert.IsNull(selectorManager.CurrentBusinessObject);
            Assert.AreEqual(0, selectorManager.NoOfItems);
        }

        protected virtual BOColTabControlManager CreateBOTabControlManager(ITabControl tabControl)
        {
            IControlFactory controlFactory = GetControlFactory();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            selectorManager.BusinessObjectControl = GetBusinessObjectControl();
            return selectorManager;
        }

        protected virtual BOColTabControlManager GetselectorManager(out BusinessObjectControlCreatorDelegate creator)
        {
            IControlFactory controlFactory = GetControlFactory();
            ITabControl tabControl = controlFactory.CreateTabControl();
            BOColTabControlManager selectorManager = new BOColTabControlManager(tabControl, controlFactory);
            creator = BusinessObjectControlCreator;
            selectorManager.BusinessObjectControlCreator = creator;
            return selectorManager;
        }
    }


}