using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class RoleController : Controller
    {
         wwwEntities db = new wwwEntities();
        public ActionResult RoleIndex()
        {
            return View();
        }



        public ActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RoleCreate(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("RoleIndex");
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult RoleEdit(int id)
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult RoleEdit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("RoleIndex");
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult RoleDelete(int id)
        {
            return View();
        }

        
    }
}
