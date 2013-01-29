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
        private bool _recalculatePortableCalls;
        private IList<MethodCall> _portableCalls;

        public PortabilityAnalyzer(IList<MemberPortability> repository)
        {
            _repository = repository;
        }

        public Platforms SupportedPlatforms 
        {
            get { return _supportedPlatforms; }
            set { _supportedPlatforms = value; _recalculatePortableCalls = true;
            }
        }

        public bool ExcludeThirdPartyReferences
        {
            get { return _excludeThirdPartyReferences; }
            set
            {
                _excludeThirdPartyReferences = value; _recalculatePortableCalls = true;
            }
        }

        public IList<MethodCall> CallCollection
        {
            get { return _callCollection; }
            set
            {
                _callCollection = value; _recalculatePortableCalls = true;
            }
        }

        public IList<MethodCall> GetPortableCalls()
        {
            if (_portableCalls != null && !_recalculatePortableCalls)
                return _portableCalls;

            _recalculatePortableCalls = false;
            var result = (from c in this.CallCollection
                          from p in _repository
                          where (c.ReferencedMethod.Equals(p.GetMember()) || AreEquivalent(c.ReferencedMethod, p.GetMember())) && 
                          (this.SupportedPlatforms & p.SupportedPlatforms) == this.SupportedPlatforms
                          select c)
                          .Union(
                          from c in this.CallCollection 
                          where IsInlineEnumerator(c.ReferencedMethod) 
                          select c)
                          .Distinct();

            if (this.ExcludeThirdPartyReferences)
                result = result.Where(x => !IsThirdPartyCall(x.ReferencedMethod));

            _portableCalls = result.ToList();
            return _portableCalls;
        }

        public IList<MethodCall> GetNonPortableCalls()
        {
            if (_portableCalls == null || _recalculatePortableCalls)
                this.GetPortableCalls();

            var result = this.CallCollection.Except(_portableCalls)
                .Distinct();
            if (this.ExcludeThirdPartyReferences)
                result = result.Where(x => !IsThirdPartyCall(x.ReferencedMethod));
            return result.ToList();
        }

        private bool IsThirdPartyCall(Member member)
        {
            return !member.Namespace.StartsWith("System.") && !member.Namespace.StartsWith("Microsoft.");
        }

        private bool IsInlineEnumerator(Member member)
        {
            return string.IsNullOrEmpty(member.Namespace) && member.TypeName == "Enumerator";
        }

        private bool AreEquivalent(Member member1, Member member2)
        {
            if (member1.Namespace == "System.Xml.Linq" &&
                member2.Namespace == "System.Xml.Linq" &&
                member1.MemberName == member2.MemberName &&
                (member1.TypeName == "Extensions" && member2.TypeName == "XElementExtensions" ||
                 member2.TypeName == "Extensions" && member1.TypeName == "XElementExtensions" ||
                 member1.TypeName == "Extensions" && member2.TypeName == "XAttributeExtensions" ||
                 member2.TypeName == "Extensions" && member1.TypeName == "XAttributeExtensions"))
            {
                return true;
            }

            return false;
        }
    }
}