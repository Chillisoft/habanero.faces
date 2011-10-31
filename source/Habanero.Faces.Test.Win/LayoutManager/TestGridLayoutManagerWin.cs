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
using System.Drawing;
using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.LayoutManager
{
    [TestFixture]
    public class TestGridLayoutManagerWin : TestGridLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        private static IPanel CreateColoredPanel(IControlFactory controlFactory, string labelPrefix)
        {
            System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)controlFactory.CreatePanel();
            ILabel label = controlFactory.CreateLabel(labelPrefix);
            label.Height = 15;
            label.BackColor = Color.White;
            panel.Controls.Add((System.Windows.Forms.Control)label);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.BackColor = Color.FromArgb(TestUtil.GetRandomInt(255), TestUtil.GetRandomInt(255), TestUtil.GetRandomInt(255));
            return (IPanel)panel;
        }

        [Test]
        [Ignore("This is for visual testing purposes")]
        public void Test_Visually()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            IGroupBox groupBox = controlFactory.CreateGroupBox("Test Layout");
            IPanel panel = controlFactory.CreatePanel();
            panel.Dock = DockStyle.Fill;
            groupBox.Controls.Add(panel);
            GridLayoutManager gridLayoutManager = new GridLayoutManager(panel, controlFactory);
            gridLayoutManager.SetGridSize(6, 2);
            int controlNumber = 1;
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()), 2, 1);
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()), 2, 1);
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++.ToString()));
            //IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            //buttonGroupControl.Dock = DockStyle.Top;
            //groupBox.Controls.Add(buttonGroupControl);
            //buttonGroupControl.AddButton("Add Control", (sender, e) => gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":")));
            //buttonGroupControl.AddButton("-Columns", (sender, e) =>
            //{
            //    if (gridLayoutManager.ColumnCount > 1)
            //    {
            //        gridLayoutManager.ColumnCount--;
            //        gridLayoutManager.Refresh();
            //    }
            //});
            //buttonGroupControl.AddButton("+Columns", (sender, e) => { gridLayoutManager.ColumnCount++; gridLayoutManager.Refresh(); });
            IFormHabanero form = controlFactory.CreateOKCancelDialogFactory().CreateOKCancelForm(groupBox, "Test Grid Layout Manager");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            form.ShowDialog();
            //---------------Test Result -----------------------
        }

    }
}