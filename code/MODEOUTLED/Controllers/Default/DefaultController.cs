using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using MODEOUTLED.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace MODEOUTLED.Controllers.Default
{
    public class DefaultController : Controller
    {
        //ModeoutleddbContext data = new ModeoutleddbContext();
         wwwEntities data = new wwwEntities();
        public ActionResult Album()
        {
            string chuoi = "";
            var listalbum = (from p in data.GroupProducts where p.Active == true orderby p.Id descending select p).ToList();
            for (int j = 0; j < listalbum.Count; j++)
            {
                ViewBag.link = "<li class='Last'><span>Danh mục sản phẩm</span></li>";
                int gid = listalbum[j].Id;
                var list = (from p in data.Products where p.IdCategory==gid orderby p.Id descending select p).ToList();
                if(list.Count>0)
                {
                    if (j % 4 != 0)
                    {
                        chuoi += "<li>";
                    }
                    else
                    {
                        chuoi += "<li class=\"right\">";
                    }
                    chuoi += "<div class=\"image\"><a href=\"/danh-muc/" + listalbum[j].Tag + "\"><img src=\"" + list[0].Image + "\" alt='' /></a></div>";
                    chuoi += "<div class=\"names\">";
                    chuoi += "<a href=\"/danh-muc/" + listalbum[j].Tag + "\">" + listalbum[j].Name + "</a><br />";
                    chuoi += "<span>" + list.Count + " sản phẩm</span>";
                    chuoi += "</div>";
                    chuoi += "</li>";
                }
                list.Clear();
                list = null;
            }
            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Count > 0) { ViewBag.tit = listconfig[0].Title; ViewBag.des = listconfig[0].Description; ViewBag.key = listconfig[0].Keyword; }
            listconfig.Clear();
            listconfig = null;
            #endregion
            ViewBag.pro = chuoi;
            listalbum.Clear();
            listalbum = null;
            return View();
        }
        public ActionResult Albums(int? page, string tag, string r)
        {
            if (Session["tag"] != null) { tag = Session["tag"].ToString(); }
            string chuoiord = "";
            string tin = "";
            if (Request.HttpMethod == "GET")
            {
                
            }
            else
            {
                page = 1;
            }
            #region[Phân trang]
            int pageSize = 20;
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
            var g = data.GroupProducts.Where(m => m.Tag == tag).SingleOrDefault();
            if (g != null)
            {
                if (g.Detail != null && g.Detail != "")
                {
                    Session["tag"] = tag;
                    return RedirectToAction("ilovestylepage");
                }
                #region[Title, Keyword, Deskription]
                if (g.Title.Length > 0) { ViewBag.tit = g.Title; } else { ViewBag.tit = g.Name; }
                if (g.Description.Length > 0) { ViewBag.des = g.Description; } else { ViewBag.des = g.Name; }
                if (g.Keyword.Length > 0) { ViewBag.key = g.Keyword; } else { ViewBag.key = g.Name; }
                #endregion
                string na = g.Name;
                ViewBag.link = "<li class='Last'><span>" + na + "</span></li>";
                int gid = g.Id;
                var list = (from p in data.Products where p.Active == true && p.IdCategory==gid select p).ToList();
                if (r == null)
                {
                    list = list.OrderByDescending(p => p.Id).ToList();
                    chuoiord+="<li><a href=\"/danh-muc/"+ tag +"\" class=\"active\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/mua-nhieu\">Mua nhiều</a></li>";
                }
                else if(r=="re-nhat")
                {
                    list = list.OrderBy(p => p.PricePromotion).ToList();
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\"  class=\"active\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/mua-nhieu\">Mua nhiều</a></li>";
                }
                else if (r == "xem-nhieu")
                {
                    list = list.OrderByDescending(p => p.View).ToList();
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\"  class=\"active\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/mua-nhieu\">Mua nhiều</a></li>";
                }
                else if(r=="mua-nhieu")
                {
                    //v_Product_DescTotalNumber
                    list = list.OrderByDescending(p => p.Id).ToList();
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/mua-nhieu\"  class=\"active\">Mua nhiều</a></li>";
                }
                ViewBag.ordcat = chuoiord;
                var gn = (from gr in data.v_GroupNews_in_GrpID select gr).ToList();
                if (gn.Count > 0)
                {
                    gn = gn.Where(grnw => grnw.GrpID == gid).ToList();
                    if (gn.Count > 0)
                    {
                        tin += "<div class=\"detail\">";
                        tin += "<div class=\"titlelink\">";
                        tin += "<ul>";
                        tin += "<li><a href='/'><i class=\"icon-home\" style=\"font-size: 14px;\"></i>Trang chủ</a></li>";
                        tin += "<li class='SecondLast'><a href='/tin-tuc'>Tin tức</a></li>";
                        tin += "<li class='Last'><span><a href=\"/danh-muc-tin/" + gn[0].Tag + "\">" + gn[0].Name + "</a></span></li>";
                        tin += "</ul>";
                        tin += "</div>";
                        int gnid = gn[0].Id;
                        var listnew = (from n in data.News where n.Active == true && n.IdGroup == gnid orderby n.Id descending select n).Take(12).ToList();
                        if (listnew.Count > 0)
                        {
                            tin += "<div class=\"box-detail\">";
                            for (int i = 0; i < listnew.Count; i++)
                            {
                                tin += "<div class=\"news-cat\">";
                                tin += "<a href=\"/new/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\"><img src=\"" + listnew[i].Image + "\" alt=\"" + listnew[i].Name + "\"/></a>";
                                tin += "<a href=\"/new/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\"><h2>" + listnew[i].Name + "</h2></a>";
                                tin += "<p>" + listnew[i].Content + "</p>";
                                tin += "</div>";
                            }
                            tin += "</div>";
                        }
                        tin += "</div>";
                    }
                }
                 
                 if (list.Count > 0)
                 {
                     return View(list.ToPagedList(pageNumber, pageSize));
                 }
                 else
                 {
                     string levelm = g.Level;
                     int gid01 = g.Id;
                     var catsub = data.GroupProducts.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == levelm && m.Active == true).OrderBy(m => m.Level).ToList();
                     for (int k = 0; k < catsub.Count; k++)
                     {
                         gid01 = catsub[k].Id;
                         list = (from p in data.Products where p.Active == true && p.IdCategory == gid01 orderby p.Ord ascending select p).ToList();
                         if (list.Count > 0)
                         {
                             k = catsub.Count;
                         }
                     }

                     return View(list.ToPagedList(pageNumber, pageSize));
                 }
                 ViewBag.news = tin;
                 g = null;
            }
            else
            {
                return View();
            }
        }
        public ActionResult muanhieu(int? page, string tag, string r)
        {
            string chuoiord = "";
            string tin = "";
            if (Request.HttpMethod == "GET")
            {

            }
            else
            {
                page = 1;
            }
            #region[Phân trang]
            int pageSize = 18;
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
            var g = data.GroupProducts.Where(m => m.Tag == tag).SingleOrDefault();
            if (g != null)
            {
                #region[Title, Keyword, Deskription]
                if (g.Title.Length > 0) { ViewBag.tit = g.Title; } else { ViewBag.tit = g.Name; }
                if (g.Description.Length > 0) { ViewBag.des = g.Description; } else { ViewBag.des = g.Name; }
                if (g.Keyword.Length > 0) { ViewBag.key = g.Keyword; } else { ViewBag.key = g.Name; }
                #endregion
                string na = g.Name;
                ViewBag.link = "<li class='Last'><span>" + na + "</span></li>";
                int gid = g.Id;
                var list = (from p in data.v_Product_DescTotalNumber where p.Active == true && p.IdCategory == gid select p).ToList();
                if (r == null)
                {
                    list = list.OrderByDescending(p => p.Id).ToList();
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\" class=\"active\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/mua-nhieu/" + tag + "/mua-nhieu\">Mua nhiều</a></li>";
                }
                else if (r == "re-nhat")
                {
                    list = list.OrderBy(p => p.PricePromotion).ToList();
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\"  class=\"active\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/mua-nhieu/" + tag + "/mua-nhieu\">Mua nhiều</a></li>";
                }
                else if (r == "xem-nhieu")
                {
                    list = list.OrderByDescending(p => p.View).ToList();
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\"  class=\"active\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/mua-nhieu/" + tag + "/mua-nhieu\">Mua nhiều</a></li>";
                }
                else if (r == "mua-nhieu")
                {
                    //v_Product_DescTotalNumber
                    if (list.Count == 0)
                    {
                        Session["tag"] = null;
                        Session["tag"] = tag;
                        return RedirectToAction("Albums");
                    }
                    chuoiord += "<li><a href=\"/danh-muc/" + tag + "\">Mới nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/re-nhat\">Rẻ nhất</a></li>";
                    chuoiord += "<li><a href=\"/phu-dat-mart/" + tag + "/xem-nhieu\">Xem nhiều</a></li>";
                    chuoiord += "<li><a href=\"/mua-nhieu/" + tag + "/mua-nhieu\"  class=\"active\">Mua nhiều</a></li>";
                }
                ViewBag.ordcat = chuoiord;
                var gn = (from gr in data.v_GroupNews_in_GrpID select gr).ToList();
                if (gn.Count > 0)
                {
                    gn = gn.Where(grnw => grnw.GrpID == gid).ToList();
                    if (gn.Count > 0)
                    {
                        tin += "<div class=\"detail\">";
                        tin += "<div class=\"titlelink\">";
                        tin += "<ul>";
                        tin += "<li><a href='/'><i class=\"icon-home\" style=\"font-size: 14px;\"></i>Trang chủ</a></li>";
                        tin += "<li class='SecondLast'><a href='/tin-tuc'>Tin tức</a></li>";
                        tin += "<li class='Last'><span><a href=\"/danh-muc-tin/" + gn[0].Tag + "\">" + gn[0].Name + "</a></span></li>";
                        tin += "</ul>";
                        tin += "</div>";
                        int gnid = gn[0].Id;
                        var listnew = (from n in data.News where n.Active == true && n.IdGroup == gnid orderby n.Id descending select n).Take(12).ToList();
                        if (listnew.Count > 0)
                        {
                            tin += "<div class=\"box-detail\">";
                            for (int i = 0; i < listnew.Count; i++)
                            {
                                tin += "<div class=\"news-cat\">";
                                tin += "<a href=\"/new/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\"><img src=\"" + listnew[i].Image + "\" alt=\"" + listnew[i].Name + "\"/></a>";
                                tin += "<a href=\"/new/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\"><h2>" + listnew[i].Name + "</h2></a>";
                                tin += "<p>" + listnew[i].Content + "</p>";
                                tin += "</div>";
                            }
                            tin += "</div>";
                        }
                        tin += "</div>";
                    }
                }
                ViewBag.news = tin;
                g = null;
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View();
            }
        }
        public ActionResult ilovestylepage(string tag)
        {
            string chuoi = "";
            if (Session["tag"] != null)
            {
                tag = Session["tag"].ToString();
                var list = data.GroupProducts.Where(m => m.Tag == tag).SingleOrDefault();
                if (list != null)
                {
                    ViewBag.link = "<li class='Last'><span>" + list.Name + "</span></li>";
                    //chuoi += "<div class=\"headname\"><h3>" + list.Name + "</h3></div>";
                    chuoi += list.Detail;
                    #region[Title, Keyword, Deskription]
                    if (list.Title.Length > 0) { ViewBag.tit = list.Title; } else { ViewBag.tit = list.Name; }
                    if (list.Description.Length > 0) { ViewBag.des = list.Description; } else { ViewBag.des = list.Name; }
                    if (list.Keyword.Length > 0) { ViewBag.key = list.Keyword; } else { ViewBag.key = list.Name; }
                    #endregion
                }
                Session["tag"] = null;
            }
            else
            {
                var list = data.Menus.Where(m => m.Tag == tag).SingleOrDefault();
                if (list != null)
                {
                    ViewBag.link = "<li class='Last'><span>" + list.Name + "</span></li>";
                    chuoi += "<div class=\"headname\"><h3>" + list.Name + "</h3></div>";
                    chuoi += list.Detail;
                    #region[Title, Keyword, Deskription]
                    if (list.Title.Length > 0) { ViewBag.tit = list.Title; } else { ViewBag.tit = list.Name; }
                    if (list.Description.Length > 0) { ViewBag.des = list.Description; } else { ViewBag.des = list.Name; }
                    if (list.Keyword.Length > 0) { ViewBag.key = list.Keyword; } else { ViewBag.key = list.Name; }
                    #endregion
                }
            }
            ViewBag.menu = chuoi;
            return View();
        }
        public ActionResult Search(string ProName)
        {
            ViewBag.link = "<li class='Last'><span>Kết quả tìm kiếm: " + ProName + "</span></li>";
            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Count > 0) { ViewBag.tit = listconfig[0].Title; ViewBag.des = listconfig[0].Description; ViewBag.key = listconfig[0].Keyword; }
            listconfig.Clear();
            listconfig = null;
            #endregion
            if (!String.IsNullOrEmpty(ProName))
            {
                var list = (from p in data.Products where p.Active == true && p.Index == true && (p.Name.ToUpper().Contains(ProName.ToUpper()) || p.Code.ToUpper().Contains(ProName.ToUpper())) orderby p.Id descending select p).Take(28).ToList();
                list = list.OrderByDescending(p => p.Id).ToList();
                return View(list);
                list.Clear();
                list = null;
            }
            
            return View();
        }
    }
}
