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

namespace Diplomacy.Tests
{
    [TestClass()]
    public class FrontDeskTests
    {
        [TestMethod()]
        public void FrontDeskTest()
        {
            var fDesk = new FrontDesk();
            int count = 0;
            foreach (var iter in fDesk.Paths)
            {
                count++;
            }
            Assert.AreEqual<int>(count, 0);
        }
        [TestMethod]
        public void FrontDeskArgsTest()
        {
            var lTest = new List<string>() { "test", "test2", "test3" };
            var fDesk = new FrontDesk(lTest);
            Assert.IsTrue(lTest.SequenceEqual(fDesk.Paths));
        }
        [TestMethod]
        public void FrontDeskGetCatalogsTest()
        {
            var lPaths = new List<string>() { "MyDir", "MyDir2", "MyFile.dll" };
            var fDesk = new FrontDesk(lPaths);
            AssemblyCatalog defaultCatalog = new AssemblyCatalog(fDesk.GetType().Assembly);
            PrivateObject pObj = new PrivateObject(fDesk);
            IEnumerable<ComposablePartCatalog> cats = pObj.Invoke("GetCatalogs") as IEnumerable<ComposablePartCatalog>;
            Assert.IsTrue(cats.Count() == 1);
            var first = cats.FirstOrDefault();
            Assert.IsInstanceOfType(first, typeof(AssemblyCatalog));
            AssemblyCatalog fInst = first as AssemblyCatalog;
            Assert.AreEqual<Assembly>(fInst.Assembly, defaultCatalog.Assembly);
        }
        [TestMethod]
        public void FrontDeskGetCatalogsValidTest()
        {
            string path1 = Environment.CurrentDirectory;
            string path2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var lPaths = new List<string>() { path1, path2, "MyFile.dll" };
            var fDesk = new FrontDesk(lPaths);
            AssemblyCatalog defaultCatalog = new AssemblyCatalog(fDesk.GetType().Assembly);
            PrivateObject pObj = new PrivateObject(fDesk);
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
        public void FrontDeskLoadClerksTest()
        {
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            IEnumerable<Lazy<IClerk, IClerkData>> clerks = pObj.Invoke("LoadClerks") as IEnumerable<Lazy<IClerk, IClerkData>>;
            Assert.AreEqual(clerks.Count(), 0);
        }
        [TestMethod]
        public void FrontDeskValidateClerkTest()
        {
            var fDesk = new FrontDesk();
            Mock<IClerkData> clerkData = new Mock<IClerkData>();
            clerkData.SetupGet<string>(f => f.ID).Returns("TEST");
            clerkData.SetupGet<string>(f => f.ProviderID).Returns("TEST");
            PrivateObject pObj = new PrivateObject(fDesk);
            var res = pObj.Invoke("ValidateClerk", clerkData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
        [TestMethod]
        public void ValidateClerkInvalidIDTest()
        {
            var fDesk = new FrontDesk();
            Mock<IClerkData> clerkData = new Mock<IClerkData>();
            clerkData.SetupGet<string>(f => f.ID).Returns("");
            clerkData.SetupGet<string>(f => f.ProviderID).Returns("TEST");
            PrivateObject pObj = new PrivateObject(fDesk);
            var res = pObj.Invoke("ValidateClerk", clerkData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.InvalidPluginID);
        }
        [TestMethod]
        public void ValidateClerkInvalidProviderTest()
        {
            var fDesk = new FrontDesk();
            Mock<IClerkData> clerkData = new Mock<IClerkData>();
            clerkData.SetupGet<string>(f => f.ID).Returns("TEST");
            clerkData.SetupGet<string>(f => f.ProviderID).Returns("");
            PrivateObject pObj = new PrivateObject(fDesk);
            var res = pObj.Invoke("ValidateClerk", clerkData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.InvalidPluginProvider);
        }
        [TestMethod()]
        public void ValidateClerkDuplicateIDTest()
        {
            var fDesk = new FrontDesk();
            Mock<IClerkData> clerkData = new Mock<IClerkData>();
            clerkData.SetupGet<string>(f => f.ID).Returns("TEST");
            clerkData.SetupGet<string>(f => f.ProviderID).Returns("TEST");
            Mock<IClerk> clerk = new Mock<IClerk>();
            ConcurrentDictionary<string, IClerk> clerks = new ConcurrentDictionary<string, IClerk>();
            clerks.TryAdd("TEST", clerk.Object);
            PrivateObject pObj = new PrivateObject(fDesk);
            pObj.SetField("_Clerks", clerks);
            var res = pObj.Invoke("ValidateClerk", clerkData.Object) as Result;
            Assert.AreEqual(res.Code, ResultCode.InvalidPluginDuplicateID);
        }
        [TestMethod()]
        public void InitializeAlreadyInitializedTest()
        {
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            pObj.SetField("isInitialized", true);
            var res = fDesk.Initialize();
            Assert.AreEqual(res.Code, ResultCode.AlreadyInitialized);
        }
        [TestMethod]
        public void ValidateProviderEmptyTest()
        {
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            var res = pObj.Invoke("ValidateProvider", "");
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void ValidateProviderTest()
        {
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            var res = pObj.Invoke("ValidateProvider", "test");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void GetClerkTest()
        {
            string cID = "myClerk";
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            Mock<IClerk> clerk = new Mock<IClerk>();
            clerk.SetupGet<string>(f => f.ID).Returns(cID);
            ConcurrentDictionary<string, IClerk> clerks = new ConcurrentDictionary<string, IClerk>();
            clerks.TryAdd(cID, clerk.Object);
            pObj.SetField("_Clerks", clerks);
            var value = fDesk.GetClerk(cID);
            Assert.IsNotNull(value);
            Assert.AreEqual(cID, value.ID);
        }
        [TestMethod]
        public void GetClerkBadTest()
        {
            string cID = "myClerk";
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            Mock<IClerk> clerk = new Mock<IClerk>();
            clerk.SetupGet<string>(f => f.ID).Returns(cID);
            ConcurrentDictionary<string, IClerk> clerks = new ConcurrentDictionary<string, IClerk>();
            clerks.TryAdd(cID, clerk.Object);
            pObj.SetField("_Clerks", clerks);
            var value = fDesk.GetClerk(cID + "invalid");
            Assert.IsNull(value);
        }

        [TestMethod()]
        public void StartClerksTest()
        {
            string cID = "myClerk";
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            Mock<IClerk> clerk = new Mock<IClerk>();
            clerk.SetupGet<string>(f => f.ID).Returns(cID);
            clerk.Setup(f => f.Start()).Returns(new Result());
            ConcurrentDictionary<string, IClerk> clerks = new ConcurrentDictionary<string, IClerk>();
            clerks.TryAdd(cID, clerk.Object);
            pObj.SetField("_Clerks", clerks);
            var res = fDesk.Initialize();
            Assert.IsTrue(res.Code == ResultCode.Success);
            res = fDesk.StartClerks();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
        [TestMethod()]
        public void StartClerksNoClerksTest()
        {
            var fDesk = new FrontDesk();
            var res = fDesk.Initialize();
            Assert.IsTrue(res.Code == ResultCode.Success);
            res = fDesk.StartClerks();
            Assert.AreEqual(res.Code, ResultCode.NoPluginsLoaded);
        }
        [TestMethod]
        public void StartClerksNotInitializedTest()
        {
            var fDesk = new FrontDesk();
            var res = fDesk.StartClerks();
            Assert.AreEqual(res.Code, ResultCode.NotInitialized);
        }
        [TestMethod]
        public void StopClerksNotInitializedTest()
        {
            var fDesk = new FrontDesk();
            var res = fDesk.StopClerks();
            Assert.AreEqual(res.Code, ResultCode.NotInitialized);
        }
        [TestMethod()]
        public void StopClerksNoClerksTest()
        {
            var fDesk = new FrontDesk();
            var res = fDesk.Initialize();
            Assert.IsTrue(res.Code == ResultCode.Success);
            res = fDesk.StopClerks();
            Assert.AreEqual(res.Code, ResultCode.NoPluginsLoaded);
        }
        [TestMethod()]
        public void StopClerksTest()
        {
            string cID = "myClerk";
            var fDesk = new FrontDesk();
            PrivateObject pObj = new PrivateObject(fDesk);
            Mock<IClerk> clerk = new Mock<IClerk>();
            clerk.SetupGet<string>(f => f.ID).Returns(cID);
            clerk.Setup(f => f.Start()).Returns(new Result());
            ConcurrentDictionary<string, IClerk> clerks = new ConcurrentDictionary<string, IClerk>();
            clerks.TryAdd(cID, clerk.Object);
            pObj.SetField("_Clerks", clerks);
            var res = fDesk.Initialize();
            Assert.IsTrue(res.Code == ResultCode.Success);
            res = fDesk.StopClerks();
            Assert.AreEqual(res.Code, ResultCode.Success);
        }
    }
}