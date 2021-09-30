using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList.Mvc;
using PagedList;
using System.Data;

namespace MODEOUTLED.Controllers
{
    public class OrderController : Controller
    {
        wwwEntities db = new wwwEntities();

        #region[OrderIndex]
        public ActionResult OrderIndex()
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
            var all = db.Ords.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = db.sp_Order_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = Phantrang.PhanTrang(25, curpage, numOfNews, url);
            string Chuoi = "";
            for (int i = 0; i < pages.Count; i++)
            {
                int pid = pages[i].IdCus;
                var cus = db.sp_Customer_GetById(pid).ToList();
                if (cus.Count > 0)
                {
                    if (i % 2 == 0)
                    {
                        Chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                        Chuoi += "<td class=\"CheckBoxsmall\"><input id='chkSelect' name='chk" + pages[i].Id + "' onclick = 'Javascript:chkSelect_OnClick(this,0)' class=\"chk\" type='checkbox'/></td>";
                        Chuoi += "<td class='Text'>" + cus[0].Name + "</td>";
                        Chuoi += "<td class='Text'>" + cus[0].Address + "</td>";
                        Chuoi += "<td class='DateTime'>" + pages[i].SDate + "</td>";
                        Chuoi += "<td class='TextSmall'>" + StringClass.ShowStateBill(pages[i].Status.ToString()) + "</td>";
                        Chuoi += "<td class='Function'>";
                        Chuoi += "<a href='/OrderEdit/" + pages[i].Id + "' class='vedit'>Sửa</a>";
                        Chuoi += "<a href='/OrderDelete/" + pages[i].Id + "' onclick='return DeleteConfirm()' class='vdelete'>Xóa</a>";

                        Chuoi += "</td>";
                        Chuoi += "</tr>";
                    }
                    else
                    {
                        Chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                        Chuoi += "<td class=\"CheckBoxsmall\"><input id='chkSelect' name='chk" + pages[i].Id + "' onclick = 'Javascript:chkSelect_OnClick(this,0)' class=\"chk\" type='checkbox'/></td>";
                        Chuoi += "<td class='Text'>" + cus[0].Name + "</td>";
                        Chuoi += "<td class='Text'>" + cus[0].Address + "</td>";
                        Chuoi += "<td class='DateTime'>" + pages[i].SDate + "</td>";
                        Chuoi += "<td class='TextSmall'>" + StringClass.ShowStateBill(pages[i].Status.ToString()) + "</td>";
                        Chuoi += "<td class='Function'>";
                        Chuoi += "<a href='/OrderEdit/" + pages[i].Id + "' class='vedit'>Sửa</a>";
                        Chuoi += "<a href='/OrderDelete/" + pages[i].Id + "' onclick='return DeleteConfirm()' class='vdelete'>Xóa</a>";
                        Chuoi += "</td>";
                        Chuoi += "</tr>";
                    }
                }
            }
            ViewBag.View = Chuoi;
            return View();
        }
        #endregion

