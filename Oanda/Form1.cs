using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Oanda
{
    public partial class Form1 : Form
    {
        Oanda o = new Oanda();
        string defaultCurrency = "EUR_USD";
        string defaultTick = "M1";

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Granularity granularity = new Granularity();
            cmbGranularity.DataSource = new BindingSource(granularity.list, null);
            cmbGranularity.DisplayMember = "Value";
            cmbGranularity.ValueMember = "Key";
            cmbGranularity.SelectedValue = defaultTick;
            //cmbGranularity.SelectedText = "--select--";

            GetInstrumnet();

            if (cmbInstruments.Items.Count > 0)
            {
                Start(sender, e);
                chart1.MouseWheel += Chart1_MouseWheel;
            }
        }

        public void GetInstrumnet()
        {
            DataTable dtInstrument = new DataTable();
            try
            {
                dtInstrument = o.GetInstrument();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Remote server not respoding. \nError - " + ex.Message, "Oanda API Error");
                this.Close();
            }

            if (dtInstrument != null)
            {
                cmbInstruments.DisplayMember = "displayName";
                cmbInstruments.ValueMember = "name";
                cmbInstruments.DataSource = dtInstrument;

                cmbInstruments.SelectedValue = defaultCurrency;

                foreach(DataRow dr in dtInstrument.Rows)
                {
                    cmbInstruments.AutoCompleteCustomSource.Add(dr["displayName"].ToString());
                }

                cmbInstruments.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbInstruments.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //cmbInstruments.SelectedText = "--select--";;
            }
        }



        private async void Start(object sender, EventArgs e)
        {
            while (true)
            {
                var baseCurrency = cmbInstruments.SelectedItem;
                var tick = cmbGranularity.SelectedValue;

                if (baseCurrency != null)
                {
                    defaultCurrency = cmbInstruments.SelectedValue.ToString();
                }

                if (tick != null)
                {
                    defaultTick = tick.ToString();
                }

                DataTable result = new DataTable();
                try {
                    result = await o.GetDataCandles(defaultCurrency, defaultTick);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, "Remote server not respoding. \nError - " + ex.Message, "Oanda API Error");
                }

                if (result != null)
                {
                    if (result.Rows.Count > 0)
                    {
                        try
                        {
                            var min = result.AsEnumerable().Min(x => x.Field<double>("o"));
                            if (min > result.AsEnumerable().Min(x => x.Field<double>("c")))
                            {
                                min = result.AsEnumerable().Min(x => x.Field<double>("c"));
                            }
                            this.chart1.ChartAreas[0].AxisY.Minimum = min;
                            this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MM-yy hh:MM";
                        }
                        catch (Exception ex)
                        { }

                        chart1.Series["Series1"].XValueMember = "time";
                        chart1.Series["Series1"].YValueMembers = "h,l,o,c";
                        chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
                        chart1.Series["Series1"].ChartType = SeriesChartType.Candlestick;
                        chart1.Series["Series1"].SetCustomProperty("PriceUpColor", "Green");
                        chart1.Series["Series1"].SetCustomProperty("PriceDownColor", "Red");
                        chart1.Series["Series1"]["OpenCloseStyle"] = "Triangle";
                        chart1.Series["Series1"]["ShowOpenClose"] = "Both";
                        chart1.DataManipulator.IsStartFromFirst = true;
                        chart1.DataSource = result;
                        chart1.DataBind();
                    }
                }
                //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "";
            }
        }

        private const float CZoomScale = 4f;
        private int FZoomLevel = 0;

        private void Chart1_MouseEnter(object sender, EventArgs e)
        {
            if (!chart1.Focused)
                chart1.Focus();
        }

        private void Chart1_MouseLeave(object sender, EventArgs e)
        {
            if (chart1.Focused)
                chart1.Parent.Focus();
        }

        private void Chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                Axis xAxis = chart1.ChartAreas[0].AxisX;
                double xMin = xAxis.ScaleView.ViewMinimum;
                double xMax = xAxis.ScaleView.ViewMaximum;
                double xPixelPos = xAxis.PixelPositionToValue(e.Location.X);

                if (e.Delta < 0 && FZoomLevel > 0)
                {
                    // Scrolled down, meaning zoom out
                    if (--FZoomLevel <= 0)
                    {
                        FZoomLevel = 0;
                        xAxis.ScaleView.ZoomReset();
                    }
                    else {
                        double xStartPos = Math.Max(xPixelPos - (xPixelPos - xMin) * CZoomScale, 0);
                        double xEndPos = Math.Min(xStartPos + (xMax - xMin) * CZoomScale, xAxis.Maximum);
                        xAxis.ScaleView.Zoom(xStartPos, xEndPos);
                    }
                }
                else if (e.Delta > 0)
                {
                    // Scrolled up, meaning zoom in
                    double xStartPos = Math.Max(xPixelPos - (xPixelPos - xMin) / CZoomScale, 0);
                    double xEndPos = Math.Min(xStartPos + (xMax - xMin) / CZoomScale, xAxis.Maximum);
                    xAxis.ScaleView.Zoom(xStartPos, xEndPos);
                    FZoomLevel++;
                }
            }
            catch { }
        }

        private void cmbGranularity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Axis xAxis = chart1.ChartAreas[0].AxisX;
                FZoomLevel = 2;
                xAxis.ScaleView.ZoomReset();
            }
            catch { }
        }

        private void cmbInstruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Axis xAxis = chart1.ChartAreas[0].AxisX;
                FZoomLevel = 2;
                xAxis.ScaleView.ZoomReset();
            }
            catch { }

        }
    }


}
