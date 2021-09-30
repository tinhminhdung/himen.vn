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
using System.IO;


namespace MODEOUTLED.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Pages/
        //ModeoutleddbContext data = new ModeoutleddbContext();
           wwwEntities data = new wwwEntities();
        #region[Dang ki]
        public ActionResult dangki()
        {
            ViewBag.TinhTP = new SelectList(Tinhtp(), "value", "text");
            return View();
        }
        [HttpPost]
        public string Loadhuyen(string tinh)
        {
            string chuoi="";
            var cat = data.Provinces.Where(m =>m.Level.Substring(0,tinh.Length)==tinh && m.Level.Length> tinh.Length).ToList();
            List<DropDownList> list = new List<DropDownList>();
            if (cat.Count > 0)
            {
                chuoi+="<select id=\"Hu\" name=\"Hu\"><option value=\"\">Chọn quận huyện</option>";
                for (int i = 0; i < cat.Count; i++)
                {
                    chuoi += "<option value=\""+ cat[i].Level +"\">"+ cat[i].Name +"</option>";
                }
                chuoi += "</select>";
            }
            else
            {
                chuoi += "<select id=\"Hu\" name=\"Hu\"><option value=\"\">Chọn quận huyện</option>";
                chuoi += "</select>";
            }
            return chuoi;
        }

        [HttpPost]
        public string Checkmail(string mail)
        {
            string chuoi = "";
            if(mail !="")
            {
                var cat = data.Customers.Where(m => m.Email == mail).ToList();
                if (cat.Count > 0)
                {
                    chuoi = "Địa chỉ mail này đã tồn tại!";
                }
            }
            return chuoi;
        }
        [HttpPost]
        public string CheckCustomer(string checks, string Name, string Add, string TinhTP, string Tel)
        {
            string chuoi = "";
            if (checks == "on")
            {
                if (Name == "")
                {
                    chuoi += " - Nhập thông tin họ tên người nhận hàng! <br />";
                }
                if (Add == "")
                {
                    chuoi += " - Nhập địa chỉ người nhận hàng! <br />";
                }
                if (TinhTP == "")
                {
                    chuoi += " - Chọn Tỉnh/Thành phố nhận hàng! <br />";
                }
                if (Name == "")
                {
                    chuoi += " - Nhập điện thoại người nhận hàng! <br />";
                }
            }
            return chuoi;
        }

        [HttpPost]
        public string Capcha(string captcha)
        {
            string chuoi = "";
            if (captcha != "")
            {
                CaptchaProvider captchaPro = new CaptchaProvider();
                if (!captchaPro.IsValidCode(captcha))
                {
                    chuoi = "Mã àn toàn không chính xác!";
                }
            }
            return chuoi;
        }

        public static List<DropDownList> Tinhtp()
        {
             wwwEntities db = new wwwEntities();
            var cat = db.Provinces.Where(n => n.Level.Length == 5).ToList();
            List<DropDownList> list = new List<DropDownList>();
            if (cat.Count > 0)
            {
                for (int i = 0; i < cat.Count; i++)
                {
                    list.Add(new DropDownList { value = cat[i].Id.ToString(), text = cat[i].Name });
                }

            }
            return list;
        }

        public static List<DropDownList> HuyenEdit(int id)
        {
             wwwEntities db = new wwwEntities();
            var cat = db.Provinces.Where(n => n.Id == id).ToList();
            List<DropDownList> list = new List<DropDownList>();
            if (cat.Count > 0)
            {
                for (int i = 0; i < cat.Count; i++)
                {
                    list.Add(new DropDownList { value = cat[i].Level, text = cat[i].Name });
                }

            }
            return list;
        }
        public static List<DropDownList> Quanhuyen()
        {
            List<DropDownList> list = new List<DropDownList>();
            return list;
        }
        #endregion
        #region[Dang ki]
        [HttpPost]
        public ActionResult dangki(FormCollection collect, onsoft.Models.Customer cus, IEnumerable<HttpPostedFileBase> fileImg)
        {
            try
            {
                string Name = collect["Name"];
                string Password = collect["Pass"];
                string Email = collect["Email"];
                string Tel = collect["Phone"];
                string Address = collect["Address"];
                int Provice = int.Parse(collect["TinhTP"].ToString());
                string Avarta = "";
                double nongthon = 0;
                bool Status = true;
                DateTime SDate = DateTime.Now;
                string images = collect["fileImg"];
                foreach (var file in fileImg)
                {
                    if (file.ContentLength > 0)
                    {
                        //var b = (from k in db.ProImages select k.Id).Max();
                        var ab = Request.Files["fileImg"];
                        String FileExtn = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                        {
                            ViewBag.error = "Only jpg, gif and png files are allowed!";
                        }
                        else
                        {
                            var Filename = Path.GetFileName(file.FileName);
                            //String imgPath = String.Format("Uploads/{0}{1}", file.FileName, FileExtn);
                            //file.Save(String.Format("{0}{1}", Server.MapPath("~"), imgPath), Img.RawFormat);
                            var path = Path.Combine(Server.MapPath(Url.Content("/Uploads")), Filename);
                            file.SaveAs(path);
                            Avarta = "/Uploads/" + Filename;
                        }
                    }
                    var fd = file;
                }
                nongthon = (collect["cNongthon"] == "false") ? 0 : 1;
                data.sp_Customer_Insert(Name, Email, Password, Tel, Address, SDate, Status, "", Provice, 1, false, 1, Avarta, nongthon, 0, true);
                data.SaveChanges();
                return Redirect("/Pages/xacnhan");
            }
            catch {
                ViewBag.TinhTP = new SelectList(Tinhtp(), "value", "text");
                ////ViewBag.Huyen = new SelectList(Quanhuyen(), "value", "text");
                ViewBag.err = "Lỗi nhập dữ liệu đăng ký thành viên!";
                return View(); 
            }
        }
        #endregion
        #region[Hoan tat dang ki]
        public ActionResult hoantat()
        {
            return View();
        }
        #endregion
        #region[xac nhan dang ki]
        public ActionResult xacnhan()
        {
            return View();
        }
        #endregion
        #region[huy dang ky]
        public ActionResult huy()
        {
            string id = "";
            if (RouteData.Values["id"] != null)
            {
                id = RouteData.Values["id"].ToString();
            }
            var list = data.Customers.Where(m => m.Id == Convert.ToInt32(id)).FirstOrDefault();
            if (list != null)
            {
                data.Customers.Add(list);
                data.SaveChanges();
            }
            var pro = data.Products.Where(m => m.Index == true && m.Active == true).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PriceRetail == 0)
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            return View();
        }
        #endregion
        #region[Dang nhap]
        public ActionResult dangnhap()
        {
            Session["Email"] = null;
            var pro = data.Products.Where(m => m.Index == true && m.Active == true).OrderByDescending(m => m.Id).Take(3).ToList();
            string sptt = "";
            sptt += "<ul>";
            for (int i = 0; i < pro.Count; i++)
            {
                sptt += "<li>";
                sptt += "<h3><a href=\"/sanpham/chitiet/" + pro[i].Tag + "\">" + pro[i].Name + "</a></h3>";
                sptt += "<a href=\"/sanpham/chitiet/" + pro[i].Tag + "\"><img src=\"" + pro[i].Image + "\" /></a>";
                sptt += "<p>Model: <span>" + pro[i].Id + "</span></p>";
                if (pro[i].PriceRetail == 0)
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</p>";
                }
                else
                {
                    sptt += "<p>Giá: <span class=\"require\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</span></p>";
                    sptt += "<p class=\"txtout\">" + StringClass.Format_Price(pro[i].PriceRetail.ToString()) + " VNĐ</p>";
                }
                sptt += "</li>";
            }
            sptt += "</ul>";
            ViewBag.Spnb = sptt;
            return View();
        }
        #endregion
        #region[Dang nhap]
        [HttpPost]
        public ActionResult dangnhap(FormCollection collect)
        {
            var Email = collect["Email"];
            var Pass = collect["Pass"];
            var list = data.Customers.Where(m => m.Email == Email && m.Password == Pass).ToList();
            if (list.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(Email, false);
                Session["Email"] = list[0].Email;
                Session["Name"] = list[0].Name;
                Session["uId"] = list[0].Id.ToString();
                return Redirect("/");
            }
            else
            {
                ViewBag.Err = "Đăng nhập không thành công!";
                return View();
            }
        }
        #endregion
        #region[logout]
        public ActionResult logout()
        {
            Session["Email"] = null;
            Session["Name"] = null;
            Session["uId"] = null;
            
            return Redirect("/");            
        }
        #endregion
        #region[thong tin ca nhan]
        public ActionResult member()
        {
            if (Session["Email"] != null)
            {
                string email = Session["Email"].ToString();
                Session["colFunc"] = 1;
                var list = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                ViewBag.Name = list.Name;
                string chuoi = "";
                chuoi += "<h3>Thông tin tài khoản</h3>";
                chuoi += "<p><b>Họ và tên:</b> " + list.Name + "</p>";
                chuoi += "<p><b>Email:</b> " + list.Email + "</p>";
                //chuoi += "<p>Địa chỉ: " + list.Address + "</p>";
                chuoi += "<p><b>Điện thoại:</b> " + list.Tel + "</p>";
                int ihuyen = int.Parse(list.Provice.ToString());
                var huyenv = data.Provinces.Where(m => m.Id == ihuyen).ToList();
                if (huyenv.Count > 0)
                {
                    chuoi += "<p><b>Địa chỉ:</b> " + list.Address + " - Tỉnh/TP " + huyenv[0].Name + "</p>";
                }
                if (list.cNongthon == 1)
                {
                    chuoi += "<p><b>Loại hình bưu điện:</b> Vùng ngoại thành, nông thôn</p>";
                }
                else
                {
                    chuoi += "<p><b>Loại hình bưu điện:</b> Vùng thành phố, đô thị</p>";
                }
                chuoi += "<p><b>Điểm tích lũy:</b> " + list.Diem + " tương đương với tổng số tiền đã mua: <b>" + StringClass.Format_Price((list.Diem * 100000).ToString()) + "đ</b></p>";
                if (list.Si == true)
                {
                    chuoi += "<p><b>Được mua sản phẩm với:</b> Giá bán sỉ</p>";
                }

                chuoi += "<p><b>Avarta của bạn: <img src=\"" + list.Avarta + "\" width=\"150\" height=\"150\"/></b></p>";
                ViewBag.Info = chuoi;
                //string str = "";
                //int idc = list.Id;
                //var bill = data.Ords.Where(m => m.IdCus == idc).OrderByDescending(m => m.Id).Take(3).ToList();
                //for (int i = 0; i < bill.Count; i++)
                //{
                //    str += "<tr>";
                //    str += "<td>" + (i + 1) + "</td>";
                //    str += "<td>" + bill[i].Id + "</td>";
                //    str += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(bill[i].SDate.ToString()) + "</td>";
                //    str += "<td>" + list.Name + "</td>";
                //    str += "<td>" + StringClass.Format_Price(bill[i].Amount.ToString()) + " VNĐ</td>";
                //    str += "<td>" + StringClass.ShowStateBill(bill[i].SDate.ToString()) + "</td>";
                //    str += "<td></td>";
                //    str += "</tr>";
                //}
                //ViewBag.View = str;
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[hien thi cot chuc nang ben trai trong trang ca nhan cua khach hang]
        public ActionResult colFunctionCus()
        {
            return PartialView();
        }
        #endregion
        #region[Chinh sua thong tin ca nhan]
        public ActionResult edit_info()
        {
            Customer list = new Customer();
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 2;
                string vatinh = "";
                string vahuyen = "";
                string level = "";
                string email = Session["Email"].ToString();
                list = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                if (list != null)
                {
                    int ihuyen = int.Parse(list.Provice.ToString());
                    //var huyenv = data.Provinces.Where(m => m.Id == ihuyen).ToList();
                    //if (huyenv.Count > 0)
                    //{
                    //    vahuyen = huyenv[0].Id.ToString();
                        //level = huyenv[0].Level;
                        //var tinhv = data.Provinces.Where(m => m.Level == level.Substring(0, level.Length - 5) && m.Level.Length < level.Length).ToList();
                        //if (tinhv.Count > 0)
                        //{
                        //    vatinh = tinhv[0].Level.ToString();
                        //}
                    //}
                    ViewBag.TinhTP = new SelectList(Tinhtp(), "value", "text", ihuyen);
                    //ViewBag.Huyen = new SelectList(HuyenEdit(ihuyen), "value", "text", level);
                }
                return View(list);
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Chinh sua thong tin ca nhan]
        [HttpPost]
        public ActionResult edit_info(FormCollection collect, IEnumerable<HttpPostedFileBase> fileImg)
        {
            if (Session["Email"] != null)
            {
                string email = Session["Email"].ToString();
                var list = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                int cid = list.Id;
                var edit = data.Customers.First(m => m.Id == cid);
                edit.Name = collect["Name"];
                edit.Tel = collect["Tel"];
                edit.Address = collect["Address"];
                string images = collect["fileImg"];
                if (fileImg != null)
                {
                    foreach (var file in fileImg)
                    {
                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                //var b = (from k in db.ProImages select k.Id).Max();
                                var ab = Request.Files["fileImg"];
                                String FileExtn = System.IO.Path.GetExtension(file.FileName).ToLower();
                                if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                                {
                                    ViewBag.error = "Only jpg, gif and png files are allowed!";
                                }
                                else
                                {
                                    var Filename = Path.GetFileName(file.FileName);
                                    //String imgPath = String.Format("Uploads/{0}{1}", file.FileName, FileExtn);
                                    //file.Save(String.Format("{0}{1}", Server.MapPath("~"), imgPath), Img.RawFormat);
                                    var path = Path.Combine(Server.MapPath(Url.Content("/Uploads")), Filename);
                                    file.SaveAs(path);
                                    edit.Avarta = "/Uploads/" + Filename;
                                }
                            }
                        }
                    }
                }
                edit.cNongthon = (collect["cNongthon"] == "false") ? 0 : 1;
                if (collect["TinhTP"] != null)
                {
                    string levelhuyen = collect["TinhTP"].ToString();
                    //var huyenv = data.Provinces.Where(m => m.Level == levelhuyen).ToList();
                    if (levelhuyen!= "")
                    {
                        edit.Provice = int.Parse(levelhuyen.ToString());
                    }
                }
                data.SaveChanges();
            }
            return RedirectToAction("member");
        }
        #endregion
        #region[Doi mat khau]
        public ActionResult doi_mat_khau()
        {
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 3;
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Doi mat khau]
        [HttpPost]
        public ActionResult doi_mat_khau(FormCollection collect)
        {
            if (Session["Email"] != null)
            {
                string email = Session["Email"].ToString();
                var list = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                if (list != null)
                {
                    if (list.Password == collect["Pass"])
                    {
                        list.Password = collect["NewPass"];
                        data.SaveChanges();
                        return RedirectToAction("member");
                    }
                    else
                    {
                        ViewBag.Err = "Mật khẩu chưa đúng";
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[Quen mat khau]
        public ActionResult forget_pass()
        {
            return View();
        }
        #endregion
        #region[Quen mat khau]
        [HttpPost]
        public ActionResult forget_pass(FormCollection collect)
        {
            string chuoi = "";
            var email = collect["Email"];
            if (email == "")
            {
                ViewBag.Err = "Bạn phải nhập email.";
            }
            else
            {
                var list = data.Customers.Where(m => m.Email == email && m.Password!=null).FirstOrDefault();
                if (list != null)
                {
                    var Pass = StringClass.RandomString(8);
                    list.Password = Pass;
                    data.SaveChanges();
                    chuoi += "<p>Bạn nhận được mail này vì bạn đã thực hiện chức năng quên mật khẩu trên Onsoft.vn</p>";
                    chuoi += "<p>Đây là thông tin đăng nhập mới của bạn:</p>";
                    chuoi += "<p>Email: " + list.Email + "</p>";
                    chuoi += "<p>Mật khẩu: " + Pass + "</p>";
                    chuoi += "<p>Cảm ơn bạn đã sử dụng dịch vụ của onsoft.vn</p>";
                    #region [Sendmail]
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    string mailto = list.Email;
                    var listconfig = data.Configs.ToList();
                    string pass = listconfig[0].Mail_Password;
                    string host = listconfig[0].Mail_Smtp;
                    int post = 0;
                    if (Convert.ToInt32(listconfig[0].Mail_Port) > 0)
                    {
                        post = Convert.ToInt32(listconfig[0].Mail_Port);
                    }
                    else
                    {
                        post = 587;
                    }
                    string cc = "tuanvd@onsoft.vn";
                    mailMessage.From = (new MailAddress(mailto, "no-reply", System.Text.Encoding.UTF8));
                    mailMessage.To.Add(list.Email);
                    mailMessage.Bcc.Add(cc);
                    mailMessage.Subject = "Thong tin dang nhap moi tai onsoft.vn";
                    mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    mailMessage.Body = chuoi;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                    System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential();
                    mailAuthentication.UserName = "voduytuan@gmail.com";
                    mailAuthentication.Password = "emkhongbietgica";
                    //System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("" + host + "", post);
                    System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    mailClient.EnableSsl = true;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = mailAuthentication;
                    try
                    {
                        mailClient.Send(mailMessage);
                        Session.Clear();
                        list = null;
                        Response.Redirect("/Home/Index");
                    }
                    catch
                    {
                        
                    }
                    #endregion
                }
                else
                {
                    ViewBag.Err = "Email này không tồn tại trong hệ thống.";
                }
            }
            return View();
        }
        #endregion
        #region[Quan ly don hang]
        public ActionResult quan_ly_don_hang()
        {
            if (Session["Email"] != null)
            {
                Session["colFunc"] = 4;
                string mail = Session["Email"].ToString();
                string chuoi = "";
                var list = data.Customers.Where(m => m.Email == mail).FirstOrDefault();
                int cid = list.Id;
                // Hóa đơn
                var bill = data.Ords.Where(m => m.IdCus == cid).OrderByDescending(m => m.Id).ToList();
                for (int i = 0; i < bill.Count; i++)
                {
                    int billId = bill[i].Id;
                    var orderDetail = data.v_OrderDetail_Product.Where(od => od.IdOr == billId).ToList();
                    float? totalPrice = (from o in bill where (o.Id == bill[i].Id) select o).Single().Amount;
                    if (orderDetail.Count > 0)
                    {
                        chuoi += "<div id=\"mn\" onclick=\"toggleXPMenu('Content-" + bill[i].Id + "');\">";
                        //chuoi += "<div id=\"mn1\">";
                        //chuoi += "<input type=\"checkbox\"></input>";
                        //chuoi += "</div>";
                        chuoi += "<div id=\"mn1\"><span id=\"sp\">Số ĐH :  <span style=\"color: #fff;\">" + bill[i].Id + "</span></span> </div>";
                        chuoi += "<div id=\"mn3\"><span id=\"sp\">Ngày đặt :  <span style=\"color:#fff;\">" + DateTimeClass.ConvertDateTime(bill[i].SDate.ToString()) + "</span></span> </div>";
                        chuoi += "<div id=\"mn3\"><span id=\"sp\">Người mua :  <span style=\"color:#fff;\">" + list.Name + "</span></span> </div>";
                        chuoi += "<div id=\"mn4\"><span id=\"sp\">Trạng thái : <span style=\"color:#fff;\">" + StringClass.ShowStateBill(bill[i].Status.ToString()) + "</span></span></div>";
                        chuoi += "</div>";
                        // Chi tiết hóa đơn

                        chuoi += "<div id=\"Content-" + bill[i].Id + "\">";
                        chuoi += "<table border=\"0\" style=\"width: 995px;\">";
                        chuoi += "<tr style=\"border-bottom: 1px solid #CDCDCD;\">";
                        chuoi += "<th style=\"text-align: left; font-size: 12px; color: #555555; border-right: 1px dashed rgba(0,0,0,0.075); border-left: 1px solid rgba(0,0,0,0.075)\" colspan=\"2\">Sản phẩm</th>";
                        chuoi += "<th style=\"text-align: center; font-size: 12px; color: #555555; border-right: 1px dashed rgba(0,0,0,0.075)\">Đơn giá</th>";
                        chuoi += " <th style=\"text-align: center; font-size: 12px; color: #555555; border-right: 1px dashed rgba(0,0,0,0.075)\">Số lượng</th>";
                        chuoi += " <th style=\"text-align: center; font-size: 12px; color: #555555; border-right: 1px dashed rgba(0,0,0,0.075)\">Thành tiền</th>";
                        chuoi += " <th style=\"text-align: center; font-size: 12px; color: #555555; border-right: 1px dashed rgba(0,0,0,0.075)\">Cước VC</th>";
                        chuoi += "<th style=\"text-align: center; width: 120px; border-right: 1px dashed rgba(0,0,0,0.075)\"><span style=\"color: #0066FF;\">Tổng tiền</span></th>";
                        chuoi += "<th style=\"text-align: center; width: 120px; border-right: 1px solid rgba(0,0,0,0.075)\">Mã bưu</th>";
                        chuoi += "</tr>";

                        for (int j = 0; j < orderDetail.Count; j++)
                        {
                            chuoi += "<tr style=\"border-bottom: 1px solid #CDCDCD\">";
                            chuoi += "<td style=\"width: 80px; border-right: 1px dashed rgba(0,0,0,0.075); border-left: 1px solid rgba(0,0,0,0.075)\"><img style=\"width: 70px;\" src=\"" + orderDetail[j].Image + "\"></img></td>";
                            chuoi += "<td style=\"color: #0066FF; font-size: 13px; border-right: 1px dashed rgba(0,0,0,0.075)\">" + orderDetail[j].Name + "<br /><span style=\"color:#555555\">Màu sắc: Xanh</span><br /><span style=\"color:#555555\">Kích thước: XXL</span></td>";
                            chuoi += "<td style=\"width: 100px; font-size: 12px; font-weight: bolder; text-align: center; border-right: 1px dashed rgba(0,0,0,0.075)\">" + os.os.Format_Price(orderDetail[j].Price.ToString()) + " (vnđ)</td>";
                            chuoi += "<td style=\"width: 70px; font-size: 12px; font-weight: bolder; text-align: center; border-right: 1px dashed rgba(0,0,0,0.075)\">" + orderDetail[j].Number + "</td>";
                            chuoi += "<td style=\"width: 70px; font-size: 12px; color: red; text-align: center; border-right: 1px dashed rgba(0,0,0,0.075)\">" + os.os.Format_Price((orderDetail[j].Price * orderDetail[j].Number).ToString()) + " (vnđ)</td>";
                            chuoi += "<td style=\"width: 250px; font-size: 12px; color: red; text-align: left; border-right: 1px dashed rgba(0,0,0,0.075)\"> - Cước bưu điện: " + os.os.Format_Price(bill[i].PriceVC.ToString()) + " (vnđ)<br /> - Cước 20% vùng ngoại thành nếu có: " + os.os.Format_Price(bill[i].PriceNT.ToString()) + " (vnđ)</td>";
                            chuoi += "<td style=\"width: 80px; font-size: 12px; color: red; text-align: center; border-right: 1px dashed rgba(0,0,0,0.075)\"><span style=\"font-size: 12px; font-weight: bolder;\">" + os.os.Format_Price(totalPrice.ToString()) + " (vnđ)</span></td>";
                            chuoi += "<td style=\"width: 150px; font-size: 12px; font-weight: bolder; text-align: center; border-right: 1px solid rgba(0,0,0,0.075)\"><span style=\"font-size: 13px; font-weight: bolder;\">" + bill[i].PostCode + "</span><p style=\"font-size: 12px; font-weight: normal; \">Khách hàng vui lòng tra cứu thông tin hàng hóa tại địa chỉ: <a hreft=\"http://vnpost.vn\" target=\"_blank\">vnpost.vn</a></p></td>";
                            chuoi += " </tr>";
                        }

                        chuoi += "</table>";
                        chuoi += "<div id=\"mnbtom\">";
                        chuoi += "<div class=\"head\">Thông tin nhận hàng:</div>";
                        if (bill[i].Name != null && bill[i].Name != "")
                        {
                            string NameProvince = "";
                            string times = "";
                            int tinhid = int.Parse(bill[i].ProviceId.ToString());
                            var provi = data.Provinces.Where(m => m.Id == tinhid).FirstOrDefault();
                            if (provi != null)
                            {
                                NameProvince = provi.Name;
                                if (bill[i].TypePay == "1")
                                {
                                    times = provi.Time;
                                }
                                else if (bill[i].TypePay == "2")
                                {
                                    times = provi.Time1;
                                }
                                else
                                {
                                    times = provi.Time2;
                                }
                            }
                            chuoi += "<div class=\"item\" style=\"margin-left: 135px;\">Họ tên: <span>" + bill[i].Name + "</span></div>";
                            chuoi += "<div class=\"item\">Điện thoại: <span>" + bill[i].Tel + "</span></div>";
                            if (bill[i].TypePay == "1")
                            {
                                chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát nhanh - </span><b>" + times + "</b></div>";
                            }
                            else if (bill[i].TypePay == "2")
                            {
                                chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát thường - </span><b>" + times + "</b></div>";
                            }
                            else
                            {
                                chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Ô tô - </span><b>" + times + "</b></div>";
                            }
                            chuoi += "<div class=\"item500\">Địa chỉ: <span>" + bill[i].Address + " - " + NameProvince + "</span></div>";
                        }
                        else
                        {
                            string times = "";
                            string NameProvince = "";
                            int tinhid = int.Parse(list.Provice.ToString());
                            var provi = data.Provinces.Where(m => m.Id == tinhid).FirstOrDefault();
                            if (provi != null)
                            {
                                NameProvince = provi.Name;
                                if (bill[i].TypePay == "1")
                                {
                                    times = provi.Time;
                                }
                                else if (bill[i].TypePay == "2")
                                {
                                    times = provi.Time1;
                                }
                                else
                                {
                                    times = provi.Time2;
                                }
                            }
                            chuoi += "<div class=\"item\" style=\"margin-left: 135px;\">Họ tên: <span>" + list.Name + "</span></div>";
                            chuoi += "<div class=\"item\">Điện thoại: <span>" + list.Tel + "</span></div>";
                            if (bill[i].TypePay == "1")
                            {
                                chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát nhanh - </span><b>" + times + "</b></div>";
                            }
                            else if (bill[i].TypePay == "2")
                            {
                                chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Chuyển phát thường - </span><b>" + times + "</b></div>";
                            }
                            else
                            {
                                chuoi += "<div class=\"item400\">Gửi hàng bằng:<span> Ô tô - </span><b>" + times + "</b></div>";
                            }
                            chuoi += "<div class=\"item500\">Địa chỉ: <span>" + list.Address + " - " + NameProvince + "</span></div>";
                        }
                        chuoi += "</div>";
                        chuoi += "</div>";
                    }
                }
                ViewBag.View = chuoi;
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap");
            }
        }
        #endregion
        #region[timkiem]
        string ddlvalue = "";
        [HttpPost]
        public ActionResult timkiem(FormCollection collect)
        {
            string ddl = collect["sx"];
            var seachWord = Session["Timkiem"].ToString().Replace(" ", "+");
            string url = "/Pages/timkiem?searchword=" + seachWord + "&order=" + ddl + "";
            return Redirect(url);
        }
        #endregion
        #region[Xoa the html khoi text]
        private string RemoveHTMLTag(string HTML)
        {
            // Xóa các thẻ html
            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex("<[^>]*>");

            return objRegEx.Replace(HTML, "");
        }
        #endregion
        #region[Cat chuoi text de hien thi]
        protected string FormatContentNews(string value, int count)
        {
            string _value = value;
            if (_value.Length >= count)
            {
                string ValueCut = _value.Substring(0, count - 3);
                string[] valuearray = ValueCut.Split(' ');
                string valuereturn = "";
                for (int i = 0; i < valuearray.Length - 1; i++)
                {
                    valuereturn = valuereturn + " " + valuearray[i];
                }
                return valuereturn;
            }
            else
            {
                return _value;
            }
        }
        #endregion
        #region[Them dau Tieng Viet doi voi tung chu]
        public string ReplaceCharSet(string input)
        {
            string charSet = input.ToLower();
            if (charSet == "a")
                return "[aàảãáạăằẳẵắặâầẩẫấậ]";
            else if (charSet == "e")
                return "[eèẻẽéẹêềểễếệ]";
            else if (charSet == "i")
                return "[iìỉĩíị]";
            else if (charSet == "o")
                return "[oòỏõóọôồổỗốộơờởỡớợ]";
            else if (charSet == "u")
                return "[uùủũúụưừửữứự]";
            else if (charSet == "y")
                return "[yỳỷỹýỵ]";
            else if (charSet == "d")
                return "[dđ]";
            return charSet;
        }
        #endregion
        #region[Loai bo dau Tieng Viet doi voi tung chu - chuyen doi chu tu Unicode sang ASCII]
        public string RemoveUnicode(string inputText, bool sqlSearch)
        {
            string stFormD = inputText.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string str = "";
            for (int i = 0; i <= stFormD.Length - 1; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc == UnicodeCategory.NonSpacingMark == false)
                {
                    if (stFormD[i] == 'đ')
                        str = "d";
                    else if (stFormD[i] == 'Đ')
                        str = "D";
                    else
                        str = stFormD[i].ToString();
                    //Neu sqlSearch = true thi sau khi khu dau xong o tren se tien hanh them dau, con neu sqlSearch = false thi nguoc lai
                    if (sqlSearch) str = ReplaceCharSet(str);
                    sb.Append(str);
                }
            }
            return sb.ToString().ToLower();
        }
        #endregion
        #region[view chon lua dang nhap hoac mua hang luon k can tai khoan dk]
        public ActionResult notLogon()
        {
            return View();
        }
        #endregion
        #region[Thanh toan]
        public ActionResult order_pay()
        {
            string chuoi = "";
            string chuoiToTal = "";
            if (Session["ShoppingCart"] != null)
            {
                var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                chuoi += "<h3>Thông tin giỏ hàng</h3>";
                foreach (var item in cart.CartItems)
                {
                    chuoi += "<div class=\"infoPro\">";
                    chuoi += "<img src=\"" + item.productImage + "\" />";
                    chuoi += "<h4>" + item.productName + "</h4>";
                    chuoi += "<p>Giá bán: <span>" + StringClass.Format_Price(item.price) + " VNĐ</span></p>";
                    chuoi += "<p>Số lượng: <span>" + item.count + "</span></p>";
                    chuoi += "</div>";
                    chuoi += "<div class=\"clear\"></div>";
                }

                chuoiToTal += "<div class=\"totalCart\">";
                chuoiToTal += "<p class=\"fl\">Giá trị đơn hàng</p>";
                chuoiToTal += "<span class=\"fr\">" + StringClass.Format_Price(cart.CartTotal.ToString()) + " VNĐ</span>";
                chuoiToTal += "<div class=\"clear\"></div>";
                chuoiToTal += "<p class=\"fl\">Phí vận chuyển</p>";
                chuoiToTal += "<span class=\"fr\">0 VNĐ</span>";
                chuoiToTal += "<div class=\"clear\"></div>";
                chuoiToTal += "<p class=\"fl\">Tổng cộng</p>";
                chuoiToTal += "<span class=\"fr\">" + StringClass.Format_Price(cart.CartTotal.ToString()) + " VNĐ</span>";
                chuoiToTal += "<div class=\"clear\"></div>";
                chuoiToTal += "<input type=\"submit\" value=\"Xác nhận thanh toán\" />";
                chuoiToTal += "</div>";
                ViewBag.ViewPro = chuoi;
                ViewBag.ViewTotal = chuoiToTal;
                Customer mem = new Customer();
                if (Session["Email"] != null)
                {
                    string email = Session["Email"].ToString();
                    mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                    ViewBag.Cus = 1;
                }
                return View(mem);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        #endregion
        #region[Thanh toan]
        [HttpPost]
        public ActionResult order_pay(FormCollection collect)
        {
            string chuoi = "";
            var Name = collect["vcusname"];
            var Email = collect["vemail"];
            var Phone = collect["vphone"];
            var Address = collect["vaddress"];
            var Note = collect["Note"];
            var radioBtn = collect["rdbPay"];
            if (Name == "" || Email == "" || Phone == "" || Address == "")
            {
                ViewBag.Err = "<div class=\"err\">Bạn phải điền đầy đủ thông tin</div>";
                return Redirect("/Pages/order_pay");
            }
            else
            {
                Customer listcus = new Customer();
                if (Session["Email"] == null)
                {
                    Customer cus = new Customer();
                    cus.Name = Name;
                    cus.Email = Email;
                    cus.Tel = Phone;
                    cus.Address = Address;
                    data.Customers.Add(cus);
                    data.SaveChanges();
                    listcus = data.Customers.OrderByDescending(m => m.Id).FirstOrDefault();
                }
                else
                {
                    string email = Session["Email"].ToString();
                    listcus = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                }
                ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                onsoft.Models.Ord ord = new onsoft.Models.Ord();
                ord.IdCus = listcus.Id;
                ord.Amount = shoppCart.CartTotal;
                ord.SDate = DateTime.Now;
                ord.TypePay = radioBtn;
                ord.Status = "0";
                data.Ords.Add(ord);
                data.SaveChanges();
                #region[Noi dung mail gui]
                chuoi += "<div style=\" margin:20px auto; width:800px; background:#6badf6; padding-bottom:10px\">";
                chuoi += "<div style=\"border-bottom:1px solid #dfdfdf; margin:20px; line-height:30px; font-size:16px;\">";
                chuoi += "<a href=\"#\" style=\"float:left; font-size:20px; margin-top:20px\">dongho.com</a>";
                chuoi += "<p style=\"float:right; color:#525461; font-weight:bold\">Ngày "+ DateTime.Now.Day +" Tháng "+ DateTime.Now.Month +" Năm "+ DateTime.Now.Year +"</p>";
                chuoi += "<div style=\"clear:both;\"></div>";
                chuoi += "</div>";
                chuoi += "<div style=\"background:#fff; border-radius:15px 0 15px 0; padding:10px; margin:20px; overflow:hidden\">";
                chuoi += "<h2 style=\"padding-bottom:20px; border-bottom:1px solid #e8e8e8\">Cám ơn bạn đã mua sản phẩm của chúng tôi!</h2>";
                chuoi += "<h3 style=\"text-align:center; margin:10px 0; font-size:16px;\">Thông tin đơn hàng của bạn</h3>";
                chuoi += "<h4 style=\"margin:10px 0; font-size:14px;\">Thông tin người mua</h4>";
                chuoi += "<p style=\"line-height:20px;\"><span style=\"padding-right:10px; font-weight:bold;\">Họ và tên:</span> "+ listcus.Name +"</p>";
                chuoi += "<p style=\"line-height:20px\"><span style=\"padding-right:36px; font-weight:bold;\">SĐT:</span> "+ listcus.Tel +"</p>";
                chuoi += "<p style=\"line-height:20px\"><span style=\"padding-right:20px; font-weight:bold;\">Địa chỉ:</span> "+ listcus.Address +"</p>";
                chuoi += "<table width=\"100%\" style=\"border-collapse:collapse; margin:10px 0; border:1px solid #efefef; font-size:14px;\" border=\"1\">";
                chuoi += "<tr style=\" background:#91d9d9; border: 1px solid #efefef; height:30px;\">";
                chuoi += "<th style=\"width:40px; border:1px solid #efefef\">STT</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; text-align:left; padding-left:5px;\">Tên sản phẩm</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; width:80px;\">Số lượng</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; width:130px;\">Đơn giá</th>";
                chuoi += "<th style=\"boder:1px solid #91d9d9; width:130px;\">Thành tiền</th>";
                chuoi += "</tr>";
                var listbillcus = data.Ords.OrderByDescending(m => m.Id).FirstOrDefault();
                int i = 1;
                foreach (var item in shoppCart.CartItems)
                {
                    OrderDetail billdetail = new OrderDetail();
                    billdetail.IdOr = listbillcus.Id;
                    billdetail.IdPro = item.productId;
                    billdetail.Number = item.count;
                    billdetail.Price = double.Parse(item.price);
                    billdetail.Total = double.Parse(item.total.ToString());
                    data.OrderDetails.Add(billdetail);
                    data.SaveChanges();
                    var sp = data.Products.Where(m => m.Id == item.productId).FirstOrDefault();
                    sp.Count += item.count;
                    data.SaveChanges();
                    chuoi += "<tr style=\"boder:1px solid #efefef; height:40px;\">";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:30px; text-align:center\">"+ i +"</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; text-align:left; padding-left:5px;\">" + item.productName + "</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:80px; text-align:center\">"+ item.count +"</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:130px; text-align:center\">"+ StringClass.Format_Price(item.price) +" VNĐ</td>";
                    chuoi += "<td style=\"boder:1px solid #efefef; width:130px; text-align:center\">"+ StringClass.Format_Price(item.total.ToString()) +" VNĐ</td>";
                    chuoi += "</tr>";
                    i++;
                }
                chuoi += "<tr style=\"boder:1px solid #efefef; height:30px; background:#f2f5ee\">";
                chuoi += "<td colspan=\"5\" style=\"text-align:right; padding-right:18px; boder:1px solid #efefef\">"+ StringClass.Format_Price(shoppCart.CartTotal.ToString()) +" VNĐ</td>";
                chuoi += "</tr>";
                chuoi += "</table>";
                chuoi += "<hr style=\"color:#dfdfdf; margin:10px 0;\"/>";
                chuoi += "<p style=\"width:50%; float:left; margin:10px 0; color:#333333; font-size:15px; line-height:22px;\"><a href=\"\">dongho.com</a> hy vọng luôn mang lại nhiều niềm vui cho các bạn. Và lúc nào cũng mong đợi thông tin phản hồi từ các bạn. Hãy gọi cho chúng tôi qua</p>";
                chuoi += "<div style=\" float:right; width:244px; background:#ff6801; border-radius:10px; padding:5px; text-align:center;\">";
                chuoi += "<p style=\"color:#525461; font-size:14px; padding:5px 0; margin:0;\">Để biết thêm thông tin vui lòng gọi</p>";
                chuoi += "<h2 style=\"color:#f5f5f5; padding:5px 0; margin:0; text-shadow:1px 1px 3px #5c625e\">098.765.4321</h2>";
                chuoi += "</div>";
                chuoi += "<div style=\"clear:both;\"><p>   </p></div>";
                chuoi += "</div>";
                chuoi += "<div style=\"clear:both;\"><p>   </p></div>";
                chuoi += "</div>";
                #endregion
                #region [Sendmail]
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                string mailto = listcus.Email;
                var listconfig = data.Configs.ToList();
                string pass = listconfig[0].Mail_Password;
                string host = listconfig[0].Mail_Smtp;
                int post = 0;
                if (Convert.ToInt32(listconfig[0].Mail_Port) > 0)
                {
                    post = Convert.ToInt32(listconfig[0].Mail_Port);
                }
                else
                {
                    post = 587;
                }
                mailMessage.From = (new MailAddress(mailto, "no-reply", System.Text.Encoding.UTF8));
                mailMessage.To.Add(listcus.Email);
                mailMessage.Bcc.Add(listconfig[0].Mail_Info);
                mailMessage.Subject = "Thong tin don hang tai DongHo";
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.Body = chuoi;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                System.Net.NetworkCredential mailAuthentication = new System.Net.NetworkCredential();
                if (listconfig[0].Mail_Noreply == null || listconfig[0].Mail_Noreply == "" || listconfig[0].Mail_Password == null || listconfig[0].Mail_Password == "")
                {
                    mailAuthentication.UserName = "info@onsoft";
                    mailAuthentication.Password = "emkhongbietgica";
                }
                else
                {
                    mailAuthentication.UserName = listconfig[0].Mail_Noreply;
                    mailAuthentication.Password = listconfig[0].Mail_Password;
                }
                System.Net.Mail.SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                mailClient.EnableSsl = true;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = mailAuthentication;
                try
                {
                    mailClient.Send(mailMessage);
                    Session.Clear();
                    listcus = null;
                    return RedirectToAction("order_success");
                }
                catch (Exception ex)
                {
                    return Redirect("/Home/Index");
                    //Raovat.Models.StringClass.Show("Đã có lỗi xảy ra trong quá trình gửi mail");
                }
                #endregion
            }
        }
        #endregion
        #region[dat hang thanh cong]
        public ActionResult order_success()
        {
            return View();
        }
        #endregion
        #region[Buy]
        public ActionResult Buy()
        {
            Session["vc"] = "vc1";
            ViewBag.TinhTP = new SelectList(Tinhtp(), "value", "text");
            string sp = "";
            string chuoiToTal = "";
            string vanchuyen="";
            int w = 0;
            int ship = 0;
            string times = "";
            string namepro = "";
            string namehuyen = "";
            string smem = "";
            string add = "";
            string nongthon = "0";
            if (Session["ShoppingCart"] != null)
            {
                Customer mem = new Customer();
                Province provi = new Province();
                if (Session["Email"] != null)
                {
                    int provid = 0;
                    string email = Session["Email"].ToString();
                    mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                    if (mem != null)
                    {
                        if (mem != null) { nongthon = mem.cNongthon.ToString(); }
                        add = mem.Address;
                        provid = int.Parse(mem.Provice.ToString());
                        provi = data.Provinces.Where(m => m.Id == provid).FirstOrDefault();
                        if (provi != null)
                        {
                            namehuyen = provi.Name;
                            times = provi.Time;
                        }
                        smem += "<p>Họ và tên: " + mem.Name + "</p>";
                        smem += "<p>Địa chỉ: " + mem.Address + " - Tỉnh/Thành phố " + namehuyen + "</p>";
                        if (mem.cNongthon == 1)
                        {
                            smem += "<p style=\"font-weight: normal; color: red\">Loại hình bưu điện: Vùng ngoại thành nông thôn (chịu thêm 20% của cước vận chuyển)</p>";
                        }
                        else
                        {
                            smem += "<p style=\"font-weight: normal; color: #3261fc\">Loại hình bưu điện: Vùng nội thành đô thị (không phải chịu thêm 20% cước vận chuyển)</p>";
                        }
                        smem += "<p>Điện thoại:" + mem.Tel + "</p>";
                        ViewBag.mems = smem;

                        var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                        #region[In ra vùng THÔNG TIN ĐƠN HÀNG]
                        sp += "<div id=\"opc-review\" class=\"box-info block-total-info section\" ><span class=\"ic-minus done-minus\" onclick=\"toggleXPMenu('Content-oreder');\">&nbsp;</span><div class=\"title step-title ttl-box\"> <span></span><h2>Thông tin đơn hàng</h2></div><div class=\"cls\"></div><div id =\"Content-oreder\"><div id=\"checkout-step-review\" class=\"step a-item\" ><div class=\"order-review info_order\" ><div class=\"product_in_order\">";
                        sp += "<div  class=\"title\"><div class=\"name_shop\">Sản phẩm</div><div class=\"sl\">Số lượng</div><div class=\"gt\">Giá thành</div><div class=\"tt\">Tổng tiền</div></div>";
                        foreach (var item in cart.CartItems)
                        {
                            sp += "<div class=\"product_box\">";
                            sp += "<div class=\"img_product\">";
                            sp += "<div class=\"img\">";
                            sp += "<img src=\"" + item.productImage + "\" width=\"60\" height=\"60\" alt=\"" + item.productName + "\" >";
                            sp += "</div>";
                            sp += "<div class=\"name_attr\">";
                            sp += "<div class=\"name\">" + item.productName + "</div>";
                            int cid = item.idcolor;
                            int sid = item.idsize;
                            var col = data.Colors.Where(m => m.Id == cid).FirstOrDefault();
                            var siz = data.Sizes.Where(m => m.Id == sid).FirstOrDefault();
                            w = (w + item.proweight);
                            if (col != null)
                            {
                                sp += "<div class=\"attr\"><p><label>Màu sắc: </label>" + col.Name + "</p></div>";
                            }
                            else
                            {
                                sp += "<div class=\"attr\"><p><label>Màu sắc: </label>Điền vào ghi chú</p></div>";
                            }
                            if (siz != null)
                            {
                                sp += "<div class=\"attr\"><p><label>Size: </label>" + siz.Name + "</p></div>";
                            }
                            else
                            {
                                sp += "<div class=\"attr\"><p><label>Size: </label>Free size</p></div>";
                            }
                            col = null;
                            siz = null;
                            sp += "</div>";
                            sp += "</div>";
                            sp += "<div class=\"sl\">" + item.count + "</div>";
                            sp += "<div class=\"gt\"><span class=\"price\">" + StringClass.Format_Price(item.price) + " VNĐ</span></div>";
                            sp += "<div class=\"tt\"><span class=\"price\">" + (int.Parse(item.price) * item.count) + " VNĐ</span></div>";
                            sp += "</div>";
                        }
                        #region[Lấy giá vận chuyển]
                        var Priceprovi = data.Province_Price.Where(m => m.ProvinceId == provid && m.From <= w && m.To>= w).FirstOrDefault();
                        if (Priceprovi!=null)
                        {
                            ship = int.Parse(Priceprovi.PriceN.ToString()); 
                        }
                        #endregion
                        sp += "<div  id=\"checkout-review-load\"><div class=\"shippingfee_total\"> ";
                        sp += "<div class=\"shippingfee\"><label>Vận chuyển: </label><div class=\"choice_supplier\"><div class=\"price\">  ";
                        sp += "<div class='shipto'>Giao hàng đến <strong>" + add + " - " + namehuyen + " </strong> với phí <strong>" + StringClass.Format_Price(ship.ToString()) + " VNĐ</strong> <br>";
                        if (mem.cNongthon == 1)
                        {
                            sp += "Cộng thêm 20% phí thuộc ngoại thành, nông thôn của Bưu điện: <strong>" + StringClass.Format_Price(((ship * 20) / 100).ToString()) + " VNĐ</strong> <br>";
                        }
                        sp += "<span class=\"txt-transport\">Giao hàng từ <strong>" + times + "</strong></span></div>";
                        sp += "</div></div></div>";

                        sp += "<div class=\"block_pay\"><ul class=\"cash\">";
                        sp += "<li>";
                        sp += "<span class=\"fl\">Tổng tiền:</span>";
                        sp += "<span class=\"fr\"><strong>" + StringClass.Format_Price(cart.CartTotal.ToString()) + "</strong>&nbsp;VNĐ</span>";
                        sp += "</li>";
                        sp += "<li>";
                        sp += "<span class=\"fl\">";
                        sp += "Phí vận chuyển (PVC):</span>";
                        sp += "<span class=\"fr\">" + StringClass.Format_Price(ship.ToString()) + "&nbsp;VNĐ</span>";
                        sp += "</li>";
                        if (mem.cNongthon == 1)
                        {
                            sp += "<li>";
                            sp += "<span class=\"fl\">";
                            sp += "20% (PVC) ngoài thành, nông thôn:</span>";
                            sp += "<span class=\"fr\">" + StringClass.Format_Price(((ship * 20) / 100).ToString()) + "&nbsp;VNĐ</span>";
                            sp += "</li>";
                            sp += "<li>";
                            sp += "<span class=\"fl red basegrandtotal\"><strong>Tổng thành tiền:</strong></span>";
                            sp += "<span class=\"fr red basegrandtotal\"><strong>" + StringClass.Format_Price((cart.CartTotal + ship + ((ship * 20) / 100)).ToString()) + "&nbsp;VNĐ</strong></span>";
                            sp += "</li>";
                        }
                        else
                        {
                            sp += "<li>";
                            sp += "<span class=\"fl red basegrandtotal\"><strong>Tổng thành tiền:</strong></span>";
                            sp += "<span class=\"fr red basegrandtotal\"><strong>" + StringClass.Format_Price((cart.CartTotal + ship).ToString()) + "&nbsp;VNĐ</strong></span>";
                            sp += "</li>";
                        }
                        sp += "<li class=\"bt-payment\"><div class=\"buttons-set\">";
                        //sp += "<input type=\"submit\" name=\"btn_save_order\" title=\"Thanh toán\" class=\"button btn-checkout\" value=\"Thanh toán\" id=\"btn_save_order\"/>";
                        sp += "</div></li></ul></div>";
                        sp += "</div></div></div></div></div></div></div>";
                        ViewBag.ViewPro = sp;
                        #endregion
                    }
                }
                return View();
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        [HttpPost]
        public string BuyCart(string vc1, string vc2, string vc3)
        {
            string sp = "";
            int w = 0;
            int ship = 0;
            string times = "";
            string namepro = "";
            string Add = "";
            if (Session["ShoppingCart"] != null)
            {
                Customer mem = new Customer();
                Province provi = new Province();
                if (Session["Email"] != null)
                {
                    int provid = 0;
                    string email = Session["Email"].ToString();
                    mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                    if (mem != null)
                    {
                        Add = mem.Address;
                        provid = int.Parse(mem.Provice.ToString());
                        provi = data.Provinces.Where(m => m.Id == provid).FirstOrDefault();
                        if (provi != null)
                        {
                            namepro = provi.Name;
                            if (vc1 != null)
                            {
                                times = provi.Time;
                                Session["vc"] = vc1;
                            }
                            else if (vc2 != null)
                            {
                                times = provi.Time1;
                                Session["vc"] = vc2;
                            }
                            else if (vc3 != null)
                            {
                                times = provi.Time2;
                                Session["vc"] = vc3;
                            }
                            else
                            {
                                times = provi.Time;
                                Session["vc"] = vc1;
                            }
                        }
                        var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                        #region[In ra vùng THÔNG TIN ĐƠN HÀNG]
                        sp += "<div id=\"opc-review\" class=\"box-info block-total-info section\" ><span class=\"ic-minus done-minus\" onclick=\"toggleXPMenu('Content-oreder');\">&nbsp;</span><div class=\"title step-title ttl-box\"> <span></span><h2>Thông tin đơn hàng</h2></div><div class=\"cls\"></div><div id =\"Content-oreder\"><div id=\"checkout-step-review\" class=\"step a-item\" ><div class=\"order-review info_order\" ><div class=\"product_in_order\">";
                        sp += "<div  class=\"title\"><div class=\"name_shop\">Sản phẩm</div><div class=\"sl\">Số lượng</div><div class=\"gt\">Giá thành</div><div class=\"tt\">Tổng tiền</div></div>";
                        foreach (var item in cart.CartItems)
                        {
                            sp += "<div class=\"product_box\">";
                            sp += "<div class=\"img_product\">";
                            sp += "<div class=\"img\">";
                            sp += "<img src=\"" + item.productImage + "\" width=\"60\" height=\"60\" alt=\"" + item.productName + "\" >";
                            sp += "</div>";
                            sp += "<div class=\"name_attr\">";
                            sp += "<div class=\"name\">" + item.productName + "</div>";
                            int cid = item.idcolor;
                            int sid = item.idsize;
                            var col = data.Colors.Where(m => m.Id == cid).FirstOrDefault();
                            var siz = data.Sizes.Where(m => m.Id == sid).FirstOrDefault();
                            w = (w + item.proweight);
                            if (col != null)
                            {
                                sp += "<div class=\"attr\"><p><label>Màu sắc: </label>" + col.Name + "</p></div>";
                            }
                            else
                            {
                                sp += "<div class=\"attr\"><p><label>Màu sắc: </label>Điền vào ghi chú</p></div>";
                            }
                            if (siz != null)
                            {
                                sp += "<div class=\"attr\"><p><label>Size: </label>" + siz.Name + "</p></div>";
                            }
                            else
                            {
                                sp += "<div class=\"attr\"><p><label>Size: </label>Free size</p></div>";
                            }
                            col = null;
                            siz = null;
                            sp += "</div>";
                            sp += "</div>";
                            sp += "<div class=\"sl\">" + item.count + "</div>";
                            sp += "<div class=\"gt\"><span class=\"price\">" + StringClass.Format_Price(item.price) + " VNĐ</span></div>";
                            sp += "<div class=\"tt\"><span class=\"price\">" + (int.Parse(item.price) * item.count) + " VNĐ</span></div>";
                            sp += "</div>";
                        }
                        #region[Lấy giá vận chuyển]
                        var Priceprovi = data.Province_Price.Where(m => m.ProvinceId == provid && m.From <= w && m.To >= w).FirstOrDefault();
                        if (Priceprovi != null)
                        {
                            if (vc1 != null)
                            {
                                ship = int.Parse(Priceprovi.PriceN.ToString()); 
                            }
                            else if (vc2 != null)
                            {
                                ship = int.Parse(Priceprovi.PriceC.ToString());
                            }
                            else if (vc3 != null)
                            {
                                ship = int.Parse(Priceprovi.PriceO.ToString());
                            }
                            else
                            {
                                ship = int.Parse(Priceprovi.PriceN.ToString()); 
                            }
                        }
                        #endregion
                        sp += "<div  id=\"checkout-review-load\"><div class=\"shippingfee_total\"> ";
                        sp += "<div class=\"shippingfee\"><label>Vận chuyển: </label><div class=\"choice_supplier\"><div class=\"price\">  ";
                        sp += "<div class='shipto'>Giao hàng đến <strong> " + Add + " - " + namepro + " </strong> với phí <strong>" + StringClass.Format_Price(ship.ToString()) + " VNĐ</strong> <br>";
                        if (mem.cNongthon == 1)
                        {
                            sp += "Cộng thêm <strong> 20% </strong>phí thuộc ngoại thành, nông thôn của Bưu điện: <strong>" + StringClass.Format_Price(((ship * 20) / 100).ToString()) + " VNĐ</strong> <br>";
                        }
                        sp += "<span class=\"txt-transport\">Giao hàng từ <strong>" + times + "</strong></span></div>";
                        sp += "</div></div></div>";

                        sp += "<div class=\"block_pay\"><ul class=\"cash\">";
                        sp += "<li>";
                        sp += "<span class=\"fl\">Tổng tiền:</span>";
                        sp += "<span class=\"fr\"><strong>" + StringClass.Format_Price(cart.CartTotal.ToString()) + "</strong>&nbsp;VNĐ</span>";
                        sp += "</li>";
                        sp += "<li>";
                        sp += "<span class=\"fl\">";
                        sp += "Phí vận chuyển (PVC):</span>";
                        sp += "<span class=\"fr\">" + StringClass.Format_Price(ship.ToString()) + "&nbsp;VNĐ</span>";
                        sp += "</li>";
                        if (mem.cNongthon == 1)
                        {
                            sp += "<li>";
                            sp += "<span class=\"fl\">";
                            sp += "20% (PVC) ngoài thành, nông thôn:</span>";
                            sp += "<span class=\"fr\">" + StringClass.Format_Price(((ship * 20) / 100).ToString()) + "&nbsp;VNĐ</span>";
                            sp += "</li>";
                            sp += "<li>";
                            sp += "<span class=\"fl red basegrandtotal\"><strong>Tổng thành tiền:</strong></span>";
                            sp += "<span class=\"fr red basegrandtotal\"><strong>" + StringClass.Format_Price((cart.CartTotal + ship + ((ship * 20) / 100)).ToString()) + "&nbsp;VNĐ</strong></span>";
                            sp += "</li>";
                        }
                        else
                        {
                            sp += "<li>";
                            sp += "<span class=\"fl red basegrandtotal\"><strong>Tổng thành tiền:</strong></span>";
                            sp += "<span class=\"fr red basegrandtotal\"><strong>" + StringClass.Format_Price((cart.CartTotal + ship).ToString()) + "&nbsp;VNĐ</strong></span>";
                            sp += "</li>";
                        }
                        sp += "<li class=\"bt-payment\"><div class=\"buttons-set\">";
                        //sp += "<input type=\"submit\" name=\"btn_save_order\" title=\"Thanh toán\" class=\"button btn-checkout\" value=\"Thanh toán\" id=\"btn_save_order\"/>";
                        sp += "</div></li></ul></div>";
                        sp += "</div></div></div></div></div></div></div>";
                        #endregion
                        ViewBag.ViewPro = sp;
                    }
                }
                return sp;
            }
            else
            {
                return sp;
            }
        }
        [HttpPost]
        public string BuyCart_Tinh(string vc1, string vc2, string vc3, string Tinh, string nongthon)
        {
            string sp = "";
            int w = 0;
            int ship = 0;
            string times = "";
            string namepro = "";
            string Add = "";
            if (Session["ShoppingCart"] != null)
            {
                Customer mem = new Customer();
                Province provi = new Province();
                if (Session["Email"] != null)
                {
                    int provid = 0;
                    string email = Session["Email"].ToString();
                    mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                    if (mem != null)
                    {
                        Add = mem.Address;
                        if (Tinh == "")
                        {
                            provid = int.Parse(mem.Provice.ToString());
                        }
                        else
                        {
                            provid = int.Parse(Tinh.ToString());
                        }
                        provi = data.Provinces.Where(m => m.Id == provid).FirstOrDefault();
                        if (provi != null)
                        {
                            namepro = provi.Name;
                            if (vc1 != null)
                            {
                                times = provi.Time;
                            }
                            else if (vc2 != null)
                            {
                                times = provi.Time1;
                            }
                            else if (vc3 != null)
                            {
                                times = provi.Time2;
                            }
                            else
                            {
                                times = provi.Time;
                            }
                        }
                        var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                        #region[In ra vùng THÔNG TIN ĐƠN HÀNG]
                        sp += "<div id=\"opc-review\" class=\"box-info block-total-info section\" ><span class=\"ic-minus done-minus\" onclick=\"toggleXPMenu('Content-oreder');\">&nbsp;</span><div class=\"title step-title ttl-box\"> <span></span><h2>Thông tin đơn hàng</h2></div><div class=\"cls\"></div><div id =\"Content-oreder\"><div id=\"checkout-step-review\" class=\"step a-item\" ><div class=\"order-review info_order\" ><div class=\"product_in_order\">";
                        sp += "<div  class=\"title\"><div class=\"name_shop\">Sản phẩm</div><div class=\"sl\">Số lượng</div><div class=\"gt\">Giá thành</div><div class=\"tt\">Tổng tiền</div></div>";
                        foreach (var item in cart.CartItems)
                        {
                            sp += "<div class=\"product_box\">";
                            sp += "<div class=\"img_product\">";
                            sp += "<div class=\"img\">";
                            sp += "<img src=\"" + item.productImage + "\" width=\"60\" height=\"60\" alt=\"" + item.productName + "\" >";
                            sp += "</div>";
                            sp += "<div class=\"name_attr\">";
                            sp += "<div class=\"name\">" + item.productName + "</div>";
                            int cid = item.idcolor;
                            int sid = item.idsize;
                            var col = data.Colors.Where(m => m.Id == cid).FirstOrDefault();
                            var siz = data.Sizes.Where(m => m.Id == sid).FirstOrDefault();
                            w = (w + item.proweight);
                            if (col != null)
                            {
                                sp += "<div class=\"attr\"><p><label>Màu sắc: </label>" + col.Name + "</p></div>";
                            }
                            else
                            {
                                sp += "<div class=\"attr\"><p><label>Màu sắc: </label>Điền vào ghi chú</p></div>";
                            }
                            if (siz != null)
                            {
                                sp += "<div class=\"attr\"><p><label>Size: </label>" + siz.Name + "</p></div>";
                            }
                            else
                            {
                                sp += "<div class=\"attr\"><p><label>Size: </label>Free size</p></div>";
                            }
                            col = null;
                            siz = null;
                            sp += "</div>";
                            sp += "</div>";
                            sp += "<div class=\"sl\">" + item.count + "</div>";
                            sp += "<div class=\"gt\"><span class=\"price\">" + StringClass.Format_Price(item.price) + " VNĐ</span></div>";
                            sp += "<div class=\"tt\"><span class=\"price\">" + (int.Parse(item.price) * item.count) + " VNĐ</span></div>";
                            sp += "</div>";
                        }
                        #region[Lấy giá vận chuyển]
                        var Priceprovi = data.Province_Price.Where(m => m.ProvinceId == provid && m.From <= w && m.To >= w).FirstOrDefault();
                        if (Priceprovi != null)
                        {
                            if (vc1 != null)
                            {
                                ship = int.Parse(Priceprovi.PriceN.ToString());
                            }
                            else if (vc2 != null)
                            {
                                ship = int.Parse(Priceprovi.PriceC.ToString());
                            }
                            else if (vc3 != null)
                            {
                                ship = int.Parse(Priceprovi.PriceO.ToString());
                            }
                            else
                            {
                                ship = int.Parse(Priceprovi.PriceN.ToString());
                            }
                        }
                        #endregion
                        sp += "<div  id=\"checkout-review-load\"><div class=\"shippingfee_total\"> ";
                        sp += "<div class=\"shippingfee\"><label>Vận chuyển: </label><div class=\"choice_supplier\"><div class=\"price\">  ";
                        sp += "<div class='shipto'>Giao hàng đến <strong> " + Add + " - " + namepro + " </strong> với phí <strong>" + StringClass.Format_Price(ship.ToString()) + " VNĐ</strong> <br>";
                        if (mem.cNongthon == 1 )
                        {
                            sp += "Cộng thêm <strong> 20% </strong>phí thuộc ngoại thành, nông thôn của Bưu điện: <strong>" + StringClass.Format_Price(((ship * 20) / 100).ToString()) + " VNĐ</strong> <br>";
                        }
                        sp += "<span class=\"txt-transport\">Giao hàng từ <strong>" + times + "</strong></span></div>";
                        sp += "</div></div></div>";

                        sp += "<div class=\"block_pay\"><ul class=\"cash\">";
                        sp += "<li>";
                        sp += "<span class=\"fl\">Tổng tiền:</span>";
                        sp += "<span class=\"fr\"><strong>" + StringClass.Format_Price(cart.CartTotal.ToString()) + "</strong>&nbsp;VNĐ</span>";
                        sp += "</li>";
                        sp += "<li>";
                        sp += "<span class=\"fl\">";
                        sp += "Phí vận chuyển (PVC):</span>";
                        sp += "<span class=\"fr\">" + StringClass.Format_Price(ship.ToString()) + "&nbsp;VNĐ</span>";
                        sp += "</li>";
                        if (mem.cNongthon == 1 || nongthon =="1")
                        {
                            sp += "<li>";
                            sp += "<span class=\"fl\">";
                            sp += "20% (PVC) ngoài thành, nông thôn:</span>";
                            sp += "<span class=\"fr\">" + StringClass.Format_Price(((ship * 20) / 100).ToString()) + "&nbsp;VNĐ</span>";
                            sp += "</li>";
                            sp += "<li>";
                            sp += "<span class=\"fl red basegrandtotal\"><strong>Tổng thành tiền:</strong></span>";
                            sp += "<span class=\"fr red basegrandtotal\"><strong>" + StringClass.Format_Price((cart.CartTotal + ship + ((ship * 20) / 100)).ToString()) + "&nbsp;VNĐ</strong></span>";
                            sp += "</li>";
                        }
                        else
                        {
                            sp += "<li>";
                            sp += "<span class=\"fl red basegrandtotal\"><strong>Tổng thành tiền:</strong></span>";
                            sp += "<span class=\"fr red basegrandtotal\"><strong>" + StringClass.Format_Price((cart.CartTotal + ship).ToString()) + "&nbsp;VNĐ</strong></span>";
                            sp += "</li>";
                        }
                        sp += "<li class=\"bt-payment\"><div class=\"buttons-set\">";
                        //sp += "<input type=\"submit\" name=\"btn_save_order\" title=\"Thanh toán\" class=\"button btn-checkout\" value=\"Thanh toán\" id=\"btn_save_order\"/>";
                        sp += "</div></li></ul></div>";
                        sp += "</div></div></div></div></div></div></div>";
                        #endregion
                    }
                }
                return sp;
            }
            else
            {
                return sp;
            }
        }
        
        public string BuyAdd(string vc1, string vc2, string vc3, string checks, string Name, string Add, string TinhTP, string Tel, string nongthon)
        {
            string chuoi = "";
            if (Session["ShoppingCart"] != null)
            {
                Customer mem = new Customer();
                Province provi = new Province();
                if (Session["Email"] != null)
                {
                    if (checks == "on")
                    {
                        if (Name == "")
                        {
                            chuoi += " - Nhập thông tin họ tên người nhận hàng! <br />";
                        }
                        if (Add == "")
                        {
                            chuoi += " - Nhập địa chỉ người nhận hàng! <br />";
                        }
                        if (TinhTP == "")
                        {
                            chuoi += " - Chọn Tỉnh/Thành phố nhận hàng! <br />";
                        }
                        if (Name == "")
                        {
                            chuoi += " - Nhập điện thoại người nhận hàng! <br />";
                        }
                        if (chuoi != "")
                        {
                            return chuoi;
                        }
                        else
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
                            string email = Session["Email"].ToString();
                            mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                            if (mem != null)
                            {
                                provid = int.Parse(TinhTP.ToString());
                                provi = data.Provinces.Where(m => m.Id == provid).FirstOrDefault();
                                #region[Lấy loại hình vận chuyển]
                                if (provi != null)
                                {
                                    if (Session["vc"].ToString() == "vc1")
                                    {
                                        sTypePay = "1";
                                    }
                                    else if (Session["vc"].ToString() == "vc2")
                                    {
                                        sTypePay = "2";
                                    }
                                    else if (Session["vc"].ToString() == "vc3")
                                    {
                                        sTypePay = "3";
                                    }
                                    else
                                    {
                                        sTypePay = "1";
                                    }
                                }
                                #endregion
                                tongdiem = float.Parse(mem.Diem.ToString());
                                var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                                ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                                #region[Lưu dữ liệu vào bảng đơn hàng]
                                onsoft.Models.Ord ord = new onsoft.Models.Ord();
                                ord.IdCus = mem.Id;
                                ord.Amount = shoppCart.CartTotal;
                                ord.SDate = DateTime.Now;
                                ord.TypePay = sTypePay;
                                ord.Status = "1";
                                ord.PriceVC = 0;
                                ord.Name = Name;
                                ord.Address = Add;
                                ord.Tel = Tel;
                                ord.ProviceId = provid;
                                ord.Nongthon = double.Parse(nongthon.ToString());
                                data.Ords.Add(ord);
                                diem = shoppCart.CartTotal/1000;
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
                                    if (mem.Si == true)
                                    {
                                        billdetail.Price = double.Parse(pro.PricePromotion.ToString());
                                        tien = float.Parse(item.count.ToString()) * float.Parse(pro.PricePromotion.ToString());
                                    }
                                    else
                                    {
                                        billdetail.Price = double.Parse(pro.PriceRetail.ToString());
                                        tien = float.Parse(item.count.ToString()) * float.Parse(pro.PriceRetail.ToString());
                                    }
                                    tongtien = tongtien + tien;
                                    billdetail.Total = double.Parse(item.total.ToString());
                                    //data.sp_OrderDetail_Insert();
                                    data.OrderDetails.Add(billdetail);

                                    pro.Num = pro.Num - int.Parse(item.count.ToString());

                                    data.Entry(pro).State = EntityState.Modified;

                                    data.SaveChanges();
                                }
                                #endregion
                                #region[Lấy giá vận chuyển]
                                var Priceprovi = data.Province_Price.Where(m => m.ProvinceId == provid && m.From <= w && m.To >= w).FirstOrDefault();
                                if (Priceprovi != null)
                                {
                                    if (Session["vc"].ToString() == "vc1")
                                    {
                                        ship = int.Parse(Priceprovi.PriceN.ToString());
                                    }
                                    else if (Session["vc"].ToString() == "vc2")
                                    {
                                        ship = int.Parse(Priceprovi.PriceC.ToString());
                                    }
                                    else if (Session["vc"].ToString() == "vc3")
                                    {
                                        ship = int.Parse(Priceprovi.PriceO.ToString());
                                    }
                                    else
                                    {
                                        ship = int.Parse(Priceprovi.PriceN.ToString());
                                    }
                                }
                                #endregion
                                #region[Update lại tổng tiền, tiền ship]
                                if (nongthon == "1")
                                {
                                    tienship = ship;
                                    listbillcus.PriceNT = ship * 20 / 100;
                                    listbillcus.PriceVC = tienship;
                                    listbillcus.Amount = tongtien + tienship + ((ship * 20) / 100);
                                }
                                else
                                {
                                    tienship = ship;
                                    listbillcus.PriceNT = 0;
                                    listbillcus.PriceVC = tienship;
                                    listbillcus.Amount = tongtien + tienship;
                                }
                                data.Entry(listbillcus).State = EntityState.Modified;
                                data.SaveChanges();
                                RemoveFromCartAll();
                                #endregion
                            }
                            #endregion
                            ViewBag.meta = "<meta http-equiv=\"Refresh\" content=\"100 ; URL=/Pages/order_success\"/>";
                            //return Json(new { success = chuoi});
                            return "";
                        }
                    }
                    else
                    {
                        #region[Lưu vào dababase theo tình huống có địa chỉ giao hàng là User đã đăng ký]
                        int provid = 0;
                        float tongdiem = 0;
                        float tongtien = 0;
                        float diem = 0;
                        float tien = 0;
                        int w = 0;
                        int ship = 0;
                        int tienship = 0;
                        string sTypePay = "";
                        string email = Session["Email"].ToString();
                        mem = data.Customers.Where(m => m.Email == email).FirstOrDefault();
                        if (mem != null)
                        {
                            provid = int.Parse(mem.Provice.ToString());
                            provi = data.Provinces.Where(m => m.Id == provid).FirstOrDefault();
                            #region[Lấy loại hình vận chuyển]
                            if (provi != null)
                            {
                                if (Session["vc"].ToString() == "vc1")
                                {
                                    sTypePay = "1";
                                }
                                else if (Session["vc"].ToString() == "vc2")
                                {
                                    sTypePay = "2";
                                }
                                else if (Session["vc"].ToString() == "vc3")
                                {
                                    sTypePay = "3";
                                }
                                else
                                {
                                    sTypePay = "1";
                                }
                            }
                            #endregion
                            tongdiem = float.Parse(mem.Diem.ToString());
                            var cart = (ShoppingCartViewModel)Session["ShoppingCart"];
                            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                            #region[Lưu dữ liệu vào bảng đơn hàng]
                            onsoft.Models.Ord ord = new onsoft.Models.Ord();
                            ord.IdCus = mem.Id;
                            ord.Amount = shoppCart.CartTotal;
                            ord.SDate = DateTime.Now;
                            ord.TypePay = sTypePay;
                            ord.Status = "1";
                            ord.PriceVC = 0;
                            data.Ords.Add(ord);
                            diem = shoppCart.CartTotal;
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
                                if (mem.Si == true)
                                {
                                    billdetail.Price = double.Parse(pro.PricePromotion.ToString());
                                    tien = float.Parse(item.count.ToString()) * float.Parse(pro.PricePromotion.ToString());
                                }
                                else
                                {
                                    billdetail.Price = double.Parse(pro.PriceRetail.ToString());
                                    tien = float.Parse(item.count.ToString()) * float.Parse(pro.PriceRetail.ToString());
                                }
                                tongtien = tongtien + tien;
                                billdetail.Total = double.Parse(item.total.ToString());
                                //data.sp_OrderDetail_Insert();
                                data.OrderDetails.Add(billdetail);

                                pro.Num = pro.Num - int.Parse(item.count.ToString());

                                data.Entry(pro).State = EntityState.Modified;

                                data.SaveChanges();
                            }
                            #endregion
                            #region[Lấy giá vận chuyển]
                            var Priceprovi = data.Province_Price.Where(m => m.ProvinceId == provid && m.From <= w && m.To >= w).FirstOrDefault();
                            if (Priceprovi != null)
                            {
                                if (Session["vc"].ToString() == "vc1")
                                {
                                    ship = int.Parse(Priceprovi.PriceN.ToString());
                                }
                                else if (Session["vc"].ToString() == "vc2")
                                {
                                    ship = int.Parse(Priceprovi.PriceC.ToString());
                                }
                                else if (Session["vc"].ToString() == "vc3")
                                {
                                    ship = int.Parse(Priceprovi.PriceO.ToString());
                                }
                                else
                                {
                                    ship = int.Parse(Priceprovi.PriceN.ToString());
                                }
                            }
                            #endregion
                            #region[Update lại tổng tiền, tiền ship]
                            if (mem.cNongthon == 1)
                            {
                                tienship = ship;
                                listbillcus.PriceNT = ship * 20 / 100;
                                listbillcus.PriceVC = tienship;
                                listbillcus.Amount = tongtien + tienship + ((ship * 20) / 100);
                            }
                            else
                            {
                                tienship = ship;
                                listbillcus.PriceNT = 0;
                                listbillcus.PriceVC = tienship;
                                listbillcus.Amount = tongtien + tienship;
                            }
                            data.Entry(listbillcus).State = EntityState.Modified;
                            data.SaveChanges();
                            RemoveFromCartAll();
                            #endregion
                        }
                        #endregion
                        ViewBag.meta = "<meta http-equiv=\"Refresh\" content=\"100 ; URL=/Pages/order_success\"/>";
                        //return Json(new { success = chuoi });
                        return "";
                    }
                }
                else
                {
                    ViewBag.meta = "<meta http-equiv=\"Refresh\" content=\"100 ; URL=/Pages/dangnhap\"/>";
                    //return Json(new { success = chuoi });
                    return "";
                }
            }
            else
            {
                ViewBag.meta = "<meta http-equiv=\"Refresh\" content=\"100 ; URL=/Home/Index\"/>";
                //return Json(new { success = chuoi });
                return "";
            }
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
        #endregion
    }
}
