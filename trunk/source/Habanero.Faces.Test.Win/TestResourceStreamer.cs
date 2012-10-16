using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Faces.Win;
using NUnit.Framework;

namespace Habanero.Faces.Test.Win
{
    [TestFixture]
    class TestResourceStreamer
    {
        [Test]
        public void RightMatchStringArray_WhenNeedlesArrayIsRightMatch_ReturnsTrue()
        {
            //---------------Set up test pack-------------------
            var haystack = new string[] { "foo", "bar", "quux", "wibble" };
            var needles = new string[] { "quux", "wibble" };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var matches = ResourceStreamer.RightMatchStringArray(haystack, needles);
            //---------------Test Result -----------------------
            Assert.IsTrue(matches);
        }

        [Test]
        public void RightMatchStringArray_WhenNeedlesArrayIsNotRightMatch_ReturnsFalse()
        {
            //---------------Set up test pack-------------------
            var haystack = new string[] { "foo", "bar", "quux", "wibble" };
            var needles = new string[] { "foo", "bar" };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var matches = ResourceStreamer.RightMatchStringArray(haystack, needles);
            //---------------Test Result -----------------------
            Assert.IsFalse(matches);
        }
    }
}
