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
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.HabaneroControls
{
    [TestFixture]
    public class TestStaticDataEditorVWG : TestStaticDataEditor
    {
        protected override IStaticDataEditor CreateEditorOnForm(out IFormHabanero frm)
        {
            frm = GetControlFactory().CreateForm();
            IStaticDataEditor editor = GetControlFactory().CreateStaticDataEditor();
            frm.Controls.Add(editor);
            //frm.Show();
            return editor;
        }

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override void TearDownForm(IFormHabanero frm)
        {

        }

        [Test, Ignore("Does not work because VWG form cannot be shown")]
        public override void TestSelectSection()
        {
            base.TestSelectSection();
        }

        [Test, Ignore("Does not work because VWG form cannot be shown")]
        public override void TestSaveChanges()
        {
            base.TestSaveChanges();
        }

        [Test, Ignore("Does not work because VWG form cannot be shown")]
        public override void TestRejectChanges()
        {
            base.TestRejectChanges();
        }
    }
}