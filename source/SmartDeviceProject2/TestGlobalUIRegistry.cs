// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    [TestFixture]
    public class TestGlobalUIRegistry
    {
        private readonly IUISettings _uiSettings = GlobalUIRegistry.UISettings;
        
        
        [SetUp]
        public void ResetRegistry()
        {
            GlobalUIRegistry.UISettings = _uiSettings;
        }

        [TearDown]
        public void RestoreRegistry()
        {
            GlobalUIRegistry.UISettings = _uiSettings;
        }

        [Test]
        public void TestGetsAndSetsOnUISettings()
        {
            GlobalUIRegistry.UISettings = new UISettings();
            Assert.IsNull(GlobalUIRegistry.UISettings.PermitComboBoxRightClick);

            GlobalUIRegistry.UISettings.PermitComboBoxRightClick += delegate { return false; };
            Assert.IsNotNull(GlobalUIRegistry.UISettings.PermitComboBoxRightClick);
            Assert.IsFalse(GlobalUIRegistry.UISettings.PermitComboBoxRightClick(typeof(String), null));

            GlobalUIRegistry.UISettings.PermitComboBoxRightClick += delegate { return true; };
            Assert.IsTrue(GlobalUIRegistry.UISettings.PermitComboBoxRightClick(typeof(String), null));
        }
    }
}
