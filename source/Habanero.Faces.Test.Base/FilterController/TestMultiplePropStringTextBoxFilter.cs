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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    public abstract class TestMultiplePropStringTextBoxFilter
    {
        protected abstract IControlFactory GetControlFactory();

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propertyName = "";
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string>{"prop1","prop2","prop3"};
            string name = propertyName;
           
            //---------------Execute Test ----------------------
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            props.ForEach(s => name = s + "/" + name);
            propertyName = name.Remove(name.Length - 1);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ITextBox), filter.Control);
            Assert.AreEqual(propertyName, filter.PropertyName);
            Assert.AreEqual(filterClauseOperator, filter.FilterClauseOperator);
            Assert.IsInstanceOf(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
        }

        [Test]
        public void TestFilterClause()
        {
            //---------------Set up test pack-------------------
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string> { "prop1", "prop2", "prop3" };
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("(({0} > '{3}') or ({1} > '{3}')) or ({2} > '{3}')", props[0],props[1],props[2], text), filterClause.GetFilterClauseString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestFilterClause_1Prop()
        {
            //---------------Set up test pack-------------------
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string> { "prop1" };
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("{0} > '{1}'", props[0], text), filterClause.GetFilterClauseString());
            //---------------Tear Down -------------------------          
        }        
        
        [Test]
        public void TestFilterClause_2Prop()
        {
            //---------------Set up test pack-------------------
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            List<string> props = new List<string> { "prop1","prop2" };
            MultiplePropStringTextBoxFilter filter = new MultiplePropStringTextBoxFilter(GetControlFactory(), props, filterClauseOperator);
            ITextBox textBox = (ITextBox) filter.Control;
            string text = TestUtil.GetRandomString();
            textBox.Text = text;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("({0} > '{2}') or ({1} > '{2}')", props[0],props[1], text), filterClause.GetFilterClauseString());
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("TODO:Peter")]
        public void TestValueChangedNotFiredIfFilterModeIsFilter()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            //---------------Tear Down -------------------------          
        }
    }


}