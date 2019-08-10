using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oanda
{
    public class Instrument
    {
        public List<Instruments> instruments { get; set; }
    }
    public class Instruments
    {
        public string name { get; set; }
        public string type { get; set; }
        public string displayName { get; set; }
        public int pipLocation { get; set; }
        public int displayPrecision { get; set; }
        public int tradeUnitsPrecision { get; set; }
        public string minimumTradeSize { get; set; }
        public string maximumTrailingStopDistance { get; set; }
        public string minimumTrailingStopDistance { get; set; }
        public string maximumPositionSize { get; set; }
        public string maximumOrderUnits { get; set; }
        public string marginRate { get; set; }
        public List<Tag> tags { get; set; }
    }

    public class Tag
    {
        public string type { get; set; }
        public string name { get; set; }
    }
}
