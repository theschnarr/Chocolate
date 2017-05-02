using Diplomacy.Kit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginDiscovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery.Tests
{
    [TestClass()]
    public class ReflectionDiscoveryTests
    {
        public class SamplePlugin : IPlugin
        {
            public string ID
            {
                get
                {
                    return "SamplePlugin";
                }
            }

            public Result Start()
            {
                return new Result();
            }

            public Result Stop()
            {
                return new Result(ResultCode.NotInitialized);
            }
        }
        [TestMethod()]
        public void LoadPluginsTest()
        {
            var plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(typeof(SamplePlugin).Assembly);
            Assert.AreEqual<int>(1, plugins.Count());
            Assert.AreEqual("SamplePlugin", plugins.First().ID);
            Assert.AreEqual(ResultCode.Success, plugins.First().Start().Code);
            Assert.AreEqual(ResultCode.NotInitialized, plugins.First().Stop().Code);
        }
    }
}