using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Globalization;
using MODEOUTLED.ViewModels;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using System.Net.Mail;
using System.IO;
using PagedList;
using PagedList.Mvc;
using os;

namespace onsoft.Controllers
{
    public class HomeController : Controller
    {
        wwwEntities data = new wwwEntities();
        public ActionResult Index(int? page, string ProName, string currentProName)
        {
            //if (Request.HttpMethod == "GET")
            //{
            if (Session["CurrentProName"] != null)
            {
                currentProName = Session["CurrentProName"].ToString();
                Session["CurrentProName"] = null;
            }
            //}
            //else
            //{
            //    page = 1;
            //}

            //#region[Phân trang]
            //int pageSize = 16;
            //int pageNumber = (page ?? 1);

            //// Thiết lập phân trang
            //PagedListRenderOptions ship = new PagedListRenderOptions();

            //ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            //ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            //ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            //ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            //ship.DisplayLinkToIndividualPages = true;
            //ship.DisplayPageCountAndCurrentLocation = false;
            //ship.MaximumPageNumbersToDisplay = 5;
            //ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            //ship.EllipsesFormat = "&#8230;";
            //ship.LinkToFirstPageFormat = "Trang đầu";
            //ship.LinkToPreviousPageFormat = "«";
            //ship.LinkToIndividualPageFormat = "{0}";
            //ship.LinkToNextPageFormat = "»";
            //ship.LinkToLastPageFormat = "Trang cuối";
            //ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            //ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            //ship.FunctionToDisplayEachPageNumber = null;
            //ship.ClassToApplyToFirstListItemInPager = null;
            //ship.ClassToApplyToLastListItemInPager = null;
            //ship.ContainerDivClasses = new[] { "pagination-container" };
            //ship.UlElementClasses = new[] { "pagination" };
            //ship.LiElementClasses = Enumerable.Empty<string>();

            //ViewBag.ship = ship;
            //#endregion


            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Any())
            {
                ViewBag.tit = listconfig[0].Title;
                ViewBag.des = listconfig[0].Description;
                ViewBag.key = listconfig[0].Keyword;
                ViewBag.livechat = listconfig[0].Canhbao;
            }
            listconfig.Clear();
            listconfig = null;
            #endregion
            if (!String.IsNullOrEmpty(ProName))
            {
                var list = (from p in data.Products where p.Active == true && p.Index == true && (p.Name.ToUpper().Contains(ProName.ToUpper()) || p.Code.ToUpper().Contains(ProName.ToUpper())) orderby p.Id descending select p).Take(28).ToList();
                list = list.OrderByDescending(p => p.Id).ToList();
                return View(list);
            }
            else
            {
                var list = (from p in data.Products where p.Active == true && p.Index == true orderby p.Ord ascending select p).Take(28).ToList();
                list = list.OrderByDescending(p => p.Id).ToList();
                return View(list);
            }

        }
        public ActionResult Detail(string tag)
        {
            string chuoi = "";
            string chuoilink = "";
            string image = "";
            string imagethumb = "";
            string Cart = "";
            int category = 0;
            int proId = 0;
            string baohanh = "";
            string noibat = "";
            string chitiet = "";
            string thanhtoan = "";
            string canhbao = "";
            string khuyenmai = "";
            string fb = "";
            var pro = (from p in data.Products where p.Tag == tag select p).ToList();
            if (pro.Count > 0)
            {
                proId = pro[0].Id;
                data.sp_Product_Update(proId, pro[0].Name, pro[0].Tag, pro[0].Image, pro[0].Description, pro[0].Weight, pro[0].ChatLieu, pro[0].Status, pro[0].Detail, pro[0].SpTon, pro[0].Count, (pro[0].View + 1), pro[0].Like, pro[0].IdCategory, pro[0].Date, pro[0].Index, pro[0].Priority, pro[0].Keyword, pro[0].Title, pro[0].Content, pro[0].Ord, pro[0].Active, pro[0].PriceRetail, pro[0].PricePromotion, pro[0].Num, pro[0].Noibat, pro[0].Baohanh, pro[0].Giamgia);
                data.SaveChanges();

                category = pro[0].IdCategory;
                var cate = (from p in data.GroupProducts where p.Id == category select p).ToList();
                if (cate.Count > 0)
                {
                    chuoilink = "<li class='Last'><a href='/danh-muc/" + cate[0].Tag + "'>" + cate[0].Name + "</a></li>";
                }
                cate.Clear();
                cate = null;
                ViewBag.link = chuoilink;
                int g = 0;
                if (pro[0].Num != null) { g = int.Parse(pro[0].Num.ToString()); }
                chuoi += "<input type=\"hidden\" id=\"item_id\" value=\"" + pro[0].Id + "\">";
                //imagethumb += "<li class='active' rel='1'><img src=\"" + pro[0].Image + "\" alt=\"" + pro[0].Name + "\" /></li>";
                imagethumb += "<li class='active' rel='1'><img src=\"" + pro[0].Image + "\" alt=\"" + pro[0].Name + "\" title=\"Click to enlarge\" /></li>";
                if (g == 0)
                {
                    image += "<a href=\"" + pro[0].Image + "\" class=\"highslide\" rel=\"position: 'inside' , showTitle: false\" id=\"zoom1\" style=\"position: relative; display: block;\" onclick=\"return hs.expand(this)\"><img src=\"" + pro[0].Image + "\" alt=\"" + pro[0].Name + "\" /></a>";
                }
                else
                {
                    image += "<a href=\"" + pro[0].Image + "\" class=\"highslide\" rel=\"position: 'inside' , showTitle: false\" id=\"zoom1\" style=\"position: relative; display: block;\" onclick=\"return hs.expand(this)\"><img src=\"" + pro[0].Image + "\" alt=\"" + pro[0].Name + "\" /></a>";
                }
                chuoi += "<input type=\"hidden\" id=\"item_id\" value=\"" + pro[0].Id + "\" />";
                chuoi += "<p id=\"titPro\">" + pro[0].Name + "</p>";
                chuoi += "<p>Mã SP:<span class=\"value\">" + pro[0].Code + "</span></p>";
                chuoi += "<div class=\"divInfo\">";
                if (g > 0)
                {
                    chuoi += "<p><span class=\"txtw80 color-firs\">Tình trạng:</span><span class=\"stock\">Còn hàng</span></p>";
                    if ((pro[0].PricePromotion / pro[0].PriceRetail) * 100 < 100)
                    {
                        chuoi += "<p><span id=\"valBanle-cu\">Giá cũ: " + StringClass.Format_Price(pro[0].PriceRetail.ToString()) + "đ</span></p>";

                        Double a = Convert.ToDouble((pro[0].PriceRetail - pro[0].PricePromotion) / pro[0].PriceRetail * 100);
                        string giam = Math.Round(a, 0).ToString();

                        chuoi += "<p>Giá bán: <span id=\"valBanle\"> " + StringClass.Format_Price(pro[0].PricePromotion.ToString()) + "đ</span></p>";
                        chuoi += "<p>Giảm: <span id=\"valBanle\"> " + giam + "%</span> (tiết kiệm " + StringClass.Format_Price((pro[0].PriceRetail - pro[0].PricePromotion).ToString()) + "đ)</p>";
                    }
                    else
                    {
                        chuoi += "<p><span class=\"color-firs\">Giá:</span> <span id=\"valBanle\"> " + StringClass.Format_Price(pro[0].PricePromotion.ToString()) + "đ</span></p>";
                    }
                }
                else
                {
                    chuoi += "<p><span class=\"txtw80 color-firs\">Tình trạng:</span><span class=\"nostock\">Hết hàng</span></p>";
                    chuoi += "<p><span class=\"color-firs\">Giá bán:</span> <span id=\"valBanle\">" + StringClass.Format_Price(pro[0].PricePromotion.ToString()) + "đ</span></p>";
                }
                chuoi += "<span class=\"lblProduct\">&nbsp;</span>";
                chuoi += "</div>";
                chuoi += "<p><span class=\"color-firs\">Trọng lượng:</span> <span class=\"value\">" + pro[0].Weight + "gram</span></p>";
                int pid = pro[0].Id;
                var proimages = (from im in data.ProImages where im.IdPro == pid select im).ToList();
                int k = 2;
                for (int j = 0; j < proimages.Count; j++)
                {
                    //imagethumb += "<li class='active' rel='"+ (k+j) +"'><img src=\"" + proimages[j].Image + "\" alt=\"" + pro[0].Name + "\" /></li>";
                    if (proimages.Count > 1)
                    {
                        imagethumb += "<li class='active' rel='" + (k + j) + "'><img src=\"" + proimages[j].Image + "\" alt=\"" + pro[0].Name + "\" /></li>";
                    }
                    image += "<a href=\"" + proimages[j].Image + "\" class=\"highslide\" onclick=\"return hs.expand(this)\"><img src=\"" + proimages[j].Image + "\" alt=\"" + pro[0].Name + "\" /></a>";
                }
                if (g > 0)
                {
                    Cart += "<p id=\"Cart\"><a id=\"btnAddCart\" href=\"javascript:;\" class=\"btnBuyNow add-to-cart\" title=\"Thêm sản phẩm này vào giỏ hàng\"><span id=\"txt\"></span></a>";
                    Cart += "<a id=\"btnAddCart_New\" href=\"javascript:;\" title=\"Mua ngay\" class=\"btnAddCart\" rel=\"/Home/checkout\"><span id=\"txt1\">Mua ngay</span></a></p>";
                }
                else
                {
                    Cart += "<p id=\"Cart\"><a id=\"btnAddCart\" href=\"javascript:;\" class=\"btnBuyNow Disabled\" title=\"Thêm sản phẩm này vào giỏ hàng\"><span id=\"txt\"></span></a><a id=\"btnAddCart\" title=\"Mua ngay\" href=\"javascript:;\" class=\"btnAddCart Disabled\" rel=\"/Home/checkout\"><span id=\"txt1\">Mua ngay</span></a></p>";
                }
                chitiet = pro[0].Content;
                noibat = pro[0].Noibat;
                int idcate = pro[0].IdCategory;
                var bh = data.GroupProducts.Find(idcate);
                if (bh != null)
                {
                    baohanh = bh.BaoHanh;// bao hanh 
                }



                #region[Title, Keyword, Deskription]
                string t = pro[0].Title;
                string tt = pro[0].Description;
                string ttt = pro[0].Keyword;
                if (pro[0].Title != null && pro[0].Title.Length > 0) { ViewBag.tit = pro[0].Title; } else { ViewBag.tit = pro[0].Name; }
                if (pro[0].Description != null && pro[0].Description.Length > 0) { ViewBag.des = pro[0].Description; } else { ViewBag.des = pro[0].Name; }
                if (pro[0].Keyword != null && pro[0].Keyword.Length > 0) { ViewBag.key = pro[0].Keyword; } else { ViewBag.key = pro[0].Name; }
                #endregion
                ViewBag.cart = Cart;
                proimages.Clear();
                proimages = null;

                fb += "<div class=\"fb-like\" style=\"margin-top: 15px; padding-bottom: 3px\" data-href=\"" + Request.Url.ToString() + "\" data-send=\"true\" data-max-width=\"950\" data-show-faces=\"true\"></div>";
                fb += "<div class=\"fb-comments\" style=\"margin-top: 15px\" data-href=\"" + Request.Url.ToString() + "\" data-max-width=\"950\" data-num-posts=\"5\"></div>";

                #region[Sản phẩm cùng loại]
                string chuoicungloai = "";
                var list = (from p in data.Products where p.Active == true && p.IdCategory == category && p.Tag != tag orderby p.Ord ascending select p).Take(21).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    //if (i % 3 != 0)
                    //{
                    //    chuoicungloai += "<li class=\"other\">";
                    //}
                    //else
                    //{
                    //    chuoicungloai += "<li class=\"right\">";
                    //}
                    //chuoicungloai += "<div class=\"imagespro\"><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" title=\"" + list[i].Name + "\"/></a></div>";
                    //    chuoicungloai += "<div>";
                    //    if (list[i].Code != null && list[i].Code != "")
                    //    {
                    //        chuoicungloai += "<p class=\"gia_cu\">" + list[i].Code + " đ</p>";
                    //    }
                    //    else
                    //    {
                    //        chuoicungloai += "<p class=\"gia_cu\">No size</p>";
                    //    }
                    //    if (list[i].PricePromotion == 0)
                    //    {
                    //        chuoicungloai += "<p class=\"gia_sp\">Giá: Vui lòng liên hệ</p>";
                    //    }
                    //    else
                    //    {
                    //        chuoicungloai += "<p class=\"gia_sp\">Giá: " + os.os.Format_Price(list[i].PricePromotion.ToString()) + " đ</p>";
                    //    }
                    //    chuoicungloai += "</div>";
                    //    chuoicungloai += "<p class=\"sp_code\"><a href=\"/thong-tin/" + list[i].Tag + "\">" + list[i].Name + "</a></p>";
                    //chuoicungloai += "</li>";



                    if (i % 3 != 0)
                    {
                        chuoicungloai += "<li class=\"other\">";
                        chuoicungloai += "<div class=\"imagespro\"><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"  title=\"" + list[i].Name + "\"/></a></div>";
                        if ((list[i].PricePromotion / list[i].PriceRetail) * 100 < 100)
                        {
                            chuoicungloai += "<div>";
                            if (list[i].Code != null && list[i].Code != "")
                            {
                                chuoicungloai += "<p class=\"sp_code\">Mã SP: " + list[i].Code + "</p>";
                            }
                            else
                            {
                                chuoicungloai += "<p class=\"sp_code\">No size</p>";
                            }
                            if (list[i].PricePromotion == 0)
                            {
                                chuoicungloai += "<p class=\"gia_sp\">Giá: Vui lòng liên hệ</p>";
                            }
                            else
                            {
                                chuoicungloai += "<p class=\"gia_cu\">Giá cũ: " + os.os.Format_Price(list[i].PriceRetail.ToString()) + "đ</p>";
                                chuoicungloai += "<p class=\"gia_sp\">Giá bán: " + os.os.Format_Price(list[i].PricePromotion.ToString()) + "đ</p>";
                            }
                            chuoicungloai += "</div>";
                            chuoicungloai += "<p class=\"sp_name\"><a href=\"/thong-tin/" + list[i].Tag + "\"> " + list[i].Name + "</a></p>";
                            chuoicungloai += "<div class=\"giamgia\">";
                            chuoicungloai += "<span class=\"discount-label\">Giảm</span>";
                            chuoicungloai += "<span class=\"discount-value\">" + onsoft.Models.StringClass.Round(double.Parse((100 - (list[i].PricePromotion / list[i].PriceRetail) * 100).ToString()), 2) + "%</span>";
                            chuoicungloai += "</div>";
                        }
                        else
                        {
                            chuoicungloai += "<div>";
                            if (list[i].Code != null && list[i].Code != "")
                            {
                                chuoicungloai += "<p class=\"sp_code\">Mã SP: " + list[i].Code + "</p>";
                            }
                            else
                            {
                                chuoicungloai += "<p class=\"sp_code\">No size</p>";
                            }
                            if (list[i].PricePromotion == 0)
                            {
                                chuoicungloai += "<p class=\"gia_sp\">Giá: Vui lòng liên hệ</p>";
                            }
                            else
                            {
                                chuoicungloai += "<p class=\"gia_sp\">Giá bán: " + os.os.Format_Price(list[i].PricePromotion.ToString()) + "đ</p>";
                            }
                            chuoicungloai += "</div>";
                            chuoicungloai += "<p class=\"sp_name\"><a href=\"/thong-tin/" + list[i].Tag + "\">" + list[i].Name + "</a></p>";
                        }
                        chuoicungloai += "</li>";
                    }
                    else
                    {
                        chuoicungloai += "<li class=\"right\">";
                        chuoicungloai += "<div class=\"imagespro\"><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"list[i].Name\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"  title=\"" + list[i].Name + "\"/></a></div>";
                        chuoicungloai += "<div>";
                        if (list[i].Code != null && list[i].Code != "")
                        {
                            chuoicungloai += "<p class=\"sp_code\">Mã SP: " + list[i].Code + "</p>";
                        }
                        else
                        {
                            chuoicungloai += "<p class=\"sp_code\">No size</p>";
                        }
                        if (list[i].PricePromotion == 0)
                        {
                            chuoicungloai += "<p class=\"gia_sp\">Giá: Vui lòng liên hệ</p>";
                        }
                        else
                        {
                            chuoicungloai += "<p class=\"gia_sp\">Giá bán: " + os.os.Format_Price(list[i].PricePromotion.ToString()) + "đ</p>";
                        }
                        chuoicungloai += "</div>";
                        chuoicungloai += "<p class=\"sp_name\"><a href=\"/thong-tin/" + list[i].Tag + "\">" + list[i].Name + "</a></p>";
                        chuoicungloai += "</li>";
                    }

                }
                ViewBag.cungloai = chuoicungloai;
                list.Clear();
                list = null;
                #endregion
                #region[Sản phẩm nhiều người mua]
                string chuoinhieu = "";
                var listnn = (from p in data.v_Product_DescTotalNumber select p).Take(20).ToList();
                for (int e = 0; e < listnn.Count; e++)
                {
                    chuoinhieu += "<li>";
                    chuoinhieu += "<a href=\"/dong-ho/" + listnn[e].Tag + "\"><img src=\"" + listnn[e].Image + "\" /></a>";
                    chuoinhieu += "<a href=\"/dong-ho/" + listnn[e].Tag + "\">" + listnn[e].Name + "</a>";
                    if ((listnn[e].PricePromotion / listnn[e].PriceRetail) * 100 < 100)
                    {
                        chuoinhieu += "<p><span>" + os.os.Format_Price(listnn[e].PriceRetail.ToString()) + "đ</span></p>";
                        chuoinhieu += "<p>" + os.os.Format_Price(listnn[e].PricePromotion.ToString()) + "đ</p>";
                    }
                    else
                    {
                        chuoinhieu += "<p>" + os.os.Format_Price(listnn[e].PricePromotion.ToString()) + "đ</p>";
                    }
                    chuoinhieu += "</li>";
                }
                ViewBag.nhieu = chuoinhieu;
                listnn.Clear();
                listnn = null;
                #endregion
            }
            ViewBag.images = image;
            ViewBag.imagethumb = imagethumb;
            ViewBag.pro = chuoi;
            pro.Clear();
            pro = null;
            #region[Hiển thị thông tin các Tab]
            var listconf = data.Configs.ToList();
            if (listconf.Count > 0)
            {
                thanhtoan = listconf[0].Thanhtoan;
                canhbao = listconf[0].Canhbao;
                khuyenmai = listconf[0].Helpsize;
            }
            listconf.Clear();
            listconf = null;
            string chuoitab_head = "";
            string chuoitab_content = "";
            if (chitiet != null && chitiet != "")
            {
                chuoitab_head += "<li><a href=\"#tabs-1\">Thông tin sản phẩm</a></li>";
                chuoitab_content += "<div id=\"tabs-1\"><p>" + chitiet + " " + canhbao + "</p></div>";
            }
            if (noibat != null && noibat != "")
            {
                chuoitab_head += "<li><a href=\"#tabs-2\">Ưu điểm nổi bật</a></li>";
                chuoitab_content += "<div id=\"tabs-2\"><p>" + noibat + "</p></div>";
            }
            if (baohanh != null && baohanh != "")
            {
                chuoitab_head += "<li><a href=\"#tabs-3\">Thông tin bảo hành</a></li>";
                chuoitab_content += "<div id=\"tabs-3\"><p>" + baohanh + "</p></div>";
            }
            if (thanhtoan != null && thanhtoan != "")
            {
                chuoitab_head += "<li><a href=\"#tabs-4\">Phương thức thanh toán</a></li>";
                chuoitab_content += "<div id=\"tabs-4\"><p>" + thanhtoan + "</p></div>";
            }
            //if (canhbao != null && canhbao != "")
            //{
            //    chuoitab_head += "<li><a href=\"#tabs-5\">Cảnh báo</a></li>";
            //    chuoitab_content += "<div id=\"tabs-5\"><p>" + canhbao + "</p></div>";
            //}
            if (khuyenmai != null && khuyenmai != "")
            {
                chuoitab_head += "<li><a href=\"#tabs-6\">Khuyến mại</a></li>";
                chuoitab_content += "<div id=\"tabs-6\"><p>" + khuyenmai + "</p></div>";
            }
            chuoitab_head += "<li><a href=\"#tabs-7\">Bình luận đánh giá</a></li>";
            chuoitab_content += "<div id=\"tabs-7\"><p>" + fb + "</p></div>";
            ViewBag.tabhead = chuoitab_head;
            ViewBag.tabcontent = chuoitab_content;
            #endregion
            #region[Hỗ trợ trực tuyến]
            string chuoisp = "";
            string chuoi_tel = "";
            string chuoi_hotline = "";
            string chuoi_mobile = "";
            var listsp = data.Supports.ToList();
            if (listsp.Count > 0)
            {
                for (int i = 0; i < listsp.Count; i++)
                {
                    if (listsp[i].Type == 0)
                    {
                        chuoisp += "<p><a href=\"ymsgr:sendim?" + listsp[i].Nick + "\" title=\"" + listsp[i].Name + "\"><img alt=\"" + listsp[i].Name + "\" src=\"http://opi.yahoo.com/online?u=" + listsp[i].Nick + "&amp;m=g&amp;t=1\" border=\"0\" align=\"middle\"></a></p>";
                    }
                    else if (listsp[i].Type == 1)
                    {
                        chuoisp += "<p><a href=\"skype:" + listsp[i].Nick + "?chat\"><img src=\"HTTP://MYSTATUS.SKYPE.COM/smallclassic/" + listsp[i].Nick + "\" title=\"" + listsp[i].Name + "\"></a></p>";
                    }
                    else if (listsp[i].Type == 2)
                    {
                        chuoi_hotline += "<p><a href=\"#\" class=\"hl\" title=\"" + listsp[i].Name + "\"></a><span>HOTLINE: " + listsp[i].Tel + "</span></p>";
                    }
                    else if (listsp[i].Type == 3)
                    {
                        chuoi_tel += "<p><a href=\"#\" class=\"cel\" title=\"" + listsp[i].Name + "\"></a>" + listsp[i].Tel + "</p>";
                    }
                    else if (listsp[i].Type == 4)
                    {
                        chuoi_mobile += "<p><a href=\"#\" class=\"mob\" title=\"" + listsp[i].Name + "\"></a>" + listsp[i].Tel + "</p>";
                    }
                }
                chuoisp = chuoisp + chuoi_tel + chuoi_mobile + chuoi_hotline;
            }
            ViewBag.support = chuoisp;
            #endregion

