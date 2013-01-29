using System.Collections.Generic;
using PclAnalyzer.Core;
using PclAnalyzer.Data;
using PclAnalyzer.Reflection;

namespace PclAnalyzer.UI.Services
{
    public class AnalyzerService
    {
        private readonly string _assemblyPath;
        private readonly Platforms _requestedPlatforms;
        private readonly PortabilityAnalyzer _portabilityAnalyzer;
        private readonly AssemblyParser _assemblyParser;

        public AnalyzerService(string assemblyPath, Platforms requestedPlatforms)
        {
            _assemblyPath = assemblyPath;
            _requestedPlatforms = requestedPlatforms;
            _portabilityAnalyzer = new PortabilityAnalyzer(PortabilityDatabase.Collection);
            _assemblyParser = new AssemblyParser(_assemblyPath);
            _portabilityAnalyzer.SupportedPlatforms = _requestedPlatforms;
            _portabilityAnalyzer.CallCollection = _assemblyParser.GetAssemblyCalls();
        }

        public IList<MethodCall> GetPortableCalls()
        {
            return _portabilityAnalyzer.GetPortableCalls();
        }

        public IList<MethodCall> GetNonPortableCalls()
        {
            return _portabilityAnalyzer.GetNonPortableCalls();
        }
    }
}