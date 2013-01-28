using System;

namespace PclAnalyzer.Core
{
    [Flags]
    public enum Platforms
    {
        None,
        Net4 = 0x0001,
        Net403 = 0x0002,
        Net45 = 0x0004,
        SL4 = 0x0010,
        SL5 = 0x0020,
        WP7 = 0x0100,
        WP75 = 0x0200,
        WP8 = 0x0400,
        NetForWsa = 0x1000,
        Xbox360 = 0x2000,

        AllKnown = Net4 | Net403 | Net45 | SL4 | SL5 | WP7 | WP75 | WP8 | NetForWsa | Xbox360,
    }
}