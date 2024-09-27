using App.Models;
namespace App.ViewModel
{
    public class DataCollectionViewModel
    {
        public List<EnergyDataViewModel> list;

    }
    public class EnergyDataViewModel 
    {
        public int? Month;
        public int? Year;
        public double? Production;
    }
}
