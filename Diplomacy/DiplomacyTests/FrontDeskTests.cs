using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diplomacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Diplomacy.Kit;
using Moq;
using System.Collections.Concurrent;
using Diplomacy.Kit.MEF;

namespace Diplomacy.Tests
{
    [ExportClerk("SampleClerk", "UnitTest")]
    public class SampleClerk : IClerk
    {
        public string ID
        {
            get
            {
                return "SampleClerk";
            }
        }

        public Result SetConsul(IConsulRequest consul)
        {
            throw new NotImplementedException();
        }

        public Result Start()
        {
            return new Result();
        }

        public Result Stop()
        {
            return new Result();
        }
    }
    [TestClass()]
    public class FrontDeskTests
    {
        [TestMethod()]
        public void FrontDeskTest()
        {
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            var plugins = pObj.GetField("_Handler") as PluginHandler<IClerk, IPluginData>;
            PrivateObject pObj2 = new PrivateObject(plugins);
            var paths = pObj2.GetField("_Paths") as List<String>;
            Assert.AreEqual(paths.Count, 0);
        }
        [TestMethod]
        public void FrontDeskArgsTest()
        {
            var lTest = new List<string>() { "test", "test2", "test3" };
            var fDesk = new FrontDesk(lTest);
            PrivateObject pObj = new PrivateObject(fDesk);
            var plugins = pObj.GetField("_Handler") as PluginHandler<IClerk, IPluginData>;
            PrivateObject pObj2 = new PrivateObject(plugins);
            var paths = pObj2.GetField("_Paths") as List<String>;
            Assert.IsTrue(lTest.SequenceEqual(paths));
        }
        [TestMethod]
        public void GetClerkTest()
        {
            var fDesk = new FrontDesk(assemblies: new List<Assembly>() { typeof(SampleClerk).Assembly });
            var value = fDesk.GetClerk("SampleClerk");
            Assert.IsNotNull(value);
            Assert.AreEqual("SampleClerk", value.ID);
        }
        [TestMethod]
        public void GetClerkBadTest()
        {
            var fDesk = new FrontDesk(assemblies: new List<Assembly>() { typeof(SampleClerk).Assembly });
            var value = fDesk.GetClerk("invalid");
            Assert.IsNull(value);
        }

        [TestMethod()]
        public void StartClerksTest()
        {
            var fDesk = new FrontDesk(assemblies: new List<Assembly>() { typeof(SampleClerk).Assembly });
            var res = fDesk.StartClerks();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
        [TestMethod()]
        public void StartClerksNoClerksTest()
        {
            var fDesk = new FrontDesk();
            var res = fDesk.StartClerks();
            Assert.AreEqual(res.Code, ResultCode.NoPluginsLoaded);
        }
        [TestMethod()]
        public void StopClerksNoClerksTest()
        {
            var fDesk = new FrontDesk();
            var res = fDesk.StopClerks();
            Assert.AreEqual(res.Code, ResultCode.NoPluginsLoaded);
        }
        [TestMethod()]
        public void StopClerksTest()
        {
            var fDesk = new FrontDesk(assemblies: new List<Assembly>() { typeof(SampleClerk).Assembly });
            var res = fDesk.StopClerks();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
    }
}