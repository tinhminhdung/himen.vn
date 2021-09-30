using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class ImportController : Controller
    {
         wwwEntities db = new wwwEntities();
        public ActionResult ImportIndex()
        {
            return View();
        }


        public ActionResult ImportDetails(int id)
        {
            return View();
        }

        public ActionResult ImportCreate()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult ImportCreate(FormCollection collection)
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

        
        public ActionResult ImportEdit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportEdit(int id, FormCollection collection)
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

        
        public ActionResult ImportDelete(int id)
        {
            return View();
        }

        
    }
}
