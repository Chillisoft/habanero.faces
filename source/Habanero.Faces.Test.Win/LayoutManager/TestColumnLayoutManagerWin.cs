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
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win.LayoutManager
{
    [TestFixture]
    public class TestColumnLayoutManagerWin : TestColumnLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.Win.ControlFactoryWin();
        }

        private static IPanel CreateColoredPanel(IControlFactory controlFactory, string labelPrefix)
        {
            System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)controlFactory.CreatePanel();
            ILabel label = controlFactory.CreateLabel(labelPrefix + "Left-Click to grow, Right-Click to shrink.");
            label.Height = 15;
            label.BackColor = Color.White;
            panel.Controls.Add((System.Windows.Forms.Control)label);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.MouseDown += (sender, e) =>
                                   {
                                       if (e.Button == System.Windows.Forms.MouseButtons.Left) panel.Height += 5;
                                       if (e.Button == System.Windows.Forms.MouseButtons.Right) panel.Height -= 5;
                                   };
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
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(panel, controlFactory) { ColumnCount = 2 };
            int controlNumber = 1;
            columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
            columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
            columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
            IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            buttonGroupControl.Dock = DockStyle.Top;
            groupBox.Controls.Add(buttonGroupControl);
            buttonGroupControl.AddButton("Add Control", (sender, e) => columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":")));
            buttonGroupControl.AddButton("-Columns", (sender, e) =>
                                                         {
                                                             if (columnLayoutManager.ColumnCount > 1)
                                                             {
                                                                 columnLayoutManager.ColumnCount--;
                                                                 columnLayoutManager.Refresh();
                                                             }
                                                         });
            buttonGroupControl.AddButton("+Columns", (sender, e) => { columnLayoutManager.ColumnCount++; columnLayoutManager.Refresh(); });
            IFormHabanero form = controlFactory.CreateOKCancelDialogFactory().CreateOKCancelForm(groupBox, "Test Column Layout Manager");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            form.ShowDialog();
            //---------------Test Result -----------------------
        }

        [Test]
        [Ignore("This is for visual testing purposes")]
        public void Test_Visually_Advanced()
        {
            //---------------Set up test pack-------------------
            IControlFactory controlFactory = GetControlFactory();
            IGroupBox groupBox = controlFactory.CreateGroupBox("Test Layout");
            IPanel panel = controlFactory.CreatePanel();
            panel.Dock = DockStyle.Fill;
            groupBox.Controls.Add(panel);
            ColumnLayoutManager columnLayoutManager = new ColumnLayoutManager(panel, controlFactory) { ColumnCount = 1 };
            int controlNumber = 1;
            IPanel panel1 = CreateColoredPanel(controlFactory, controlNumber++ + ":");
            panel1.Controls.Clear();
            GridLayoutManager gridLayoutManager = new GridLayoutManager(panel1, controlFactory);
            gridLayoutManager.SetGridSize(4, 3);
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "a:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "b:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "c:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "d:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "e:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "f:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "g:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "h:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "i:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "j:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "k:"));
            gridLayoutManager.AddControl(CreateColoredPanel(controlFactory, "l:"));


            columnLayoutManager.AddControl(panel1);
            columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":"));
            IButtonGroupControl buttonGroupControl = controlFactory.CreateButtonGroupControl();
            buttonGroupControl.Dock = DockStyle.Top;
            groupBox.Controls.Add(buttonGroupControl);
            buttonGroupControl.AddButton("Add Control", (sender, e) => columnLayoutManager.AddControl(CreateColoredPanel(controlFactory, controlNumber++ + ":")));
            buttonGroupControl.AddButton("-Columns", (sender, e) =>
                                                         {
                                                             if (columnLayoutManager.ColumnCount > 1)
                                                             {
                                                                 columnLayoutManager.ColumnCount--;
                                                                 columnLayoutManager.Refresh();
                                                             }
                                                         });
            buttonGroupControl.AddButton("+Columns", (sender, e) => { columnLayoutManager.ColumnCount++; columnLayoutManager.Refresh(); });
            IFormHabanero form = controlFactory.CreateOKCancelDialogFactory().CreateOKCancelForm(groupBox, "Test Column Layout Manager");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            form.ShowDialog();
            //---------------Test Result -----------------------
        }
    }
}