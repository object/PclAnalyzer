using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using PclAnalyzer.Core;

namespace PclAnalyzer.Reflection.Tests
{
    [TestFixture]
    public class AssemblyParserTests
    {
        private AssemblyParser _assemblyParser;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _assemblyParser = new AssemblyParser(Assembly.GetExecutingAssembly().Location);
        }

        [Test]
        public void GetAssemblyCalls()
        {
            var calls = _assemblyParser.GetAssemblyCalls();
            Assert.IsNotEmpty(calls);
        }

        [Test]
        public void GetTypeCalls()
        {
            var calls = _assemblyParser.GetTypeCalls("PclAnalyzer.Reflection.Tests.ClassToParse");
            Assert.IsNotEmpty(calls);
        }

        [Test]
        public void GetMethodCalls_Equals()
        {
            var calls = _assemblyParser.GetMethodCalls("PclAnalyzer.Reflection.Tests.ClassToParse", "Equals");
            Assert.IsNotEmpty(calls);
            Assert.Contains(new Member("System.Object.Equals"), calls.Select(x => x.ReferencedMethod).ToList());
            Assert.Contains(new Member("System.String.Equals"), calls.Select(x => x.ReferencedMethod).ToList());
        }

        [Test]
        public void GetMethodCalls_AssemblyLocationOneStep()
        {
            var calls = _assemblyParser.GetMethodCalls("PclAnalyzer.Reflection.Tests.ClassToParse", "GetAssemblyLocationOneStep");
            Assert.IsNotEmpty(calls);
        }

        [Test]
        public void GetMethodCalls_AssemblyLocationTwoSteps()
        {
            var calls = _assemblyParser.GetMethodCalls("PclAnalyzer.Reflection.Tests.ClassToParse", "GetAssemblyLocationTwoSteps");
            Assert.IsNotEmpty(calls);
        }
    }
}
