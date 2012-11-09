using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.Faces.Test.Base.Mappers;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.Mappers
{
    [TestFixture]
    public class TestLookupComboBoxMapperWin : TestLookupComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            Guid guidResult;
            StringUtilities.GuidTryParse(Sample.LookupCollection[LOOKUP_ITEM_1], out guidResult);
            s.SampleLookupID = guidResult;
            mapper.BusinessObject = s;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(3, Sample.LookupCollection.Count);
            Assert.IsNotNull(mapper.LookupList);
            Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
            //---------------Execute Test ----------------------

            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop Value");
        }

        [Test]
        public void TestChangePropValueUpdatesBusObj_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------

            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);

            //---------------Test Result -----------------------
            Assert.AreEqual(LOOKUP_ITEM_2, cmbox.SelectedItem, "Value is not set after changing bo prop");

            //---------------Tear Down -------------------------
        }

        [Test]
        public override void TestChangeComboBoxDoesntUpdateBusinessObject()
        {
            //For Windows the value should be changed.
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            Dictionary<string, string> collection = mapper.LookupList = GetLookupList();
            Guid guidResult;
            StringUtilities.GuidTryParse(collection[LOOKUP_ITEM_1], out guidResult);
            s.SampleLookupID = guidResult;
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;

            //---------------Test Result -----------------------
            Assert.AreEqual(collection[LOOKUP_ITEM_2], s.SampleLookupID.ToString(), "For Windows the value should be changed");
        }

        private static Dictionary<string, string> GetLookupList()
        {
            Sample sample1 = new Sample();
            sample1.Save();
            Sample sample2 = new Sample();
            sample2.Save();
            Sample sample3 = new Sample();
            sample3.Save();
            return new Dictionary<string, string>
                       {
                           {"Test3", sample3.ID.GetAsValue().ToString()},
                           {"Test2", sample2.ID.GetAsValue().ToString()},
                           {"Test1", sample1.ID.GetAsValue().ToString()}
                       };
        }

        [Test]
        public void TestChangeComboBoxUpdatesBusinessObject()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
        }
        //            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
        //            Sample s = new Sample();
        //            mapper.LookupList = Sample.LookupCollection;
        //            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
        //            //---------------Assert Precondition----------------
        //            Assert.AreEqual(3, Sample.LookupCollection.Count);
        //            Assert.IsNull(cmbox.SelectedItem);
        //            //---------------Execute Test ----------------------
        //            mapper.BusinessObject = s;
        [Test]
        public void TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
        }

        [Test]
        public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            var item1Guid = (Guid) GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            var item2Guid = (Guid) GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2);
            s.SampleLookupID = item1Guid;
            mapper.BusinessObject = s;
            //---------------Assert Precondition----------------
            Assert.AreEqual(item1Guid, s.SampleLookupID);
            //---------------Execute Test ----------------------
            cmbox.Text = LOOKUP_ITEM_2;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxDefaultMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual(item2Guid, s.SampleLookupID);
        }

        [Test]
        [Ignore("Visual Testing")]
        [STAThread]
        public void VisualTesting_AsDropDownList()
        {
            //---------------Set up test pack-------------------
            var controlFactory = GetControlFactory();
            ComboBoxWin cmbox = (ComboBoxWin) controlFactory.CreateComboBox();
            var lookupList = new Dictionary<string, string>
                                 {
                                     {"aaa", Guid.NewGuid().ToString()}, 
                                     {"bbb1", Guid.NewGuid().ToString()}, 
                                     {"bbb2", Guid.NewGuid().ToString()}, 
                                     {"bbb3", Guid.NewGuid().ToString()}, 
                                     {"ccc", Guid.NewGuid().ToString()},
                                 };
            var listDetails =
                lookupList.Select(pair => String.Format("{0} -> {1}", pair.Key, pair.Value)).Aggregate(
                    (s1, s2) => s1 + Environment.NewLine + s2);
            Sample s = new Sample();
            bool useMapper = false;
            if (useMapper)
            {
                const string propName = "SampleLookupID";
                LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, controlFactory);
                mapper.LookupList = lookupList;
                mapper.BusinessObject = s;
            }
            else
            {
                PopulateCombo(cmbox, lookupList);
            }
            
            cmbox.DropDownStyle = ComboBoxStyle.DropDownList;
            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            var comboTextValueLabel = new LabelWin();
            cmbox.TextChanged += (sender, args) => comboTextValueLabel.Text = "Combo Text: " + cmbox.Text;
            var comboSelectedIndexValueLabel = new LabelWin();
            cmbox.SelectedIndexChanged += (sender, args) => comboSelectedIndexValueLabel.Text = "Combo Selected Index: " + cmbox.SelectedIndex;
            var comboSelectedValueLabel = new LabelWin();
            cmbox.SelectedValueChanged += (sender, args) => comboSelectedValueLabel.Text = "Combo Selected Value: " + cmbox.SelectedItem;
            cmbox.SelectionChangeCommitted += (sender, args) => comboSelectedValueLabel.Text = "Combo Selected Value: " + cmbox.SelectedItem;
            var boPropValueLabel = new LabelWin();
            s.PropertyUpdated += (sender, args) => boPropValueLabel.Text = args.Prop.PropertyName + " " + args.Prop.Value;
            var panelWin = new PanelWin();
            var columnLayoutManager = new ColumnLayoutManager(panelWin, controlFactory);
            columnLayoutManager.AddControl(new LabelWin{Height = 100 ,Text = listDetails});
            columnLayoutManager.AddControl(comboTextValueLabel);
            columnLayoutManager.AddControl(comboSelectedIndexValueLabel);
            columnLayoutManager.AddControl(comboSelectedValueLabel);
            columnLayoutManager.AddControl(boPropValueLabel);
            columnLayoutManager.AddControl(new TextBoxWin());
            columnLayoutManager.AddControl(cmbox);
            var comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            PopulateCombo(comboBox, lookupList);
            columnLayoutManager.AddControl(new PanelWin() {Controls = {comboBox}});
            var textBoxWin = new TextBoxWin();
            columnLayoutManager.AddControl(textBoxWin);

            //cmbox.PreviewKeyDown += (sender, args) => comboTextValueLabel.Text += ",pKD:" + (args.KeyCode == Keys.Tab) + " " + args.KeyCode;
            //cmbox.KeyDown += (sender, args) => comboTextValueLabel.Text += ",KD";
            //cmbox.KeyUp += (sender, args) => comboTextValueLabel.Text += ",KU";
            //cmbox.KeyPress += (sender, args) => comboTextValueLabel.Text += ",KP";
            cmbox.PreviewKeyDown += (sender, args) =>
                                        {
                                            if (args.KeyCode != Keys.Tab) return;
                                            cmbox.Enabled = true;
                                            comboTextValueLabel.Text = "Combo Text: " + cmbox.Text;
                                            comboSelectedIndexValueLabel.Text = "Combo Selected Index: " + cmbox.SelectedIndex;
                                            comboSelectedValueLabel.Text = "Combo Selected Value: " + cmbox.SelectedItem;
                                        };
            TestUtilsUI.ShowInVisualTestingForm(panelWin);
            
            //---------------Test Result -----------------------
        }

        private static void PopulateCombo(ComboBox cmbox, Dictionary<string, string> lookupList)
        {
            cmbox.Items.Clear();
            cmbox.Items.Add(new ComboPair("", null));
            foreach (KeyValuePair<string, string> pair in lookupList)
            {
                cmbox.Items.Add(new ComboPair(pair.Key, pair.Value));
            }
        }

        [Test]
        [Ignore("Visual Testing")]
        [STAThread]
        public void VisualTesting_AsVanillaComboBoxDropDownList()
        {
            var comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            comboBox.Items.Add(new ComboPair("aaa", "ItemAAA"));
            comboBox.Items.Add(new ComboPair("bbb1", "ItemBBB1"));
            comboBox.Items.Add(new ComboPair("bbb2", "ItemBBB2"));
            comboBox.Items.Add(new ComboPair("bbb3", "ItemBBB3"));
            comboBox.Items.Add(new ComboPair("ccc", "ItemCCC"));

            
    
            var textBox = new TextBox{ Multiline = true };
            comboBox.Leave += (sender, args) => textBox.Text = " ... On Leave: " + comboBox.SelectedItem;
            comboBox.DropDownClosed += (sender, args) => textBox.Text += " ... Drop Down Closed: " + comboBox.SelectedItem;
            comboBox.LostFocus += (sender, args) => textBox.Text += " ... On LostFocus: " + comboBox.SelectedItem;
    
            var frm = new Form();
            frm.Width = 300;
            frm.Height = 100;
            comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            frm.Controls.Add(comboBox);
            frm.Controls.Add(textBox);
            Application.EnableVisualStyles();
            Application.Run(frm);
        }

        [Test]
        public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.Text = "Test2";
            cmbox.CallSendKeyBob();

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_2), s.SampleLookupID);
        }


        [Test]
        public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            const string propName = "SampleLookupID";
            LookupComboBoxMapper mapper = new LookupComboBoxMapper(cmbox, propName, false, GetControlFactory());
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            Sample s = new Sample();
            mapper.LookupList = Sample.LookupCollection;
            s.SampleLookupID = (Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1);
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = LOOKUP_ITEM_2;

            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreEqual((Guid)GetGuidValue(Sample.LookupCollection, LOOKUP_ITEM_1), s.SampleLookupID);
            //---------------Tear Down -------------------------
        }
        //
        private class ComboBoxWinStub : ComboBoxWin
        {
            public void CallSendKeyBob()
            {
                this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
            }
        }
    }
}