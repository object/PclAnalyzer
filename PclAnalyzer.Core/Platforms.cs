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
        NetForWsa = 0x0008,
        WP7 = 0x0100,
        WP75 = 0x0200,
        WP8 = 0x0400,
        SL4 = 0x1000,
        SL5 = 0x2000,
        Xbox360 = 0x8000,

        AllKnown = Net4 | Net403 | Net45 | NetForWsa | WP7 | WP75 | WP8 | SL4 | SL5 | Xbox360,
    }
}