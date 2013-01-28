using System.Net;

namespace PclAnalyzer.Core.Tests
{
    public class HttpWebRequestClass
    {
        public void SetCredentials(ICredentials credentials)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.myweb.com");
            request.PreAuthenticate = true;
            request.Credentials = credentials;
        }

        public void SetDefaultCredentialsFromCache()
        {
            SetCredentials(CredentialCache.DefaultCredentials);
        }

        public void SetDefaultCredentials()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.myweb.com");
            request.UseDefaultCredentials = true;
        }
    }
}