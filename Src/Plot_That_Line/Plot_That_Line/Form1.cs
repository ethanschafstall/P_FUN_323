using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using System.Collections.Generic;
using Plot_That_Line.Tools;
using System.IO;
using Plot_That_Line.Models;

namespace Plot_That_Line
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Dictionary for the colors used by the graph
        /// </summary>
        private static Dictionary<string, Color> colors_ = new Dictionary<string, Color>
            {
                { "HydroStroke", Color.FromArgb(200, 65, 169, 242) },
                { "HydroFill", Color.FromArgb(40, 65, 169, 242) },
                { "ThermalStroke", Color.FromArgb(200, 242, 65, 68) },
                { "ThermalFill", Color.FromArgb(40, 242, 65, 68) },
                { "NuclearStroke", Color.FromArgb(200, 27, 181, 53) },
                { "NuclearFill", Color.FromArgb(40, 27, 181, 53) },
                { "TotalStroke", Color.FromArgb(200, 255, 165, 0) },
                { "TotalFill", Color.FromArgb(40, 255, 165, 0) }
            };

        /// <summary>
        /// Energy Data Collection
        /// </summary>
        private List<EnergyData> energyDataList_;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Non hardcoded Path
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(baseDir, @"..\..\..\..\..\Data\5634-Zeitreihe_Elektrizitätsbilanz_Schweiz_Monatswerte.csv"); /* Not sure if there is a prettier way of doing this */
            string fullPath = Path.GetFullPath(relativePath);

            energyDataList_ = Load_Data(fullPath);

            // Anon obj list with avg values for each energy type & total for each year key
            var groupedData = energyDataList_
                .Where(data => data.Year.HasValue) /* HasValue since the property is nullable */
                .GroupBy(data => data.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    AvgHydro = g.Average(data => data.Hydropower ?? 0),
                    AvgThermal = g.Average(data => data.ThermalPower ?? 0),
                    AvgNuclear = g.Average(data => data.NuclearPower ?? 0),
                    AvgTotalProduction = g.Average(data => data.TotalProduction ?? 0)
                })
                .OrderBy(g => g.Year)
                .ToList();

            // Total year values for X axis labels
            var years = groupedData.Select(g => g.Year.ToString()).ToArray();

            // Average year values for Hydro, Thermal, Nuclear, and Total Production
            var hydroValues = new ChartValues<double>(groupedData.Select(g => g.AvgHydro));
            var thermalValues = new ChartValues<double>(groupedData.Select(g => g.AvgThermal));
            var nuclearValues = new ChartValues<double>(groupedData.Select(g => g.AvgNuclear));
            var totalProductionValues = new ChartValues<double>(groupedData.Select(g => g.AvgTotalProduction));

            // Create the series collection for each line displayed on the graph
            var seriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Hydro",
                    Values = hydroValues,
                    Stroke = new SolidColorBrush(colors_["HydroStroke"]),
                    Fill = new SolidColorBrush(colors_["HydroFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh" /* Without the LabelPoint attribute the values are rounded correctly
                                                                        * but when added there is always a .66666 for some reason, hence why Math.Round */ 
                },
                new LineSeries
                {
                    Title = "Thermal",
                    Values = thermalValues,
                    Stroke = new SolidColorBrush(colors_["ThermalStroke"]),
                    Fill = new SolidColorBrush(colors_["ThermalFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh"
                },
                new LineSeries
                {
                    Title = "Nuclear",
                    Values = nuclearValues,
                    Stroke = new SolidColorBrush(colors_["NuclearStroke"]),
                    Fill = new SolidColorBrush(colors_["NuclearFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh"
                },
                new LineSeries
                {
                    Title = "Total Production",
                    Values = totalProductionValues,
                    Stroke = new SolidColorBrush(colors_["TotalStroke"]),
                    Fill = new SolidColorBrush(colors_["TotalFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh"
                }
            };

            // Assign the series to the chart
            cartesianChart1.Series = seriesCollection;

            // Configure the axis for X
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Year",
                Labels = years,
                FontSize = 14,
                FontWeight = System.Windows.FontWeights.Bold,
                Foreground = Brushes.Black,
                Separator = new Separator /* Seperates Axis label values. So Step 5 shows 0, 5, 10, etc */
                {
                    Step = 5
                }
            });
            // Configure the axis for  Y
            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Production (GWh)",
                LabelFormatter = value => value.ToString("N0"), /* So that the values are formated correctly 1000 => 1'000 */
                FontSize = 14,
                FontWeight = System.Windows.FontWeights.Bold,
                Foreground = Brushes.Black,
                MinValue = 0, /* Min 0 otherwise Y axis starts at -200 despite not having any values below 0 */
                Separator = new Separator
                {
                    Step = 500
                }
            });
        }

        private List<EnergyData> Load_Data(string dataPath)
        {
            return new CsvReader().ReadCsv(dataPath);
        }

    }
}
