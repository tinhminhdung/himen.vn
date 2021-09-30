using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class My_PartialviewController : Controller
    {
         wwwEntities db = new wwwEntities();
        #region [Menu top]
        public ActionResult _MenuTop()
        {
            string chuoi = "";
            string strMobileMenu = "<ul data-breakpoint=\"1025\" class=\"flexnav\">";
            var cat = db.Menus.Where(m => m.Level.Length == 5 && m.Active==true && m.Position==1).OrderBy(m => m.Ord).ToList();
            
            for (int i = 0; i < cat.Count; i++)
            {
                List<Menu> menus = db.Menus.ToList();
                List<Menu> catsub = new List<Menu>();
                string levelm = cat[i].Level;
                catsub = db.Menus.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == levelm && m.Active == true && m.Position == 1).OrderBy(m => m.Level).ToList();
                if (catsub.Count > 0)
                {
                    if (cat[i].Images != null && cat[i].Images != "")
                    {
                        chuoi += "<li style=\"background: url(" + cat[i].Images + ") left center no-repeat\"><a href=\"" + cat[i].Link + "\">" + cat[i].Name.Replace("-", "<br />") + "</a>";

                        strMobileMenu += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                    }
                    else
                    {
                        chuoi += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";

                        strMobileMenu += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";

                    }
                        chuoi += "<ul>";

                        strMobileMenu += "<ul>";
                        for (int k = 0; k < catsub.Count; k++)
                        {
                            string levelm10 = catsub[k].Level;
                            List<Menu> catsub10 = new List<Menu>();
                            catsub10 = db.Menus.Where(m => m.Level.Length == 15 && m.Level.Substring(0, 10) == levelm10 && m.Active==true && m.Position==1).OrderBy(m => m.Level).ToList();
                            if (catsub10.Count == 0)
                            {
                                chuoi += "<li><a href=\"" + catsub[k].Link + "\">" + catsub[k].Name + "</a></li>";

                                strMobileMenu += "<li><a href=\"" + catsub[k].Link + "\">" + catsub[k].Name + "</a></li>";
                            }
                            else
                            {
                                chuoi += "<li class=\"parent\"><a href=\"" + catsub[k].Link + "\">" + catsub[k].Name + "</a>";
                                chuoi += "<ul>";
                                for (int n = 0; n < catsub10.Count; n++)
                                {
                                    string levelm15 = catsub10[n].Level;
                                    List<Menu> catsub15 = new List<Menu>();
                                    catsub15 = db.Menus.Where(m => m.Level.Length == 20 && m.Level.Substring(0, 15) == levelm15).OrderBy(m => m.Level).ToList();
                                    if (catsub15.Count == 0)
                                    {
                                        chuoi += "<li><a href=\"" + catsub10[n].Link + "\">" + catsub10[n].Name + "</a></li>";
                                    }
                                    else
                                    {
                                        chuoi += "<li class=\"parent\"><a href=\"" + catsub10[n].Link + "\">" + catsub10[n].Name + "</a>";
                                        chuoi += "<ul>";
                                        for (int m = 0; m < catsub15.Count; m++)
                                        {
                                            chuoi += "<li><a href=\"" + catsub15[m].Link + "\">" + catsub15[m].Name + "</a></li>";
                                        }
                                        chuoi += "</ul></li>";
                                    }
                                }
                                chuoi += "</ul></li>";
                            }
                        }
                        chuoi += "</ul></li>";
                        strMobileMenu += "</ul></li>";
                }
                else
                {
                    if (cat[i].Images != null && cat[i].Images != "")
                    {
                        chuoi += "<li style=\"background: url(" + cat[i].Images + ") left center no-repeat\"><a href=\"" + cat[i].Link + "\">" + cat[i].Name.Replace("-", "<br />") + "</a></li>";
                        strMobileMenu += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                    }
                    else
                    {
                        chuoi += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                        strMobileMenu += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                    }
                }
            }

            strMobileMenu += "</ul>";


            ViewBag.CatMobile = strMobileMenu;
           
            ViewBag.Cat = chuoi;


            return PartialView();
        }
        #endregion
        // 1.
        public ActionResult _TopLine()
        {
            return PartialView();
        }

        public ActionResult dk_khuyenmai()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult dk_khuyenmaiSave(FormCollection fc)
        {
            string email = fc["Email"];
            var list = db.Customers.Where(m => m.Email == email).ToList();
            if (list.Count > 0)
            {
                
            }
            return View();
        }
        //[HttpPost]
        public string dkkm(string gt, string mail)
        {
            if (mail != null && mail != "")
            {
                var list = db.Customers.Where(m => m.Email == mail).ToList();
                if (list.Count > 0)
                {
                    return "Email của bạn đã được đăng ký!";
                }
                else
                {
                    string gtinh = "";
                    bool gioitinh = true;
                    if (gt == "nam")
                    {
                        gioitinh = true;
                    } 
                    else
                    {
                        gioitinh = false;
                    }
                    
                    string ten = "";
                    db.sp_Customer_Insert("Đăng ký nhận thông tin KM", mail, mail, "", mail, DateTime.Now, true, "", 0, 1, false, 1, "", 0, 0, gioitinh);
                    db.SaveChanges();
                    return "Bạn đã đăng ký thông tin thành công!";
                }
            }
            else
            {
                return "Bạn chưa nhập Email!";
            }
        }

        

        #region[Login - Register Form]
        public ActionResult _Login()
        {
            return PartialView();
        }
        #endregion

        #region[_MenuSearch]
        public ActionResult _Search()
        {
            return PartialView();
        }
        #endregion

        #region[_Logo]
        public ActionResult _Logo()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Position == 0 && m.Active == true).Take(1).ToList();
            if (list.Count > 0)
            {
                chuoi += "<a href=\""+ list[0].Link +"\">";
               chuoi += "<img src=\"" + list[0].Image + "\" alt=\""+ list[0].Name +"" + "\" />";
                chuoi += "</a>";
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion

        #region[Quảng cáo menu bên trái]
        public ActionResult _AdvLeft()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Position == 3 && m.Active == true).OrderBy(a => a.Orders).Take(10).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Image!=null && list[i].Image != "")
                {
                    chuoi += "<a href=\"" + list[i].Link + "\" title=\"" + list[i].Name + "\" >";
                    chuoi += "<img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" />";
                    chuoi += "</a>";
                }
                else
                {
                    chuoi += list[i].Description;
                } 
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion

        #region[_Slide]
        public ActionResult _Slide(string tag)
        {
            string chuoi = "";
            int grpid = 0;
            var g = db.GroupProducts.Where(m => m.Tag == tag).SingleOrDefault();
            if (g != null)
            {
                grpid = g.Id;
            }
            if (grpid != 0)
            {
               var list = (from a in db.v_Advertise_in_GrpID where a.Position == 2 && a.Active == true select a).ToList();
                list = list.Where(m=>m.GrpID==grpid).Take(10).ToList();
                if (list.Count == 0)
                {
                    var list1 = db.Advertises.Where(a => a.Position == 2 && a.Active == true).Take(10).ToList();
                    chuoi += "<div id='rotator'>\n";
                    chuoi += "<ul>\n";
                    for (int i = 0; i < list1.Count; i++)
                    {
                        if (i % list1.Count == 0)
                        {
                            chuoi += "<li class='show'><a href='" + list1[i].Link + "' title=\"" + list1[i].Name + "\"><img src='" + list1[i].Image + "' alt=\"" + list1[i].Name + "\"/></a></li>\n";
                        }
                        else
                        {
                            chuoi += "<li><a href='" + list1[i].Link + "' title=\"" + list1[i].Name + "\"><img src='" + list1[i].Image + "' alt=\"" + list1[i].Name + "\"/></a></li>\n";
                        }
                    }
                    chuoi += "</ul>\n";
                    chuoi += "</div>\n";
                }
                else
                {
                    chuoi += "<div id='rotator'>\n";
                    chuoi += "<ul>\n";
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i % list.Count == 0)
                        {
                            chuoi += "<li class='show'><a href='" + list[i].Link + "' title=\"" + list[i].Name + "\"><img src='" + list[i].Image + "' alt=\"" + list[i].Name + "\"/></a></li>\n";
                        }
                        else
                        {
                            chuoi += "<li><a href='" + list[i].Link + "' title=\"" + list[i].Name + "\"><img src='" + list[i].Image + "' alt=\"" + list[i].Name + "\"/></a></li>\n";
                        }
                    }
                    chuoi += "</ul>\n";
                    chuoi += "</div>\n";
                }
            }
            else
            {
               var list = db.Advertises.Where(a=>a.Position == 2 && a.Active == true).Take(10).ToList();
               chuoi += "<div id='rotator'>\n";
               chuoi += "<ul>\n";
               for (int i = 0; i < list.Count; i++)
               {
                   if (i % list.Count == 0)
                   {
                       chuoi += "<li class='show'><a href='" + list[i].Link + "' title=\"" + list[i].Name + "\"><img src='" + list[i].Image + "' alt=\"" + list[i].Name + "\"/></a></li>\n";
                   }
                   else
                   {
                       chuoi += "<li><a href='" + list[i].Link + "' title=\"" + list[i].Name + "\"><img src='" + list[i].Image + "' alt=\"" + list[i].Name + "\"/></a></li>\n";
                   }
               }
               chuoi += "</ul>\n";
               chuoi += "</div>\n";
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion

        #region[_Popup]
        public ActionResult _Popup()
        {
            string link = "";
            string anh = "";
            var list = db.Advertises.Where(m => m.Position == 1 && m.Active == true).ToList();
            if (list.Count > 0)
            {
                link = list[0].Link;
                anh = list[0].Image;
            }
            ViewBag.vlink = link;
            ViewBag.vanh = anh;
            return PartialView();
        }
        #endregion

        #region[_Right_DuoiMenu]
        public ActionResult _Right_DuoiMenu()
        {
            string chuoi = "";
            string chuoib = "";
            var list = db.Advertises.Where(m => (m.Position == 5 || m.Position == 6) && m.Active == true).Take(10).ToList();
            chuoi += "<div class=\"adv-home-right-top\">";
            chuoib += "<div class=\"adv-home-right-bt\">";
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Image != null && list[i].Image != "")
                {
                    if (list[i].Image.IndexOf(".swf") > 0)
                    {
                        if (list[i].Position == 5)
                        {
                            chuoi += "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://active.macromedia.com/ flash5/cabs/swflash.cab#version=5,0,0,0\" style=\"width: 269px; height: 130px\">";
                            chuoi += "<param name=\"MOVIE\" value=\"" + list[i].Image + "\">";
                            chuoi += "<param name=\"PLAY\" value=\"true\">";
                            chuoi += "<param name=\"LOOP\" value=\"true\">";
                            chuoi += "<param name=\"WMODE\" value=\"opaque\">";
                            chuoi += "<param name=\"QUALITY\" value=\"high\">";
                            chuoi += "<embed src=\"" + list[i].Image + "\" width=\"269px\" height=\"130px\" play=\"true\" loop=\"true\" wmode=\"opaque\" quality=\"high\" pluginspage=\"http://www.macromedia.com/shockwave/ download/index.cgi?P1_Prod_Version=ShockwaveFlash\">";
                            chuoi += "</object>";
                        }
                        else
                        {
                            chuoib += "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://active.macromedia.com/ flash5/cabs/swflash.cab#version=5,0,0,0\" style=\"width: 269px; height: 130px\">";
                            chuoib += "<param name=\"MOVIE\" value=\"" + list[i].Image + "\">";
                            chuoib += "<param name=\"PLAY\" value=\"true\">";
                            chuoib += "<param name=\"LOOP\" value=\"true\">";
                            chuoib += "<param name=\"WMODE\" value=\"opaque\">";
                            chuoib += "<param name=\"QUALITY\" value=\"high\">";
                            chuoib += "<embed src=\"" + list[i].Image + "\" width=\"269px\" height=\"130px\" play=\"true\" loop=\"true\" wmode=\"opaque\" quality=\"high\" pluginspage=\"http://www.macromedia.com/shockwave/ download/index.cgi?P1_Prod_Version=ShockwaveFlash\">";
                            chuoib += "</object>";
                        }
                    }
                    else
                    {
                        if (list[i].Position == 5)
                        {
                            chuoi += "<a href=\"" + list[i].Link + "\" title=\"" + list[i].Name + "\" >";
                            chuoi += "<img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" />";
                            chuoi += "</a>";
                        }
                        else
                        {
                            chuoib += "<a href=\"" + list[i].Link + "\" title=\"" + list[i].Name + "\" >";
                            chuoib += "<img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" />";
                            chuoib += "</a>";
                        }
                    }
                }
                else
                {
                    chuoi += list[i].Description;
                }
            }
            chuoi += "</div>";
            chuoib += "</div>";
            ViewBag.View = chuoi;
            ViewBag.Viewb = chuoib;
            return PartialView();
        }
        #endregion

        #region[_Footer]
        public ActionResult _Footer()
        {
            string chuoi = "";
            var list = db.Configs.ToList();
            if (list.Count>0)
            {
                chuoi = list[0].Copyright;
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion

        #region[_CopyRight]
        public ActionResult _CopyRight()
        {
            string chuoi = "";
            var list = db.Configs.Where(m => m.Id >0).Take(1).ToList();
            if (list.Count > 0)
            {
                ViewBag.View = list[0].Copyright;
            }
            return PartialView();
        }
        #endregion

        #region[_TopHead]
        public ActionResult _TopHead()
        {
            string chuoi = "";
            var list = db.Configs.Where(m => m.Id > 0).Take(1).ToList();
            if (list.Count > 0)
            {
                ViewBag.head = list[0].Contact;
            }
            return PartialView();
        }
        #endregion

        #region[Menu-left]
        public ActionResult _MenuLeft()
        {
            string chuoi = "";
            string chuoi1 = "";
            var list = db.GroupProducts.Where(m => m.Active == true && m.Level.Length==5).OrderBy(g =>g.Level).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                List<GroupProduct> catsub = new List<GroupProduct>();
                string levelm = list[i].Level;
                catsub = db.GroupProducts.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == levelm).OrderBy(m => m.Level).ToList();
                if (catsub.Count > 0)
                {
                    chuoi += "<li class=\"parent\"><a><img src=\""+ list[i].Icon +"\" />" + list[i].Name + "</a>";
                    //chuoi += "<li class=\"parent\"><a href=\"/danh-muc/" + list[i].Tag + "\"><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a>";
                    if (list[i].Images.Length > 0)
                    {
                        chuoi += "<ul class=\"navl\" style=\"background: #fff url(" + list[i].Images + ") right bottom no-repeat\">";
                    }
                    else
                    {
                        chuoi += "<ul class=\"navl\">";
                    }
                    for (int j = 0; j < catsub.Count; j++)
                    {
                         List<GroupProduct> catsub10 = new List<GroupProduct>();
                        string levelm10 = catsub[j].Level;
                        catsub10 = db.GroupProducts.Where(m => m.Level.Length == 15 && m.Level.Substring(0, 10) == levelm10).OrderBy(m => m.Level).ToList();
                        if (catsub10.Count > 0)
                        {
                            chuoi += "<li><a>" + catsub[j].Name + "</a>";
                            //chuoi += "<li><a href=\"/danh-muc/" + catsub[j].Tag + "\">" + catsub[j].Name + "</a>";
                            //chuoi += "<ul>";
                            for (int k = 0; k < catsub10.Count; k++)
                            {
                                chuoi += "<p><a href=\"/danh-muc/" + catsub10[k].Tag + "\">" + catsub10[k].Name + "</a></p>";
                            }
                            chuoi += "</li>";
                            //chuoi += "</ul></li>";
                        }
                        else
                        {
                            chuoi += "<li><a href=\"/danh-muc/" + catsub[j].Tag + "\">" + catsub[j].Name + "</a></li>";
                        }
                    }
                    chuoi += "</ul></li>";
                }
                else
                {
                    chuoi += "<li><a href=\"/danh-muc/" + list[i].Tag + "\"><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a></li>";
                }
                if (Request.Url.ToString().IndexOf(list[i].Tag) > 0)
                {
                    chuoi1 += "<li><a href=\"/danh-muc/" + list[i].Tag + "\"><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a></li>";
                    List<GroupProduct> catsubmobi = new List<GroupProduct>();
                    string levelmobi = list[i].Level;
                    catsubmobi = db.GroupProducts.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == levelmobi).OrderBy(m => m.Level).ToList();
                    for (int k = 0; k < catsubmobi.Count; k++)
                    {
                        List<GroupProduct> catsub10 = new List<GroupProduct>();
                        string levelm10 = catsub[k].Level;
                        catsub10 = db.GroupProducts.Where(m => m.Level.Length == 15 && m.Level.Substring(0, 10) == levelm10).OrderBy(m => m.Level).ToList();
                        if (catsub10.Count > 0)
                        {
                            chuoi1 += "<li class=\"current\"><a href=\"/danh-muc/" + catsubmobi[k].Tag + "\">+ " + catsubmobi[k].Name + "</a></li>";
                            for (int l = 0; l < catsub10.Count; l++)
                            {
                                chuoi1 += "<li class=\"currentsub\"><a href=\"/danh-muc/" + catsub10[l].Tag + "\">- " + catsub10[l].Name + "</a></li>";
                            }
                        }
                        else
                        {
                            chuoi1 += "<li class=\"current\"><a href=\"/danh-muc/" + catsubmobi[k].Tag + "\">- " + catsubmobi[k].Name + "</a></li>";
                        }
                    }
                }
                else
                {
                    chuoi1 += "<li><a href=\"/danh-muc/" + list[i].Tag + "\"><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a></li>";
                }
            }
            ViewBag.View = chuoi;
            ViewBag.mobi = chuoi1;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion

        #region[Danh mục hang đầu - chân trang]
        public ActionResult _Danhmuchangdau()
        {
            string chuoi = "";
            var list = db.GroupProducts.Where(m => m.Active == true && m.Priority==true).OrderBy(g => g.Level).Take(50).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                chuoi += "<li>- <a href=\"/danh-muc/" + list[i].Tag + "\">" + list[i].Name + "</a></li>";
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion

        #region[Tin tức hỗ trợ - chân trang]
        public ActionResult _Tintuchotro()
        {
            string chuoi = "";
            var list = db.Menus.Where(m => m.Active == true && m.Position == 2).Take(1).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                chuoi += "<li><a href=\"" + list[i].Link + "\">" + list[i].Name + "</a></li>";
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion
        #region[News-left]
        public ActionResult _NewsLeft()
        {
            string chuoi = "";
            var list = (from p in db.v_Product_DescTotalNumber select p).Take(20).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                chuoi += "<li><p><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\">" + list[i].Name + "</a></p><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"/></a><p>Giá bán: " + StringClass.Format_Price(list[i].PricePromotion.ToString()) + "đ</p></li>";
                //chuoi += "<li><p><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\">" + list[i].Name + "</a></p><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"/></a><p class=\"cu\">Giá cũ: " + StringClass.Format_Price(list[i].PriceRetail.ToString()) + "đ</p><p>Giá bán: " + StringClass.Format_Price(list[i].PricePromotion.ToString()) + "đ</p></li>";
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion

        #region[Product-left]
        public ActionResult _ProducLeft()
        {
            string chuoi = "";
            var list = (from p in db.v_Product_DescTotalNumber select p).Take(30).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                chuoi += "<li><p><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\">" + list[i].Name + "</a></p><a href=\"/thong-tin/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"/></a><p>Giá bán: " + StringClass.Format_Price(list[i].PricePromotion.ToString()) + "đ</p></li>";
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion

        #region[Quảng cao trượt bên phải]
        public ActionResult _quangCao2Ben_benTrai()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Active == true && m.Position==2).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                chuoi += "<a href=\"" + list[i].Link + "\"><img src=\"" + list[i].Image + "\" /></a>";
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion
        #region[Quảng cáo trượt bên trái]
        public ActionResult _Adv_truot_benphai()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Active == true && m.Position == 3).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                chuoi += "<a href=\"" + list[i].Link + "\"><img src=\"" + list[i].Image + "\" /></a>";
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion
        #region[QuangCao2Ben_benPhai]
        public ActionResult _quangCao2Ben_benPhai()
        {
            //var tong = db.Ords.ToList();
            //ViewBag.Tong = tong.Count();
            //ViewBag.Moi = tong.Where(o => o.Status == "1").Count();
            string chuoi = "";
            string chuoi_tel = "<p><span>Tel</span>: ";
            string chuoi_hotline = "<p><span>Hotline</span>: ";
            string chuoi_mobile = "<p><span>Mobile</span>: ";
            var list = db.Supports.ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Type == 0)
                    {
                        chuoi += "<a href=\"ymsgr:sendim?" + list[i].Nick + "\" title=\"" + list[i].Name + "\"><img alt=\"" + list[i].Name + "\" src=\"http://opi.yahoo.com/online?u=" + list[i].Nick + "&amp;m=g&amp;t=1\" border=\"0\" align=\"middle\"></a>";
                    }
                    else if (list[i].Type == 1)
                    {
                        chuoi += "<a href=\"skype:"+ list[i].Nick +"?chat\"><img src=\"HTTP://MYSTATUS.SKYPE.COM/smallclassic/"+ list[i].Nick +"\" title=\""+ list[i].Name +"\"></a>";
                    }
                    else if (list[i].Type == 2)
                    {
                        chuoi_hotline += "<span class=\"black bold\">" + list[i].Tel + "</span></p>";
                    }
                    else if (list[i].Type == 3)
                    {
                        chuoi_tel += "<span class=\"black bold\">" + list[i].Tel + "</span></p>";
                    }
                    else if (list[i].Type == 4)
                    {
                        chuoi_mobile += "<span class=\"black bold\">" + list[i].Tel + "</span></p>";
                    }
                }
                chuoi = chuoi_tel + chuoi_mobile + chuoi_hotline + chuoi;
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion

        #region[Autocomplete Product Name]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var productNames = from p in db.Products
                               select p.Name;
            string[] items = productNames.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region[Thống kê đơn hàng]
        public ActionResult _ThongKeDonHang()
        {   
            var tong= db.Ords.ToList();
            ViewBag.Tong = tong.Count();
            ViewBag.Moi = tong.Where(o => o.Status == "1").Count();
            ViewBag.Nhantien = tong.Where(o => o.Status == "2").Count();
            ViewBag.Nhanguihang = tong.Where(o => o.Status == "3").Count();
            ViewBag.Huy = tong.Where(o => o.Status == "4").Count();
            return PartialView();
        }
        #endregion
        #region[DropCategory]
        public ActionResult _DropCategory()
        {
            string chuoi = "";
            var list = db.GroupProducts.Where(m => m.Active == true && m.Level.Length == 5).OrderBy(g => g.Level).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                List<GroupProduct> catsub = new List<GroupProduct>();
                string levelm = list[i].Level;
                catsub = db.GroupProducts.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == levelm).OrderBy(m => m.Level).ToList();
                if (catsub.Count > 0)
                {
                    chuoi += "<li class=\"parent\"><a><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a>";
                    //chuoi += "<li class=\"parent\"><a href=\"/danh-muc/" + list[i].Tag + "\"><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a>";
                    if (list[i].Images.Length > 0)
                    {
                        chuoi += "<ul class=\"topdrop\" style=\"background: #fff url(" + list[i].Images + ") right bottom no-repeat\">";
                    }
                    else
                    {
                        chuoi += "<ul class=\"topdrop\">";
                    }
                    for (int j = 0; j < catsub.Count; j++)
                    {
                        List<GroupProduct> catsub10 = new List<GroupProduct>();
                        string levelm10 = catsub[j].Level;
                        catsub10 = db.GroupProducts.Where(m => m.Level.Length == 15 && m.Level.Substring(0, 10) == levelm10).OrderBy(m => m.Level).ToList();
                        if (catsub10.Count > 0)
                        {
                            chuoi += "<li><a>" + catsub[j].Name + "</a>";
                            //chuoi += "<li><a href=\"/danh-muc/" + catsub[j].Tag + "\">" + catsub[j].Name + "</a>";
                            //chuoi += "<ul>";
                            for (int k = 0; k < catsub10.Count; k++)
                            {
                                chuoi += "<p><a href=\"/danh-muc/" + catsub10[k].Tag + "\">" + catsub10[k].Name + "</a></p>";
                            }
                            chuoi += "</li>";
                            //chuoi += "</ul></li>";
                        }
                        else
                        {
                            chuoi += "<li><a href=\"/danh-muc/" + catsub[j].Tag + "\">" + catsub[j].Name + "</a></li>";
                        }
                    }
                    chuoi += "</ul></li>";
                }
                else
                {
                    chuoi += "<li><a href=\"/danh-muc/" + list[i].Tag + "\"><img src=\"" + list[i].Icon + "\" />" + list[i].Name + "</a></li>";
                }
            }
            ViewBag.View = chuoi;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion
        #region[Sản phẩm nhiều người mua]
        public ActionResult _ProductTab2()
        {
            var listn = (from p in db.v_Product_DescTotalNumber where p.Active == true select p).Take(28).ToList();
            return PartialView(listn);
        }
         public ActionResult _ProductTab3()
        {
            var listn = (from p in db.Products where p.PricePromotion < p.PriceRetail && p.PriceRetail>0  && p.Active == true select p).Take(28).ToList();
            return PartialView(listn);
        }
        #endregion
    }
}