            #region[view drop Colors]

            //Color New
            var _dsColor = data.ProColors.Where(n => n.IdPro == proId).ToList();
            string _strColor = "";
            string _idDefault = "";
            if (_dsColor.Count > 0)
            {
                _strColor += "<ul class=\"ul_option\">";

                for (int k = 0; k < _dsColor.Count; k++)
                {
                    int _cID = _dsColor[k].IdColor;
                    var _color = data.Colors.Where(n => n.Id == _cID).ToList();
                    for (int i = 0; i < _color.Count; i++)
                    {
                        string sid = _color[i].Id.ToString();
                        string sname = _color[i].Name.ToString();
                        string bg = _color[i].Image;
                        if (k == 0)
                        {
                            _idDefault = sid;
                            _strColor += "<li style='background:" + bg + "' rel=\"" + sid + "\" class=\"active\">" + sname + "</li>";
                        }
                        else
                        {
                            _strColor += "<li style='background:" + bg + "' rel=\"" + sid + "\">" + sname + "</li>";
                        }


                    }
                }

                _strColor += "</ul>";
                _strColor += "<input id=\"hidColor\" type=\"hidden\" value=\"" + _idDefault + "\"/>";

            }
            ViewBag.ColorNew = _strColor;


            var procolor = data.ProColors.Where(n => n.IdPro == proId).ToList();
            List<DropDownList> listcolor = new List<DropDownList>();
            listcolor.Add(new DropDownList { value = "0", text = "----" });
            if (procolor.Count > 0)
            {
                List<DropDownList> listcolor1 = new List<DropDownList>();
                for (int k = 0; k < procolor.Count; k++)
                {
                    int clId = procolor[k].IdColor;
                    var color = data.Colors.Where(n => n.Id == clId).ToList();
                    for (int i = 0; i < color.Count; i++)
                    {
                        string sid = color[i].Id.ToString();
                        string sname = color[i].Name.ToString();
                        listcolor1.Add(new DropDownList { value = "" + sid + "", text = "" + sname + "" });
                        //ViewBag.color = new SelectList(color, "Id", "Name");
                    }
                }
                ViewBag.color = new SelectList(listcolor1, "value", "text");
            }
            else
            {
                ViewBag.color = new SelectList(listcolor, "value", "text");
            }
            #endregion
            #region[view drop Sizes]


