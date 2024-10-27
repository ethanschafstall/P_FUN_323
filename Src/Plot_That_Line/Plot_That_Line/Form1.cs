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








            {
                {
            };


            {
            {
            });


        private static string GetDataPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(baseDir, @"..\..\..\..\..\Data\5634-Zeitreihe_Elektrizitätsbilanz_Schweiz_Monatswerte.csv");
            return Path.GetFullPath(relativePath);
        }
    }
}
