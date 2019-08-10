using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oanda
{
    public class Oanda
    {
        string API_URL = "https://api-fxpractice.oanda.com/v3/";
        string ACC_ID = "101-001-9700658-002";
        string TOKEN = "Bearer 7b11eee1671f7f2948a5ef870a63e14a-b80a569143239aeb6e9d9b6bbae4b435";

        public DataTable GetInstrument()
        {
            DataTable dataTable = new DataTable();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL + "accounts/" + ACC_ID + "/instruments");
            request.Headers.Add("Authorization", TOKEN);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var res = JsonConvert.DeserializeObject<Instrument>(content);


            dataTable.Columns.Add("name", typeof(string));
            dataTable.Columns.Add("displayName", typeof(string));

            foreach (var itm in res.instruments)
            {
                dataTable.Rows.Add(itm.name, itm.displayName.Replace("/", ""));
            }
            return dataTable;
        }


        public async Task<DataTable> GetDataCandles(string currency, string tick)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", TOKEN);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("time", typeof(DateTime));
            dataTable.Columns.Add("o", typeof(double));
            dataTable.Columns.Add("h", typeof(double));
            dataTable.Columns.Add("l", typeof(double));
            dataTable.Columns.Add("c", typeof(double));

            var url =  API_URL + "instruments/" + currency + "/candles?count=500&price=M&granularity=" + tick + "&star";

            var context = await client.GetAsync(url);

            if (context != null)
            {
                var content = await context.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<Candles>(content);

                for (int i = 0; i < res.candles.Count; i++)
                {
                    var number = res.candles[i].mid.c.Replace(".", "");
                    DataRow dr = dataTable.NewRow();
                    dr["time"] = Convert.ToDateTime(res.candles[i].time);//.ToString("dd-MM-yyyy HH:mm:ss");
                    dr["o"] = res.candles[i].mid.o;
                    dr["h"] = res.candles[i].mid.h;
                    dr["l"] = res.candles[i].mid.l;
                    dr["c"] = res.candles[i].mid.c;
                    dataTable.Rows.Add(dr);
                }
            }
            return dataTable;
        }
    }
}
