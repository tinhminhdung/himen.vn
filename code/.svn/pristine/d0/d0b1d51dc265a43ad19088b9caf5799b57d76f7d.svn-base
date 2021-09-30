using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using onsoft.Models;
using MODEOUTLED.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using PagedList;
using PagedList.Mvc;


namespace MODEOUTLED.Controllers.Admins.New
{
    public class NewsController : Controller
    {
        wwwEntities db = new wwwEntities();
        #region[NewsIndexot]
        public ActionResult NewsIndexot(string sortOrder, string sortName, string sortGroup, int? page, string currentNewsCodeFilter, string currentNewsNameFilter, string Newsname, string currentGroupNewsFilter, string GroupNews, string currentNewsAmount)
        {

            if (Request.HttpMethod == "GET")
            {
                Newsname = currentNewsNameFilter;
                GroupNews = currentGroupNewsFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentNewsNameFilter = Newsname;
            ViewBag.currentGroupNewsFilter = GroupNews;

            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

            // Sort Name (Truyền sortName khi phân trang)
            if (sortName != "")
            {
                ViewBag.CurrentSortName = sortName;
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            }
            // Sort Name (Truyền sortName khi sắp xếp)
            if (Session["sortName"] != null)
            {
                sortName = Session["sortName"].ToString();
                ViewBag.CurrentSortName = Session["sortName"].ToString();
                Session["sortName"] = null;
            }
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

            ViewBag.CurrentSortGroup = sortGroup;
            ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";

            var all = db.sp_News_GetByAll().OrderByDescending(p => p.Id).ToList();

            #region[Tìm kiếm]
            // Tìm theo tên Tin tức
            if (!String.IsNullOrEmpty(Newsname))
            {
                if (Newsname != "Tiêu đề tin")
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(Newsname.ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
            }

            // Tìm theo nhóm Tin tức
            if (!String.IsNullOrEmpty(GroupNews))
            {
                int groupid = Int32.Parse(GroupNews);
                all = all.Where(p => p.IdGroup == groupid).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["Namesearch"] != null)
            {
               all = all.Where(p => p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["GroupNews"] != null)
            {
                if (Session["Namesearch"] != null)
                {
                    all = all.Where(p => p.IdGroup == int.Parse(Session["GroupNews"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                else
                {
                    all = all.Where(p => p.IdGroup == int.Parse(Session["GroupNews"].ToString())).OrderByDescending(p => p.Id).ToList();
                }
                Session["GroupNews"] = null;
            }
            if (Session["Namesearch"] != null) { Session["Namesearch"] = null; }
            #endregion

            #region[Sắp xếp]
            switch (sortOrder)
            {
                case "ord desc":
                    all = all.OrderByDescending(p => p.Ord).ToList();
                    break;
                case "ord asc":
                    all = all.OrderBy(p => p.Ord).ToList();
                    break;
                default:
                    break;
            }

            switch (sortName)
            {
                case "name desc":
                    all = all.OrderByDescending(p => p.Name).ToList();
                    break;
                case "name asc":
                    all = all.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    break;
            }
            #endregion

            if (Session["DeletePro"] != null)
            {
                var a = Session["DeletePro"].ToString();
                ViewBag.DelErr = "<p class='require'>" + a + "</p>";
                Session["DeletePro"] = null;
            }

            int pageSize = 20;
            if (currentNewsAmount != null && currentNewsAmount != "")
            {
                pageSize = Convert.ToInt32(currentNewsAmount);
            }
            int pageNumber = (page ?? 1);

            #region[Get Last Page]
            if (page != null)
            {
                ViewBag.mPage = (int)page;
            }
            else
            {
                ViewBag.mPage = 1;
            }


            int lastPage = all.Count / pageSize;
            if (all.Count % pageSize > 0)
            {
                lastPage++;
            }
            ViewBag.LastPage = lastPage;

            ViewBag.PageSize = pageSize;
            #endregion

            #region[view drop search]
            var cat = db.GroupNews.Where(n => n.Active == true).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.GroupNews = new SelectList(cat, "Id", "Name");
            }
            #endregion

            #region[Số Tin tức hiển thị trên trang]
            // Số Tin tức hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Tin tức / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Tin tức / trang", Value = i + "" });
                }
            }
            ViewBag.ddlNewsAmount = items;
            #endregion

            ViewBag.TotalNews = db.News.Count();

            // Phân trang
            PagedList<sp_News_GetByAll_Result> Newss = (PagedList<sp_News_GetByAll_Result>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialNewsIndex", Newss);

            return View(Newss);
        }
        #endregion
        #region[NewsCreateot]
        public ActionResult NewsCreateot()
        {

            string chuoi = "";
            var cat = db.GroupNews.Where(n => n.Active == true).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.GroupNews = new SelectList(cat, "Id", "Name");
            }
            return View();
        }
        #endregion
        #region[NewsCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsCreate(FormCollection collection, News news, HttpPostedFileBase fileImg)
        {
            int idmem = 0;
            if (Session["uId"] != null) { idmem = int.Parse(Session["uId"].ToString()); }
            if (Request.Cookies["Username"] != null)
            {
                news.Name = collection["Name"];
                news.Tag = StringClass.NameToTag(collection["Name"]);
                news.Image = collection["Image"];
                news.Content = collection["Content"];
                news.Detail = collection["Detail"];
                int GroupNews = Convert.ToInt32(collection["GroupNews"]);
                news.IdGroup = GroupNews;
                news.Keyword = collection["Keyword"];
                news.Description = collection["Description"];
                news.Title = collection["Title"];
                news.Ord = Convert.ToInt32(collection["Ord"]);
                news.Index = (collection["Index"] == "false") ? false : true;
                news.Active = (collection["Active"] == "false") ? false : true;
                news.View = 0;//so luot xem
                news.SDate = DateTime.Now;
                db.sp_News_Insert(news.Name, news.Tag, news.Title, news.Keyword, news.Description, news.Image, news.SDate, news.Content, news.Detail, news.Index, news.View, news.IdGroup, news.Ord, news.Active);
                db.SaveChanges();
                return RedirectToAction("NewsIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[NewsEditot]
        public ActionResult NewsEditot(int id)
        {
            string chuoi = "";
            var Edit = db.News.First(m => m.Id == id);
            var cat = db.GroupNews.Where(n => n.Active == true).ToList();
            ViewBag.GroupNew = new SelectList(cat, "Id", "Name", Edit.IdGroup);
            return View(Edit);
        }
        #endregion
        #region[NewsEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsEdit(int id, FormCollection collection, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                string Name = collection["Name"];
                string Tag = StringClass.NameToTag(collection["Name"]);
                string Image = collection["Image"];
                string Content = collection["Content"];
                string Detail = collection["Detail"];
                int IdGroup = Convert.ToInt32(collection["GroupNew"]);
                string Keyword = collection["Keyword"];
                string Description = collection["Description"];
                string Title = collection["Title"];
                int Ord = Convert.ToInt32(collection["Ord"]);
                bool Index = (collection["Index"] == "false") ? false : true;
                bool Active = (collection["Active"] == "false") ? false : true;
                int View = 0;//so luot xem
                DateTime SDate = DateTime.Now;
                db.sp_News_Update(id, Name, Tag, Title, Keyword, Description, Image, SDate, Content, Detail, Index, View, IdGroup, Ord, Active);
                db.SaveChanges();
                return RedirectToAction("NewsIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[NewsDelete]
        public ActionResult NewsDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.News.First(p => p.Id == id);
                db.News.Remove(del);
                db.SaveChanges();
                List<News> news = db.News.ToList();
                if ((news.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("NewsIndexot");
                    }
                }

                return RedirectToAction("NewsIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiCommand]
        public ActionResult MultiCommand(FormCollection collect)
        {
            int m = int.Parse(collect["mPage"]);
            int pagesize = int.Parse(collect["PageSize"]);

            List<News> news = db.News.ToList();
            int lastpage = news.Count / pagesize;
            if (news.Count % pagesize > 0)
            {
                lastpage++;
            }
            if (Request.Cookies["Username"] != null)
            {

                if (collect["btnDelete"] != null)
                {
                    //string str = "";
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.Contains("chk"))
                        {
                            if (key.Contains("chkIndex") || key.Contains("chkActive"))
                            {

                            }
                            else
                            {
                                checkbox = Request.Form["" + key];
                                if (checkbox != "false")
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 3));
                                    var Del = (from del in db.News where del.Id == id select del).SingleOrDefault();
                                    db.News.Remove(Del);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("NewsIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("NewsIndexot", new { page = m });
                }
                else if (collect["ddlNewsAmount"] != null)
                {
                    if (collect["ddlNewsAmount"].Length > 0)
                    {
                        Session["ddlNewsAmount"] = collect["ddlNewsAmount"];
                    }
                    return RedirectToAction("NewsIndexot");
                }

                else
                {
                    string Namesearch = collect["NewsName"];
                    string cat = collect["GroupNews"];
                    if (Namesearch.Length > 0)
                    {
                        Session["Namesearch"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["GroupNews"] = cat;
                    }
                    return RedirectToAction("NewsIndexot", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[Ajax Autocomplete for search]
        // Autocomplete for search productcode 
        [HttpGet]
        public ActionResult ProductAutocomplete(string term)
        {
            var productCodes = from p in db.Products
                               select p.Code;
            string[] items = productCodes.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region[Ajax Change Product]

        // AJAX: /Product/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var news = db.News.Find(id);
            if (news != null)
            {
                news.Active = news.Active == true ? false : true;
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }

        // AJAX: /Product/ChangeIndex
        [HttpPost]
        public ActionResult ChangeIndex(int id)
        {
            var news = db.News.Find(id);
            if (news != null)
            {
                news.Index = news.Index == true ? false : true;
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái hiển thị trang chủ đã được thay đổi.";
            return Json(results);
        }

        // AJAX: /Product/ChangeProduct
        [HttpPost]
        public ActionResult ChangeNews(int id, string name, string active, string index)
        {
            var results = "";
            var news = db.News.Find(id);
            if (news != null)
            {
                if (name != null)
                {
                    news.Name = name;
                    results = "Tiêu đề tin đã được thay đổi.";
                }
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }

        #endregion
        #region[Ajax LoadProductAmount]

        // AJAX: /Product/LoadProductAmount
        [HttpPost]
        public ActionResult LoadProductAmount(string productAmount, string sortName)
        {

            if (productAmount != null)
            {
                Session["ProductAmount"] = productAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("ProductIndexot");
        }

        #endregion
        #region[Ajax Sort Product]

        // AJAX: /Product/SortByName
        [HttpPost]
        public ActionResult SortByName(string sortName, string currentProductAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentProductAmount != null)
                {
                    Session["ProductAmount"] = currentProductAmount;
                }
            }
            return RedirectToAction("ProductIndexot");
        }

        #endregion
    }
}