            //Size New
            var _dsSize = data.ProSizes.Where(n => n.IdPro == proId).ToList();
            string _strSize = "";
            string _idSDefault = "";

            if (_dsSize.Count > 0)
            {
                //_strSize += "<select id=\"sz\" onchange='myFunction()'>";
                _strSize += "<ul class=\"ul_option_size\">";

                for (int s = 0; s < _dsSize.Count; s++)
                {
                    int _cID = _dsSize[s].IdSize;
                    var _size = data.Sizes.Where(n => n.Id == _cID).ToList();
                    for (int i = 0; i < _size.Count; i++)
                    {
                        string sid = _size[i].Id.ToString();
                        string sname = _size[i].Name.ToString();
                        if (s == 0)
                        {
                            _idSDefault = sid;
                            //_strSize += "<option rel=\"" + sid + "\" value='" + sid + "'>" + sname + "</option>";
                            _strSize += "<li rel=\"" + sid + "\" class=\"active\">" + sname + "</li>";
                        }
                        else
                        {
                           // _strSize += "<option rel=\"" + sid + "\" value='" + sid + "'>" + sname + "</option>";
                            _strSize += "<li rel=\"" + sid + "\">" + sname + "</li>";
                        }


                    }
                }
                _strSize += "</ul>";
               // _strSize += "</select>";
                _strSize += "<input id=\"hidSize\" type=\"hidden\" value=\"" + _idSDefault + "\"/>";
            }
            ViewBag.SizeNew = _strSize;

