using PclAnalyzer.Core;

namespace PclAnalyzer.UI.ViewModel
{
    public class CallInfo
    {
        public string Caller { get; set; }
        public string Reference { get; set; }

        public CallInfo(MethodCall methodCall)
        {
            this.Caller = methodCall.CallingMethod.ToString();
            this.Reference = methodCall.ReferencedMethod.ToString();
        }
    }
}