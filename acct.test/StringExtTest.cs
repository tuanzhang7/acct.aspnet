using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using acct.service.Helper;
namespace acct.test
{
    [TestClass]
    public class StringExtTest
    {
        [TestMethod]
        public void TruncateLongString()
        {
            string s = "1234567890123456789012345";
            string expect = "12345678901234567890";
            string actual = s.TruncateLongString(20);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TruncateLongString_empty()
        {
            string s = "";
            string expect = "";
            string actual = s.TruncateLongString(20);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TruncateLongString_null()
        {
            string s = null;
            string expect = null;
            string actual = s.TruncateLongString(20);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TruncateLongString_length_less_than_string()
        {
            string s = "12345";
            string expect = "12345";
            string actual = s.TruncateLongString(20);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "maxLength could not be less than zero")]
        public void TruncateLongString_length_nagtive()
        {
            string s = "12345";
            string expect = "";
            string actual = s.TruncateLongString(-20);
            Assert.AreEqual(expect, actual);
        }
    }
}
