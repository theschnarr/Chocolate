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
    public class ResultCodeTests
    {
        [TestMethod()]
        public void ResultCodeSuccessTest()
        {
            Assert.AreEqual<string>(ResultCode.Success.DisplayName, "Success");
            Assert.AreEqual<int>(ResultCode.Success.Value, 0);
        }
        [TestMethod]
        public void ResultCodeInvalidArgumentTest()
        {
            Assert.AreEqual<string>(ResultCode.InvalidArgument.DisplayName, "Invalid argument specified.");
            Assert.AreEqual<int>(ResultCode.InvalidArgument.Value, -100);
        }
        [TestMethod]
        public void ResultCodeNotInitializedTest()
        {
            Assert.AreEqual(ResultCode.NotInitialized.DisplayName, "Class has not been initialized.");
            Assert.AreEqual(ResultCode.NotInitialized.Value, -400);
        }
        [TestMethod]
        public void ResultCodeAlreadyInitializedTest()
        {
            Assert.AreEqual(ResultCode.AlreadyInitialized.DisplayName, "Class has already been initialized.");
            Assert.AreEqual(ResultCode.AlreadyInitialized.Value, -401);
        }
        [TestMethod]
        public void ResultCodeInvalidPluginIDTest()
        {
            Assert.AreEqual(ResultCode.InvalidPluginID.DisplayName, "Plugin ID is an invalid format.");
            Assert.AreEqual(ResultCode.InvalidPluginID.Value, -410);
        }
        [TestMethod]
        public void ResultCodeInvalidPluginDuplicateIDTest()
        {
            Assert.AreEqual(ResultCode.InvalidPluginDuplicateID.DisplayName, "Plugin ID is already registerred.");
            Assert.AreEqual(ResultCode.InvalidPluginDuplicateID.Value, -411);
        }
        [TestMethod]
        public void ResultCodeInvalidPluginProviderTest()
        {
            Assert.AreEqual(ResultCode.InvalidPluginProvider.DisplayName, "Plugin Provider is not a viable ID.");
            Assert.AreEqual(ResultCode.InvalidPluginProvider.Value, -412);
        }
        [TestMethod]
        public void ResultCodeInvalidPluginAssemblyTest()
        {
            Assert.AreEqual(ResultCode.InvalidPluginAssembly.DisplayName, "Plugin assembly is not valid for the Provider.");
            Assert.AreEqual(ResultCode.InvalidPluginAssembly.Value, -413);
        }
    }
}