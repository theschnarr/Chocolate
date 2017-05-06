using Diplomacy.Kit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PluginDiscoveryTests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PluginDiscovery.Tests
{
    [TestClass()]
    public class MefDiscoveryTests
    {
        private string MefPluginPath;
        private string MefPluginSource = @"
        using System;
        using Diplomacy.Kit;
        using System.Collections.Generic;
        using System.ComponentModel.Composition;
        namespace PluginDiscovery.Tests
        {
            [Export(typeof(IPlugin))]
            public class MefSamplePlugin : IPlugin
            {
                public string ID
                {
                    get
                    {
                        return ""MefSamplePlugin"";
                    }
                }

                public Result Start()
                {
                    return new Result(ResultCode.Success);
                }

                public Result Stop()
                {
                    return new Result(ResultCode.Success);
                }
            }
            [Export(typeof(IAmbassador))]
            public class MefSampleAmbassador : IAmbassador
            {
                private List<string> _SupportActions = new List<string>() { ""Magic"" };

                public string ID
                {
                    get
                    {
                        return ""MefSampleAmbassador"";
                    }
                }

                public IEnumerable<string> SupportedActions
                {
                    get
                    {
                        return _SupportActions;
                    }
                }

                public Result Process(string requestData)
                {
                    if (String.IsNullOrWhiteSpace(requestData))
                    {
                        return new Result(ResultCode.InvalidArgument);
                    }
                    return new Result(ResultCode.Success);
                }

                public Result Start()
                {
                    return new Result(ResultCode.Success);
                }

                public Result Stop()
                {
                    return new Result(ResultCode.Success);
                }
            }
            [Export(typeof(IClerk))]
            public class MefSampleClerk : IClerk
            {
                private IConsulRequest _ConsulRequest = null;
                public string ID
                {
                    get
                    {
                        return ""MefSampleClerk"";
                    }
                }

                public Result SetConsul(IConsulRequest consul)
                {
                    if (consul == null)
                        return new Result(ResultCode.InvalidArgument);
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
        }";
        private PluginTestUtility util = null;
        public MefDiscoveryTests()
        {
            util = new PluginTestUtility();
            if (!File.Exists(util.PluginTempDirectory + "MefPlugins.dll"))
            {
                MefPluginPath = Path.GetDirectoryName(util.GeneratePluginDll(MefPluginSource, "MefPlugins.dll"));
            }
            else
            {
                MefPluginPath = util.PluginTempDirectory;
            }
        }
        [AssemblyInitialize()]
        public static void AssemblyInitialize(TestContext context)
        {
            //Make sure we remove any previously compiled assemblies.
            string path = Environment.CurrentDirectory + "\\PluginsTemp\\";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
        [TestMethod()]
        public void LoadPluginsTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IPlugin>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSamplePlugin").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IAmbassador>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSampleAmbassador").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "Magic" }, plugin.SupportedActions.ToList());
        }
        [TestMethod()]
        public void LoadPluginsClerkTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IClerk>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSampleClerk").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }
        [TestMethod]
        public void LoadPluginsDirectoryTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IPlugin>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSamplePlugin").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod]
        public void LoadPluginsDirectoryAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IAmbassador>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSampleAmbassador").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "Magic" }, plugin.SupportedActions.ToList());
        }
        [TestMethod]
        public void LoadPluginsDirectoryClerkTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IClerk>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSampleClerk").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }

    }
}