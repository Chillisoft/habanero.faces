using Habanero.Faces.Test.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG
{
    [TestFixture]
    public class TestDialogResultVWG:TestBaseWithDisposing
    {

        [Test]
        public void TestDialogResultVWG_Abort()
        {
            //---------------Set up test pack-------------------
            FormVWG formVWG = new FormVWG();
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
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
            DisposeOnTearDown(formVWG);
            //---------------Execute Test ----------------------
            formVWG.DialogResult = Habanero.Faces.Base.DialogResult.Yes;
            //---------------Test Result -----------------------
            Assert.AreEqual((int)Gizmox.WebGUI.Forms.DialogResult.Yes, (int)formVWG.DialogResult);
            Assert.AreEqual(Habanero.Faces.Base.DialogResult.Yes.ToString(), formVWG.DialogResult.ToString());
            //---------------Tear Down -------------------------
        }
    }
}