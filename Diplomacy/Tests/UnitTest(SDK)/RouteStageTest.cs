using Diplomacy.Kit;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diplomacy.Kit.Tests
{
    [TestClass()]
    public class RouteStageTest
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            string id = "testID";
            string action = "myCustomAction";
            var test = new RouteStage(id, action);
            Assert.AreEqual<string>(test.ID, id);
            Assert.AreEqual<string>(test.Action, action);
        }
        [TestMethod()]
        public void DefaultConstructorTest()
        {
            var test = new RouteStage();
            Assert.AreEqual<string>(test.ID, "");
            Assert.AreEqual<string>(test.Action, "");
        }
    }
}