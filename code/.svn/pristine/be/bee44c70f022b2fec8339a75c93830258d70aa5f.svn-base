using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class WareHouseController : Controller
    {
         wwwEntities db = new wwwEntities();
        #region[WareHouseIndex]
        public ActionResult WareHouseIndex()
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
            var all = db.WareHouses.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = db.sp_WareHouse_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = onsoft.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion
        
        #region[CascadingDropdown - GetMember]
        public ActionResult _GetMember(int id)
        {
            var mem = db.Members.Where(m => m.IdGroupMb == id).ToList();
            for (int i = 0; i < mem.Count; i++)
            {
                ViewBag.MemId = new SelectList(mem, "Id", "Name");
            }
            return PartialView();
        }
        #endregion
        #region[WareHouseCreate]
        public ActionResult WareHouseCreate()
        {
            ViewBag.GroupMember = new SelectList(db.GroupMembers, "Id", "Name");
            ViewBag.MemId = new SelectList(db.Members, "Id", "Name");
            return View();
        }
        #endregion
        #region[WareHouseCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WareHouseCreate(FormCollection collec, WareHouse wh)
        {
            if (Request.Cookies["Username"] != null)
            {
                wh.Name = collec["Name"];
                wh.Address = collec["Address"];
                wh.Tel = collec["Tel"];
                wh.Description = collec["Description"];
                if (collec["SDate"] == "") { wh.SDate = null; } else { wh.SDate = DateTime.Parse(collec["SDate"]); }
                if (collec["EDate"] == "") { wh.EDate = null; } else { wh.EDate = DateTime.Parse(collec["EDate"]); }
                var u = collec["MemId"];
                db.WareHouses.Add(wh);
                db.SaveChanges();
                var list = db.WareHouses.OrderByDescending(m => m.Id).ToList();
                List<Member> members = db.Members.ToList();
                Member user = null;
                foreach (Member item in members)
                {
                    if (item.Id == int.Parse(u))
                    {
                        user = item;
                        break;
                    }
                }
                MemberWareHouse userWh = new MemberWareHouse();
                userWh.IdMember = user.Id;
                userWh.IdWareHouse = list[0].Id;
                userWh.SDate = DateTime.Now;
                userWh.EDate = null;
                db.sp_MemberWareHouse_Insert(user.Id, list[0].Id, wh.SDate, null, 1);
                db.SaveChanges();
                return RedirectToAction("WareHouseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[WareHouseEdit]
        public ActionResult WareHouseEdit(int id)
        {
            var Edit = db.WareHouses.First(m => m.Id == id);
            var uWh = db.MemberWareHouses.Where(m => m.IdWareHouse == Edit.Id).OrderByDescending(m => m.SDate).ToList();
            var uWhId = uWh[0].IdMember;
            var u = db.Members.Where(m => m.Id == uWhId).FirstOrDefault();
            ViewBag.GroupMember = new SelectList(db.GroupMembers, "Id", "Name", u.IdGroupMb);
            var mem = db.Members.Where(m => m.IdGroupMb == u.IdGroupMb).ToList();
            ViewBag.MemId = new SelectList(mem, "Id", "Name", uWh[0].IdMember);
            return View(Edit);
        }
        #endregion
        #region[WareHouseEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WareHouseEdit(FormCollection collec, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var wh = db.WareHouses.First(m => m.Id == id);
                wh.Name = collec["Name"];
                wh.Address = collec["Address"];
                wh.Tel = collec["Tel"];
                wh.Description = collec["Description"];
                if (collec["SDate"] == "") { wh.SDate = null; } else { wh.SDate = DateTime.Parse(collec["SDate"]); }
                if (collec["EDate"] == "") { wh.EDate = null; } else { wh.EDate = DateTime.Parse(collec["EDate"]); }
                var u = int.Parse(collec["MemId"]);
                db.SaveChanges();
                var uWh = db.MemberWareHouses.Where(m => m.IdWareHouse == id).OrderByDescending(m => m.SDate).FirstOrDefault();
                if (u != uWh.IdMember)
                {
                    uWh.SDate = DateTime.Now;
                    db.SaveChanges();
                    MemberWareHouse userWh = new MemberWareHouse();
                    userWh.IdMember = u;
                    userWh.IdWareHouse = id;
                    userWh.SDate = DateTime.Now;
                    db.MemberWareHouses.Add(userWh);
                    db.SaveChanges();
                }
                return RedirectToAction("WareHouseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[WareHouseDelete]
        public ActionResult WareHouseDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (ModelState.IsValid)
                {
                    var del = db.WareHouses.First(m => m.Id == id);
                    db.WareHouses.Remove(del);
                    db.SaveChanges();
                }
                return RedirectToAction("WareHouseIndex");
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
                string str = "";
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in db.WareHouses where emp.Id == id select emp).SingleOrDefault();
                            db.WareHouses.Remove(Del);
                            str += id.ToString() + ",";
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("WareHouseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[HangTon]
        public ActionResult HangTon()
        {
            string chuoi = "";
            int? countPro;
            var imp = (from b in db.Imports select b).ToList();
            //Lay tong so sp da nhap theo tung sp
            var impTotal = (from a in db.ImportDetails group a by a.IdPro into s select new { ProImpId = s.Key, total = s.Sum(a => a.Number) }).ToList();
            //Lay tong so sp da xuat theo tung sp
            var expTotal = (from a in db.ExportDetails group a by a.IdPro into s select new { ProExpId = s.Key, total = s.Sum(a => a.Number) }).ToList();
            var im = impTotal;
            if (expTotal.Count > 0)
            {
                for (int i = 0; i < expTotal.Count; i++)
                {
                    var ProExp = expTotal[i].ProExpId.ToString();
                    var countExp = expTotal[i].total.ToString();
                    for (int k = 0; k < impTotal.Count; k++)
                    {
                        var ProImp = impTotal[k].ProImpId.ToString();
                        var countImp = impTotal[k].total.ToString();
                        if (ProExp == ProImp)
                        {
                            var pro = db.Products.Where(m => m.Id == int.Parse(ProExp)).FirstOrDefault();
                            countPro = int.Parse(countImp) - int.Parse(countExp);
                            if (pro != null)
                            {
                                if (i % 2 == 0)
                                {
                                    chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + pro.Id + "</td>";
                                    chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                                else
                                {
                                    chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + pro.Id + "</td>";
                                    chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                                pro.SpTon = countPro;
                                db.SaveChanges();
                            }
                            else
                            {
                                if (i % 2 == 0)
                                {
                                    chuoi += "<tr id=\"trOdd_" + i + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + impTotal[k].ProImpId + "</td>";
                                    chuoi += "<td class=\"Text\">" + impTotal[k].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[k].ProImpId + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                                else
                                {
                                    chuoi += "<tr id=\"trEven_" + i + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + i + ")\">";
                                    chuoi += "<td class=\"NumberM\">" + (i + 1) + " _ " + impTotal[k].ProImpId + "</td>";
                                    chuoi += "<td class=\"Text\">" + impTotal[k].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                                    chuoi += "<td class=\"Number\">" + countPro + "</td>";
                                    chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[k].ProImpId + "' class='views'>Xem</a></td>";
                                    chuoi += "</tr>";
                                }
                            }
                            im.RemoveAt(k);
                            break;
                        }
                    }
                }
                for (int j = 0; j < im.Count; j++)
                {
                    var ProImp = im[j].ProImpId;
                    var countImp = im[j].total;
                    var pro = db.Products.Where(m => m.Id == ProImp).FirstOrDefault();
                    if (pro != null)
                    {
                        if (j % 2 == 0)
                        {
                            chuoi += "<tr id=\"trOdd_" + j + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + pro.Id + "</td>";
                            chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                        else
                        {
                            chuoi += "<tr id=\"trEven_" + j + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + j + ")\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + pro.Id + "</td>";
                            chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                        pro.SpTon = int.Parse(countImp.ToString());
                        db.SaveChanges();
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            chuoi += "<tr id=\"trOdd_" + j + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + impTotal[j].ProImpId + "</td>";
                            chuoi += "<td class=\"Text\">" + impTotal[j].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[j].ProImpId + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                        else
                        {
                            chuoi += "<tr id=\"trEven_" + j + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + j + ")\">";
                            chuoi += "<td class=\"NumberM\">" + (j + 1) + " _ " + impTotal[j].ProImpId + "</td>";
                            chuoi += "<td class=\"Text\">" + impTotal[j].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</td>";
                            chuoi += "<td class=\"Number\">" + countImp + "</td>";
                            chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + impTotal[j].ProImpId + "' class='views'>Xem</a></td>";
                            chuoi += "</tr>";
                        }
                    }
                }
            }
            else
            {
                if (impTotal.Count > 0)
                {
                    for (int k = 0; k < impTotal.Count; k++)
                    {
                        var ProImp = impTotal[k].ProImpId;
                        var countImp = impTotal[k].total;
                        var pro = db.Products.Where(m => m.Id == ProImp).FirstOrDefault();
                        if (pro != null)
                        {
                            if (k % 2 == 0)
                            {
                                chuoi += "<tr id=\"trOdd_" + k + "\" class=\"trOdd\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this,0)\">";
                                chuoi += "<td class=\"NumberM\">" + (k + 1) + " _ " + pro.Id + "</td>";
                                chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                chuoi += "<td class=\"Number\">" + countImp + "</td>";
                                chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                chuoi += "</tr>";
                            }
                            else
                            {
                                chuoi += "<tr id=\"trEven_" + k + "\" class=\"trEven\" onmousemove=\"Javascript:chkSelect_OnMouseMove1(this)\" onmouseout=\"Javascript:chkSelect_OnMouseOut1(this," + k + ")\">";
                                chuoi += "<td class=\"NumberM\">" + (k + 1) + " _ " + pro.Id + "</td>";
                                chuoi += "<td class=\"Text\">" + pro.Name + "</td>";
                                chuoi += "<td class=\"Number\">" + countImp + "</td>";
                                chuoi += "<td class=\"Function\"><a href='/WareHouse/ChiTietHangTon/" + pro.Id + "' class='views'>Xem</a></td>";
                                chuoi += "</tr>";
                            }
                            pro.SpTon = int.Parse(countImp.ToString());
                        }
                        break;
                    }
                }
            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
        #region[ChiTietHangTon]
        public ActionResult ChiTietHangTon(int id)
        {
            string chuoi = "";
            int? countPro;
            int? countTotal = 0;
            //Lay ra so san pham da nhap theo id tren cua tung kho
            var impTotal = (from a in db.ImportDetails join b in db.Imports on a.IdImport equals b.Id where a.IdPro == id group a by new { a.IdPro, b.IdWareHouse } into s select new { ProImpId = s.Key.IdPro, WHImpId = s.Key.IdWareHouse, total = s.Sum(a => a.Number) }).ToList();
            //Lay ra tong so san pham da xuat theo id tren cua tung kho
            var expTotal = (from a in db.ExportDetails join b in db.Exports on a.IdExport equals b.Id where a.IdPro == id group a by new { a.IdPro, b.IdWareHouse } into s select new { ProExpId = s.Key.IdPro, WHExpId = s.Key.IdWareHouse, total = s.Sum(a => a.Number) }).ToList();
            var product = db.Products.Where(m => m.Id == id).FirstOrDefault();
            chuoi += "<div class=\"viewInfo\">";
            chuoi += "<div>";
            chuoi += "<p>Mã sản phẩm</p>";
            chuoi += "<p>Tên sản phẩm</p>";
            chuoi += "</div>";
            chuoi += "<div>";
            if (product != null)
            {
                chuoi += "<p>" + product.Id + "</p>";
                chuoi += "<p>" + product.Name + "</p>";
            }
            else
            {
                chuoi += "<p>" + impTotal[0].ProImpId + "</p>";
                chuoi += "<p>" + impTotal[0].ProImpId + "(Đã xóa khỏi danh sách sản phẩm)</p>";
            }
            chuoi += "</div>";
            chuoi += "<table border=\"1\">";
            chuoi += "<tr>";
            chuoi += "<th>Kho</th>";
            chuoi += "<th>Hàng nhập</th>";
            chuoi += "<th>Hàng xuất</th>";
            chuoi += "<th>Hàng tồn</th>";
            chuoi += "</tr>";
            if (expTotal.Count > 0)
            {
                for (int j = 0; j < expTotal.Count; j++)
                {
                    var ProExp = expTotal[j].ProExpId;
                    var countExp = expTotal[j].total;
                    for (int t = 0; t < impTotal.Count; t++)
                    {
                        var ProImp = impTotal[t].ProImpId;
                        var countImp = impTotal[t].total;
                        if (expTotal[j].WHExpId == impTotal[t].WHImpId)
                        {
                            countPro = int.Parse(countImp.ToString()) - int.Parse(countExp.ToString());
                            var wh = db.WareHouses.First(m => m.Id == impTotal[t].WHImpId);
                            chuoi += "<tr>";
                            chuoi += "<td>" + wh.Name + "</td>";
                            chuoi += "<td>" + countImp + "</td>";
                            chuoi += "<td>" + countExp + "</td>";
                            chuoi += "<td>" + countPro + "</td>";
                            chuoi += "</tr>";
                            countTotal = countTotal + countPro;
                            //impTotal.RemoveAt(t);
                        }
                        else
                        {
                            var wh = db.WareHouses.First(m => m.Id == impTotal[t].WHImpId);
                            chuoi += "<tr>";
                            chuoi += "<td>" + wh.Name + "</td>";
                            chuoi += "<td>" + countImp + "</td>";
                            chuoi += "<td>0</td>";
                            chuoi += "<td>" + impTotal[t].total + "</td>";
                            chuoi += "</tr>";
                            countTotal = int.Parse(countTotal.ToString()) + int.Parse(impTotal[t].total.ToString());
                        }
                    }
                }
            }
            else
            {
                for (int p = 0; p < impTotal.Count; p++)
                {
                    var wh = db.WareHouses.First(m => m.Id == impTotal[p].WHImpId);
                    countPro = int.Parse(impTotal[p].total.ToString());
                    chuoi += "<tr>";
                    chuoi += "<td>" + wh.Name + "</td>";
                    chuoi += "<td>" + impTotal[p].total + "</td>";
                    chuoi += "<td> 0 </td>";
                    chuoi += "<td>" + countPro + "</td>";
                    chuoi += "</tr>";
                    countTotal = countTotal + countPro;
                }
            }
            chuoi += "<tr>";
            chuoi += "<td colspan=\"3\">Tổng</td>";
            chuoi += "<td>" + countTotal + "</td>";
            chuoi += "</tr>";
            chuoi += "</table>";
            chuoi += "</div>";
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
        #region[WareHouseViews]
        public ActionResult WareHouseViews(int id)
        {
            string chuoi = "";
            var wh = db.WareHouses.Where(m => m.Id == id).FirstOrDefault();
            var userWh = db.MemberWareHouses.Where(m => m.IdWareHouse == wh.Id).ToList();
            chuoi += "<h2>Xem thông tin kho hàng</h2>";
            chuoi += "<div class=\"viewInfo\">";
            chuoi += "<div>";
            chuoi += "<p>Tên kho</p>";
            chuoi += "<p>Địa chỉ kho</p>";
            chuoi += "<p>Số điện thoại</p>";
            chuoi += "<div>Ghi chú</div>";
            chuoi += "</div>";
            chuoi += "<div>";
            chuoi += "<p>" + wh.Name + "</p>";
            chuoi += "<p>" + wh.Address + "</p>";
            chuoi += "<p>" + wh.Tel + "</p>";
            chuoi += "</div>";
            chuoi += "</div>";
            chuoi += "<div class=\"clearfix\"></div>";
            chuoi += "<div class='note'>" + wh.Description + "</div>";
            if (userWh.Count > 0)
            {
                chuoi += "<div class=\"divShowHideHistory\">";
                chuoi += "<div class=\"showHideHistory\">Xem thông tin lịch sử nhân viên quản kho</div>";
                chuoi += "<div id=\"divHistory\">";
                chuoi += "<table border=\"1\">";
                chuoi += "<tr>";
                chuoi += "<th>STT</th>";
                chuoi += "<th>Tên nhân viên</th>";
                chuoi += "<th>Ngày bắt đầu</th>";
                chuoi += "<th>Ngày kết thúc</th>";
                chuoi += "</tr>";
                var useWhId = 0;
                for (int i = 0; i < userWh.Count; i++)
                {
                    useWhId = (int)userWh[i].IdMember;
                    var user = db.Members.Where(m => m.Id == useWhId).FirstOrDefault();
                    chuoi += "<tr>";
                    chuoi += "<td>" + (i + 1) + "</td>";
                    chuoi += "<td>" + user.Name + "</td>";
                    chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(userWh[i].SDate.ToString()) + "</td>";
                    if (userWh[i].SDate == null)
                    {
                        chuoi += "<td>Hiện vẫn đang làm</td>";
                    }
                    else
                    {
                        chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(userWh[i].SDate.ToString()) + "</td>";
                    }
                    chuoi += "</tr>";
                }
                chuoi += "</table>";
                chuoi += "</div>";
                chuoi += "</div>";
            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion
    }
}
