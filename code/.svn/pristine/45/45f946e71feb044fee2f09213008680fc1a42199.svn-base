using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class CountryController : Controller
    {
         wwwEntities db = new wwwEntities();
        #region[CountryIndex]
        public ActionResult CountryIndex()
        {
            if (Request.Cookies["Username"] != null)
            {
                string page = "1";//so phan trang hien tai
                var pagesize = 25;//so ban ghi tren 1 trang
                var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
                int curpage = 0; // trang hien tai dung cho phan trang
                if (Request["page"] != null)
                {
                    page = Request["page"];
                    curpage = Convert.ToInt32(page) - 1;
                }
                var all = (from adv in db.Countries select adv).ToList();
                var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
                //var pages = data.sp_Advertise_Phantrang(page, pagesize, "Position=" + position + "", "Id desc").ToList();
                var url = Request.Path;
                numOfNews = all.Count;
                if (numOfNews > 0)
                {
                    ViewBag.Pager = onsoft.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
                }
                else
                {
                    ViewBag.Pager = "";
                }
                return View(pages);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CountryCreate]
        public ActionResult CountryCreate()
        {
            if (Request.Cookies["Username"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CountryCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CountryCreate(FormCollection collection, Country pro)
        {
            if (Request.Cookies["Username"] != null)
            {
                pro.Name = collection["Name"];
                if (!collection["Ord"].Equals(""))
                {
                    pro.Ord = int.Parse(collection["Ord"]);
                }
                pro.Active = (collection["Active"] == "false") ? false : true;
                pro.Logo = collection["Logo"];
                pro.Description = collection["Description"];
                db.Countries.Add(pro);
                db.SaveChanges();
                return RedirectToAction("CountryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CountryEdit]
        public ActionResult CountryEdit(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var edit = db.Countries.AsEnumerable().Where(m => m.Id == id).FirstOrDefault();
                return View(edit);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CountryEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CountryEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var pro = db.Countries.AsEnumerable().Where(m => m.Id == id).FirstOrDefault();
                pro.Name = collection["Name"];
                pro.Ord = int.Parse(collection["Ord"]);
                pro.Active = (collection["Active"] == "false") ? false : true;
                pro.Logo = collection["Logo"];
                pro.Description = collection["Description"];
                db.SaveChanges();
                return RedirectToAction("CountryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region [CountryDelete]
        public ActionResult CountryDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var delete = db.Countries.AsEnumerable().Where(m => m.Id == id).FirstOrDefault();
                db.Countries.Remove(delete);
                db.SaveChanges();
                return RedirectToAction("CountryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[CountryActive]
        public ActionResult CountryActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from news in db.Countries where news.Id == id select news).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return RedirectToAction("CountryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiDelete]
        [HttpPost]
        public ActionResult MultiDelete()
        {
            if (Request.Cookies["Username"] != null)
            {
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in db.Countries where emp.Id == id select emp).SingleOrDefault();
                            db.Countries.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("CountryIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

    }
}
