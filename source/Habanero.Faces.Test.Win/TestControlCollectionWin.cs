using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    public class TestControlCollectionWin : TestControlCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void TestAddControl()
        {
            TextBoxWin tb = (TextBoxWin)GetControlFactory().CreateTextBox();
            IControlCollection col = new ControlCollectionWin(new System.Windows.Forms.Control.ControlCollection(tb));
            IControlHabanero ctl = GetControlFactory().CreateControl();
            col.Add(ctl);
            Assert.AreSame(ctl, col[0], "Control added should be the same object.");
        }
    }
}