using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class SupplierController : Controller
    {
         wwwEntities db = new wwwEntities();
        #region[SupplierIndex]
        public ActionResult SupplierIndex()
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
            var all = db.Suppliers.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = db.sp_Supplier_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (pages.Count > 0)
            {
                ViewBag.Pager = Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            return View(pages);
        }
        #endregion
        #region[SupplierCreate]
        public ActionResult SupplierCreate()
        {
            ViewBag.IdCountry = new SelectList(db.Countries, "Id","Name");
            return View();
        }
        #endregion
        #region[SupplierCreate]
        [HttpPost]
        public ActionResult SupplierCreate(FormCollection collect, Supplier sup)
        {
            if (Request.Cookies["Username"] != null)
            {
                sup.Name = collect["Name"];
                sup.Tel = collect["Tel"];
                sup.Email = collect["Email"];
                sup.Website = collect["Website"];
                sup.Address = collect["Address"];
                sup.IdCountry = int.Parse(collect["IdCountry"]);
                db.Suppliers.Add(sup);
                db.SaveChanges();
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupplierEdit]
        public ActionResult SupplierEdit(int id)
        {
            var Edit = db.Suppliers.First(m => m.Id == id);
            ViewBag.IdCountry = new SelectList(db.Countries, "Id", "Name",Edit.IdCountry);
            return View(Edit);
        }
        #endregion
        #region[SupplierEdit]
        [HttpPost]
        public ActionResult SupplierEdit(FormCollection collect, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var sup = db.Suppliers.First(m => m.Id == id);
                sup.Name = collect["Name"];
                sup.Tel = collect["Tel"];
                sup.Email = collect["Email"];
                sup.Website = collect["Website"];
                sup.Address = collect["Address"];
                sup.IdCountry = int.Parse(collect["IdCountry"]);
                db.SaveChanges();
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[SupplierDelete]
        public ActionResult SupplierDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.Suppliers.First(m => m.Id == id);
                db.Suppliers.Remove(del);
                db.SaveChanges();
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiDelete]
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
                            var Del = (from emp in db.Suppliers where emp.Id == id select emp).SingleOrDefault();
                            db.Suppliers.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
