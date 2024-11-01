using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using System.Collections.Generic;
using Plot_That_Line.Tools;
using Plot_That_Line.Models;
using System.IO;
using System.Drawing;
using LiveCharts.WinForms;
using LiveCharts.Wpf.Charts.Base;
namespace Plot_That_Line
{
    public partial class Form1 : Form
    {

        // variables for holding the current filters selected, clears the list and adds the newest filter.
        private List<bool> _showHydro = new List<bool> { true };
        private List<bool> _showNuclear = new List<bool> { true };
        private List<bool> _showThermal = new List<bool> { true };
        private List<bool> _showTotal = new List<bool> { true };
        private List<int> _startYear = new List<int> { 1990 };
        private List<int> _endYear = new List<int> { 2023 };
        private List<int> _selectedMonths = new List<int>{ 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12 };

        // variables for holding components
        private List<CartesianChart> _chart = new List<CartesianChart>();

        public Form1()
        {
            InitializeComponent();
            _chart.Add(new CartesianChart()
            {
                Location = new Point(378, 34),
                Size = new Size(1030, 600)
            });
            this.Controls.Add(_chart[0]); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitYearComponent();
            InitEnergyTypeComponent();
            InitGraph();
        }
        private void InitYearComponent()
        {
            var years = Enumerable.Range(1990, 34).ToList();

            ComboBox startYearBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(15, 30)
            };

            ComboBox endYearBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(15, 80)
            };

            foreach (var year in years)
            {
                startYearBox.Items.Add(year);
                endYearBox.Items.Add(year);
            }

            startYearBox.SelectedIndexChanged += (sender, e) =>
            {
                _startYear.Clear();
                _startYear.Add((int)startYearBox.SelectedItem);
                InitGraph();
            };

            endYearBox.SelectedIndexChanged += (sender, e) =>
            {
                _endYear.Clear();
                _endYear.Add((int)endYearBox.SelectedItem);
                InitGraph();
            };
            
            startYearBox.SelectedIndex = startYearBox.Items.IndexOf(_startYear[0]);
            endYearBox.SelectedIndex = endYearBox.Items.IndexOf(_endYear[0]);

            this.Controls.Add(startYearBox);
            this.Controls.Add(endYearBox);
        }
        private void InitEnergyTypeComponent()
        {
            CheckedListBox energyTypeList = new CheckedListBox
            {
                Location = new Point(160, 30),
                Size = new Size(125, 70)
            };

            foreach (var item in new List<string> { "Hydro", "Nuclear", "Thermal", "Total" })
            {
                energyTypeList.Items.Add(item);
            }

            energyTypeList.ItemCheck += (sender, e) =>
            {
                _showHydro.Clear();
                _showNuclear.Clear();
                _showThermal.Clear();
                _showTotal.Clear();

                this.BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < energyTypeList.Items.Count; i++)
                    {
                        bool isChecked = energyTypeList.GetItemChecked(i);
                        switch (energyTypeList.Items[i].ToString())
                        {
                            case "Hydro":
                                _showHydro.Add(isChecked);
                                break;
                            case "Nuclear":
                                _showNuclear.Add(isChecked);
                                break;
                            case "Thermal":
                                _showThermal.Add(isChecked);
                                break;
                            case "Total":
                                _showTotal.Add(isChecked);
                                break;
                        }
                    }
                    InitGraph();
                }));
            };

            this.Controls.Add(energyTypeList);
        }
        private void InitGraph()
        {

            var data = new EnergyDataService(GetDataPath()).GetData(
            new FilterModel()
            {
                ShowHydro = _showHydro[0],
                ShowNuclear = _showNuclear[0],
                ShowThermal = _showThermal[0],
                ShowTotal = _showTotal[0],
                StartYear = _startYear[0],
                EndYear = _endYear[0],
                SelectedMonths = _selectedMonths,
            });

            // Total year values for X axis labels
            var xLabels = data.Select(g => g.Year.ToString()).ToArray();

            GraphManager graphManager = new GraphManager();


            graphManager.GenerateGraph(data, _chart[0], xLabels);
        }

        private static string GetDataPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(baseDir, @"..\..\..\..\..\Data\5634-Zeitreihe_Elektrizitätsbilanz_Schweiz_Monatswerte.csv");
            return Path.GetFullPath(relativePath);
        }
    }
}
