using Diplomacy.Kit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PluginDiscovery;
using PluginDiscoveryTests;
using System;
using System.Collections.Generic;
using System.IO;
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
            Assert.AreNotEqual<int>(0, plugins.Count()); //Given the multiple MEF and Reflection discovery sample plugins in the current assembly, this will be at least one.
            var plugin = plugins.Where(i => i.ID == "SamplePlugin").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual("SamplePlugin", plugin.ID);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.NotInitialized, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsPathTest()
        {
            var pluginPath = PluginDiscoveryUtilities.GetPluginDirectoryPath();
            var plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(pluginPath);
            Assert.AreEqual<int>(2, plugins.Count());
            var fileClerk = plugins.Where(i => i.ID == "FileClerk").SingleOrDefault();
            Assert.IsNotNull(fileClerk);
            Assert.AreEqual(ResultCode.Success, fileClerk.Start().Code);
            Assert.AreEqual(ResultCode.Success, fileClerk.Stop().Code);
            var wordAmbassador = plugins.Where(i => i.ID == "WordCountAmbassador").SingleOrDefault();
            Assert.IsNotNull(wordAmbassador);
            Assert.AreEqual(ResultCode.Success, wordAmbassador.Start().Code);
            Assert.AreEqual(ResultCode.Success, wordAmbassador.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsClerksTest()
        {
            var pluginPath = PluginDiscoveryUtilities.GetPluginDirectoryPath();
            var plugins = ReflectionDiscovery.LoadPlugins<IClerk>(pluginPath);
            Assert.AreEqual<int>(1, plugins.Count());
            var fileClerk = plugins.Where(i => i.ID == "FileClerk").SingleOrDefault();
            Assert.IsNotNull(fileClerk);
            Assert.AreEqual(ResultCode.Success, fileClerk.Start().Code);
            Assert.AreEqual(ResultCode.Success, fileClerk.Stop().Code);
            Mock<IConsulRequest> cRequestContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, fileClerk.SetConsul(cRequestContainer.Object).Code);
            var wordAmbassador = plugins.Where(i => i.ID == "WordCountAmbassador").SingleOrDefault();
            Assert.IsNull(wordAmbassador);
        }
        [TestMethod()]
        public void LoadPluginsAmbassadorsTest()
        {
            var pluginPath = PluginDiscoveryUtilities.GetPluginDirectoryPath();
            var plugins = ReflectionDiscovery.LoadPlugins<IAmbassador>(pluginPath);
            Assert.AreEqual<int>(1, plugins.Count());
            var fileClerk = plugins.Where(i => i.ID == "FileClerk").SingleOrDefault();
            Assert.IsNull(fileClerk);
            var wordAmbassador = plugins.Where(i => i.ID == "WordCountAmbassador").SingleOrDefault();
            Assert.IsNotNull(wordAmbassador);
            Assert.AreEqual(ResultCode.Success, wordAmbassador.Start().Code);
            Assert.AreEqual(ResultCode.Success, wordAmbassador.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, wordAmbassador.Process(null).Code);
        }
    }
}