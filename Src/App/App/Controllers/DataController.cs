using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using App.Tools;
using App.ViewModel;
using App.Models;


namespace App.Controllers
{

    public class DataController : Controller
    {
        // GET: DataController
        public ActionResult Data()
        {
            string filename = "C:\\Users\\po01imj\\Documents\\Github\\P_FUN_323\\Data\\5634-Zeitreihe_Elektrizitätsbilanz_Schweiz_Monatswerte.csv";

            CsvReader reader = new CsvReader();
            var energyDataList = reader.ReadCsv(filename);
            DataViewModel dataViewModel = new DataViewModel();
            dataViewModel.Data = energyDataList
                .Where(a => a.Year >= 1995 && a.Year <= 2000)
                .GroupBy(a => a.Year)
                .Select(g => new Dictionary{ 
                    Year = g.Key ,
                    HydroProduction = g.Average(p => p.Hydropower)})
                .ToList();

            List<Tuple<int, double?>> hydropower = new List<Tuple<int, double?>>();
            foreach (var item in hydropowerFrom1995To2000)
            {
                hydropower.Add(new Tuple<int, double?>(item.Year, item.HydroProduction));
            }
            Debug.WriteLine(hydropower.GetType());

            return View(hydropower);
        }

        // GET: DataController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DataController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DataController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DataController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DataController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DataController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
