namespace App.Models
{
    public class EnergyData
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Hydropower { get; set; }
        public int? NuclearPower { get; set; }
        public int? ThermalPower { get; set; }
        public int? TotalProduction { get; set; }
        public int? PumpingStorage { get; set; }
        public int? NetProduction { get; set; }
        public int? Import { get; set; }
        public int? Export { get; set; }
        public int? DomesticConsumption { get; set; }
        public int? Losses { get; set; }
        public int? FinalConsumption { get; set; }
        public int? ExportImportBalance { get; set; }
    }
}