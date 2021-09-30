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

namespace MODEOUTLED.Controllers
{
    public class PermisstionController : Controller
    {
        //
        // GET: /Permisstion/
         wwwEntities db = new wwwEntities();

        #region[ModuleIndex]
        public ActionResult PermisstionIndex(string sortOrder, string sortName, int? page, string currentFilter, string searchString, string currentSllSupport)
        {

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
                //if (Session["Support"] != null)
                //{
                //    currentSllSupport = Session["Support"].ToString();
                //}
            }
            else
            {
                page = 1;
            }


            ViewBag.CurrentFilter = searchString;
            ViewBag.NumberSupport = currentSllSupport;


            
            //ViewBag.CurrentSortOrder = sortOrder;
            //ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";
            ViewBag.CurrentSortName = sortName;
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

            var all = db.v_ModuleUser.OrderByDescending(module => module.Active).ToList();

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
            
            int pageSize= 10;
           
            int pageNumber = (page ?? 1);

            // begin [get last page]
            if (page != null)
                ViewBag.mPage = (int)page;
            else
                ViewBag.mPage = 1;


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


        #region[PermisstionActiveALL]
        public ActionResult PermisstionActiveAll(bool status)
        {
            //var pStatus = db.UserModules.Find(id);

            if (status == true)
            {
                foreach (var item in db.UserModules.ToList())
                {
                    item.Active = true;
                }
            }
            else
            {
                foreach (var item in db.UserModules.ToList())
                {
                    item.Active = false;
                }
            }
            db.SaveChanges();
            var result = "Trạng thái đã được cập nhật.";// userModule.Active;
            return Json(result);
        }
        #endregion
    }
}
