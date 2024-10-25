using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plot_That_Line.Models
{
    internal class FilterModel
    {
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public List<int> SelectedMonths { get; set; }
        public bool ShowHydro { get; set; }
        public bool ShowThermal { get; set; }
        public bool ShowNuclear { get; set; }
    }
}
