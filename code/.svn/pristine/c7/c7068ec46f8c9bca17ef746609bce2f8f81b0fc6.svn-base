using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class PageController : Controller
    {
         wwwEntities db = new wwwEntities();
        public ActionResult NotLogon()
        {
            return View();
        }
        public ActionResult pagedetail(string tag)
        {
            string chuoi = "";

            var module = db.Menus.Where(m => m.Tag == tag).ToList();
            if (module.Count > 0)
            {
                chuoi = module[0].Name;
            }
            ViewBag.menu = chuoi;
            return View();
        }
        public ActionResult Order_Pay()
        {
            return View();
        }
        public ActionResult Order_Success()
        {
            return View();
        }
        public ActionResult SearchNull()
        {
            return View();
        }
    }
}
