using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plot_That_Line.ViewModels
{
    internal class EnergyDataViewModel
    {
        public int Year { get; set; }
        public double? HydroProduction { get; set; }
        public double? ThermalProduction { get; set; }
        public double? NulcearProduction { get; set; }
        public double? TotalProduction { get; set; }
    }
}
