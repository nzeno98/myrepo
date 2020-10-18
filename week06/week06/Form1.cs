using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using week06.Entities;
using week06.MnbServiceReference;


namespace week06
{
    public partial class Form1 : Form
    {
        string xmleredmeny;
        BindingList<RateData> Rates = new BindingList<RateData>();
        public Form1()
        {
            InitializeComponent();
            
        }
        public void cucc()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.Text,
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
                
            };
            var response = mnbService.GetExchangeRates(request);
            string result = response.GetExchangeRatesResult;
            xmleredmeny = result;
            //richTextBox1.Text = result;
        }
        public void xmlfeldolgozocucc()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmleredmeny);
            foreach (XmlElement item in xml)
            {
                RateData rd = new RateData();
                Rates.Add(rd);
                rd.Date = DateTime.Parse(item.GetAttribute("Date"));
                var childElement = (XmlElement)item.ChildNodes[0];
                rd.Currency = childElement.GetAttribute("curr");
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rd.Value = value / unit;
            }
        }
        public void fuggvenyke()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;

        }
        public void refreshdata()
        {
            Rates.Clear();
            cucc();
            dataGridView1.DataSource = Rates;
            cucc();
            xmlfeldolgozocucc();
            fuggvenyke();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            refreshdata();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            refreshdata();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshdata();
        }
    }
}
