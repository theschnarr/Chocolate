using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diplomacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Diplomacy.Kit;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Concurrent;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using Diplomacy.Kit.MEF;

namespace Diplomacy.Tests
{
    [ExportPlugin("SamplePlugin", "UnitTest")]
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
            return new Result();
        }
    }
    [TestClass()]
    public class PluginHandlerTests
    {
        private PluginHandler<IPlugin, IPluginData> GetHandler(IEnumerable<string> paths = null, IEnumerable<Assembly> assemblies = null)
        {
            return new PluginHandler<IPlugin, IPluginData>(paths, assemblies);
        }
        [TestMethod]
        public void PluginHandlerTest()
        {
            var pHandler = GetHandler();
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Paths") as List<String>;
            Assert.AreEqual<int>(paths.Count, 0);
        }
        [TestMethod]
        public void PluginHandlerArgsTest()
        {
            var lTest = new List<string>() { "test", "test2", "test3" };
            var pHandler = GetHandler(lTest);
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Paths") as List<String>;
            Assert.IsTrue(lTest.SequenceEqual(paths));
        }
        [TestMethod]
        public void PluginHandlerAssembliesTest()
        {
            var asm = new List<Assembly>() { typeof(SamplePlugin).Assembly };
            var pHandler = GetHandler(assemblies: asm);
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Assemblies") as List<Assembly>;
            Assert.IsTrue(asm.SequenceEqual(paths));
        }
        [TestMethod]
        public void PluginHandlerGetCatalogsTest()
        {
            var lPaths = new List<string>() { "MyDir", "MyDir2", "MyFile.dll" };
            var pHandler = GetHandler(lPaths);
            AssemblyCatalog defaultCatalog = new AssemblyCatalog(typeof(PluginHandler<IPlugin, IPluginData>).Assembly);
            PrivateObject pObj = new PrivateObject(pHandler);
            IEnumerable<ComposablePartCatalog> cats = pObj.Invoke("GetCatalogs") as IEnumerable<ComposablePartCatalog>;
            Assert.IsTrue(cats.Count() == 1);
            var first = cats.FirstOrDefault();
            Assert.IsInstanceOfType(first, typeof(AssemblyCatalog));
            AssemblyCatalog fInst = first as AssemblyCatalog;
            Assert.AreEqual<Assembly>(fInst.Assembly, defaultCatalog.Assembly);
        }
        [TestMethod]
        public void PluginHandlerGetCatalogsAssembliesTest()
        {
            var lPaths = new List<string>() { "MyDir", "MyDir2", "MyFile.dll" };
            var pHandler = GetHandler(lPaths, new List<Assembly>() { typeof(SamplePlugin).Assembly });
            AssemblyCatalog defaultCatalog = new AssemblyCatalog(typeof(PluginHandler<IPlugin, IPluginData>).Assembly);
            AssemblyCatalog expCatalog = new AssemblyCatalog(typeof(SamplePlugin).Assembly);
            PrivateObject pObj = new PrivateObject(pHandler);
            IEnumerable<ComposablePartCatalog> cats = pObj.Invoke("GetCatalogs") as IEnumerable<ComposablePartCatalog>;
            Assert.IsTrue(cats.Count() == 2);
            IEnumerable<AssemblyCatalog> expAssemblies = from a in cats
                                                         where a.GetType() == typeof(AssemblyCatalog) && ((AssemblyCatalog)a).Assembly == expCatalog.Assembly
                                                         select a as AssemblyCatalog;
            Assert.AreEqual(1, expAssemblies.Count());
        }
        [TestMethod]
        public void PluginHandlerGetCatalogsValidTest()
        {
            string path1 = Environment.CurrentDirectory;
            string path2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var lPaths = new List<string>() { path1, path2, "MyFile.dll" };
            var pHandler = GetHandler(lPaths);
            AssemblyCatalog defaultCatalog = new AssemblyCatalog(typeof(PluginHandler<IPlugin, IPluginData>).Assembly);
            PrivateObject pObj = new PrivateObject(pHandler);
            IEnumerable<ComposablePartCatalog> cats = pObj.Invoke("GetCatalogs") as IEnumerable<ComposablePartCatalog>;
            Assert.IsTrue(cats.Count() == 3);
            var first = cats.FirstOrDefault();
            Assert.IsInstanceOfType(first, typeof(AssemblyCatalog));
            AssemblyCatalog fInst = first as AssemblyCatalog;
            Assert.AreEqual<Assembly>(fInst.Assembly, defaultCatalog.Assembly);
            var second = cats.ElementAt(1);
            Assert.IsInstanceOfType(second, typeof(DirectoryCatalog));
            var sInst = second as DirectoryCatalog;
            Assert.AreEqual<string>(path1.ToLower(), sInst.FullPath.ToLower());
            var third = cats.ElementAt(2);
            Assert.IsInstanceOfType(third, typeof(DirectoryCatalog));
            var tInst = third as DirectoryCatalog;
            Assert.AreEqual<string>(path2.ToLower(), tInst.FullPath.ToLower());
        }
        [TestMethod]
        public void PluginHandlerLoadPluginsTest()
        {
            var pHandler = GetHandler();
            PrivateObject pObj = new PrivateObject(pHandler);
            IEnumerable<Lazy<IPlugin, IPluginData>> plugins = pObj.Invoke("GetCatalogExports") as IEnumerable<Lazy<IPlugin, IPluginData>>;
            Assert.AreEqual(plugins.Count(), 0);
        }
        [TestMethod]
        public void PluginHandlerValidatePluginTest()
        {
            var pHandler = GetHandler();
            Mock<IPluginData> pluginData = new Mock<IPluginData>();
            pluginData.SetupGet<string>(f => f.ID).Returns("TEST");
            pluginData.SetupGet<string>(f => f.ProviderID).Returns("TEST");
            PrivateObject pObj = new PrivateObject(pHandler);
            var res = pObj.Invoke("ValidatePlugin", pluginData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
        [TestMethod]
        public void PluginHandlerValidatePluginInvalidIDTest()
        {
            var pHandler = GetHandler();
            Mock<IPluginData> pluginData = new Mock<IPluginData>();
            pluginData.SetupGet<string>(f => f.ID).Returns("");
            pluginData.SetupGet<string>(f => f.ProviderID).Returns("TEST");
            PrivateObject pObj = new PrivateObject(pHandler);
            var res = pObj.Invoke("ValidatePlugin", pluginData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.InvalidPluginID);
        }
        [TestMethod]
        public void PluginHandlerValidatePluginInvalidProviderTest()
        {
            var pHandler = GetHandler();
            Mock<IPluginData> pluginData = new Mock<IPluginData>();
            pluginData.SetupGet<string>(f => f.ID).Returns("TEST");
            pluginData.SetupGet<string>(f => f.ProviderID).Returns("");
            PrivateObject pObj = new PrivateObject(pHandler);
            var res = pObj.Invoke("ValidatePlugin", pluginData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.InvalidPluginProvider);
        }
        [TestMethod()]
        public void PluginHandlerValidatePluginDuplicateIDTest()
        {
            var pHandler = GetHandler();
            Mock<IPluginData> pluginData = new Mock<IPluginData>();
            pluginData.SetupGet<string>(f => f.ID).Returns("TEST");
            pluginData.SetupGet<string>(f => f.ProviderID).Returns("TEST");
            Mock<IPlugin> plugin = new Mock<IPlugin>();
            ConcurrentDictionary<string, IPlugin> plugins = new ConcurrentDictionary<string, IPlugin>();
            plugins.TryAdd("TEST", plugin.Object);
            PrivateObject pObj = new PrivateObject(pHandler);
            pObj.SetField("_Plugins", plugins);
            var res = pObj.Invoke("ValidatePlugin", pluginData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.InvalidPluginDuplicateID);
        }
        [TestMethod]
        public void PluginHandlerValidateProviderEmptyTest()
        {
            var pHandler = GetHandler();
            PrivateObject pObj = new PrivateObject(pHandler);
            var res = pObj.Invoke("ValidateProvider", "");
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void PluginHandlerValidateProviderTest()
        {
            var pHandler = GetHandler();
            PrivateObject pObj = new PrivateObject(pHandler);
            var res = pObj.Invoke("ValidateProvider", "test");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void PluginHandlerGetPluginTest()
        {
            var pHandler = GetHandler(assemblies: new List<Assembly>() { typeof(SamplePlugin).Assembly });
            var res = pHandler.LoadPlugins();
            Assert.AreEqual(res.Code, ResultCode.Success);
            var plugin = pHandler.GetPlugin("SamplePlugin");
            Assert.IsNotNull(plugin);
            Assert.AreEqual(plugin.ID, "SamplePlugin");
        }

        [TestMethod()]
        public void AddPathsTest()
        {
            var pHandler = GetHandler();
            pHandler.AddPaths();
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Paths") as List<String>;
            Assert.AreEqual<int>(paths.Count, 0);
        }
        [TestMethod()]
        public void AddPathsValuesTest()
        {
            var pHandler = GetHandler();
            pHandler.AddPaths("Test", "Test2", "Test3");
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Paths") as List<String>;
            Assert.AreEqual<int>(paths.Count, 3);
        }
        [TestMethod()]
        public void AddAssembliesTest()
        {
            var pHandler = GetHandler();
            pHandler.AddAssemblies();
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Assemblies") as List<Assembly>;
            Assert.AreEqual<int>(paths.Count, 0);
        }
        [TestMethod()]
        public void AddAssembliesValuesTest()
        {
            var pHandler = GetHandler();
            pHandler.AddAssemblies(typeof(SamplePlugin).Assembly);
            PrivateObject pObj = new PrivateObject(pHandler);
            var paths = pObj.GetField("_Assemblies") as List<Assembly>;
            Assert.AreEqual<int>(paths.Count, 1);
        }
    }
}