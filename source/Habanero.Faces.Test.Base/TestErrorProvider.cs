using System;
using Habanero.Faces.Base;
using NUnit.Framework;

namespace Habanero.Faces.Test.Base
{
    /// <summary>
    /// This test class tests the Button class.
    /// </summary>
    public abstract class TestErrorProvider
    {
        protected abstract IControlFactory GetControlFactory();

     

        [Test]
        public void TestCreateErrorProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            var errorProvider = GetControlFactory().CreateErrorProvider();

            //---------------Test Result -----------------------
            Assert.IsNotNull(errorProvider);

            //---------------Tear Down -------------------------   
        }

    }
}