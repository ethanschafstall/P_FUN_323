using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Windows.Forms;
using System.Windows.Media;
using System.Collections.Generic;


namespace Plot_That_Line
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var seriesCollection = new SeriesCollection
            {
                
                new LineSeries
                {
                    Title = "Hydro",
                    Values = new ChartValues<double> { 3, 5, 7, 4, 6, 8, 9, 2, 1, 3, 4, 6 }
                },
                new LineSeries
                {
                    Title = "Thermal",
                    Values = new ChartValues<double> { 1, 7, 2, 3, 5, 10, 50, 65, 45, 67, 2, 14 }
                },
                new LineSeries
                {
                    Title = "Nuclear",
                    Values = new ChartValues<double> { 1, 7, 32, 23, 15, 10, 12, 11, 6, 7, 11, 10 }
                }
            };

            // Assign the series to the chart
            cartesianChart1.Series = seriesCollection;

            // Configure the axis
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                Separator = new Separator
                {
                    Step = 1
                }
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Production (GWh)"
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
