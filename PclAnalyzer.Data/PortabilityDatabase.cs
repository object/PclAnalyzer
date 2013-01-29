using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PclAnalyzer.Core;

namespace PclAnalyzer.Data
{
    public static class PortabilityDatabase
    {
        private static readonly Lazy<IList<MemberPortability>> _collection;

        static PortabilityDatabase()
        {
            _collection = new Lazy<IList<MemberPortability>>(LoadCollection);
        }

        public static IList<MemberPortability> Collection
        {
            get { return _collection.Value; }
        }

        private static IList<MemberPortability> LoadCollection()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("PclAnalyzer.Data.Resources.PLIB APIs.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var collection = new List<MemberPortability>();
                    reader.ReadLine();
                    var text = reader.ReadLine();
                    while (text != null)
                    {
                        var items = text.Split(';');
                        var item = new MemberPortability();
                        item.ID = items[0];
                        item.Namespace = items[1];
                        item.TypeName = items[2];
                        item.MemberName = items[3];
                        item.SupportedPlatforms = (Platforms)uint.Parse(items[4]);
                        collection.Add(item);
                        text = reader.ReadLine();
                    }
                    return collection;
                }
            }
        }
    }
}
