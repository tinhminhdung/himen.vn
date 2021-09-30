using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;

namespace MODEOUTLED.Controllers.NewsView
{
    public class NewsViewController : Controller
    {
        //ModeoutleddbContext data = new ModeoutleddbContext();
           wwwEntities data = new wwwEntities();
        #region[Danh sách tin tức]
        public ActionResult List(int? page)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                page = 1;
            }

            #region[Phân trang]
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            // Thiết lập phân trang
            PagedListRenderOptions ship = new PagedListRenderOptions();

            ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToIndividualPages = true;
            ship.DisplayPageCountAndCurrentLocation = false;
            ship.MaximumPageNumbersToDisplay = 5;
            ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            ship.EllipsesFormat = "&#8230;";
            ship.LinkToFirstPageFormat = "Trang đầu";
            ship.LinkToPreviousPageFormat = "«";
            ship.LinkToIndividualPageFormat = "{0}";
            ship.LinkToNextPageFormat = "»";
            ship.LinkToLastPageFormat = "Trang cuối";
            ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            ship.FunctionToDisplayEachPageNumber = null;
            ship.ClassToApplyToFirstListItemInPager = null;
            ship.ClassToApplyToLastListItemInPager = null;
            ship.ContainerDivClasses = new[] { "pagination-container" };
            ship.UlElementClasses = new[] { "pagination" };
            ship.LiElementClasses = Enumerable.Empty<string>();

            ViewBag.ship = ship;
            #endregion
            string chuoi = "";
            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Count > 0) { ViewBag.tit = listconfig[0].Title; ViewBag.des = listconfig[0].Description; ViewBag.key = listconfig[0].Keyword; }
            listconfig.Clear();
            listconfig = null;
            #endregion
            var list = (from n in data.News where n.Active == true orderby n.IdGroup, n.Id descending select n).ToList();
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        #endregion
        #region[Danh sách tin theo nhóm]
        public ActionResult ListNews(int? page,string tag)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                page = 1;
            }

            #region[Phân trang]
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            // Thiết lập phân trang
            PagedListRenderOptions ship = new PagedListRenderOptions();

            ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToIndividualPages = true;
            ship.DisplayPageCountAndCurrentLocation = false;
            ship.MaximumPageNumbersToDisplay = 5;
            ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            ship.EllipsesFormat = "&#8230;";
            ship.LinkToFirstPageFormat = "Trang đầu";
            ship.LinkToPreviousPageFormat = "«";
            ship.LinkToIndividualPageFormat = "{0}";
            ship.LinkToNextPageFormat = "»";
            ship.LinkToLastPageFormat = "Trang cuối";
            ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            ship.FunctionToDisplayEachPageNumber = null;
            ship.ClassToApplyToFirstListItemInPager = null;
            ship.ClassToApplyToLastListItemInPager = null;
            ship.ContainerDivClasses = new[] { "pagination-container" };
            ship.UlElementClasses = new[] { "pagination" };
            ship.LiElementClasses = Enumerable.Empty<string>();

            ViewBag.ship = ship;
            #endregion
            var g = data.GroupNews.Where(m => m.Tag == tag).SingleOrDefault();
            if (g != null)
            {
                ViewBag.link = "<li class='Last'><span>" + g.Name + "</span></li>";
                int gid = g.Id;
                var list = (from n in data.News where n.Active == true && n.IdGroup==gid orderby n.Id descending select n).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
                list.Clear();
                list = null;
                #region[Title, Keyword, Deskription]
                if (g.Title.Length > 0) { ViewBag.tit = g.Title; } else { ViewBag.tit = g.Name; }
                if (g.Description.Length > 0) { ViewBag.des = g.Description; } else { ViewBag.des = g.Name; }
                if (g.Keyword.Length > 0) { ViewBag.key = g.Keyword; } else { ViewBag.key = g.Name; }
                #endregion
            }
            else
            {
                return View();
            }
            
        }
        #endregion
        #region[Chi tiết tin]
        public ActionResult NewsDetail(string tag)
        {
            string chuoi = "";
            string chuoilink = "";
            int IdGroup = 0;
            int NewId = 0;
            var news = (from n in data.News where n.Tag == tag select n).ToList();
            if (news.Count > 0)
            {
                #region[Title, Keyword, Deskription]
                string t = news[0].Title;
                string tt = news[0].Description;
                string ttt = news[0].Keyword;
                if (news[0].Title != null && news[0].Title.Length > 0) { ViewBag.tit = news[0].Title; } else { ViewBag.tit = news[0].Name; }
                if (news[0].Description != null && news[0].Description.Length > 0) { ViewBag.des = news[0].Description; } else { ViewBag.des = news[0].Name; }
                if (news[0].Keyword != null && news[0].Keyword.Length > 0) { ViewBag.key = news[0].Keyword; } else { ViewBag.key = news[0].Name; }
                #endregion
                NewId = news[0].Id;
                IdGroup = news[0].IdGroup;
                var Group = (from g in data.GroupNews where g.Id == IdGroup select g).ToList();
                if (Group.Count > 0)
                {
                    chuoilink = "<li class='Last'><a href='/danh-muc-tin/" + Group[0].Tag + "'>" + Group[0].Name + "</a></li>";
                }
                Group.Clear();
                Group = null;
                ViewBag.link = chuoilink;
                #region[View thông tin chi tiết tin]
                chuoi += "<h2>"+ news[0].Name +"</h2>";
                chuoi += "<p>"+ news[0].Detail +"</p>";
                #endregion
                
            }
            ViewBag.newsdetail = chuoi;
            news.Clear();
            news = null;
            return View();
        }
        #endregion
    }
}
