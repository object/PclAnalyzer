using System.Collections.Generic;
using System.Linq;

namespace PclAnalyzer.Core
{
    public class PortabilityAnalyzer
    {
        private IList<MemberPortability> _repository;

        public PortabilityAnalyzer(IList<MemberPortability> repository)
        {
            _repository = repository;
        }

        public Platforms SupportedPlatforms { get; set; }
        public bool ExcludeThirdPartyReferences { get; set; }
        public IList<MethodCall> CallCollection { get; set; } 

        public IList<MethodCall> GetPortableCalls()
        {
            var result = (from c in this.CallCollection
                          from p in _repository
                          where c.ReferencedMethod.Equals(p.GetMember()) && 
                          (this.SupportedPlatforms & p.SupportedPlatforms) == this.SupportedPlatforms
                          select c).Distinct().ToList();
            return result;
        }

        public IList<MethodCall> GetNonPortableCalls()
        {
            var result = this.CallCollection.Except(this.GetPortableCalls())
                .Distinct().ToList();
            return result;
        }
    }
}