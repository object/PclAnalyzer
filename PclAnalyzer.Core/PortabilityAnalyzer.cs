using System.Collections.Generic;
using System.Linq;

namespace PclAnalyzer.Core
{
    public class PortabilityAnalyzer
    {
        private IList<MemberPortability> _repository;
        private Platforms _supportedPlatforms;
        private bool _excludeThirdPartyReferences;
        private IList<MethodCall> _callCollection;
        private IList<MethodCall> _portableCalls;

        public PortabilityAnalyzer(IList<MemberPortability> repository)
        {
            _repository = repository;
        }

        public Platforms SupportedPlatforms 
        {
            get { return _supportedPlatforms; }
            set
            {
                _supportedPlatforms = value; _portableCalls = null;
            }
        }

        public bool ExcludeThirdPartyReferences
        {
            get { return _excludeThirdPartyReferences; }
            set
            {
                _excludeThirdPartyReferences = value; _portableCalls = null;
            }
        }

        public IList<MethodCall> CallCollection
        {
            get { return _callCollection; }
            set
            {
                _callCollection = value; _portableCalls = null;
            }
        }

        public IList<MethodCall> GetPortableCalls()
        {
            if (_portableCalls != null)
                return _portableCalls;

            var result = (from c in this.CallCollection
                          from p in _repository
                          where c.ReferencedMethod.Equals(p.GetMember()) && 
                          (this.SupportedPlatforms & p.SupportedPlatforms) == this.SupportedPlatforms
                          select c)
                          .Union(
                          from c in this.CallCollection 
                          where IsInlineEnumerator(c.ReferencedMethod) 
                          select c)
                          .Distinct();

            if (this.ExcludeThirdPartyReferences)
                result = result.Where(x => x.ReferencedMethod.IsClrMember());

            _portableCalls = result.ToList();
            return _portableCalls;
        }

        public IList<MethodCall> GetNonPortableCalls()
        {
            if (_portableCalls == null)
                this.GetPortableCalls();

            var result = this.CallCollection.Except(_portableCalls)
                .Distinct();
            if (this.ExcludeThirdPartyReferences)
                result = result.Where(x => x.ReferencedMethod.IsClrMember());
            return result.ToList();
        }

        private bool IsInlineEnumerator(Member member)
        {
            return string.IsNullOrEmpty(member.Namespace) && member.TypeName == "Enumerator";
        }
    }
}