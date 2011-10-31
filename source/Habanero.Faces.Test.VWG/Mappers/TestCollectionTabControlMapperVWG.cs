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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Test.Base.Mappers;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Mappers
{
    [TestFixture]
    public class TestCollectionTabControlMapperVWG : TestCollectionTabControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }

        protected override IBusinessObjectControl CreateBusinessObjectControl()
        {
            return new BusinessObjectControlVWG();
        }
        class BusinessObjectControlVWG : Habanero.Faces.VWG.ControlVWG, IBusinessObjectControl
        {

            //        /// <summary>
            //        /// Specifies the business object being represented
            //        /// </summary>
            //        /// <param name="bo">The business object</param>
            //        public void SetBusinessObject(IBusinessObject bo)
            //        {
            //            
            //        }

            #region Implementation of IBusinessObjectControl
            // ReSharper disable ValueParameterNotUsed
            /// <summary>
            /// Gets or sets the business object being represented
            /// </summary>
            public IBusinessObject BusinessObject
            {
                get { return null; }

                set { }
            }
            // ReSharper restore ValueParameterNotUsed
            #endregion
        }
    }
}