using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit.Tests
{
    [TestClass()]
    public class RouteInfoTests
    {
        [TestMethod()]
        public void RouteInfoTest()
        {
            var test = new RouteInfo();
            Assert.IsTrue(test.Stages.Count == 0);
        }
        [TestMethod()]
        public void AddStageTest()
        {
            var test = new RouteInfo();
            test.AddStage(new RouteStage("123", "convert"));
            Assert.AreEqual<int>(test.Stages.Count, 1);
        }
    }
}