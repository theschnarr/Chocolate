using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilePlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Diplomacy.Kit;

namespace FilePlugins.Tests
{
    [TestClass()]
    public class FileClerkTests
    {
        [TestMethod()]
        public void SetConsulTest()
        {
            var fClerk = new FileClerk();
            PrivateObject pObj = new PrivateObject(fClerk);
            Mock<IConsulRequest> mConsulRequestContainer = new Mock<Diplomacy.Kit.IConsulRequest>();
            mConsulRequestContainer.Setup(x => x.ProcessRequest(It.IsAny<RouteInfo>(), It.IsAny<string>())).Returns(new Result(ResultCode.AlreadyInitialized));
            var result = fClerk.SetConsul(mConsulRequestContainer.Object);
            Assert.AreEqual(result.Code, ResultCode.Success);
            var actualInterface = pObj.GetField("_ConsulRequest") as IConsulRequest;
            Assert.IsNotNull(actualInterface);
            result = actualInterface.ProcessRequest(null, "Imagine JSON data here");
            Assert.AreEqual(result.Code, ResultCode.AlreadyInitialized);
        }

        [TestMethod()]
        public void StartTest()
        {
            var fClerk = new FileClerk();
            var result = fClerk.Start();
            Assert.AreEqual(result.Code, ResultCode.Success);
        }

        [TestMethod()]
        public void StopTest()
        {
            var fClerk = new FileClerk();
            var result = fClerk.Stop();
            Assert.AreEqual(result.Code, ResultCode.Success);
        }
        [TestMethod()]
        public void IDTest()
        {
            var fClerk = new FileClerk();
            Assert.AreEqual(fClerk.ID, "FileClerk");
        }
    }
}