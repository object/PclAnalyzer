namespace PclAnalyzer.Core
{
    public class MethodCall
    {
        public Member CallingMethod { get; set; }
        public Member ReferencedMethod { get; set; }

        public MethodCall()
        {
        }

        public MethodCall(Member callingMethod, Member referencedMethod)
        {
            this.CallingMethod = callingMethod;
            this.ReferencedMethod = referencedMethod;
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", this.CallingMethod, this.ReferencedMethod);
        }

        public override bool Equals(object obj)
        {
            if (obj is MethodCall)
            {
                var call = obj as MethodCall;
                return
                    this.CallingMethod.Equals(call.CallingMethod) &&
                    this.ReferencedMethod.Equals(call.ReferencedMethod);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return this.CallingMethod.GetHashCode() ^ this.ReferencedMethod.GetHashCode();
        }
    }
}