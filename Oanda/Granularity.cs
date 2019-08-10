using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oanda
{
    public class Granularity
    {
        public Dictionary<string, string> list { get; private set; }

        public Granularity()
        {
            list = new Dictionary<string, string>();

            list.Add("S5", "5 second candlesticks, minute alignment");
            list.Add("S10", "10 second candlesticks, minute alignment");
            list.Add("S15", "15 second candlesticks, minute alignment");
            list.Add("S30", "30 second candlesticks, minute alignment");
            list.Add("M1", "1 minute candlesticks, minute alignment");
            list.Add("M2", "2 minute candlesticks, hour alignment");
            list.Add("M4", "4 minute candlesticks, hour alignment");
            list.Add("M5", "5 minute candlesticks, hour alignment");
            list.Add("M10", "10 minute candlesticks, hour alignment");
            list.Add("M15", "15 minute candlesticks, hour alignment");
            list.Add("M30", "30 minute candlesticks, hour alignment");
            list.Add("H1", "1 hour candlesticks, hour alignment");
            list.Add("H2", "2 hour candlesticks, day alignment");
            list.Add("H3", "3 hour candlesticks, day alignment");
            list.Add("H4", "4 hour candlesticks, day alignment");
            list.Add("H6", "6 hour candlesticks, day alignment");
            list.Add("H8", "8 hour candlesticks, day alignment");
            list.Add("H12", "12 hour candlesticks, day alignment");
            list.Add("D", "1 day candlesticks, day alignment");
            list.Add("W", "1 week candlesticks, aligned to start of week");
            list.Add("M", "1 month candlesticks, aligned to first day of the month");
        }
    }
}
