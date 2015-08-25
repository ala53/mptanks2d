using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameCore
    {
        private TimescaleValue _timescale = TimescaleValue.One;
        public TimescaleValue Timescale
        {
            get { return _timescale; }
            set
            {
                _timescale = value;
                EventEngine.RaiseGameTimescaleChanged(value);
            }
        }

        public struct TimescaleValue
        {
            public static readonly IReadOnlyList<TimescaleValue> Values = new[] {
                new TimescaleValue(64d, "64", 0),
                new TimescaleValue(32d, "32", 1),
                new TimescaleValue(16d, "16", 2),
                new TimescaleValue(8d, "8", 3),
                new TimescaleValue(6d, "6", 4),
                new TimescaleValue(4d, "4", 5),
                new TimescaleValue(3d, "3", 6),
                new TimescaleValue(2d, "2", 7),
                new TimescaleValue(1.75d, "1 3/4", 8),
                new TimescaleValue(1.5d, "1 1/2", 9),
                new TimescaleValue(1.25d, "1 1/4", 10),
                new TimescaleValue(1d, "1", 11),

                new TimescaleValue(3d / 4d, "3/4", 12),
                new TimescaleValue(1d / 2d, "1/2", 13),
                new TimescaleValue(1d / 4d, "1/4", 14),
                new TimescaleValue(1d / 6d, "1/6", 15),
                new TimescaleValue(1d / 8d, "1/8", 16),
                new TimescaleValue(1d / 16d, "1/16", 17),
                new TimescaleValue(1d / 32d, "1/32", 18),
                new TimescaleValue(1d / 64d, "1/64", 19),
                new TimescaleValue(1d / 128d, "1/128", 20),
                new TimescaleValue(1d / 256d, "1/256", 21),
                new TimescaleValue(1d / 512d, "1/512", 22),
                new TimescaleValue(1d / 1024d, "1/1024", 23),
            };
            public static readonly TimescaleValue SixtyFour = Values[0];
            public static readonly TimescaleValue ThirtyTwo = Values[1];
            public static readonly TimescaleValue Sixteen = Values[2];
            public static readonly TimescaleValue Eight = Values[3];
            public static readonly TimescaleValue Six = Values[4];
            public static readonly TimescaleValue Four = Values[5];
            public static readonly TimescaleValue Three = Values[6];
            public static readonly TimescaleValue Two = Values[7];
            public static readonly TimescaleValue OneAndThreeQuarters = Values[8];
            public static readonly TimescaleValue OneAndOneHalf = Values[9];
            public static readonly TimescaleValue OneAndOneQuarter = Values[10];
            public static readonly TimescaleValue One = Values[11];


            public static readonly TimescaleValue ThreeQuarters = Values[12];
            public static readonly TimescaleValue Half = Values[13];
            public static readonly TimescaleValue OneQuarter = Values[14];
            public static readonly TimescaleValue OneSixth = Values[15];
            public static readonly TimescaleValue OneEighth = Values[16];
            public static readonly TimescaleValue OneSixteenth = Values[17];
            public static readonly TimescaleValue OneThirtySecond = Values[18];
            public static readonly TimescaleValue OneSixtyFourth = Values[19];
            public static readonly TimescaleValue OneOneHundredTwentyEighth = Values[20];
            public static readonly TimescaleValue OneTwoHundredFiftySixth = Values[21];
            public static readonly TimescaleValue OneFiveHundrenTwelfth = Values[22];
            public static readonly TimescaleValue OneOneThousandTwentyFourth = Values[23];

            public double Fractional { get; private set; }
            public string DisplayString { get; private set; }
            public int Index { get; private set; }
            public TimescaleValue(double fractional, string displayName)
            {
                Fractional = fractional;
                DisplayString = displayName;
                Index = -1;
            }
            private TimescaleValue(double value, string display, int index)
            {
                Fractional = value;
                DisplayString = display;
                Index = index;
            }
        }
    }
}
