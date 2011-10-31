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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    /// <summary>
    /// This test class tests the <see cref="IEditableGridControl"/> class but can be overridden to 
    /// test any class that implements the IBOSelectorControl Interface.
    /// The methods to override are <see cref="GetControlFactory"/><br/> 
    /// <see cref="CreateSelector"/><br/>

    /// 
    /// You should also override this for the VWG implementation of each control
    /// override the <see cref="GetControlFactory"/> to return a VWG control Factory.
    /// </summary>
    public abstract class TestBOSelectorAndEditor
    {
        protected abstract IControlFactory GetControlFactory();

        protected virtual IBOSelectorAndEditor CreateSelector()
        {
            return GetControlFactory().CreateEditableGridControl();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [Test]
        public virtual void Test_AllowUsersToAddBO_False()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            //---------------Assert Precondition----------------
            Assert.IsTrue(colSelector.AllowUsersToAddBO);
            //---------------Execute Test ----------------------
            colSelector.AllowUsersToAddBO = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(colSelector.AllowUsersToAddBO);
        }

        [Test]
        public virtual void Test_AllowUsersToAddBO_True()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            colSelector.AllowUsersToAddBO = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(colSelector.AllowUsersToAddBO);
            //---------------Execute Test ----------------------
            colSelector.AllowUsersToAddBO = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(colSelector.AllowUsersToAddBO);
        }

        [Test]
        public virtual void Test_AllowUsersToEditBO_False()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            //---------------Assert Precondition----------------
            Assert.IsTrue(colSelector.AllowUsersToEditBO);
            //---------------Execute Test ----------------------
            colSelector.AllowUsersToEditBO = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(colSelector.AllowUsersToEditBO);
        }

        [Test]
        public virtual void Test_AllowUsersToEditBO_True()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            colSelector.AllowUsersToEditBO = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(colSelector.AllowUsersToEditBO);
            //---------------Execute Test ----------------------
            colSelector.AllowUsersToEditBO = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(colSelector.AllowUsersToEditBO);
        }

        [Test]
        public virtual void Test_AllowUsersToDeleteBO_False()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            //---------------Assert Precondition----------------
            Assert.IsTrue(colSelector.AllowUsersToDeleteBO);
            //---------------Execute Test ----------------------
            colSelector.AllowUsersToDeleteBO = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(colSelector.AllowUsersToDeleteBO);
        }

        [Test]
        public virtual void Test_AllowUsersToDeleteBO_True()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            colSelector.AllowUsersToDeleteBO = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(colSelector.AllowUsersToDeleteBO);
            //---------------Execute Test ----------------------
            colSelector.AllowUsersToDeleteBO = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(colSelector.AllowUsersToDeleteBO);
        }

        [Test]
        public void Test_Set_ConfirmDelete_False_SetsFalse()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            //---------------Assert Precondition----------------
            Assert.IsFalse(colSelector.ConfirmDeletion);
            //---------------Execute Test ----------------------
            colSelector.ConfirmDeletion = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(colSelector.ConfirmDeletion);
        }

        [Test]
        public void Test_Set_ConfirmDelete_True_SetsTrue()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            colSelector.ConfirmDeletion = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(colSelector.ConfirmDeletion);
            //---------------Execute Test ----------------------
            colSelector.ConfirmDeletion = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(colSelector.ConfirmDeletion);
        }

        [Test]
        public void Test_Set_CheckUserConfirmsDeletionDelegate_NewDelegate_SetsNewDelegate()
        {
            //---------------Set up test pack-------------------
            IBOSelectorAndEditor colSelector = CreateSelector();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(colSelector.CheckUserConfirmsDeletionDelegate);
            //---------------Execute Test ----------------------
            colSelector.CheckUserConfirmsDeletionDelegate = DummyDeletionDelegate;
            //---------------Test Result -----------------------
            Assert.IsNotNull(colSelector.CheckUserConfirmsDeletionDelegate);
        }

        private bool DummyDeletionDelegate()
        {
            return false;
        }
    }


}