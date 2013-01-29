using System;
using System.Collections.Generic;
using System.Linq;
using PclAnalyzer.Core;

namespace ExcelConverter
{
    class ExcelAdapter
    {
        public IList<MemberPortability> Convert(ExcelReader.ExcelTable table)
        {
            var data = new List<MemberPortability>();
            foreach (var row in table.Data)
            {
                var item = new MemberPortability();
                int col = 0;

                item.ID = row[col++];
                item.Namespace = row[col++];
                item.TypeName = row[col++];
                item.MemberName = row[col++];
                item.SupportedPlatforms = Platforms.None;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.NetForWsa;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.Net4;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.Net403;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.Net45;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.SL4;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.SL5;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.WP7;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.WP75;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.WP8;
                if (ConvertToBoolean(row[col++]))
                    item.SupportedPlatforms |= Platforms.Xbox360;

                var index = item.TypeName.IndexOf("<");
                if (index > 0)
                {
                    var cardinality = item.TypeName.Split().Count();
                    item.TypeName = item.TypeName.Substring(0, index);
                    item.TypeName += "`" + cardinality.ToString();
                }
                var prefix = item.ID.Substring(0, 2);
                switch (prefix)
                {
                    case "T:":
                        item.Category = MemberCategory.Type;
                        break;
                    case "M:":
                        item.Category = MemberCategory.Method;
                        item.MemberName = item.MemberName.Substring(0, item.MemberName.IndexOf('('));
                        break;
                    case "P:":
                        item.Category = MemberCategory.Property;
                        item.MemberName = item.MemberName.Split(' ').Take(1).Single().Split('.').Last();
                        break;
                    case "F:":
                        item.Category = MemberCategory.Field;
                        item.MemberName = item.MemberName.Split(' ').Skip(1).Take(1).Single();
                        break;
                    case "E:":
                        item.Category = MemberCategory.Event;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown category " + prefix);
                }

                data.Add(item);
            }
            return data;
        }

        private bool ConvertToBoolean(string text)
        {
            return string.Equals(text, "yes", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}