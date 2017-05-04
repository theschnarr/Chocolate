﻿using Diplomacy.Kit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PluginDiscovery;
using PluginDiscoveryTests;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery.Tests
{
    [TestClass()]
    public class MefDiscoveryTests
    {
        /// <summary>
        /// Demo Class used for unit tests of MefDiscovery methods.
        /// </summary>
        [Export(typeof(IPlugin))]
        public class MefSamplePlugin : IPlugin
        {
            public string ID
            {
                get
                {
                    return "MefSamplePlugin";
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
            private List<string> _SupportActions = new List<string>() { "Magic" };

            public string ID
            {
                get
                {
                    return "MefSampleAmbassador";
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
                    return "MefSampleClerk";
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
        [TestMethod()]
        public void LoadPluginsTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IPlugin>(typeof(MefSamplePlugin).Assembly);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "MefSamplePlugin").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
        }
        [TestMethod()]
        public void LoadPluginsAmbassadorTest()
        {
            var plugins = MefDiscovery.LoadPlugins<IAmbassador>(typeof(MefSamplePlugin).Assembly);
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
            var plugins = MefDiscovery.LoadPlugins<IClerk>(typeof(MefSamplePlugin).Assembly);
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
            string directoryPath = PluginDiscoveryUtilities.GetPluginDirectoryPath();
            var plugins = MefDiscovery.LoadPlugins<IPlugin>(directoryPath);
            Assert.AreEqual(0, plugins.Count());
        }
        [TestMethod]
        public void LoadPluginsDirectoryAmbassadorTest()
        {
            string directoryPath = PluginDiscoveryUtilities.GetPluginDirectoryPath();
            var plugins = MefDiscovery.LoadPlugins<IAmbassador>(directoryPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "WordCountAmbassador").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.Process("").Code);
            Assert.AreEqual(ResultCode.Success, plugin.Process("Amazing JSON Code here").Code);
            CollectionAssert.AreEquivalent(new List<String>() { "WordCount" }, plugin.SupportedActions.ToList());
        }
        [TestMethod]
        public void LoadPluginsDirectoryClerkTest()
        {
            string directoryPath = PluginDiscoveryUtilities.GetPluginDirectoryPath();
            var plugins = MefDiscovery.LoadPlugins<IClerk>(directoryPath);
            Assert.AreEqual(1, plugins.Count());
            var plugin = plugins.Where(i => i.ID == "FileClerk").SingleOrDefault();
            Assert.IsNotNull(plugin);
            Assert.AreEqual(ResultCode.Success, plugin.Start().Code);
            Assert.AreEqual(ResultCode.Success, plugin.Stop().Code);
            Assert.AreEqual(ResultCode.InvalidArgument, plugin.SetConsul(null).Code);
            Mock<IConsulRequest> mConsulContainer = new Mock<IConsulRequest>();
            Assert.AreEqual(ResultCode.Success, plugin.SetConsul(mConsulContainer.Object).Code);
        }

    }
}