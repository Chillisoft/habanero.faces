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
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG
{
    [TestFixture]
    public class TestDialogResultVWG
    {
        [Test]
        public void TestDialogResultVWG_Abort()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.Abort;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.Abort, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.Abort.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestDialogResultVWG_Cancel()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.Cancel;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.Cancel, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.Cancel.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_Ignore()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.Ignore;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.Ignore, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.Ignore.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_No()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.No;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.No, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.No.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_None()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.None;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.None, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.None.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_OK()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.OK;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.OK, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.OK.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_Retry()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.Retry;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.Retry, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.Retry.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDialogResultVWG_Yes()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();

            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.Yes;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.Yes, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.Yes.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }
    }
}