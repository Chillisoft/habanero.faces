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
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Faces.Test.Win.StandardControls
{
    [TestFixture]
    public class TestComboBoxWin : TestComboBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_DataSource_WhenSet_ShouldLoadComboBox()
        {
            //---------------Set up test pack-------------------
            FormWin form = new FormWin();
            List<string> defs = new List<string> {"AA", "BBB"};
            IComboBox selector = GetControlFactory().CreateComboBox();
            form.Controls.Add((System.Windows.Forms.Control)selector);
            System.Windows.Forms.ComboBox winCombo = (System.Windows.Forms.ComboBox)selector;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, winCombo.Items.Count);
            Assert.AreEqual(0, selector.Items.Count);
            //---------------Execute Test ----------------------
            selector.DataSource = defs;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, winCombo.Items.Count);
            Assert.AreEqual(2, selector.Items.Count);
        }
        protected override string GetUnderlyingAutoCompleteSourceToString(IComboBox controlHabanero)
        {
            System.Windows.Forms.ComboBox control = (System.Windows.Forms.ComboBox)controlHabanero;
            return control.AutoCompleteSource.ToString();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_None()
        {
            base.TestConversion_AutoCompleteSource_None();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_AllSystemSources()
        {
            base.TestConversion_AutoCompleteSource_AllSystemSources();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_AllUrl()
        {
            base.TestConversion_AutoCompleteSource_AllUrl();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_CustomSource()
        {
            base.TestConversion_AutoCompleteSource_CustomSource();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_FileSystem()
        {
            base.TestConversion_AutoCompleteSource_FileSystem();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_FileSystemDirectories()
        {
            base.TestConversion_AutoCompleteSource_FileSystemDirectories();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_HistoryList()
        {
            base.TestConversion_AutoCompleteSource_HistoryList();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_ListItems()
        {
            base.TestConversion_AutoCompleteSource_ListItems();
        }

        [Test]
        [RequiresSTA]
        public override void TestConversion_AutoCompleteSource_RecentlyUsedList()
        {
            base.TestConversion_AutoCompleteSource_RecentlyUsedList();
        }
    }
}