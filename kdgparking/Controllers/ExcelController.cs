using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using kdgparking.BL;
using kdgparking.BL.Domain;

using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

namespace kdgparking.Controllers
{
    public class ExcelController : Controller
    {
        private IManager mgr = new Manager();

        // GET: Excel
        public ActionResult Index()
        {
            List<InputHolder> inputHolderList = new List<InputHolder>();
            if (TempData["report"] != null)
            {
                ViewBag.Message = TempData["report"].ToString();
            }
            return View(inputHolderList);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            List<InputHolder> inputHolderList = new List<InputHolder>();
            // TODO : Add security restrictions
            if (file != null && file.ContentLength > 0)
                try
                {
                    // Send file to BL for processing and validation
                    // Deze List doorsturen naar view voor overzicht, bij 'ok' commit naar DB
                    inputHolderList = mgr.ProcessFile(file);
                    TempData["myModel"] = inputHolderList;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR: " + ex.Message.ToString() + " Upoad wordt onderbroken.";
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View(inputHolderList);
        }

        //[HttpPost]
        public ActionResult Commit()
        {
            // InputHolder data van excel
            List<InputHolder> model = TempData["myModel"] as List<InputHolder>;
            // Elke InputHolder van excel omzetten naar respectievelijke klasse
            List<InputHolder> holderList = mgr.ProcessInputholderList(model);
            
            return View(holderList);
        }

        public ActionResult CsvExport()
        {
            // TODO : Testen of Holder &Vehicle Distinct zijn
            try
            {

                IEnumerable<Vehicle> vehicles = mgr.GetVehicles();
                string csvToFilepath = mgr.CsvExport(vehicles);
                TempData["report"] = "Succesfully exported CSV to " + csvToFilepath;
            }
            catch (Exception ex)
            {
                TempData["report"] = "Export failed : " + ex;
            }

            return RedirectToAction("Index");
        }
    }
}