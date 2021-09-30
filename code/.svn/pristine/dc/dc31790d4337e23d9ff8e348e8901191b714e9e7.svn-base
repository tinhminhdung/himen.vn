using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;
using System.Data;

namespace MODEOUTLED.Controllers
{
    public class CustomerController : Controller
    {

         wwwEntities db = new wwwEntities();

        #region[CustomerIndex]
        public ActionResult CustomerIndex()
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
            var all = db.Customers.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = db.sp_Customer_Phantrang(page, pagesize, "", "").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            ViewBag.Pager = Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            return View(pages);
        }
        #endregion

        #region[CustomerIndexot]
        public ActionResult CustomerIndexot(int? page, string sortName, string sortDate, string currentCustomerAmount, string currentCusName)
        {
            if (Request.HttpMethod == "GET")
            {
                if (Session["CustomerAmount"] != null)
                {
                    currentCustomerAmount = Session["CustomerAmount"].ToString();
                }

                if (Session["CusName"] != null)
                {
                    currentCusName = Session["CusName"].ToString();
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentCustomerAmount = currentCustomerAmount;
            ViewBag.CurrentCusName = currentCusName;

            ViewBag.CurrentSortName = sortName;
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

            ViewBag.CurrentSortDate = sortDate;
            ViewBag.SortDateParm = sortDate == "date asc" ? "date desc" : "date asc";

            var all = db.Customers.OrderByDescending(c => c.Id).ToList();


            if (!String.IsNullOrEmpty(currentCusName))
            {
                all = all.Where(c => c.Name.ToUpper().Contains(currentCusName.ToUpper())).OrderByDescending(c => c.Id).ToList();
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

            switch (sortDate)
            {
                case "date desc":
                    all = all.OrderByDescending(p => p.SDate).ToList();
                    break;
                case "date asc":
                    all = all.OrderBy(p => p.SDate).ToList();
                    break;
                default:
                    break;
            }

            int pageSize = 10;
            if (currentCustomerAmount != null)
            {
                pageSize = Convert.ToInt32(currentCustomerAmount);
            }
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
            ViewBag.PageSize = pageSize;

            // Số khách hàng hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Khách hàng / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Khách hàng / trang", Value = i + "" });
                }
            }
            ViewBag.ddlCustomerAmount = items;

            ViewBag.TotalCustomer = db.Customers.Count();

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

        #region[CustomerEdit]
        public ActionResult CustomerEdit(int id)
        {
            var Edit = db.Customers.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion

        #region[CustomerEdit]
        [HttpPost]
        public ActionResult CustomerEdit(FormCollection collection, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var cus = db.Customers.First(m => m.Id == id);
                cus.Name = collection["Name"];
                cus.Email = collection["Email"];
                cus.Password = StringClass.Encrypt(collection["Password"]);
                cus.Tel = collection["Tel"];
                cus.Address = collection["Address"];
                cus.SDate = DateTime.Now;
                cus.Status = (collection["Status"] == "false") ? false : true;
                db.SaveChanges();
                return RedirectToAction("CustomerIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[CustomerEditot]
        public ActionResult CustomerEditot(int id, int page, int pagesize)
        {

            var Cus = db.Customers.First(m => m.Id == id);
            ViewBag.Name = Cus.Name;
            ViewBag.mPage = page;
            ViewBag.PageSize = pagesize;

            return View(Cus);
        }
        #endregion

        #region[CustomerEditot]
        [HttpPost]
        public ActionResult CustomerEditot(FormCollection collection, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var cus = db.Customers.First(m => m.Id == id);
                cus.Name = collection["Name"];
                cus.Email = collection["Email"];
                cus.Password = StringClass.Encrypt(collection["Password"]);
                cus.Tel = collection["Tel"];
                cus.Address = collection["Address"];
                cus.SDate = DateTime.Now;
                cus.Status = (collection["Status"] == "false") ? false : true;
                db.SaveChanges();
                return RedirectToAction("CustomerIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[CustomerDelete]
        public ActionResult CustomerDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.Customers.First(m => m.Id == id);
                db.Customers.Remove(del);
                db.SaveChanges();

                if (db.Customers.ToList().Count % pagesize == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("CustomerIndexot");
                    }

                }
                return RedirectToAction("CustomerIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiDelete(FormCollection collection)
        {
            int m = int.Parse(collection["mPage"]);
            int pagesize = int.Parse(collection["PageSize"]);

            List<Product> products = db.Products.ToList();
            int lastpage = products.Count / pagesize;
            if (products.Count % pagesize > 0)
            {
                lastpage++;
            }

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
                                int id = Convert.ToInt32(key.Remove(0,3));
                                var Del = (db.Customers.Where(c => c.Id == id)).FirstOrDefault();
                                db.Customers.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    if (collection["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("CustomerIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }

                    return RedirectToAction("CustomerIndexot", new { page = m });
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["CusName"].Length > 0)
                    {
                        Session["CusName"] = collection["CusName"];
                    }
                    return RedirectToAction("CustomerIndexot");
                }
                else if (collection["ddlCustomerAmount"] != null)
                {
                    if (collection["ddlCustomerAmount"].Length > 0)
                    {
                        Session["CustomerAmount"] = collection["ddlCustomerAmount"];
                    }
                    return RedirectToAction("CustomerIndexot");
                }
                
                else {
                    return RedirectToAction("UserIndex", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Autocomplete Customer Name]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var customerNames = from c in db.Customers
                                select c.Name;
            string[] items = customerNames.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region[Ajax Change Customer]

        // AJAX: /Customer/ChangeStatus
        [HttpPost]
        public ActionResult ChangeStatus(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                customer.Status = customer.Status == true ? false : true;
            }
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái đã được thay đổi.";

            return Json(results);
        }

        // AJAX: /Customer/ChangeSi
        [HttpPost]
        public ActionResult ChangeSi(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                customer.Si = customer.Si == true ? false : true;
            }
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Loại thành viên đã được thay đổi.";

            return Json(results);
        }

        // AJAX: /Customer/ChangeCustomer
        [HttpPost]
        public ActionResult ChangeCustomer(int id, string sdate, string email, string tel, string address, string name, string diem)
        {
            var results = "";
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                if (sdate != null)
                {
                    customer.SDate = DateTime.Parse(sdate);
                    results = "Ngày đăng ký đã được thay đổi.";
                }

                if (email != null)
                {
                    customer.Email = email;
                    results = "Email đã được thay đổi.";
                }

                if (tel != null)
                {
                    customer.Tel = tel;
                    results = "Số điện thoại đã được thay đổi.";
                }

                if (address != null)
                {
                    customer.Address = address;
                    results = "Địa chỉ đã được thay đổi.";
                }

                if (name != null)
                {
                    customer.Name = name;
                    results = "Họ tên đã được thay đổi.";
                }
                if (diem != null)
                {
                    customer.Diem = double.Parse(diem.ToString());
                    results = "Điểm tích lũy đã được thay đổi.";
                }
            }
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        #endregion

    }
}
