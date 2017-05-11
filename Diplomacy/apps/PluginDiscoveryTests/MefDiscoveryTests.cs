using PluginDiscovery;
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
        using Diplomacy.Kit.MEF;
        using System.Collections.Generic;
        using System.ComponentModel.Composition;
        namespace PluginDiscovery.Tests
        {
            [ExportPlugin(""MefSamplePlugin"",""MefDiscoveryTests"")]
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
            [ExportAmbassador(""MefSampleAmbassador"",""MefDiscoveryTests"")]
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
            [ExportClerk(""MefSampleClerk"",""MefDiscoveryTests"")]
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

        [TestMethod()]
        public void LoadLazyPluginsTest()
        {
            var plugins = MefDiscovery.LoadLazyPlugins<IPlugin>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.Value.ID == "MefSamplePlugin").Select(i => i.Value).SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadLazyPluginsAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadLazyPlugins<IAmbassador>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.Value.ID == "MefSampleAmbassador").Select(i => i.Value).SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "Magic" }, plugin.SupportedActions.ToList());
        }
        [TestMethod()]
        public void LoadLazyPluginsClerkTest()
        {
            var plugins = MefDiscovery.LoadLazyPlugins<IClerk>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.Value.ID == "MefSampleClerk").Select(i => i.Value).SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }
        [TestMethod]
        public void LoadLazyPluginsDirectoryTest()
        {
            var plugins = MefDiscovery.LoadLazyPlugins<IPlugin>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.Value.ID == "MefSamplePlugin").Select(i => i.Value).SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod]
        public void LoadLazyPluginsDirectoryAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadLazyPlugins<IAmbassador>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.Value.ID == "MefSampleAmbassador").Select(i => i.Value).SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "Magic" }, plugin.SupportedActions.ToList());
        }
        [TestMethod]
        public void LoadLazyPluginsDirectoryClerkTest()
        {
            var plugins = MefDiscovery.LoadLazyPlugins<IClerk>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.Value.ID == "MefSampleClerk").Select(i => i.Value).SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }
        [TestMethod()]
        public void LoadPluginsWithMetadataTest()
        {
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IPlugin, IPluginData>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var lazyPlugin = plugins.Where(i => i.Metadata.ID == "MefSamplePlugin").SingleOrDefault();
            Assert.IsNotNull(lazyPlugin);
            Assert.IsFalse(lazyPlugin.IsValueCreated);
            Assert.AreEqual("MefDiscoveryTests", lazyPlugin.Metadata.ProviderID);
            var plugin = lazyPlugin.Value;
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsWithMetadataAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IAmbassador, IPluginData>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var lazyPlugin = plugins.Where(i => i.Metadata.ID == "MefSampleAmbassador").SingleOrDefault();
            Assert.IsNotNull(lazyPlugin);
            Assert.IsFalse(lazyPlugin.IsValueCreated);
            Assert.AreEqual("MefDiscoveryTests", lazyPlugin.Metadata.ProviderID);
            var plugin = lazyPlugin.Value;
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "Magic" }, plugin.SupportedActions.ToList());
        }
        [TestMethod()]
        public void LoadPluginsWithMetadataClerkTest()
        {
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IClerk,IPluginData>(util.GeneratePluginAssembly(MefPluginSource));
            Assert.AreEqual(1, plugins.Count());
            var lazyPlugin = plugins.Where(i => i.Metadata.ID == "MefSampleClerk").SingleOrDefault();
            Assert.IsNotNull(lazyPlugin);
            Assert.IsFalse(lazyPlugin.IsValueCreated);
            Assert.AreEqual("MefDiscoveryTests", lazyPlugin.Metadata.ProviderID);
            var plugin = lazyPlugin.Value;
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }
        [TestMethod()]
        public void LoadPluginsWithMetadataDirectoryTest()
        {
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IPlugin, IPluginData>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var lazyPlugin = plugins.Where(i => i.Metadata.ID == "MefSamplePlugin").SingleOrDefault();
            Assert.IsNotNull(lazyPlugin);
            Assert.IsFalse(lazyPlugin.IsValueCreated);
            Assert.AreEqual("MefDiscoveryTests", lazyPlugin.Metadata.ProviderID);
            var plugin = lazyPlugin.Value;
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsWithMetadataDirectoryAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IAmbassador, IPluginData>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var lazyPlugin = plugins.Where(i => i.Metadata.ID == "MefSampleAmbassador").SingleOrDefault();
            Assert.IsNotNull(lazyPlugin);
            Assert.IsFalse(lazyPlugin.IsValueCreated);
            Assert.AreEqual("MefDiscoveryTests", lazyPlugin.Metadata.ProviderID);
            var plugin = lazyPlugin.Value;
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "Magic" }, plugin.SupportedActions.ToList());
        }
        [TestMethod()]
        public void LoadPluginsWithMetadataDirectoryClerkTest()
        {
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IClerk, IPluginData>(MefPluginPath);
            Assert.AreEqual(1, plugins.Count());
            var lazyPlugin = plugins.Where(i => i.Metadata.ID == "MefSampleClerk").SingleOrDefault();
            Assert.IsNotNull(lazyPlugin);
            Assert.IsFalse(lazyPlugin.IsValueCreated);
            Assert.AreEqual("MefDiscoveryTests", lazyPlugin.Metadata.ProviderID);
            var plugin = lazyPlugin.Value;
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }
    }
}