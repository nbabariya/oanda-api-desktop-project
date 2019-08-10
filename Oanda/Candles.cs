using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oanda
{
    public class Candles
    {
        public string instrument { get; set; }
        public string granularity { get; set; }
        public List<Candle> candles { get; set; }
    }

    public class Mid
    {
        public string o { get; set; }
        public string h { get; set; }
        public string l { get; set; }
        public string c { get; set; }
    }

    public class Candle
    {
        public bool complete { get; set; }
        public int volume { get; set; }
        public string time { get; set; }
        public Mid mid { get; set; }

    }
}