        #region[OrderIndexot]
        public ActionResult OrderIndexot(int? page, string SoDH)
        {
            int imPage = 0;
            var all = db.Ords.OrderByDescending(o => o.Id).ToList();

            ViewBag.Bill = db.v_OrderDetail_Product.OrderByDescending(o => o.Id).ToList();

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            if (Request.HttpMethod == "GET")
            {
                if (Session["SoDH"] != null)
                {
                    SoDH = Session["SoDH"].ToString();
                    Session["SoDH"] = null;
                }
            }
            else
            {
                page = 1;
            }

            if (!String.IsNullOrEmpty(SoDH))
            {
                int oId = Int32.Parse(SoDH);
                all = all.Where(p => p.Id == oId || p.Tel == oId.ToString() || p.Name == oId.ToString()).OrderByDescending(p => p.Id).ToList();
            }

            // begin [get last page]
            if (page != null)
            {
                ViewBag.mPage = (int)page;
                imPage = (int)page;
            }
            else
            {
                ViewBag.mPage = 1;
                imPage = 1;
            }
            ViewBag.PageSize = pageSize;

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

            string Chuoi = "";
            for (int i = 0; i < all.Count; i++)
            {
                Chuoi+="<table class=\"table table-striped table-bordered dataTable table-hover\" cellspacing=\"0\"  style=\"border-collapse: collapse;\">";
                    Chuoi+="<tr>";
                        Chuoi+="<th scope=\"col\" class=\"CheckBoxsmall\" style=\"width: 10px; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb;\">";
                        Chuoi += "<input name=\"mPage\" id=\"mPage\" value=\"" + imPage.ToString() + "\" hidden=\"hidden\" />";
                        Chuoi += "<input name=\"PageSize\" id=\"PageSize\" value=\"" + pageSize.ToString() + "\" hidden=\"hidden\" />";
                            //Chuoi+="@Html.CheckBox(\"chk\" "+ all[i].Id.ToString() +", new { id = \"chkSelect\", onclick = \"Javascript:chkSelect_OnClick(this,0)\", @class = \"chk\" })";
                            Chuoi += "<input type=\"checkbox\" Name=\"chk"+ all[i].Id.ToString() +"\" id = \"chkSelect\" onclick = \"Javascript:chkSelect_OnClick(this,0)\" class =\"chk\" />";
                        Chuoi+="</th>";
                        Chuoi += "<th scope=\"col\" id=\"name\" style=\"width: 250px; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb; color: #fff\">Số đơn hàng: " + all[i].Id.ToString() + "</th>";
                        Chuoi += "<th scope=\"col\" id=\"date\" style=\"width: 150px; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb; color: #fff\">Ngày đặt hàng: " + all[i].SDate.Value.ToShortDateString() + "</th>";
                        Chuoi+="<th scope=\"col\" id=\"date\" style=\"width: 200px; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb; color: #fff\">Họ tên: " + all[i].Customer.Name + "</th>";
                        Chuoi+="<th scope=\"col\" id=\"date\" style=\"width: 200px; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb; color: #fff\">Điện thoại: " + all[i].Customer.Tel + "</th>";
                        Chuoi+="<th scope=\"col\" id=\"status\" style=\"width: 200px; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb; color: #fff;\">Tình trạng: ";
                        Chuoi += "<label id=\"" + all[i].Id.ToString() + "\" style=\"display:inline; font-weight:bold;\">" + StringClass.ShowStateBill(all[i].Status.ToString()) + "</label>";
                        Chuoi+="</th>";
                        Chuoi+="<th scope=\"col\" class=\"number\" style=\"text-align: center; vertical-align: middle; padding: 1px 10px; background-color: #22c0cb;\">";
                        Chuoi += "<a style=\"width:50px; float:left;color:#fff\" href=\"../../Order/OrderEdit?id=" + all[i].Id.ToString() + "\" class=\"action-link-button\" title=\"Chi tiết hóa đơn\"><i class=\"icon-info-sign\"></i></a>";
                        Chuoi += "<a style=\"width:10px; float:left; color:#fff\" href=\"OrderDelete?id=" + all[i].Id.ToString() + "&page=" + imPage.ToString() + "&pagesize=" + pageSize.ToString() + "\" class=\"yesno action-link-button\" title=\"Bạn chắc chắn muốn xóa hóa đơn này ?\"><i class=\"icon-trash\"></i></a>";
                        Chuoi+="</th>";
                    Chuoi+="</tr>";
                Chuoi+="</table>";
                Chuoi+="<!--Chi tiết hóa đơn-->";
                int oID = all[i].Id;
                var bill = (from b in db.v_OrderDetail_Product where b.IdOr == oID select b).ToList();
                //Chuoi+="float? totalPrice = (from o in Model where (o.Id == Model[i].Id) select o).Single().Amount;";
                Chuoi+="<table class=\"table table-striped table-bordered dataTable table-hover\" cellspacing=\"0\" id=\"cph_Main_ctl00_ctl00_grvProducts\" style=\"border-collapse: collapse;\">";
                    Chuoi+="<tr>";
                        Chuoi+="<th scope=\"col\" style=\"width: 60px; text-align: center;\">Ảnh SP</th>";
                        Chuoi+="<th scope=\"col\" style=\"width: 170px;\">Tên Sản phẩm</th>";
                        Chuoi+="<th scope=\"col\" style=\"width: 60px;\">Số lượng</th>";
                        Chuoi+="<th scope=\"col\" style=\"width: 90px;\">Giá bán(vnđ)</th>";
                        Chuoi+="<th scope=\"col\" style=\"width: 90px;\">Thành tiền(vnđ)</th>";
                       //Chuoi+="<th scope=\"col\" style=\"width: 330px;\">Tiền vận chuyển(vnđ)</th>";
                        Chuoi+="<th scope=\"col\" style=\"width: 120px; text-align: center;\" rowspan=\"500\">Tổng tiền(vnđ)";

                            //Chuoi+="<p style=\"color: red\">"+ os.os.Format_Price(all[i].Amount.ToString()) +" VNĐ</p>Nhập mã bưu điện<br />";
                            //Chuoi += "<input type=\"text\" Name=\"Ord" + all[i].Id.ToString() + "\" value = \"\" id=\"PostCode" + all[i].Id.ToString() + "\" class =\"PostCode\" style=\"width:120px;\"/>";
                        Chuoi+="</th>";
                        Chuoi+="<th scope=\"col\" style=\"width: 120px; text-align: center;\" rowspan=\"500\">";
                            if (all[i].Status == "1")
                            {
                                Chuoi+="<label id=\"Nhantien\" class=\"btn Nhantien\" data-id=\"" + all[i].Id +"\"><i class=\"icon-save\"></i>&nbsp;Đã nhận tiền</label>";
                            }
                            else if (all[i].Status == "2")
                            {
                                Chuoi+="<label id=\"Guihang\" class=\"btn Guihang\" data-id=\"" + all[i].Id +"\"><i class=\"icon-save\"></i>&nbsp;Đã gửi hàng</label>";
                            }
                            else if (all[i].Status == "3")
                            {
                                Chuoi += "<label>Đơn hàng đã gửi</label>";
                            }
                            else
                            {
                                Chuoi += "<label>Đơn hàng bị hủy</label>";
                            }
                        Chuoi+="</th>";
                   Chuoi+=" </tr>";
                    for(int j=0; j<bill.Count; j++)
                    {
                        Chuoi+="<tr style=\"background-color: #fff\">";
                            Chuoi+="<td class=\"text-center\" style=\"vertical-align: middle; padding: 2px 0px;\">";
                                Chuoi+="<img width=\"60px\" src=\""+ bill[j].Image +"\" alt=\"\"/></td>";
                                Chuoi += "<td style=\"vertical-align: middle;\">" + bill[j].Name + "";
                                //Chuoi += "<p style=\"color: #22c0cb; text-align: left; padding: 0; margin: 0\"> - Màu: " + bill[j].CName + " <br /> - Size: " + bill[j].SName + "";
                                Chuoi+="</p>";
                            Chuoi+="</td>";
                            Chuoi += "<td class=\"text\" style=\"vertical-align: middle; text-align: center\">" + bill[j].Name + "</td>";
                            Chuoi+="<td class=\"text\" style=\"vertical-align: middle; text-align: center\">" + StringClass.Format_Price(bill[j].Price.ToString()) +"</td>";
                            Chuoi += "<td class=\"text\" style=\"vertical-align: middle; text-align: center\">" + StringClass.Format_Price(bill[j].Total.ToString()) + "</td>";

                            //Chuoi += " <td class=\"text\" style=\"vertical-align: middle;\">- Tiền VC: <span style=\"color: red\">" + StringClass.Format_Price(all[i].PriceVC.ToString()) + "<br />";
                            //Chuoi += "- Cộng thêm 20% cước VC ngoại thành, nông thôn nếu có:<span style=\"color: red\">" + StringClass.Format_Price(all[i].PriceNT.ToString()) + "</span><br />";
                            //    if (all[i].TypePay == "1")
                            //    {
                            //        Chuoi+="<label style=\"font-weight: bold\">- Hình thức VC: Chuyển phát nhanh</label>";
                            //    }
                            //    else if (all[i].TypePay == "2")
                            //    {
                            //        Chuoi+="<label style=\"font-weight: bold\">- Hình thức VC: Chuyển phát bình thường</label>";
                            //    }
                            //    else if (all[i].TypePay == "3")
                            //    {
                            //       Chuoi+=" <label style=\"font-weight: bold\">- Hình thức VC: Chuyển bằng ô tô</label>";
                            //    }
                            //Chuoi+="</td>";
                        Chuoi+="</tr>";
                   }
                Chuoi+="</table>";
                Chuoi += "<div id=\"mnbtomad\">";
                Chuoi += "<div class=\"head\">Thông tin nhận hàng:</div>";
                if (all[i].Name != null && all[i].Name != "")
                {
                    string NameProvince = "";
                    string times = "";
                    int tinhid = int.Parse(all[i].ProviceId.ToString());
                    var provi = db.Provinces.Where(m => m.Id == tinhid).FirstOrDefault();
                    if (provi != null)
                    {
                        NameProvince = provi.Name;
                        if (all[i].TypePay == "1")
                        {
                            times = provi.Time;
                        }
                        else if (all[i].TypePay == "2")
                        {
                            times = provi.Time1;
                        }
                        else
                        {
                            times = provi.Time2;
                        }
                    }
                    Chuoi += "<div class=\"item\" style=\"margin-left: 135px;\">Họ tên: <span>" + all[i].Name + "</span></div>";
                    Chuoi += "<div class=\"item\">Điện thoại: <span>" + all[i].Tel + "</span></div>";
                    //if (all[i].TypePay == "1")
                    //{
                    //    Chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát nhanh - </span><b>" + times + "</b></div>";
                    //}
                    //else if (all[i].TypePay == "2")
                    //{
                    //    Chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát thường - </span><b>" + times + "</b></div>";
                    //}
                    //else
                    //{
                    //    Chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Ô tô - </span><b>" + times + "</b></div>";
                    //}
                    Chuoi += "<div class=\"item500\">Địa chỉ: <span>" + all[i].Address + " - " + NameProvince + "</span></div>";
                }
                else
                {
                    string times = "";
                    string NameProvince = "";
                    int tinhid = int.Parse(all[i].Customer.Provice.ToString());
                    var provi = db.Provinces.Where(m => m.Id == tinhid).FirstOrDefault();
                    if (provi != null)
                    {
                        NameProvince = provi.Name;
                        if (all[i].TypePay == "1")
                        {
                            times = provi.Time;
                        }
                        else if (all[i].TypePay == "2")
                        {
                            times = provi.Time1;
                        }
                        else
                        {
                            times = provi.Time2;
                        }
                    }
                    Chuoi += "<div class=\"item\" style=\"margin-left: 135px;\">Họ tên: <span>" + all[i].Customer.Name + "</span></div>";
                    Chuoi += "<div class=\"item\">Điện thoại: <span>" + all[i].Customer.Tel + "</span></div>";
                    //if (all[i].TypePay == "1")
                    //{
                    //    Chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát nhanh - </span><b>" + times + "</b></div>";
                    //}
                    //else if (all[i].TypePay == "2")
                    //{
                    //    Chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát thường - </span><b>" + times + "</b></div>";
                    //}
                    //else
                    //{
                    //    Chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Ô tô - </span><b>" + times + "</b></div>";
                    //}
                    Chuoi += "<div class=\"item500\">Địa chỉ: <span>" + all[i].Customer.Address + " - " + NameProvince + "</span></div>";
                }
                Chuoi += "</div>";
                Chuoi += "</div>";
            }
            ViewBag.or = Chuoi;
            ViewBag.Pro = pro;

            return View(all.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region[OrderEdit]
        public ActionResult OrderEdit(int id)
        {
            var Edit = db.Ords.First(m => m.Id == id);
            string Chuoicus = "";
            string Chuoirec = "";
            string Chuoibill = "";
            string Chuoi = "";
            var cus = db.Customers.Where(m => m.Id == Edit.IdCus).ToList();
            int idtinh = cus[0].Id;
            var tinh = db.Provinces.Where(m => m.Id == idtinh).ToList();
            var rec = db.Recipients.Where(m => m.IdCus == Edit.IdCus).ToList();
            var bill = db.OrderDetails.Where(m => m.IdOr == Edit.Id).ToList();

            #region[Các nút cập nhật]
            Chuoi += "<div class=\"ui-corner-top ui-corner-bottom\">";
            Chuoi += "<div id=\"toolbox\">";
            Chuoi += "<div style=\"float: right;\" class=\"toolbox-content\">";
            Chuoi += "<table class=\"toolbar\">";
            Chuoi += "<tr>";
            //if (Request.Cookies["Username"] != null)
            //{
            //    Chuoi += "<td align=\"center\">";
            //    Chuoi += "<a title=\"Đã giao hàng\" class=\"toolbar btn btn-info\" href='/Order/OrderUpdateState/" + Edit.Id + "'><i class=\"icon-check\"></i>&nbsp;Đã giao hàng</a>";
            //    Chuoi += "</td>";
            //    Chuoi += "<td align=\"center\">";
            //    Chuoi += "<a title=\"Hủy đơn hàng\" class=\"toolbar btn btn-info\" href='/Order/OrderUpdateState/" + Edit.Id + "?del=2'><i class=\"icon-trash\"></i>&nbsp;Hủy đơn hàng</a>";
            //    Chuoi += " </td>";
            //}
            //else
            //{
            //    Chuoi += "<td align=\"center\">";
            //    Chuoi += "<p class='uupdate' onclick='AlertErr()'>Đã giao hàng</p>";
            //    Chuoi += "</td>";
            //    Chuoi += "<td align=\"center\">";
            //    Chuoi += "<p class='uupdate' onclick='AlertErr()'>Hủy đơn hàng</p>";
            //    Chuoi += " </td>";
            //}
            Chuoi += "<td align=\"center\">";
            Chuoi += "<a title=\"Trở lại\" class=\"toolbar btn btn-info\" href='/Order/OrderIndexot'><i class=\"icon-chevron-left\"></i>&nbsp;Trở lại</a>";
            Chuoi += " </td>";

            Chuoi += "</tr>";
            Chuoi += "</table>";
            Chuoi += "</div>";
            Chuoi += " </div>";
            Chuoi += "</div>";
            #endregion

            #region[Thông tin khách hàng]
            if (cus.Count > 0)
            {
                Chuoicus += "<h4><i class=\"icon-user\"></i>&nbsp;Thông tin khách hàng</h4>";
                Chuoicus += " <table class=\"admintable\" style=\"width: 100%;\">";

                Chuoicus += "<tr>";
                Chuoicus += "<td class=\"key\">Họ tên";
                Chuoicus += "</td>";
                Chuoicus += "<td>" + cus[0].Name;
                Chuoicus += "</td>";
                Chuoicus += "</tr>";

                Chuoicus += "<tr>";
                Chuoicus += "<td class=\"key\">Địa chỉ";
                Chuoicus += "</td>";
                Chuoicus += "<td>" + cus[0].Address + "";
                Chuoicus += "</td>";
                Chuoicus += "</tr>";

                Chuoicus += "<tr>";
                Chuoicus += "<td class=\"key\">Điện thoại";
                Chuoicus += "</td>";
                Chuoicus += "<td>" + cus[0].Tel;
                Chuoicus += "</td>";
                Chuoicus += "</tr>";

                Chuoicus += "<tr>";
                Chuoicus += "<td class=\"key\">Email";
                Chuoicus += "</td>";
                Chuoicus += "<td>" + cus[0].Email;
                Chuoicus += "</td>";
                Chuoicus += "</tr>";

                Chuoicus += "</table>";

            }
            #endregion

            #region[Thông tin người nhận hàng]
            if (rec.Count > 0)
            {
                Chuoirec += "<h4><i class=\"icon-user\"></i>&nbsp;Thông tin người nhận hàng</h4>";
                Chuoirec += " <table class=\"admintable\" style=\"width: 100%;\">";

                Chuoirec += "<tr>";
                Chuoirec += "<td class=\"key\">Họ tên";
                Chuoirec += "</td>";
                Chuoirec += "<td>" + rec[0].Name;
                Chuoirec += "</td>";
                Chuoirec += "</tr>";

                Chuoirec += "<tr>";
                Chuoirec += "<td class=\"key\">Địa chỉ";
                Chuoirec += "</td>";
                Chuoirec += "<td>" + rec[0].Address;
                Chuoirec += "</td>";
                Chuoirec += "</tr>";

                Chuoirec += "<tr>";
                Chuoirec += "<td class=\"key\">Điện thoại";
                Chuoirec += "</td>";
                Chuoirec += "<td>" + rec[0].Tel;
                Chuoirec += "</td>";
                Chuoirec += "</tr>";

                Chuoirec += "<tr>";
                Chuoirec += "<td class=\"key\">Email";
                Chuoirec += "</td>";
                Chuoirec += "<td>" + rec[0].Email;
                Chuoirec += "</td>";
                Chuoirec += "</tr>";

                Chuoirec += "</table>";
            }
            #endregion

            #region[Thông tin hóa đơn]
            Chuoibill += "<table class=\"table table-striped table-bordered dataTable table-hover\" cellspacing=\"0\" style=\"border-collapse: collapse;\">";
            Chuoibill += "<tr>";
            Chuoibill += "<th scope=\"col\" class=\"text\">Sản phẩm</th>";
            Chuoibill += "<th scope=\"col\" class=\"text\">Số lượng</th>";
            Chuoibill += "<th scope=\"col\" class=\"text\">Giá bán</th>";
            Chuoibill += "<th scope=\"col\" class=\"text\">Thành tiền (VND)</th>";
            Chuoibill += "</tr>";
            for (int i = 0; i < bill.Count; i++)
            {
                int billitem = bill[i].IdPro;
                var pro = db.Products.Where(m => m.Id == billitem).ToList();
                if (pro.Count > 0)
                {
                    Chuoibill += "<tr style=\"background-color: #fff\">";
                    Chuoibill += "<td style=\"text-align: left; width: 250px;\">" + pro[0].Name + "</td>";
                    Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + bill[i].Number + "</td>";
                    Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + Format_Price(bill[i].Price.ToString()) + " VNĐ</td>";
                    Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + Format_Price(bill[i].Total.ToString()) + " VNĐ</td>";
                    Chuoibill += "</tr>";
                }
                else
                {
                    Chuoibill += "<tr style=\"background-color: #fff\">";
                    Chuoibill += "<td style=\"text-align: left; width: 250px;\">" + bill[i].IdPro + "(đã xóa khỏi danh sách sản phẩm)</td>";
                    Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + bill[i].Number + "</td>";
                    Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + Format_Price(bill[i].Price.ToString()) + " VNĐ</td>";
                    Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + Format_Price(bill[i].Total.ToString()) + " VNĐ</td>";
                    Chuoibill += "</tr>";
                }
            }

            Chuoibill += "<tr style=\"background-color: #fff\">";
            Chuoibill += "<td colspan='3' class='totalprice' style=\"text-align: center; width: 250px;\">Tổng tiền</td>";
            Chuoibill += "<td style=\"text-align: left; width: 170px;\">" + Format_Price(Edit.Amount.ToString()) + " VNĐ</td>";
            Chuoibill += "</tr>";
            Chuoibill += "</table>";
            #endregion

            ViewBag.cus = Chuoicus;
            ViewBag.rec = Chuoirec;
            ViewBag.billdetail = Chuoibill;
            ViewBag.li = Chuoi;
            return View();
        }
        #endregion

        #region[Cap nhat trang thai don hang]
        public ActionResult OrderUpdateStatus(int id, string status)
        {
            var results = "";
            //if (Request.Cookies["Username"] != null)
            //{
            var up = db.Ords.First(m => m.Id == id);

            if (status != null)
            {
                if (status == "1")
                {
                    up.Status = "2";
                    results = "Đã nhận tiền";
                }

                if (status == "2")
                {
                    up.Status = "3";
                    results = "Đã gửi hàng";
                }

                if (status == "4")
                {
                    var detail = db.OrderDetails.Where(m => m.IdOr == up.Id).ToList();
                    for (int k = 0; k < detail.Count; k++)
                    {
                        int idPro = detail[k].IdPro;
                        var sp = db.Products.Where(m => m.Id == idPro).FirstOrDefault();
                        sp.Count = (sp.Count - detail[k].Number);
                        db.SaveChanges();
                    }
                    up.Status = "4";
                    results = "Đã hủy đơn hàng";
                }
            }
            db.SaveChanges();

            return Json(results);
            //}
            //else
            //{
            //    return Redirect("/Admins/admins");
            //}
        }
        #endregion

        #region[OrderDelete]
        public ActionResult OrderDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.Ords.First(m => m.Id == id);
                db.Ords.Remove(del);
                db.SaveChanges();
                return RedirectToAction("OrderIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Format_Price]
        protected string Format_Price(string Price)
        {
            Price = Price.Replace(".", "");
            Price = Price.Replace(",", "");
            string tmp = "";
            while (Price.Length > 3)
            {
                tmp = "." + Price.Substring(Price.Length - 3) + tmp;
                Price = Price.Substring(0, Price.Length - 3);
            }
            tmp = Price + tmp;
            return tmp;
        }
        #endregion

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (collection["btnDelete"] != null)
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
                                var Del = (from emp in db.Ords where emp.Id == id select emp).SingleOrDefault();
                                db.Ords.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("OrderIndexot");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["SoDH"].Length > 0)
                    {
                        Session["SoDH"] = collection["SoDH"];
                    }
                    return RedirectToAction("OrderIndexot");
                }
                //else
                //{
                //    return RedirectToAction("Province_PriceIndex");
                //}
            }
            return Redirect("/Admins/admins");
        }

        #endregion

        #region[Ajax Change Order]

        // AJAX: /Order/ChangeOrder
        [HttpPost]
        public ActionResult ChangeOrder(int id, string postcode)
        {
            var results = "";
            var order = db.Ords.Find(id);
            if (order != null)
            {
                if (postcode != null)
                {
                    order.PostCode = postcode;
                    results = "Mã bưu điện đã được cập nhật.";
                }
            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Json(results);
        }
        #endregion

    }
}
