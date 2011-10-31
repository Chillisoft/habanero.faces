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
using System;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestEnumerationsWin
    {
        [Test]
        public void TestHorizontalAlignment_HabaneroSameAsWin()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Center),
                            Convert.ToInt32(Habanero.Faces.Base.HorizontalAlignment.Center));
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Left),
                            Convert.ToInt32(Habanero.Faces.Base.HorizontalAlignment.Left));
            Assert.AreEqual(Convert.ToInt32(System.Windows.Forms.HorizontalAlignment.Right),
                            Convert.ToInt32(Habanero.Faces.Base.HorizontalAlignment.Right));
        }
    }
}