using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diplomacy.Kit.MEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit.MEF.Tests
{
    [TestClass()]
    public class ExportAmbassadorAttributeTests
    {
        [TestMethod()]
        public void ExportAmbassadorAttributeTest()
        {
            string id, providerId;
            id = DateTime.Now.ToString();
            providerId = DateTime.Now.ToString();
            var exportData = new ExportAmbassadorAttribute(id, providerId);
            Assert.AreEqual<string>(id, exportData.ID);
            Assert.AreEqual<string>(providerId, exportData.ProviderID);
        }
    }
}