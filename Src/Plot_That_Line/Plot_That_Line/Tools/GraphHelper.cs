using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using Plot_That_Line.ViewModels;

namespace Plot_That_Line.Tools
{
    internal class GraphManager
    {
        private static readonly Dictionary<string, Color> _colors = new Dictionary<string, Color>
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
        public GraphManager() 
        { 
        }

        public void GenerateGraph(List<EnergyDataViewModel> data, LiveCharts.WinForms.CartesianChart chart, string[] xLabels) 
        {
            var hydroValues = new ChartValues<double>();
            var thermalValues = new ChartValues<double>();
            var nuclearValues = new ChartValues<double>();
            var totalProductionValues = new ChartValues<double>();


            // Checks if production values for different types of energy exists so that it can be used as ChartValues
            if (data.Select(g => g.HydroProduction.HasValue).ToList().All(value => value))
                hydroValues = new ChartValues<double>(data.Select(g => g.HydroProduction.Value));

            if (data.Select(g => g.ThermalProduction.HasValue).ToList().All(value => value))
                thermalValues = new ChartValues<double>(data.Select(g => g.ThermalProduction.Value));

            if (data.Select(g => g.NulcearProduction.HasValue).ToList().All(value => value))
                nuclearValues = new ChartValues<double>(data.Select(g => g.NulcearProduction.Value));

            if (data.Select(g => g.TotalProduction.HasValue).ToList().All(value => value))
                totalProductionValues = new ChartValues<double>(data.Select(g => g.TotalProduction.Value));



            // Create the series collection for each line displayed on the graph
            var seriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Hydro",
                    Values = hydroValues,
                    Stroke = new SolidColorBrush(_colors["HydroStroke"]),
                    Fill = new SolidColorBrush(_colors["HydroFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh" /* Without the LabelPoint attribute the values are rounded correctly
                                                                        * but when added there is always a .66666 for some reason, hence why Math.Round */ 
                },
                new LineSeries
                {
                    Title = "Thermal",
                    Values = thermalValues,
                    Stroke = new SolidColorBrush(_colors["ThermalStroke"]),
                    Fill = new SolidColorBrush(_colors["ThermalFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh"
                },
                new LineSeries
                {
                    Title = "Nuclear",
                    Values = nuclearValues,
                    Stroke = new SolidColorBrush(_colors["NuclearStroke"]),
                    Fill = new SolidColorBrush(_colors["NuclearFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh"
                },
                new LineSeries
                {
                    Title = "Total Production",
                    Values = totalProductionValues,
                    Stroke = new SolidColorBrush(_colors["TotalStroke"]),
                    Fill = new SolidColorBrush(_colors["TotalFill"]),
                    LabelPoint = point => Math.Round(point.Y) + " GWh"
                }
            };

            // Assign the series to the chart
            chart.Series = seriesCollection;

            // Configure the axis for X
            chart.AxisX.Add(new Axis
            {
                Title = "Year",
                Labels = xLabels,
                FontSize = 14,
                FontWeight = System.Windows.FontWeights.Bold,
                Foreground = Brushes.Black,
                Separator = new Separator /* Seperates Axis label values. So Step 5 shows 0, 5, 10, etc */
                {
                    Step = 2
                }
            });
            // Configure the axis for  Y
            chart.AxisY.Add(new Axis
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
