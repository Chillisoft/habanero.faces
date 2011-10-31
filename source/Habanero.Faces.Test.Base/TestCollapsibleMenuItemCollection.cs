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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test;
using Habanero.Test.Structure;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    public abstract class TestCollapsibleMenuItemCollection
    {
        [SetUp]
        public void SetupTest()
        {
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            BORegistry.DataAccessor = new DataAccessorInMemory();
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
        }

        protected abstract IMenuItemCollection CreateControl();
        protected abstract IMenuItemCollection CreateControl(IMenuItem menuItem);
        protected abstract IControlFactory CreateNewControlFactory();
        protected abstract IMenuItem CreateMenuItem();
        protected abstract IMenuItem CreateMenuItem(HabaneroMenu.Item habaneroMenuItem);

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory factory = CreateNewControlFactory();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_Construct_ShouldSetMenuItem()
        {
            //---------------Set up test pack-------------------
            IMenuItem menuItem = CreateMenuItem();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItemCollection collapsibleMenuItemCollection = CreateControl(menuItem);
            //---------------Test Result -----------------------
            Assert.AreSame(menuItem, collapsibleMenuItemCollection.OwnerMenuItem);
        }

        [Test]
        public void Test_ConstructMenuItem_WithHabaneroMenuItem_ShouldSetName()
        {
            //---------------Set up test pack-------------------
            string name = TestUtil.GetRandomString();
            HabaneroMenu.Item item = new HabaneroMenu.Item(null, name);
            IMenuItem menuItem = CreateMenuItem(item);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IMenuItemCollection collapsibleMenuItemCollection = CreateControl(menuItem);
            //---------------Test Result -----------------------
            Assert.AreSame(menuItem, collapsibleMenuItemCollection.OwnerMenuItem);
        }


      
    }
}