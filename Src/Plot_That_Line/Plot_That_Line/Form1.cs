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
                Location = new Point(378, 54),
                Size = new Size(1030, 600)
            });
            this.Controls.Add(_chart[0]); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitYearComponent();
            InitEnergyTypeComponent();
            InitGraph();
            InitTitles();
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
                int selectedStartYear = (int)startYearBox.SelectedItem;
                if (selectedStartYear > _endYear[0])
                {
                    MessageBox.Show("Start year cannot be greater than the end year.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    startYearBox.SelectedIndex = startYearBox.Items.IndexOf(_startYear[0]);
                }
                else
                {
                    _startYear.Clear();
                    _startYear.Add(selectedStartYear);
                    InitGraph();
                }
            };

            endYearBox.SelectedIndexChanged += (sender, e) =>
            {
                int selectedEndYear = (int)endYearBox.SelectedItem;
                if (selectedEndYear < _startYear[0])
                {
                    MessageBox.Show("End year cannot be less than the start year.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    endYearBox.SelectedIndex = endYearBox.Items.IndexOf(_endYear[0]);
                }
                else
                {
                    _endYear.Clear();
                    _endYear.Add(selectedEndYear);
                    InitGraph();
                }
            };
            
            startYearBox.SelectedIndex = startYearBox.Items.IndexOf(_startYear[0]);
            endYearBox.SelectedIndex = endYearBox.Items.IndexOf(_endYear[0]);

            this.Controls.Add(startYearBox);
            this.Controls.Add(endYearBox);
        }
        private void InitTitles()
        {
            // Title for Start Year ComboBox
            Label startYearLabel = new Label
            {
                Text = "Select Start Year:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            this.Controls.Add(startYearLabel);

            // Title for End Year ComboBox
            Label endYearLabel = new Label
            {
                Text = "Select End Year:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(15, 60),
                AutoSize = true
            };
            this.Controls.Add(endYearLabel);

            // Title for Energy Type CheckedListBox
            Label energyTypeLabel = new Label
            {
                Text = "Select Energy Types:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(160, 10),
                AutoSize = true
            };
            this.Controls.Add(energyTypeLabel);
            // Title for Chart
            Label chartLabel = new Label
            {
                Text = "Swiss Energy Production",
                Font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Underline),
                Location = new Point(778, 14),
                AutoSize = true
            };
            this.Controls.Add(chartLabel);
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
                energyTypeList.Items.Add(item, true);
            }

            energyTypeList.ItemCheck += (sender, e) =>
            {
                if (energyTypeList.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked)
                {
                    e.NewValue = CheckState.Checked;
                    MessageBox.Show("At least one energy type must be selected.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
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
