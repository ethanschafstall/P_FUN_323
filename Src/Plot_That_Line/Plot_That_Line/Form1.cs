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
            Dictionary<string, Color> colors = new Dictionary<string, Color>();
            colors.Add("HydroStroke", Color.FromArgb(200, 65, 169, 242));
            colors.Add("HydroFill", Color.FromArgb(40, 65, 169, 242));
            colors.Add("ThermalStroke", Color.FromArgb(200, 242, 65, 68));
            colors.Add("ThermalFill", Color.FromArgb(40, 242, 65, 68));
            colors.Add("NuclearStroke", Color.FromArgb(200, 27, 181, 53));
            colors.Add("NuclearFill", Color.FromArgb(40, 27, 181, 53));

            var seriesCollection = new SeriesCollection
            {

                new LineSeries
                {
                    Title = "Hydro",
                    Values = new ChartValues<double> { 3, 5, 7, 4, 6, 8, 9, 2, 1, 3, 4, 6 },
                    Stroke = new SolidColorBrush(colors["HydroStroke"]),
                    Fill = new SolidColorBrush(colors["HydroFill"])
                },
                new LineSeries
                {
                    Title = "Thermal",
                    Values = new ChartValues<double> { 1, 7, 2, 3, 5, 10, 50, 65, 45, 67, 2, 14 },
                    Stroke = new SolidColorBrush(colors["ThermalStroke"]),
                    Fill = new SolidColorBrush(colors["ThermalFill"])
                }
                ,
                new LineSeries
                {
                    Title = "Nuclear",
                    Values = new ChartValues<double> { 1, 7, 32, 23, 15, 10, 12, 11, 6, 7, 11, 10 },
                    Stroke = new SolidColorBrush(colors["NuclearStroke"]),
                    Fill = new SolidColorBrush(colors["NuclearFill"])
                }
            };

            // Assign the series to the chart
            cartesianChart1.Series = seriesCollection;

            // Configure the axis
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
                FontSize = 14,
                FontWeight = System.Windows.FontWeights.Bold,
                Foreground = Brushes.Black,
                Separator = new Separator
                {
                    Step = 1
                }
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Production (GWh)",
                FontSize = 14,
                FontWeight = System.Windows.FontWeights.Bold,
                Foreground = Brushes.Black,
                Separator = new Separator
                {
                    Step = 10
                }
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
