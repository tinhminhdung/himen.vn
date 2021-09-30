using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class BankController : Controller
    {
         wwwEntities db = new wwwEntities();
        public ActionResult BankIndex()
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
            var all = db.Banks.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var product = db.sp_Colors_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (numOfNews > 0)
            {
                ViewBag.Pager = Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
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

        public ActionResult BankCreate()
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

        [HttpPost]
        public ActionResult BankCreate(FormCollection collection, Bank bak)
        {
             if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];
                var Img = collection["Image"];
                bak.Name = Name;
                bak.Image = Img;
                bak.Active = (collection["Active"] == "false") ? false : true;
                db.Banks.Add(bak);
                db.SaveChanges();
                return RedirectToAction("BankIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }

        }

        public ActionResult BankEdit(int id)
        {
              if (Request.Cookies["Username"] != null)
            {
            var bak = db.Banks.Where(m => m.Id == id).FirstOrDefault();
            return View(bak);
            }
              else
              {
                  return Redirect("/Admins/admins");
              }
        }

        [HttpPost]
        public ActionResult BankEdit(int id, FormCollection collection)
        {
             if (Request.Cookies["Username"] != null)
            {
                var bak = db.Banks.Where(m => m.Id == id).FirstOrDefault();
                var Name = collection["Name"];
                var Img = collection["Image"];
                bak.Name = Name;
                bak.Image = Img;
                bak.Active = (collection["Active"] == "false") ? false : true;
                db.SaveChanges();
                return RedirectToAction("BankIndex");
            }
             else
             {
                 return Redirect("/Admins/admins");
             }
        }

        public ActionResult BankDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
            var bak = db.Banks.Where(m => m.Id == id).FirstOrDefault();
            db.Banks.Remove(bak);
            db.SaveChanges();
             return RedirectToAction("BankIndex");
            }
             else
             {
                 return Redirect("/Admins/admins");
             }
        }

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
                            var Del = (from emp in db.Banks where emp.Id == id select emp).SingleOrDefault();
                            db.Banks.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("BankIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        //
        #region[BankActive]
        public ActionResult BankActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from news in db.Banks where news.Id == id select news).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return RedirectToAction("BankIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        // 
        #region[BankView]
        public ActionResult BankView(int id = 0, string bankname="")
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
            if (id != 0)
            {
                Session["IdBank"] = id.ToString();
            }
            var a = Session["IdBank"].ToString();
            var all = db.AccBanks.AsEnumerable().Where(m => m.IdBank == int.Parse(Session["IdBank"].ToString())).ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (numOfNews > 0)
            {
                ViewBag.NameBank = bankname;
                ViewBag.Pager = Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            else
            {
                ViewBag.NameBank = bankname;
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

        //
        public ActionResult AccBankCreate()
        { if (Request.Cookies["Username"] != null)
            {
            
             return View();
             }
             else
             {
                 return Redirect("/Admins/admins");
             }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AccBankCreate(FormCollection colect, AccBank acc)
        {
            if (Request.Cookies["Username"] != null)
            {
            acc.IdBank = int.Parse(Session["IdBank"].ToString());
            acc.Account = colect["Account"];
            acc.Number = colect["Number"];
            acc.Description = colect["Description"];
            acc.Ord = int.Parse(colect["Ord"]);
            acc.Active = (colect["Active"] == "false") ? false : true;
            db.AccBanks.Add(acc);
            db.SaveChanges();
            return Redirect("/Bank/BankView/" + Session["IdBank"].ToString() + "");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        public ActionResult AccBankEdit(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var acc = db.AccBanks.AsEnumerable().Where(m => m.Id == id).FirstOrDefault();
                return View(acc);
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AccBankEdit(FormCollection colect, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var acc = db.AccBanks.AsEnumerable().Where(m => m.Id == id).FirstOrDefault();
                acc.IdBank = int.Parse(Session["IdBank"].ToString());
                acc.Account = colect["Account"];
                acc.Number = colect["Number"];
                acc.Description = colect["Description"];
                acc.Ord = int.Parse(colect["Ord"]);
                acc.Active = (colect["Active"] == "false") ? false : true;
                db.SaveChanges();
                return Redirect("/Bank/BankView/" + Session["IdBank"].ToString() + "");
            }
            else
            {
                return Redirect("/Admins/admins");
            }

        }
        public ActionResult AccBankDelete(int id)
        {
            var acc = db.AccBanks.AsEnumerable().Where(m => m.Id == id).FirstOrDefault();
            db.AccBanks.Remove(acc);
            db.SaveChanges();
            return Redirect("/Bank/BankView/" + Session["IdBank"].ToString() + "");
        }

        #region[AccMultiDelete]
        [HttpPost]
        public ActionResult AccMultiDelete()
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
                            var Del = (from emp in db.AccBanks where emp.Id == id select emp).SingleOrDefault();
                            db.AccBanks.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return Redirect("/Bank/BankView/" + Session["IdBank"].ToString() + "");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[AccBankActive]
        public ActionResult AccBankActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from news in db.AccBanks where news.Id == id select news).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return Redirect("/Bank/BankView/" + Session["IdBank"].ToString() + "");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
    }
}
