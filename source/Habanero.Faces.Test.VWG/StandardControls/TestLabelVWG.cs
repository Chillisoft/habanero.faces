using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.StandardControls
{
    [TestFixture]
    public class TestLabelVWG : TestLabel
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.Faces.VWG.ControlFactoryVWG();
        }

        [Test]
        public void TestPreferredSize()
        {
            //---------------Set up test pack-------------------
            ILabel myLabel = CreateLabel();
            string labelText = "sometext";
            myLabel.Text = labelText;

            //---------------Execute Test ----------------------
            int preferredWidth = myLabel.PreferredWidth;
            //---------------Test Result -----------------------

            Assert.AreEqual(labelText.Length * 6, preferredWidth);
            //---------------Tear Down -------------------------          
        }
    }
}