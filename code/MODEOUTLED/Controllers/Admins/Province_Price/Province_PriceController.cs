using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;
using onsoft.Models;


namespace MODEOUTLED.Controllers.Admins.Province_Price
{
    public class Province_PriceController : Controller
    {
         wwwEntities db = new wwwEntities();
        #region[Province_PriceIndex]
        public ActionResult Province_PriceIndex(int? page, string ProvincePrice, string ProvinceID)
        {
            #region[view drop search]
            var Province = db.Provinces.ToList();
            for (int i = 0; i < Province.Count; i++)
            {
                ViewBag.Province = new SelectList(Province, "Id", "Name");
            }
            #endregion
            if (Request.HttpMethod == "GET")
            {
                if (Session["ProvincePrice"] != null)
                {
                    ProvincePrice = Session["ProvincePrice"].ToString();
                    Session["ProvincePrice"] = null;
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.ProvincePrice = ProvincePrice;

            var all = db.sp_Province_Price_GetByAll().OrderBy(o =>o.ProvinceId).ToList();
            if (!String.IsNullOrEmpty(ProvincePrice))
            {
                int ProvinceId = Int32.Parse(ProvincePrice);
                all = all.Where(p => p.ProvinceId == ProvinceId).OrderByDescending(p => p.Id).ToList();
            }
            if (!String.IsNullOrEmpty(ProvinceID))
            {
                int ProvinceId = Int32.Parse(ProvinceID);
                all = all.Where(p => p.ProvinceId == ProvinceId).OrderByDescending(p => p.Id).ToList();
            }
            int pageSize = 25;
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
            return View(all.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region[Province_PriceDelete]
        public ActionResult Province_PriceDelete(int id)
        {
            var del = db.Province_Price.First(p => p.Id == id);

            db.Province_Price.Remove(del);
            db.SaveChanges();

            return RedirectToAction("Province_PriceIndex", "Province_Price");

        }
        #endregion

        #region[PriCityCreate]

        // GET: /PriceCity/Province_PriceCreate
        [HttpGet]
        public ActionResult Province_PriceCreate()
        { 
            #region[view drop]
            var Province = db.Provinces.ToList();
            for (int i = 0; i < Province.Count; i++)
            {
                ViewBag.Province = new SelectList(Province, "Id", "Name");
            }
            #endregion
            return View();
        }

        // POST: /PriceCity/ Province_PriceCreate
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Province_PriceCreate(FormCollection collection, onsoft.Models.Province_Price pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.ProvinceId = int.Parse(collection["Province"].ToString());
                pc.From =  double.Parse(collection["From"].ToString());
                pc.To = double.Parse(collection["To"].ToString());
                if (collection["PriceN"] != "") { pc.PriceN = double.Parse(collection["PriceN"]); } else { pc.PriceN = 0; }
                if (collection["PriceC"] != "") { pc.PriceC = double.Parse(collection["PriceC"]); } else { pc.PriceC = 0; }
                if (collection["PriceO"] != "") { pc.PriceO = double.Parse(collection["PriceO"]); } else { pc.PriceO = 0; }
                db.Province_Price.Add(pc);
                db.SaveChanges();
                return RedirectToAction("Province_PriceIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Province_PriceEdit]

        [HttpGet]
        public ActionResult Province_PriceEdit(int id)
        {
            var Province_Price = db.Province_Price.Find(id);
            #region[view drop]
            var Province = db.Provinces.ToList();
            for (int i = 0; i < Province.Count; i++)
            {
                ViewBag.Province = new SelectList(Province, "Id", "Name", Province_Price.ProvinceId);
            }
            #endregion
            return View(Province_Price);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Province_PriceEdit(FormCollection collection, onsoft.Models.Province_Price pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.ProvinceId = int.Parse(collection["Province"].ToString());
                pc.From = double.Parse(collection["From"].ToString());
                pc.To = double.Parse(collection["To"].ToString());
                if (collection["PriceN"] != "") { pc.PriceN = double.Parse(collection["PriceN"]); } else { pc.PriceN = 0; }
                if (collection["PriceC"] != "") { pc.PriceC = double.Parse(collection["PriceC"]); } else { pc.PriceC = 0; }
                if (collection["PriceO"] != "") { pc.PriceO = double.Parse(collection["PriceO"]); } else { pc.PriceO = 0; }
                db.sp_Province_Price_Update(pc.Id, pc.ProvinceId, pc.From, pc.To, pc.PriceN, pc.PriceC, pc.PriceO);
                db.SaveChanges();

                return RedirectToAction("Province_PriceIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion

        #region[Ajax Change Province_Prices]

        // AJAX: /PriceCity/ChangePriceCity
        [HttpPost]
        public ActionResult ChangeProvince_Price(int id, string From, string To, string PriceN, string PriceC, string PriceO)
        {
            var results = "";
            var Province = db.Province_Price.Find(id);
            if (Province != null)
            {
                if (From != null)
                {
                    Province.From = double.Parse(From);
                    results = "Trọng lượng từ đã được thay đổi.";
                }
                else if (To != null)
                {
                    Province.To = double.Parse(To);
                    results = "Trọng lượng đến đã được thay đổi!";
                }
                else if (PriceN != null)
                {
                    Province.PriceN = double.Parse(PriceN);
                    results = "Cước CP nhanh đã được thay đổi!";
                }
                else if (PriceC != null)
                {
                    Province.PriceC = double.Parse(PriceC);
                    results = "Cước CP thường đã được thay đổi!";
                }
                else if (PriceO != null)
                {
                    Province.PriceO = double.Parse(PriceO);
                    results = "Cước chuyển Ô tô đã được thay đổi!";
                }
            }
            db.Entry(Province).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }

        #endregion

        #region[MultiCommand]
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
                                var Del = (from emp in db.Province_Price where emp.Id == id select emp).SingleOrDefault();
                                db.Province_Price.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("Province_PriceIndex");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["Province"].Length > 0)
                    {
                        Session["ProvincePrice"] = collection["Province"];
                    }
                    return RedirectToAction("Province_PriceIndex");
                }
                else
                {
                    return RedirectToAction("Province_PriceIndex");
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

    }
}
