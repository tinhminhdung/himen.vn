using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace onsoft.Controllers
{
    public class TestController : Controller
    {
         wwwEntities db = new wwwEntities();
        public ActionResult Index(string gid)
        {
            string chuoi = "";
            var gro = (from p in db.GroupProducts select p).ToList();
            if (gid != null)
            {
                int ggid = int.Parse(gid);
                var pro = (from p in db.Products where p.IdCategory == ggid select p).Take(5).ToList();
                for (int i = 0; i < pro.Count; i++)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + pro[i].Name + "</td>";
                    chuoi += "<td>" + pro[i].PricePromotion.ToString() + "</td>";
                    chuoi += "<td><img src=\"" + pro[i].Image + "\" style=\"width: 30px\" /></td>";
                    chuoi += "</tr>";
                }
            }
            else
            {
                var pro = (from p in db.Products select p).Take(5).ToList();
                for (int i = 0; i < pro.Count; i++)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + pro[i].Name + "</td>";
                    chuoi += "<td>" + pro[i].PricePromotion.ToString() + "</td>";
                    chuoi += "<td><img src=\"" + pro[i].Image + "\" style=\"width: 30px\" /></td>";
                    chuoi += "</tr>";
                }
            }
            ViewBag.name = chuoi;
            ViewBag.cat = new SelectList(gro, "Id", "Name");
            if (Request.IsAjaxRequest())
                return View();

            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Pa1()
        {
            string chuoi = "";
            var pro = (from p in db.Products select p).Take(5).ToList();
            for (int i = 0; i < pro.Count; i++)
            {
                chuoi += "<tr>";
                chuoi += "<td>" + pro[i].Name + "</td>";
                chuoi += "<td>" + pro[i].PricePromotion.ToString() + "</td>";
                chuoi += "<td><img src=\"" + pro[i].Image + "\" style=\"width: 30px\" /></td>";
                chuoi += "</tr>";
            }
            ViewBag.name = chuoi;
            return PartialView();
        }

    }
}
