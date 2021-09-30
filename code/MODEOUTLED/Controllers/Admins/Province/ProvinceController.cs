using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;
using onsoft.Models;

namespace MODEOUTLED.Controllers.Admins.Province
{
    public class ProvinceController : Controller
    {
        wwwEntities db = new wwwEntities();
        #region[PriceCityIndex]
        public ActionResult ProvinceIndex(int? page, string currentProvinceName)
        {
            if (Request.HttpMethod == "GET")
            {
                if (Session["ProvinceName"] != null)
                {
                    currentProvinceName = Session["ProvinceName"].ToString();
                    Session["ProvinceName"] = null;
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentProvinceName = currentProvinceName;

            var all = db.sp_Province_GetByAll().OrderBy(p => p.Level).ToList();

            if (!String.IsNullOrEmpty(currentProvinceName))
            {
                all = all.Where(p => p.Name.ToUpper().Contains(currentProvinceName.ToUpper())).OrderBy(p => p.Level).ToList();
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

        #region[ProvinceDelete]
        public ActionResult ProvinceDelete(int id)
        {
            var del = db.Provinces.First(p => p.Id == id);

            db.Provinces.Remove(del);
            db.SaveChanges();

            return RedirectToAction("ProvinceIndex", "Province");

        }
        #endregion

        #region[PriCityCreate]

        // GET: /PriceCity/ProvinceCreate
        [HttpGet]
        public ActionResult ProvinceCreate()
        {
            var priceCities = db.Provinces.OrderBy(p => p.Level).ToList();
            List<SelectListItem> listPriceCity = new List<SelectListItem>();
            listPriceCity.Clear();
            for (int i = 0; i < priceCities.Count; i++)
            {
                listPriceCity.Add(new SelectListItem { Text = StringClass.ShowNameLevel(priceCities[i].Name, priceCities[i].Level), Value = priceCities[i].Level.ToString() });
            }

            ViewBag.Province = listPriceCity;

            return View();
        }

        // GET: /PriceCity/CreateSub
        [HttpGet]
        public ActionResult CreateSub(string level)
        {
            //var priceCities = db.Provinces.OrderBy(p => p.Level).ToList();

            //foreach (var item in priceCities)
            //{
            //    item.Name = StringClass.ShowNameLevel(item.Name, item.Level);
            //}

            //ViewBag.Province = new SelectList(priceCities, "Level", "Name", level);

            return View();
        }

        // POST: /PriceCity/ ProvinceCreate
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProvinceCreate(FormCollection collection, onsoft.Models.Province pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"];
                pc.Ord = Int32.Parse(collection["Ord"]);
                //if (collection["Price"] != "") { pc.Price = int.Parse(collection["Price"]); } else { pc.Price = 0; }
                //if (collection["Price1"] != "") { pc.Price1 = double.Parse(collection["Price1"]); } else { pc.Price1 = 0; }
                //if (collection["Price2"] != "") { pc.Price2 = double.Parse(collection["Price2"]); } else { pc.Price2 = 0; }
                pc.Time = collection["Time"];
                pc.Time1 = collection["Time1"];
                pc.Time2 = collection["Time2"];
                string le = collection["Province"];
                //if (le.Length > 0)
                //{
                //    pc.Level = le + "00000";
                //}
                //else
                //{
                    pc.Level = "00000";
                //}
                db.Provinces.Add(pc);
                db.SaveChanges();
                return RedirectToAction("ProvinceIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[ProvinceEdit]

        [HttpGet]
        public ActionResult ProvinceEdit(int id)
        {
            var priceCity = db.Provinces.Find(id);

            //var priceCities = db.Provinces.OrderBy(p => p.Level).ToList();

            //foreach (var item in priceCities)
            //{
            //    item.Name = StringClass.ShowNameLevel(item.Name, item.Level);
            //}

            //if (priceCity.Level.Length > 5)
            //{
            //    string parentLevel = priceCity.Level.Substring(0, priceCity.Level.Length - 5);
            //    ViewBag.Province = new SelectList(priceCities, "Level", "Name", parentLevel);
            //}
            //else
            //{
            //    ViewBag.Province = new SelectList(priceCities, "Level", "Name");
            //}

            return View(priceCity);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProvinceEdit(FormCollection collection, onsoft.Models.Province pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"].Replace(".", "");
                pc.Ord = int.Parse(collection["Ord"]);
                //if (collection["Price"] != "") { pc.Price = int.Parse(collection["Price"]); } else {pc.Price = 0;}
                //if (collection["Price1"] != "") { pc.Price1 = double.Parse(collection["Price1"]); } else { pc.Price1 = 0; }
                //if (collection["Price2"] != "") { pc.Price2 = double.Parse(collection["Price2"]); } else { pc.Price2 = 0; }
                string parentLevel = collection["Province"];
                //pc.Level = parentLevel + "00000";
                pc.Level =  "00000";
                pc.Time = collection["Time"];
                pc.Time1 = collection["Time1"];
                pc.Time2 = collection["Time2"];
                db.sp_Province_Update(pc.Id, pc.Name, pc.Level, pc.Price, pc.Ord, true, pc.Time, 0, 0, pc.Time1, pc.Time2);
                db.SaveChanges();

                return RedirectToAction("ProvinceIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion

        #region[Ajax Change Provinces]

        // AJAX: /PriceCity/ChangePriceCity
        [HttpPost]
        public ActionResult ChangeProvince(int id, string ord, string price, string time, string Price1, string Time1, string Price2, string Time2, string Name)
        {
            var results = "";
            var Province = db.Provinces.Find(id);
            if (Province != null)
            {
                if (ord != null)
                {
                    Province.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                else if (price != null)
                {
                    Province.Price = int.Parse(price);
                    results = "Cước CPN đã được thay đổi!";
                }
                else if (Price1 != null)
                {
                    Province.Price1 = double.Parse(Price1);
                    results = "Cước CP thường đã được thay đổi!";
                }
                else if (Price2 != null)
                {
                    Province.Price2 = double.Parse(Price2);
                    results = "Cước CP ô tô đã được thay đổi!";
                }
                else if (time != null)
                {
                    Province.Time = time;
                    results = "Thời gian CPN đã được thay đổi!";
                }
                else if (Time1 != null)
                {
                    Province.Time1 = Time1;
                    results = "Thời gian CP thường đã được thay đổi!";
                }
                else if (Time2 != null)
                {
                    Province.Time2 = Time2;
                    results = "Thời gian CP ô tô đã được thay đổi!";
                }
                else if (Name != null)
                {
                    Province.Name = Name;
                    results = "Tên tỉnh thành phố đã được thay đổi!";
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
                                var Del = (from emp in db.Provinces where emp.Id == id select emp).SingleOrDefault();
                                db.Provinces.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("ProvinceIndex");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["Province"].Length > 0)
                    {
                        Session["ProvinceName"] = collection["Province"];
                    }
                    return RedirectToAction("ProvinceIndex");
                }
                else
                {
                    return RedirectToAction("ProvinceIndex");
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Autocomplete Provinces Name]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var priceCityNames = from p in db.Provinces
                                 select p.Name;
            string[] items = priceCityNames.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
