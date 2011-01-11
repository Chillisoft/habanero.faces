using System;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG
{
    [TestFixture]
    public class TestEnumerationsVWG
    {

        // VWG has the HorizontalAlignment done in different order to WinForms
        [Test]
        public void TestHorizontalAlignment_HabaneroDifferentToVWG()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Center),
                            Convert.ToInt32(Habanero.Faces.Base.HorizontalAlignment.Center));
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Right),
                            Convert.ToInt32(Habanero.Faces.Base.HorizontalAlignment.Right));
            Assert.AreEqual(Convert.ToInt32(Gizmox.WebGUI.Forms.HorizontalAlignment.Left),
                            Convert.ToInt32(Habanero.Faces.Base.HorizontalAlignment.Left));
        }
    }
}