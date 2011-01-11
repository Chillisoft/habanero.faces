using System;
using Habanero.Base;
using Habanero.Faces.Base;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.FilterController
{
    [TestFixture]
    public abstract class TestEnumComboBoxFilter
    {
        protected abstract IControlFactory GetControlFactory();

        public enum PurchaseOrderStatus
        {
            Open,
            Processed
        }
        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpEquals;
            
            //---------------Execute Test ----------------------
            EnumComboBoxFilter filter = new EnumComboBoxFilter(GetControlFactory(), propertyName, filterClauseOperator, typeof(PurchaseOrderStatus));

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
            Assert.AreEqual(propertyName, filter.PropertyName);
            Assert.AreEqual(filterClauseOperator, filter.FilterClauseOperator);
            Assert.IsInstanceOf(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
        }
        [Test]
        public void TestConstructor_ShouldSetUpComboBoxItems()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpEquals;
            
            //---------------Execute Test ----------------------
            EnumComboBoxFilter filter = new EnumComboBoxFilter(GetControlFactory(), propertyName, filterClauseOperator, typeof(PurchaseOrderStatus));

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
            IComboBox comboBox = (IComboBox) filter.Control;
            Assert.AreEqual(3, comboBox.Items.Count, "Two Items and Blank");
       }

        [Test]
        public void TestFilterClause()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            const FilterClauseOperator filterClauseOperator = FilterClauseOperator.OpGreaterThan;
            EnumComboBoxFilter filter = new EnumComboBoxFilter(GetControlFactory(), propertyName, filterClauseOperator, typeof(PurchaseOrderStatus));
            IComboBox comboBox = (IComboBox)filter.Control;
            string text = Convert.ToString( PurchaseOrderStatus.Processed);
            comboBox.Text = text;
            comboBox.SelectedIndex = 2;

            //---------------Execute Test ----------------------

            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------

            Assert.AreEqual(string.Format("{0} > '{1}'", propertyName, PurchaseOrderStatus.Processed), filterClause.GetFilterClauseString());      
        }

    }

}