using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilePlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplomacy.Kit;

namespace FilePlugins.Tests
{
    [TestClass()]
    public class WordCountAmbassadorTests
    {
        [TestMethod()]
        public void ProcessTest()
        {
            var wAmbassador = new WordCountAmbassador();
            var result = wAmbassador.Process("Some JSON string here");
            Assert.AreEqual(result.Code, ResultCode.Success);
        }
        [TestMethod]
        public void ProcessNullTest()
        {
            var wAmbassador = new WordCountAmbassador();
            var result = wAmbassador.Process(null);
            Assert.AreEqual(result.Code, ResultCode.InvalidArgument);
        }
        [TestMethod()]
        public void StartTest()
        {
            var wAmbassador = new WordCountAmbassador();
            var result = wAmbassador.Start();
            Assert.AreEqual(result.Code, ResultCode.Success);
        }

        [TestMethod()]
        public void StopTest()
        {
            var wAmbassador = new WordCountAmbassador();
            var result = wAmbassador.Stop();
            Assert.AreEqual(result.Code, ResultCode.Success);
        }
        [TestMethod()]
        public void IDTest()
        {
            var wAmbassador = new WordCountAmbassador();
            Assert.AreEqual(wAmbassador.ID, "WordCountAmbassador");
        }
        [TestMethod()]
        public void SupportedActionsTest()
        {
            var wAmbassador = new WordCountAmbassador();
            CollectionAssert.AreEquivalent(wAmbassador.SupportedActions.ToList(), new List<string>() { "WordCount" });
        }
    }
}