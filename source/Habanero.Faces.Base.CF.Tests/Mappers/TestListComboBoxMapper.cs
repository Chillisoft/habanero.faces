//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Windows.Forms;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base.Mappers
{
    [TestFixture]
    public class TestListComboBoxMapper : TestMapperBase
    {
        protected IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "propname";
            //---------------Execute Test ----------------------
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());

            //---------------Test Result -----------------------
            Assert.AreSame(cbx, mapper.Control);
            Assert.AreSame(propName, mapper.PropertyName);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetList()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "propname";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());

            //---------------Execute Test ----------------------
            mapper.SetList("One|Two|Three|Four");
            //---------------Test Result -----------------------
            Assert.AreEqual(4, cbx.Items.Count);
            Assert.AreSame(typeof (string), cbx.Items[0].GetType());
            Assert.IsTrue(cbx.Items.Contains("Two"));
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestSetList_ToNull_ShouldNotRaiseError()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "propname";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());

            //---------------Execute Test ----------------------
            mapper.SetList(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cbx.Items.Count);
        }
        [Test]
        public void TestSetList_ToEmptyString_ShouldNotRaiseError()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "propname";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());

            //---------------Execute Test ----------------------
            mapper.SetList("");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cbx.Items.Count);
        }
/*//TODO brett 30 Mar 2011: CF
        [Test]
        public void TestSetBusinessObject()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample();
            s.SampleText = "Three";
            //---------------Execute Test ----------------------
            mapper.BusinessObject = s;
            //---------------Test Result -----------------------
            Assert.AreEqual("Three", cbx.SelectedItem, "Value is not set.");

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetBusinessObject_Null_ShouldNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample {SampleText = "Three"};
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(null, cbx.SelectedItem, "Value is not set to null.");

            //---------------Tear Down -------------------------
        }*/

        [Test]
        public void TestSetBusinessObject_Null_NoListSet_ShouldNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.AreEqual(null, cbx.SelectedItem, "Value is not set to null.");
        }
/*//TODO brett 30 Mar 2011: CF
        [Test]
        public void TestApplyChangesToBusinessObj()
        {
            //---------------Set up test pack-------------------
            ComboBox cbx = GetControlFactory().CreateComboBox();
            const string propName = "SampleText";
            ListComboBoxMapper mapper = new ListComboBoxMapper(cbx, propName, false, GetControlFactory());
            mapper.SetList("One|Two|Three|Four");
            Sample s = new Sample();
            s.SampleText = "Three";
            mapper.BusinessObject = s;
            //---------------Execute Test ----------------------
            cbx.SelectedIndex = 0;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(cbx.SelectedItem, s.SampleText,
                            "BO property value isn't changed when control value is changed.");

            //---------------Tear Down -------------------------
        }*/
    }


}