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
    public class CSVController : Controller
    {
        private IManager mgr = new Manager();

        // GET: CSV
        public ActionResult Index()
        {
            //Holder holder = mgr.AddHolder("H0001", "someperson");
            List<InputHolder> inputHolderList = new List<InputHolder>();
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
                    // Send file to BL for processing
                    // Deze List doorsturen naar view voor overzicht, bij 'ok' commit naar DB
                    inputHolderList = mgr.ProcessFile(file);
                    int serialdate = inputHolderList[0].BeginDatumSerial;
                    //DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(epoch);
                    DateTime date = DateTime.FromOADate(serialdate);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View(inputHolderList);
        }
    }
}