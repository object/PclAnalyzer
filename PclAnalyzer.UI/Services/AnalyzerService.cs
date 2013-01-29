using System.Collections.Generic;
using System.Linq;
using PclAnalyzer.Core;
using PclAnalyzer.Data;
using PclAnalyzer.Reflection;
using PclAnalyzer.UI.ViewModel;

namespace PclAnalyzer.UI.Services
{
    public class AnalyzerService
    {
        private readonly string _assemblyPath;
        private readonly Platforms _requestedPlatforms;
        private readonly PortabilityAnalyzer _portabilityAnalyzer;
        private readonly AssemblyParser _assemblyParser;

        public AnalyzerService(string assemblyPath, Platforms requestedPlatforms, bool excludeThirdPartyReferences)
        {
            _assemblyPath = assemblyPath;
            _requestedPlatforms = requestedPlatforms;
            _portabilityAnalyzer = new PortabilityAnalyzer(PortabilityDatabase.Collection);
            _assemblyParser = new AssemblyParser(_assemblyPath);
            _portabilityAnalyzer.SupportedPlatforms = _requestedPlatforms;
            _portabilityAnalyzer.ExcludeThirdPartyReferences = excludeThirdPartyReferences;
            _portabilityAnalyzer.CallCollection = _assemblyParser.GetAssemblyCalls();
        }

        public IList<CallInfo> GetPortableCalls()
        {
            return _portabilityAnalyzer.GetPortableCalls().Select(x => new CallInfo(x)).ToList();
        }

        public IList<CallInfo> GetNonPortableCalls()
        {
            return _portabilityAnalyzer.GetNonPortableCalls().Select(x => new CallInfo(x)).ToList();
        }
    }
}