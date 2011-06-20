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
		private class TestBuilder
		{
			private bool _isInitialised = false;

			public FilterClauseOperator? Operator { get; set; }
			public string PropertyName { get; private set; }
			public IControlFactory ControlFactory { get; private set; }
			public Type EnumType { get; private set; }
			public bool UsingEnumTypeConstructor { get; set; }

			public TestBuilder(IControlFactory controlFactory)
			{
				ControlFactory = controlFactory;
			}

			public EnumComboBoxFilter CreateInstance()
			{
				Initialise();
				return UsingEnumTypeConstructor 
					? new EnumComboBoxFilter(ControlFactory, PropertyName, GetFilterClauseOperator(), EnumType) 
					: new EnumComboBoxFilter(ControlFactory, PropertyName, GetFilterClauseOperator());
			}

			public FilterClauseOperator GetFilterClauseOperator()
			{
				return Operator.GetValueOrDefault(FilterClauseOperator.OpEquals);
			}

			public TestBuilder Initialise()
			{
				if (_isInitialised) return this;

				PropertyName = TestUtil.GetRandomString();
				Operator = Operator ?? FilterClauseOperator.OpEquals;
				EnumType = typeof (PurchaseOrderStatus);

				_isInitialised = true;
				return this;
			}
		}

		[SetUp]
		public void SetupTest()
		{
			_testBuilder = new TestBuilder(GetControlFactory());
		}

    	private TestBuilder _testBuilder;
    	protected abstract IControlFactory GetControlFactory();

        public enum PurchaseOrderStatus
        {
            Open,
            Processed
        }

		[Test]
		public void TestConstructor_ShouldHaveDefaultConstructor()
		{
			//---------------Set up test pack-------------------
			_testBuilder.Initialise();

			//---------------Execute Test ----------------------
			EnumComboBoxFilter filter = new EnumComboBoxFilter(_testBuilder.ControlFactory, _testBuilder.PropertyName, _testBuilder.GetFilterClauseOperator());

			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
			Assert.AreEqual(_testBuilder.PropertyName, filter.PropertyName);
			Assert.AreEqual(_testBuilder.GetFilterClauseOperator(), filter.FilterClauseOperator);
			Assert.IsInstanceOf(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
		}

		[Test]
		public void TestConstructor_WhenDefaultConstructor_ShouldNotSetupComboBoxItems()
		{
			//---------------Set up test pack-------------------
			_testBuilder.Initialise();
			//---------------Execute Test ----------------------
			var filter = new EnumComboBoxFilter(GetControlFactory(), _testBuilder.PropertyName, _testBuilder.GetFilterClauseOperator());
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
			IComboBox comboBox = (IComboBox)filter.Control;
			Assert.AreEqual(0, comboBox.Items.Count, "Should have no Items in combo");
		}

		[Test]
		public void TestConstructor()
		{
			//---------------Set up test pack-------------------
			_testBuilder.Initialise();
			//---------------Execute Test ----------------------
			EnumComboBoxFilter filter = new EnumComboBoxFilter(GetControlFactory(), _testBuilder.PropertyName, _testBuilder.GetFilterClauseOperator(), typeof(PurchaseOrderStatus));
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
			Assert.AreEqual(_testBuilder.PropertyName, filter.PropertyName);
			Assert.AreEqual(_testBuilder.GetFilterClauseOperator(), filter.FilterClauseOperator);
			Assert.IsInstanceOf(typeof(DataViewNullFilterClause), filter.GetFilterClause(new DataViewFilterClauseFactory()));
		}

        [Test]
        public void TestConstructor_ShouldSetUpComboBoxItems()
        {
            //---------------Set up test pack-------------------
        	//---------------Execute Test ----------------------
            var filter = new EnumComboBoxFilter(GetControlFactory(), _testBuilder.PropertyName, _testBuilder.GetFilterClauseOperator(), typeof(PurchaseOrderStatus));
			//---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
            IComboBox comboBox = (IComboBox) filter.Control;
            Assert.AreEqual(3, comboBox.Items.Count, "Two Items and Blank");
       }

    	[Test]
        public void TestFilterClause()
        {
            //---------------Set up test pack-------------------
			_testBuilder.Operator = FilterClauseOperator.OpGreaterThan;
    		_testBuilder.UsingEnumTypeConstructor = true;
    		var filter = _testBuilder.CreateInstance();
    		var comboBox = (IComboBox)filter.Control;
            var text = Convert.ToString( PurchaseOrderStatus.Processed);
            comboBox.Text = text;
            comboBox.SelectedIndex = 2;
            //---------------Execute Test ----------------------
            IFilterClause filterClause = filter.GetFilterClause(new DataViewFilterClauseFactory());
            //---------------Test Result -----------------------
            Assert.AreEqual(string.Format("{0} > '{1}'", _testBuilder.PropertyName, PurchaseOrderStatus.Processed), filterClause.GetFilterClauseString());      
        }

    	[Test]
    	public void TestEnumTypeName_WhenSet_ShouldSetupComboBoxItems()
    	{
    		//---------------Set up test pack-------------------
    		var filter = _testBuilder.CreateInstance();
    		var enumTypeName = typeof (PurchaseOrderStatus).AssemblyQualifiedName;
    		//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
    		filter.EnumTypeQualifiedName = enumTypeName;
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(IComboBox), filter.Control);
			IComboBox comboBox = (IComboBox)filter.Control;
    		var items = comboBox.Items;
    		Assert.AreEqual(3, items.Count, "Two Items and Blank");
			Assert.IsTrue(items.Contains(""));
			Assert.IsTrue(items.Contains(PurchaseOrderStatus.Open));
			Assert.IsTrue(items.Contains(PurchaseOrderStatus.Processed));
    	}

		[Test]
		public void TestEnumTypeName_ShouldReturnAssemblyQualifiedName()
		{
			//---------------Set up test pack-------------------
			_testBuilder.UsingEnumTypeConstructor = true;
			var filter = _testBuilder.CreateInstance();
			var expected = typeof(PurchaseOrderStatus).AssemblyQualifiedName;
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			var enumTypeQualifiedName = filter.EnumTypeQualifiedName;
			//---------------Test Result -----------------------
			Assert.AreEqual(expected, enumTypeQualifiedName);
		}

		[Test]
		public void TestEnumTypeName_WhenEnumTypeNotSpecified_ShouldReturnNull()
		{
			//---------------Set up test pack-------------------
			_testBuilder.UsingEnumTypeConstructor = false;
			var filter = _testBuilder.CreateInstance();
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			var enumTypeQualifiedName = filter.EnumTypeQualifiedName;
			//---------------Test Result -----------------------
			Assert.IsNull(enumTypeQualifiedName);
		}

    }

}