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
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // TODO : Add security restrictions
            if (file != null && file.ContentLength > 0)
                try
                {
                    // Send file to BL for processing
                    mgr.ProcessFile(file);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
    }
}