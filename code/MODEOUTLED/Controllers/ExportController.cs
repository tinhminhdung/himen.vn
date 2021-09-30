using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class ExportController : Controller
    {

         wwwEntities db = new wwwEntities();
        public ActionResult ExportIndex()
        {
            return View();
        }


        public ActionResult ExportDetails(int id)
        {
            return View();
        }

        public ActionResult ExportCreate()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult ExportCreate(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ExportEdit(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult ExportEdit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ExportDelete(int id)
        {
            return View();
        }

        
    }
}
