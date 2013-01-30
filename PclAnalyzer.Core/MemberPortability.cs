using System;

namespace PclAnalyzer.Core
{
    public class MemberPortability
    {
        public MemberCategory Category { get; set; }
        public string ID { get; set; }
        public string Namespace { get; set; }
        public string TypeName { get; set; }
        public string MemberName { get; set; }
        public Platforms SupportedPlatforms { get; set; }

        public Member GetMember()
        {
            return new Member(string.Empty, this.Namespace, this.TypeName, this.MemberName);
        }
    }
}