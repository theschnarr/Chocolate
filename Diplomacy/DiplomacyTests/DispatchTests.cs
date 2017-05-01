using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diplomacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Diplomacy.Kit;
using System.Collections.Concurrent;
using System.Reflection;
using Diplomacy.Kit.MEF;

namespace Diplomacy.Tests
{
    [TestClass()]
    public class DispatchTests
    {
        [ExportAmbassador("SampleAmbassador", "UnitTests")]
        public class SampleAmbassador : IAmbassador
        {
            private List<string> actions = new List<string>() { "Mail", "WordCount" };
            public string ID
            {
                get
                {
                    return "SampleAmbassador";
                }
            }

            public IEnumerable<string> SupportedActions
            {
                get
                {
                    return actions;
                }
            }

            public Result Process(string requestData)
            {
                return new Result();
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
        [TestMethod()]
        public void DispatchTest()
        {
            var dispatch = new Dispatch();
            PrivateObject pObj = new PrivateObject(dispatch);
            var plugins = pObj.GetField("_Handler") as PluginHandler<IAmbassador, IPluginData>;
            PrivateObject pObj2 = new PrivateObject(plugins);
            var paths = pObj2.GetField("_Paths") as List<String>;
            Assert.AreEqual<int>(paths.Count, 0);
        }
        [TestMethod]
        public void DispatchArgsTest()
        {
            var lTest = new List<string>() { "test", "test2", "test3" };
            var dispatch = new Dispatch(lTest);
            PrivateObject pObj = new PrivateObject(dispatch);
            var plugins = pObj.GetField("_Handler") as PluginHandler<IAmbassador, IPluginData>;
            PrivateObject pObj2 = new PrivateObject(plugins);
            var paths = pObj2.GetField("_Paths") as List<String>;
            Assert.IsTrue(lTest.SequenceEqual(paths));
        }

        [TestMethod]
        public void GetAmbassadorTest()
        {
            var dispatch = new Dispatch(assemblies: new List<Assembly>() { typeof(SamplePlugin).Assembly });
            var value = dispatch.GetAmbassador("SampleAmbassador");
            Assert.IsNotNull(value);
            Assert.AreEqual("SampleAmbassador", value.ID);
        }
        [TestMethod]
        public void GetAmbassadorBadTest()
        {
            string cID = "myAmbassador";
            var dispatch = new Dispatch();
            var value = dispatch.GetAmbassador(cID + "invalid");
            Assert.IsNull(value);
        }

        [TestMethod()]
        public void StartAmbassadorsTest()
        {
            var dispatch = new Dispatch(assemblies: new List<Assembly>() { typeof(SamplePlugin).Assembly });
            var res = dispatch.StartAmbassadors();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
        [TestMethod()]
        public void StartAmbassadorsNoClerksTest()
        {
            var dispatch = new Dispatch();
            var res = dispatch.StartAmbassadors();
            Assert.AreEqual(res.Code, ResultCode.NoPluginsLoaded);
        }
        [TestMethod()]
        public void StopAmbassadorsNoClerksTest()
        {
            var dispatch = new Dispatch();
            var res = dispatch.StopAmbassadors();
            Assert.AreEqual(res.Code, ResultCode.NoPluginsLoaded);
        }
        [TestMethod()]
        public void StopAmbassadorsTest()
        {
            var dispatch = new Dispatch(assemblies: new List<Assembly>() { typeof(SamplePlugin).Assembly });
            var res = dispatch.StopAmbassadors();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
    }
}