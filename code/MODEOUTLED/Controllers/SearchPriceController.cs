using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class SearchPriceController : Controller
    {
         wwwEntities db = new wwwEntities();
        public ActionResult SearchPriceIndex()
        {
            return View();
        }

        public ActionResult SearchPriceCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchPriceCreate(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("SearchPriceIndex");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SearchPriceEdit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchPriceEdit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("SearchPriceIndex");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult SearchPriceDelete(int id)
        {
            return View();
        }

       
    }
}
