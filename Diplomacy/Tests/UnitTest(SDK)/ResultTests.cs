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
    public class ResultTests
    {
        [TestMethod()]
        public void ResultTest()
        {
            var res = new Result();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
        [TestMethod()]
        public void ResultArgTest()
        {
            var code = ResultCode.AlreadyInitialized;
            var res = new Result(code);
            Assert.AreEqual(res.Code, code);
        }
    }
}