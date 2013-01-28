using System.Collections.Generic;
using System.Xml.Linq;

namespace PclAnalyzer.Core.Tests
{
    public class XElementClass
    {
        public IEnumerable<XNode> ParseXml(string xml)
        {
            var element = XElement.Parse(xml);
            return element.Nodes();
        }
    }
}