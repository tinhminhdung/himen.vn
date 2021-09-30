using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;


namespace MODEOUTLED.Controllers
{
    public class GroupMemberController : Controller
    {
         wwwEntities db = new wwwEntities();

        #region[GroupMemberIndex]
        public ActionResult GroupMemberIndex(int? page)
        {      
            var all = db.sp_GroupMember_GetByAll().OrderByDescending(p => p.Name).ToList();
           
            int pageSize = 2;
            int pageNumber = (page ?? 1);

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


        #region[GroupMemberIndex_back]
        public ActionResult GroupMemberIndex_back()
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
            var all = db.GroupMembers.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = db.sp_GroupMember_Phantrang(page, productize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = Phantrang.PhanTrang(25, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        
        
        #region[GroupMemberCreate]
        public ActionResult GroupMemberCreate()
        {
            return View();
        }
        #endregion
        #region[GroupMemberCreate]
        [HttpPost]
        public ActionResult GroupMemberCreate(FormCollection collect, GroupMember grMem)
        {
            if (Request.Cookies["Username"] != null)
            {
                grMem.Name = collect["Name"];
                Boolean Active = (collect["Active"] == "false") ? false : true;
                grMem.Active = Active;
                db.GroupMembers.Add(grMem);
                db.SaveChanges();
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupMemberEdit]
        public ActionResult GroupMemberEdit(int id)
        {
            var Edit = db.GroupMembers.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion
        #region[GroupMemberEdit]
        [HttpPost]
        public ActionResult GroupMemberEdit(int id, FormCollection collect)
        {
            if (Request.Cookies["Username"] != null)
            {
                var grMem = db.GroupMembers.First(m => m.Id == id);
                grMem.Name = collect["Name"];
                Boolean Active = (collect["Active"] == "false") ? false : true;
                grMem.Active = Active;
                db.SaveChanges();
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupMemberActive]
        public ActionResult GroupMemberActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = db.GroupMembers.First(m => m.Id == id);
                act.Active = (act.Active == true) ? false : true;
                db.SaveChanges();
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[GroupMemberDelete]
        public ActionResult GroupMemberDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.GroupMembers.First(m => m.Id == id);
                db.GroupMembers.Remove(del);
                db.SaveChanges();
                return RedirectToAction("GroupMemberIndex");
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
                            var Del = (from emp in db.GroupMembers where emp.Id == id select emp).SingleOrDefault();
                            db.GroupMembers.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("GroupMemberIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

    }
}
