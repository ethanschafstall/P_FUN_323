using Plot_That_Line.Models;
using Plot_That_Line.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Plot_That_Line.Tools
{
    internal class EnergyDataService
    {
        private List<EnergyData> energyDataList_;

        public EnergyDataService(string filePath)
        {
            energyDataList_ = Load_Data(filePath);
        }

        public List<EnergyDataViewModel> GetData(FilterModel filter)
        {
            
            // Optional (Nullable) filters, have a default value if user doesn't use them.
            int? startYear = filter.StartYear.HasValue ? filter.StartYear.Value : energyDataList_.Select(d => d.Year).Min();
            int? endYear = filter.EndYear.HasValue ? filter.EndYear.Value : energyDataList_.Select(d => d.Year).Max();


            // Anon obj list with avg values for each energy type & total for each year key
            var filteredData = energyDataList_
                .Where(data => data.Year.HasValue && data.Year >= startYear && data.Year <= endYear) /* Where for if Year is not null, and if it's within filter range */
                .GroupBy(data => data.Year)
                .Select(g => new EnergyDataViewModel
                {
                    Year = (int)g.Key,
                    HydroProduction = filter.ShowHydro ? g.Average(data => data.Hydropower ?? 0) : (double?)null, /* Not sure why I need the cast here but it won't work otherwise */
                    ThermalProduction = filter.ShowThermal ? g.Average(data => data.ThermalPower ?? 0) : (double?)null,
                    NulcearProduction = filter.ShowNuclear ? g.Average(data => data.NuclearPower ?? 0) : (double?)null,
                    TotalProduction = (filter.ShowNuclear && filter.ShowThermal && filter.ShowHydro) ? g.Average(data => data.NuclearPower ?? 0) : (double?)null
                })
                .OrderBy(g => g.Year)
                .ToList();

            return filteredData;
        }

        private List<EnergyData> Load_Data(string dataPath)
        {
            return new CsvReader().ReadCsv(dataPath);
        }
    }


}