            var prosize = data.ProSizes.Where(n => n.IdPro == proId).ToList();
            List<DropDownList> listsize = new List<DropDownList>();
            listsize.Add(new DropDownList { value = "0", text = "Freesize" });
            if (prosize.Count > 0)
            {
                List<DropDownList> listsize1 = new List<DropDownList>();
                for (int s = 0; s < prosize.Count; s++)
                {
                    int szId = prosize[s].IdSize;
                    var size = data.Sizes.Where(n => n.Id == szId).ToList();
                    for (int i = 0; i < size.Count; i++)
                    {
                        string sid = size[i].Id.ToString();
                        string sname = size[i].Name.ToString();
                        listsize1.Add(new DropDownList { value = "" + sid + "", text = "" + sname + "" });
                        //ViewBag.sizes = new SelectList(size, "Id", "Name");
                    }
                }
                ViewBag.size = new SelectList(listsize1, "value", "text");
            }
            else
            {
                ViewBag.size = new SelectList(listsize, "value", "text");
            }
            #endregion

            return View();
        }
        #region[Gio hang]
        public ActionResult checkout()
        {
            if (Session["ShoppingCart"] != null)
            {
                var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                return View(cart);
            }
            else
            {
                return Redirect("/");
            }
        }
        #endregion
        #region[Gio hang]
        public ActionResult BuyNow()
        {
            if (Session["ShoppingCart"] != null)
            {
                var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                return View(cart);
            }
            else
            {
                return Redirect("/");
            }
        }
        #endregion
        [HttpPost]
        public ActionResult BuyNow(string Name, string Email, string Phone, string Address)
        {
            string chuoi = "";
            if (Session["ShoppingCart"] != null)
            {
                string tongtienmail = "";
                Customer mem = new Customer();
                Province provi = new Province();
                if (Name != "" && Email != "" && Phone != "" && Address != "")
                {
                    #region[Lưu vào dababase theo tình huống có địa chỉ giao hàng là Địa chỉ mới]
                    int provid = 0;
                    float tongdiem = 0;
                    float tongtien = 0;
                    float diem = 0;
                    float tien = 0;
                    int w = 0;
                    int ship = 0;
                    int tienship = 0;
                    string sTypePay = "";
                    string email = Email;
                    #region[Lưu vào bảng khách hàng]
                    //onsoft.Models.Customer customer = new onsoft.Models.Customer();
                    //customer.Name = Name;
                    //customer.Email = Email;
                    //customer.Password = Email;
                    //customer.Address = Address;
                    //customer.Tel = Phone;
                    //customer.SDate = DateTime.Now;
                    //customer.Status = false;
                    //customer.P_xa = Address;
                    //customer.Provice = 0;
                    //customer.Diem = 0;
                    //customer.Si = false;
                    //customer.Vip = 0;
                    //customer.Avarta = "";
                    //customer.cPriceNT = 0;
                    //customer.cNongthon = 0;
                    data.sp_Customer_Insert(Name, Email, Email, Phone, Address, DateTime.Now, false, Address, 0, 0, false, 0, "", 0, 0, true);
                    //data.Entry(mem).State = EntityState.Modified;
                    data.SaveChanges();
                    #endregion
                    mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                    if (mem != null)
                    {
                        provid = 0;
                        tongdiem = float.Parse(mem.Diem.ToString());
                        var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                        ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                        #region[Lưu dữ liệu vào bảng đơn hàng]
                        onsoft.Models.Ord ord = new onsoft.Models.Ord();
                        ord.IdCus = mem.Id;
                        ord.Amount = shoppCart.CartTotal;
                        tongtienmail = shoppCart.CartTotal.ToString();
                        ord.SDate = DateTime.Now;
                        ord.TypePay = sTypePay;
                        ord.Status = "1";
                        ord.PriceVC = 0;
                        ord.Name = Name;
                        ord.Address = Address;
                        ord.Tel = Phone;
                        ord.ProviceId = provid;
                        ord.Nongthon = 0;
                        data.Ords.Add(ord);
                        diem = shoppCart.CartTotal / 1000;
                        tongdiem = tongdiem + diem;
                        mem.Diem = tongdiem;
                        data.Entry(mem).State = EntityState.Modified;
                        data.SaveChanges();
                        #endregion
                        #region[Lưu vào bảng chi tiết đơn hàng]
                        var listbillcus = data.Ords.OrderByDescending(m => m.Id).FirstOrDefault();
                        foreach (var item in shoppCart.CartItems)
                        {
                            var pro = data.Products.Where(m => m.Id == item.productId).FirstOrDefault();
                            OrderDetail billdetail = new OrderDetail();
                            billdetail.IdOr = listbillcus.Id;
                            billdetail.IdPro = item.productId;
                            billdetail.IdSize = item.idsize;
                            billdetail.IdColor = item.idcolor;
                            billdetail.Number = item.count;
                            w = (w + item.proweight * item.count);
                            //if (mem.Si == true)
                            //{
                            billdetail.Price = double.Parse(pro.PricePromotion.ToString());
                            tien = float.Parse(item.count.ToString()) * float.Parse(pro.PricePromotion.ToString());
                            //}
                            //else
                            //{
                            //    billdetail.Price = double.Parse(pro.PriceRetail.ToString());
                            //    tien = float.Parse(item.count.ToString()) * float.Parse(pro.PriceRetail.ToString());
                            //}
                            tongtien = tongtien + tien;
                            billdetail.Total = double.Parse(item.total.ToString());
                            //data.sp_OrderDetail_Insert();
                            data.OrderDetails.Add(billdetail);

                            pro.Num = pro.Num - int.Parse(item.count.ToString());

                            data.Entry(pro).State = EntityState.Modified;

                            data.SaveChanges();
                        }
                        #endregion
                        #region[Update lại tổng tiền, tiền ship]
                        tienship = ship;
                        listbillcus.PriceNT = 0;
                        listbillcus.PriceVC = tienship;
                        listbillcus.Amount = tongtien + tienship;
                        data.Entry(listbillcus).State = EntityState.Modified;
                        data.SaveChanges();
                        RemoveFromCartAll();
                        #endregion
                    }
                    #endregion

                    #region[code gui mail]
                    try
                    {
                        var configmail = data.Configs.FirstOrDefault();
                        MailMessage mailsend = new MailMessage();
                        mailsend.To.Add(configmail.Mail_Info);

                        mailsend.From = new MailAddress(configmail.Mail_Noreply);

                        mailsend.Subject = "Đã có một đơn hàng của khách được gửi từ email: " + email;
                        mailsend.Body = "Xin chào quản trị himen.vn! đã có một đơn hàng được gửi từ email của khách hàng là: " + email + ",vào thời gian: " + DateTime.Now + ", Tổng tiền hóa đơn:" + tongtienmail;
                        mailsend.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        NetworkCredential credentials = new NetworkCredential(configmail.Mail_Noreply, configmail.Mail_Password);

                        client.Credentials = credentials;
                        client.Send(mailsend);
                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                    return RedirectToAction("order_success", "Pages");
                }
                return View();
            }
            return RedirectToAction("/");
        }
        int cartTotal;
        public void RemoveFromCartAll()
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                shoppCart.CartItems.RemoveAt(i);
            }
            if (shoppCart.CartItems.Count > 0)
            {
                for (int j = 0; j < shoppCart.CartItems.Count; j++)
                {
                    cartTotal += shoppCart.CartItems[j].total;
                }
                shoppCart.CartTotal = cartTotal;
            }
            else
            {
                shoppCart.CartTotal = 0;
            }

            Session["ShoppingCart"] = shoppCart;
        }
        public ActionResult bando()
        {
            return View();
        }
        public ActionResult nuochoa404()
        {
            return View();
        }
        //void SendMail(string to, string cc, string bbc, string reply, string subject, string messages)
        //{
            
        //    var configmail = data.Configs.FirstOrDefault();
        //    SendMail(to, cc, bbc, reply, subject, messages, "smtp.gmail.com", "465", configmail.Mail_Noreply, configmail.Mail_Noreply, configmail.Mail_Password);
        //}
        //void SendMail(string to, string cc, string bbc, string reply, string subject, string messages, string smtp, string port, string from, string user, string password)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.To = to;
        //        if (!string.IsNullOrEmpty(cc)) mail.CC = cc;
        //        if (!string.IsNullOrEmpty(bbc)) mail.Bcc = bbc;
        //        if (!string.IsNullOrEmpty(reply)) mail.Headers.Add("Reply-To", reply);
        //        mail.From = from;
        //        mail.Subject = subject;
        //        mail.BodyEncoding = Encoding.GetEncoding("utf-8");
        //        mail.BodyFormat = MailFormat.Html;
        //        mail.Body = messages;
        //        mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;
        //        mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = smtp;
        //        mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = port;
        //        if (port != "465")
        //        {
        //            mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = 0; // "true";
        //        }
        //        else
        //        {
        //            mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = 1; // "true";
        //        }
        //        //mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"] = 60;
        //        mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
        //        mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = user;
        //        mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = password;
        //        //SmtpMail.SmtpServer = smtp;
        //        SmtpMail.Send(mail);
        //    }
        //    catch { }
        //}

    }
}
