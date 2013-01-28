using System.Collections.Generic;
using System.IO;
using PclAnalyzer.Core;

namespace ExcelConverter
{
    public class TextWriter
    {
        private readonly string _path;

        public TextWriter(string path)
        {
            _path = path;
        }

        public void Write(IList<MemberPortability> collection)
        {
            using (var writer = new StreamWriter(_path))
            {
                writer.WriteLine("ID;Namespace;TypeName;MemberName;SupportedPlatforms");
                foreach (var item in collection)
                {
                    writer.WriteLine("{0};{1};{2};{3};{4}",
                        item.ID,
                        item.Namespace,
                        item.TypeName,
                        item.MemberName,
                        (uint)item.SupportedPlatforms);
                }
            }
        }
    }
}