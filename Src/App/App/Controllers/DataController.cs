using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.Tools;
using System.Data;
using System.Linq;
using System.Text;
using System;
namespace App.Controllers
{
    public class DataController : Controller
    {
        // GET: DataController
        public ActionResult Data()
        {
            DataTable dataTable = DataManager.ConvertCsvToDataTable("C:\\Users\\po01imj\\Documents\\Github\\P_FUN_323\\Data\\5634-Zeitreihe_Elektrizitätsbilanz_Schweiz_Monatswerte.csv");

            var dataList = DataManager.ConvertDataTableToList(dataTable);
            dataList.RemoveRange(0, 9);

            var niceFormatedData = dataList.Select(list => list[0]);


            return View(dataList);
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
