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
using Habanero.Base;
using Habanero.BO;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestBOColTabControlVWG : TestBOColTabControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new BusinessObjectControlVWGStub();
        }
        protected override Type ExpectedTypeOfBOControl()
        {
            return typeof(BusinessObjectControlVWGStub);
        }

        /// <summary>
        /// This test doesn't work for Win as it doesn't raise an event when setting the SelectedIndex
        /// </summary>
        [Test]
        public void TestBusinessObjectControlHasDifferentBOWhenTabChanges()
        {
            //---------------Set up test pack-------------------
            IBOColTabControl boColTabControl = GetControlFactory().CreateBOColTabControl();

            IBusinessObjectControl busControl = GetBusinessObjectControlStub();
            boColTabControl.BusinessObjectControl = busControl;

            BusinessObjectCollection<MyBO> myBoCol = new BusinessObjectCollection<MyBO>();
            MyBO firstBo = new MyBO();
            myBoCol.Add(firstBo);
            myBoCol.Add(new MyBO());
            MyBO thirdBO = new MyBO();
            myBoCol.Add(thirdBO);
            boColTabControl.BusinessObjectCollection = myBoCol;

            //---------------Assert Precondition----------------
            Assert.AreEqual(firstBo, boColTabControl.BusinessObjectControl.BusinessObject);

            //---------------Execute Test ----------------------
            boColTabControl.TabControl.SelectedIndex = 2;

            //---------------Test Result -----------------------
            Assert.AreNotSame(firstBo, boColTabControl.BusinessObjectControl.BusinessObject);
            Assert.AreEqual(thirdBO, boColTabControl.BusinessObjectControl.BusinessObject);
        }


        public class BusinessObjectControlVWGStub : ControlVWG, IBusinessObjectControl
        {
            private IBusinessObject _bo;

            /// <summary>
            /// Specifies the business object being represented
            /// </summary>
            /// <param name="value">The business object</param>
            public IBusinessObject BusinessObject
            {
                get { return _bo; }
                set { _bo = value; }
            }
        }

    }
}