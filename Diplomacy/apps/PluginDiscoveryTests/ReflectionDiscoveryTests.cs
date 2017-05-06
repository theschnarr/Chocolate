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
        private string SamplePluginPath;
        private PluginTestUtility util;
        private string SamplePluginSource = @"
        using Diplomacy.Kit;
        using System;
        using System.Collections.Generic;
        namespace PluginDiscovery.Tests
        {
            public class SamplePlugin : IPlugin
            {
                public string ID
                {
                    get
                    {
                        return ""SamplePlugin"";
                    }
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
            public class SampleClerk : IClerk
            {
                private IConsulRequest _ConsulRequest;
                public string ID
                {
                    get
                    {
                        return ""SampleClerk"";
                    }
                }
                public Result SetConsul(IConsulRequest consul)
                {
                    if (consul == null)
                    {
                        return new Result(ResultCode.InvalidArgument);
                    }
                    _ConsulRequest = consul;
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
            public class SampleAmbassador : IAmbassador
            {
                private List<string> _SupportedActions = new List<string>() { ""SampleAction"" };
                public string ID
                {
                    get
                    {
                        return ""SampleAmbassador"";
                    }
                }
                public IEnumerable<string> SupportedActions
                {
                    get
                    {
                        return _SupportedActions;
                    }
                }
                public Result Process(string requestData)
                {
                    if (String.IsNullOrWhiteSpace(requestData))
                    {
                        return new Result(ResultCode.InvalidArgument);
                    }
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
        }";
        public ReflectionDiscoveryTests()
        {
            util = new PluginTestUtility();
            /*if (Directory.Exists(util.PluginTempDirectory))
            {
                //We need to make sure that any dlls from before this run of test(s) are removed.
                Directory.Delete(util.PluginTempDirectory, true);
            }*/
            SamplePluginPath = Path.GetDirectoryName(util.GeneratePluginDll(SamplePluginSource, "SamplePlugin.dll"));

        }
        [TestCleanup]
        public void TestCleanup()
        {
            string path = Path.Combine(SamplePluginPath,"SamplePlugin.dll");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        [TestMethod]
        public void GeneratePluginTest()
        {
            var pAssembly = util.GeneratePluginAssembly(SamplePluginSource);
        }
        [TestMethod]
        public void GeneratePluginDllTest()
        {
            string path = util.GeneratePluginDll(SamplePluginSource, "SamplePlugin.dll");
            Assert.AreEqual(Path.Combine(util.PluginTempDirectory, "SamplePlugin.dll"), path);
            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
            Assert.IsFalse(File.Exists(path));
        }
        [TestMethod()]
        public void LoadPluginsTest()
        {
            var plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(util.GeneratePluginAssembly(SamplePluginSource));
            Assert.AreEqual<int>(3, plugins.Count()); //The source string specifies 3 plugins.
            var plugin = plugins.Where(i => i.ID == "SamplePlugin").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual("SamplePlugin", plugin.ID);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);

        }
        [TestMethod()]
        public void LoadPluginsPathTest()
        {
            var plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(SamplePluginPath);
            Assert.IsTrue(3 <= plugins.Count());
            var plugin = plugins.Where(i => i.ID == "SamplePlugin").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsClerksPathTest()
        {
            var pluginPath = util.GeneratePluginDll(SamplePluginSource, "SamplePlugin.dll");
            var plugins = ReflectionDiscovery.LoadPlugins<IClerk>(Path.GetDirectoryName(pluginPath));
            Assert.AreNotEqual<int>(0, plugins.Count());
            var fileClerk = plugins.Where(i => i.ID == "SampleClerk").SingleOrDefault();
            Assert.IsNotNull(fileClerk);
            Assert.AreEqual(ResultCode.Success, fileClerk.Start().Code);
            Assert.AreEqual(ResultCode.Success, fileClerk.Stop().Code);
            Mock<IConsulRequest> cRequestContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, fileClerk.SetConsul(cRequestContainer.Object).Code);
        }
        [TestMethod()]
        public void LoadPluginsClerksTest()
        {
            var pluginAssembly = util.GeneratePluginAssembly(SamplePluginSource);
            var plugins = ReflectionDiscovery.LoadPlugins<IClerk>(pluginAssembly);
            Assert.AreNotEqual<int>(0, plugins.Count());
            var fileClerk = plugins.Where(i => i.ID == "SampleClerk").SingleOrDefault();
            Assert.IsNotNull(fileClerk);
            Assert.AreEqual(ResultCode.Success, fileClerk.Start().Code);
            Assert.AreEqual(ResultCode.Success, fileClerk.Stop().Code);
            Mock<IConsulRequest> cRequestContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, fileClerk.SetConsul(cRequestContainer.Object).Code);
        }
        [TestMethod()]
        public void LoadPluginsAmbassadorsPathTest()
        {
            var plugins = ReflectionDiscovery.LoadPlugins<IAmbassador>(SamplePluginPath);
            Assert.AreNotEqual<int>(0, plugins.Count());
            var ambassador = plugins.Where(i => i.ID == "SampleAmbassador").SingleOrDefault();
            Assert.IsNotNull(ambassador);
            Assert.AreEqual(ResultCode.Success, ambassador.Start().Code);
            Assert.AreEqual(ResultCode.Success, ambassador.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, ambassador.Process(null).Code);
        }
        [TestMethod()]
        public void LoadPluginsAmbassadorsTest()
        {
            var pluginAssembly = util.GeneratePluginAssembly(SamplePluginSource);
            var plugins = ReflectionDiscovery.LoadPlugins<IAmbassador>(pluginAssembly);
            Assert.AreEqual<int>(1, plugins.Count());
            var ambassador = plugins.Where(i => i.ID == "SampleAmbassador").SingleOrDefault();
            Assert.IsNotNull(ambassador);
            Assert.AreEqual(ResultCode.Success, ambassador.Start().Code);
            Assert.AreEqual(ResultCode.Success, ambassador.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, ambassador.Process(null).Code);
        }
    }
}