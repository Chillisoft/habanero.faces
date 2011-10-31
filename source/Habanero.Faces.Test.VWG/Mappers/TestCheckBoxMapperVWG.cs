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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestCheckBoxMapperVWG : TestCheckBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }

        [Test]
        public void TestSettingCheckBoxCheckedUpdatesBO()
        {
            //----------Setup test pack----------------------------
            _sampleBusinessObject.SampleBoolean = false;
            _mapper.BusinessObject = _sampleBusinessObject;
            //----------verify test pack --------------------------
            //----------Execute test ------------------------------
            _cb.Checked = true;
            _mapper.ApplyChangesToBusinessObject();
            //----------verify test ------------------------------
            Assert.IsTrue(_sampleBusinessObject.SampleBoolean);
        }
    }
}