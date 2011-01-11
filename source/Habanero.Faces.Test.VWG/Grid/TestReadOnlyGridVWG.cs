using Habanero.Faces.Test.Base;
using Habanero.Faces.Base;
using Habanero.Faces.VWG;
using NUnit.Framework;

namespace Habanero.Faces.Test.VWG.Grid
{
    [TestFixture]
    public class TestReadOnlyGridVWG : TestReadOnlyGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
        }
        [Test]
        public void TestCreateGridBaseVWG()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlHabanero grid = GetControlFactory().CreateReadOnlyGrid();
            ReadOnlyGridVWG readOnlyGrid = (ReadOnlyGridVWG)grid;
            ////---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGrid.ReadOnly);
            Assert.IsFalse(readOnlyGrid.AllowUserToAddRows);
            Assert.IsFalse(readOnlyGrid.AllowUserToDeleteRows);
            Assert.IsTrue(readOnlyGrid.SelectionMode == Gizmox.WebGUI.Forms.DataGridViewSelectionMode.FullRowSelect);
        }
    }
}