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
using Plot_That_Line.ViewModels;

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
        private List<EnergyDataViewModel> energyDataList_;

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

            EnergyDataService energyDataService = new EnergyDataService(fullPath);
            energyDataList_ = energyDataService.GetData(
                new FilterModel() { 
                    ShowHydro = true,
                    ShowNuclear = true,
                    StartYear = 1990,
                    EndYear = 2000,
                    SelectedMonths = Enumerable.Range(1, 12).ToList()
                });



            // Total year values for X axis labels
            var years = energyDataList_.Select(g => g.Year.ToString()).ToArray();

            
            var hydroValues = new ChartValues<double>();
            var thermalValues = new ChartValues<double>();
            var nuclearValues = new ChartValues<double>();
            var totalProductionValues = new ChartValues<double>();

            // Checks if production values for different types of energy exists so that it can be used as ChartValues
            if (energyDataList_.Select(g => g.HydroProduction.HasValue).ToList().All(value => value)) 
                hydroValues = new ChartValues<double>(energyDataList_.Select(g => g.HydroProduction.Value));

            if (energyDataList_.Select(g => g.ThermalProduction.HasValue).ToList().All(value => value))
                thermalValues = new ChartValues<double>(energyDataList_.Select(g => g.ThermalProduction.Value));

            if (energyDataList_.Select(g => g.NulcearProduction.HasValue).ToList().All(value => value))
                nuclearValues = new ChartValues<double>(energyDataList_.Select(g => g.NulcearProduction.Value));

            if (energyDataList_.Select(g => g.TotalProduction.HasValue).ToList().All(value => value))
                totalProductionValues = new ChartValues<double>(energyDataList_.Select(g => g.TotalProduction.Value));



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
                    Step = 2
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


    }
}
