using Diplomacy.Kit;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Diplomacy.Kit.Tests
{
    [TestClass()]
    public class EnumerationTest
    {
        private class MockEnumerationClass: Enumeration
        {
            public static readonly MockEnumerationClass Enum1 = new MockEnumerationClass(1, "Enum1");
            public static readonly MockEnumerationClass Enum2 = new MockEnumerationClass(2, "Enum2");
            public MockEnumerationClass(): base()
            {

            }
            public MockEnumerationClass(int value, string displayName): base(value, displayName)
            {

            }
        }
        private Enumeration CreateDefaultInstance()
        {
            return new MockEnumerationClass();
        }
        private Enumeration CreateInstance(int value, string displayName)
        {
            return new MockEnumerationClass(value, displayName);
        }
        [TestMethod()]
        public void ToStringTest()
        {
            string value = DateTime.Now.ToString();
            var test = CreateInstance(0, value);
            Assert.AreEqual<string>(value, test.ToString());
        }
        [TestMethod()]
        public void ToStringAndDisplayNameTest()
        {
            string value = DateTime.Now.ToString();
            var test = CreateInstance(0, value);
            Assert.AreEqual<string>(test.DisplayName, test.ToString());
        }
        [TestMethod()]
        public void GetAllTest()
        {
            IEnumerable<MockEnumerationClass> list = Enumeration.GetAll<MockEnumerationClass>();
            int count = 0;
            foreach (var item in list)
            {
                count++;
            }
            Assert.IsTrue(count == 2);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var test = CreateInstance(0, "Testing");
            var test2 = CreateInstance(0, "Testing");
            Assert.IsTrue(test.Equals(test2));
        }
        [TestMethod()]
        public void EqualsNegativeTest()
        {
            var test = CreateInstance(0, "Testing");
            var test3 = CreateInstance(1, "Testing");
            Assert.IsFalse(test.Equals(test3));
        }
        [TestMethod()]
        public void GetHashCodeTest()
        {
            var test = CreateInstance(123, "Testing");
            int hCode = (123).GetHashCode();
            Assert.AreEqual<int>(test.GetHashCode(), hCode);
        }

        [TestMethod()]
        public void AbsoluteDifferenceTest()
        {
            int v1 = 123;
            int v2 = 456;
            var test1 = CreateInstance(123, "123");
            var test2 = CreateInstance(456, "456");
            Assert.AreEqual<int>(Enumeration.AbsoluteDifference(test1, test2), Math.Abs(v1 - v2));
        }

        [TestMethod()]
        public void FromValueTest()
        {
            int val = 2;
            var test = MockEnumerationClass.FromValue<MockEnumerationClass>(val);
            Assert.AreEqual<int>(test.Value, val);
        }

        [TestMethod()]
        public void FromDisplayNameTest()
        {
            string dName = "Enum1";
            var test = MockEnumerationClass.FromDisplayName<MockEnumerationClass>(dName);
            Assert.AreEqual<string>(test.DisplayName, dName);
        }

        [TestMethod()]
        public void CompareToTest()
        {
            var test = CreateInstance(123, "123");
            var test2 = CreateInstance(456, "456");
            var test3 = CreateInstance(0, "0");
            var test4 = CreateInstance(123, "Should be comparable with test");
            Assert.IsTrue(test.CompareTo(test2) < 0);
            Assert.IsTrue(test.CompareTo(test3) > 0);
            Assert.IsTrue(test.CompareTo(test4) == 0);
        }
    }
}

