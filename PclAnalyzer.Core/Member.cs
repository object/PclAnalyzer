using System.Linq;

namespace PclAnalyzer.Core
{
    public class Member
    {
        public string Namespace { get; set; }
        public string TypeName { get; set; }
        public string MemberName { get; set; }

        public Member()
        {
        }

        public Member(string namespaceName, string typeName, string memberName)
        {
            this.Namespace = namespaceName;
            this.TypeName = typeName;
            this.MemberName = memberName;
        }

        public Member(string memberFullName)
        {
            var items = memberFullName.Split('.');
            this.Namespace = string.Join(".", items.Take(items.Length - 2));
            this.TypeName = string.Join(".", items.Take(items.Length - 1).Last());
            this.MemberName = items.Last();
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", this.Namespace, this.TypeName, this.MemberName);
        }

        public override bool Equals(object obj)
        {
            if (obj is Member)
            {
                var info = obj as Member;
                return
                    this.Namespace.Equals(info.Namespace) &&
                    this.TypeName.Equals(info.TypeName) &&
                    this.MemberName.Equals(info.MemberName);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return this.Namespace.GetHashCode() ^ this.TypeName.GetHashCode() ^ this.MemberName.GetHashCode();
        }
    }
}