using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PclAnalyzer.Data;
using PclAnalyzer.Reflection;

namespace PclAnalyzer.Core.Tests
{
    [TestFixture]
    public class PortabilityAnalyzerTests
    {
        private PortabilityAnalyzer _portabilityAnalyzer;
        private AssemblyParser _assemblyParser;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _portabilityAnalyzer = new PortabilityAnalyzer(PortabilityDatabase.Collection);
            _assemblyParser = new AssemblyParser(Assembly.GetExecutingAssembly().Location);
        }

        [Test]
        public void PortableClass()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.PortableClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.AllKnown;
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(calls.Count, portableCalls.Count);
        }

        [Test]
        public void XElementClass_AllPlatforms()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.XElementClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.AllKnown;
            var nonPortableCalls = _portabilityAnalyzer.GetNonPortableCalls();
            Assert.AreEqual(2, nonPortableCalls.Count);
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(calls.Count - nonPortableCalls.Count, portableCalls.Count);
        }

        [Test]
        public void XElementClass_NetPlatforms()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.XElementClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.Net403 | Platforms.Net45;
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(calls.Count, portableCalls.Count);
        }

        [Test]
        public void HttpWebRequestClass_AllPlatforms()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.HttpWebRequestClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.AllKnown;
            var nonPortableCalls = _portabilityAnalyzer.GetNonPortableCalls();
            Assert.AreEqual(6, nonPortableCalls.Count);
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(calls.Count - nonPortableCalls.Count, portableCalls.Count);
        }

        [Test]
        public void HttpWebRequestClass_Net45Platform()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.HttpWebRequestClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.Net45;
            var nonPortableCalls = _portabilityAnalyzer.GetNonPortableCalls();
            Assert.AreEqual(1, nonPortableCalls.Count);
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(calls.Count - nonPortableCalls.Count, portableCalls.Count);
        }

        [Test]
        public void GenericListClass()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.GenericListClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.AllKnown;
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(calls.Count, portableCalls.Count);
        }

        [Test]
        public void NUnitClass()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Core.Tests.NUnitClass");
            _portabilityAnalyzer.CallCollection = calls;

            _portabilityAnalyzer.SupportedPlatforms = Platforms.AllKnown;
            var portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(1, portableCalls.Count);

            _portabilityAnalyzer.ExcludeThirdPartyReferences = true;
            portableCalls = _portabilityAnalyzer.GetPortableCalls();
            Assert.AreEqual(0, portableCalls.Count);
            var nonPortableCalls = _portabilityAnalyzer.GetNonPortableCalls();
            Assert.AreEqual(0, nonPortableCalls.Count);
        }
    }
}
