using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Objects;
using System.Data;
using System.IO;


namespace MODEOUTLED.Controllers.Admins.GroupSupport
{
    public class GroupSupportController : Controller
    {
        wwwEntities db = new wwwEntities();


        #region[Index]
        public ActionResult GroupSupportIndex(string sortOrder, string sortName, int? page, string currentFilter, string searchString)
        {
            var all = db.GroupSupports.OrderByDescending(groupSupport => groupSupport.Id).ToList();

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;

            #region order
            ViewBag.CurrentSortName = sortName;
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

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
            #endregion order

            
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            
            #region getPageSize
            // begin [get last page]
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
            //end [get last page]
            #endregion getPageSize


            #region PageList
            // Thiết lập phân trang
            PagedListRenderOptions pro = new PagedListRenderOptions();
            pro.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToIndividualPages = true;
            pro.DisplayPageCountAndCurrentLocation = false;
            pro.MaximumPageNumbersToDisplay = 5;
            pro.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            pro.EllipsesFormat = "&#8230;";
            pro.LinkToFirstPageFormat = "Trang đầu";
            pro.LinkToPreviousPageFormat = "«";
            pro.LinkToIndividualPageFormat = "{0}";
            pro.LinkToNextPageFormat = "»";
            pro.LinkToLastPageFormat = "Trang cuối";
            pro.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            pro.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            pro.FunctionToDisplayEachPageNumber = null;
            pro.ClassToApplyToFirstListItemInPager = null;
            pro.ClassToApplyToLastListItemInPager = null;
            pro.ContainerDivClasses = new[] { "pagination-container" };
            pro.UlElementClasses = new[] { "pagination" };
            pro.LiElementClasses = Enumerable.Empty<string>();
            ViewBag.Pro = pro;
            #endregion Pagelist

            PagedList<onsoft.Models.GroupSupport> gSupport = (PagedList<onsoft.Models.GroupSupport>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_GroupSupportData", gSupport);
            else
                return View(gSupport);

            
        }
        #endregion       

        #region Create
        #region[GroupSupportCreate]
        public ActionResult GroupSupportCreate()
        {
            return View();
        }
        #endregion

        #region[GroupSupportCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupSupportCreate(FormCollection collection, onsoft.Models.GroupSupport groupsp)
        {
            if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];
                var Ord = collection["Ord"];
                groupsp.Name = Name;
                groupsp.Ord = int.Parse(Ord);
                groupsp.Active = (collection["Active"] == "false") ? false : true;
                db.GroupSupports.Add(groupsp);
                db.SaveChanges();
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #endregion

        #region Delete
        #region[GroupSupportDelete]
        public ActionResult GroupSupportDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.GroupSupports.SingleOrDefault(p => p.Id == id);
                //if (del.SpTon == 0)
                //{





                db.GroupSupports.Remove(del);
                db.SaveChanges();
                //}
                //else
                //{
                //    Session["DeletePro"] = "Sản phẩm " + del.Name + "  vẫn còn trong kho! Không được xóa!";
                //}

                List<onsoft.Models.GroupSupport> groupSupport = db.GroupSupports.ToList();

                if ((groupSupport.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("GroupSupportIndex");
                    }
                }

                return RedirectToAction("GroupSupportIndex", new { page = page});
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        
        #endregion

        #region Edit
        #region[GroupSupportEdit]
        public ActionResult GroupSupportEdit(int id)
        {
            var groupSupportEdit = db.GroupSupports.SingleOrDefault(m => m.Id == id);
            return View(groupSupportEdit);
        }
        #endregion

        #region[GroupSupportEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupSupportEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var groupsp = db.GroupSupports.First(model => model.Id == id);
                var Name = collection["Name"];
                var Ord = collection["Ord"];
                groupsp.Name = Name;
                groupsp.Ord = int.Parse(Ord);
                groupsp.Active = (collection["Active"] == "false") ? false : true;
                db.SaveChanges();
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #endregion

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (collect["btnDeleteAll"] != null)
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
                                var Del = (from emp in db.GroupSupports where emp.Id == id select emp).SingleOrDefault();
                                db.GroupSupports.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));

                            var Up = db.GroupSupports.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }
                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                //db.ObjectStateManager.ChangeObjectState(provider, EntityState.Modified);
                                db.SaveChanges();
                            }

                        }
                    }
                    //return RedirectToAction("ProductIndexot", new { page = m });
                }
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        
        #region[GroupSupportActive]
        public ActionResult GroupSupportActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from news in db.GroupSupports where news.Id == id select news).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion



        #region Backup
        #region[GroupSupportIndex]
        public ActionResult GroupSupportIndexot(string sortOrder, string sortName, int? page, string currentFilter, string searchString)
        {

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;

            ViewBag.CurrentSortName = sortName;

            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

            var all = db.GroupSupports.OrderByDescending(groupSupport => groupSupport.Ord).ToList();

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

            //if (Session["DeletePro"] != null)
            //{
            //    var a = Session["DeletePro"].ToString();
            //    ViewBag.DelErr = "<p class='require'>" + a + "</p>";
            //    Session["DeletePro"] = null;
            //}


            int pageSize = 5;
            int pageNumber = (page ?? 1);

            // begin [get last page]
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
            //end [get last page]




            // Thiết lập phân trang
            PagedListRenderOptions pro = new PagedListRenderOptions();


            pro.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            pro.DisplayLinkToIndividualPages = true;
            pro.DisplayPageCountAndCurrentLocation = false;
            pro.MaximumPageNumbersToDisplay = 5;
            pro.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            pro.EllipsesFormat = "&#8230;";
            pro.LinkToFirstPageFormat = "Trang đầu";
            pro.LinkToPreviousPageFormat = "«";
            pro.LinkToIndividualPageFormat = "{0}";
            pro.LinkToNextPageFormat = "»";
            pro.LinkToLastPageFormat = "Trang cuối";
            pro.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            pro.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            pro.FunctionToDisplayEachPageNumber = null;
            pro.ClassToApplyToFirstListItemInPager = null;
            pro.ClassToApplyToLastListItemInPager = null;
            pro.ContainerDivClasses = new[] { "pagination-container" };
            pro.UlElementClasses = new[] { "pagination" };
            pro.LiElementClasses = Enumerable.Empty<string>();


            ViewBag.Pro = pro;

            return View(all.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region[GroupSupportIndex_back]
        public ActionResult GroupSupportIndex_back()
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
            var all = db.GroupSupports.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = onsoft.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion

        #region[GroupSupportCreate_back]
        public ActionResult GroupSupportCreate_back()
        {
            return View();
        }
        #endregion

        #region[GroupSupportDelete_back]
        public ActionResult GroupSupportDelete_back(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from news in db.GroupSupports where news.Id == id select news).Single();
                db.GroupSupports.Remove(del);
                db.SaveChanges();
                return RedirectToAction("GroupSupportIndex");
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
                            var Del = (from emp in db.GroupSupports where emp.Id == id select emp).SingleOrDefault();
                            db.GroupSupports.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("GroupSupportIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        
        #endregion

    }
}
