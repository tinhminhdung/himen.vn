using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class ShopController : Controller
    {
        //ModeoutleddbContext data = new ModeoutleddbContext();
           wwwEntities data = new wwwEntities();
        #region[ShopIndex]
        public ActionResult ShopIndex()
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
            var all = data.Shops.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = data.sp_Shop_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = onsoft.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        #region[ShopCreate]
        public ActionResult ShopCreate()
        {
            return View();
        }
        #endregion
        #region[ShopCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ShopCreate(FormCollection collec, Shop shop)
        {
            if (Request.Cookies["Username"] != null)
            {
                shop.Name = collec["Name"];
                shop.Address = collec["Address"];
                shop.Tel = collec["Tel"];
                shop.Description = collec["Description"];
                if (collec["SDate"] == "") { shop.EDate = null; } else { shop.EDate = DateTime.Parse(collec["SDate"]); }
                if (collec["EDate"] == "") { shop.EDate = null; } else { shop.EDate = DateTime.Parse(collec["EDate"]); }
                data.Shops.Add(shop);
                data.SaveChanges();
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ShopEdit]
        public ActionResult ShopEdit(int id)
        {
            var Edit = data.Shops.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[ShopEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ShopEdit(FormCollection collec, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var shop = data.Shops.First(m => m.Id == id);
                shop.Name = collec["Name"];
                shop.Address = collec["Address"];
                shop.Tel = collec["Tel"];
                shop.Description = collec["Description"];
                if (collec["SDate"] == "") { shop.EDate = null; } else { shop.EDate = DateTime.Parse(collec["SDate"]); }
                if (collec["EDate"] == "") { shop.EDate = null; } else { shop.EDate = DateTime.Parse(collec["EDate"]); }
                data.SaveChanges();
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[ShopDelete]
        public ActionResult ShopDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = data.Shops.First(m => m.Id == id);
                data.Shops.Remove(del);
                data.SaveChanges();
                return RedirectToAction("ShopIndex");
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
                string str = "";
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in data.Shops where emp.Id == id select emp).SingleOrDefault();
                            data.Shops.Remove(Del);
                            str += id.ToString() + ",";
                            data.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("ShopIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

       
    }
}
