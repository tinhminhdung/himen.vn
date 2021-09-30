using onsoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;

namespace MODEOUTLED.Controllers
{
    public class PriceCityController : Controller
    {
         wwwEntities db = new wwwEntities();


        #region[PriceCityIndex]
        public ActionResult PriceCityIndex(int? page, string currentPriceCityName)
        {
            if (Request.HttpMethod == "GET")
            {
                if (Session["PriceCityName"] != null)
                {
                    currentPriceCityName = Session["PriceCityName"].ToString();
                    Session["PriceCityName"] = null;
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentPriceCityName = currentPriceCityName;

            var all = db.sp_PriceCity_GetByAll().OrderBy(p => p.Level).ToList();

            if (!String.IsNullOrEmpty(currentPriceCityName))
            {
                all = all.Where(p => p.Name.ToUpper().Contains(currentPriceCityName.ToUpper())).OrderBy(p => p.Level).ToList();
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

        #region[PriceCityDelete]
        public ActionResult PriceCityDelete(int id)
        {
            var del = db.PriceCities.First(p => p.Id == id);

            db.PriceCities.Remove(del);
            db.SaveChanges();

            return RedirectToAction("PriceCityIndex", "PriceCity");

        }
        #endregion

        #region[PriCityCreate]

        // GET: /PriceCity/PriceCityCreate
        [HttpGet]
        public ActionResult PriceCityCreate()
        {
            var priceCities = db.PriceCities.OrderBy(p => p.Level).ToList();
            List<SelectListItem> listPriceCity = new List<SelectListItem>();
            listPriceCity.Clear();
            for (int i = 0; i < priceCities.Count; i++)
            {
                listPriceCity.Add(new SelectListItem { Text = StringClass.ShowNameLevel(priceCities[i].Name, priceCities[i].Level), Value = priceCities[i].Level.ToString() });
            }

            ViewBag.PriceCity = listPriceCity;

            return View();
        }

        // GET: /PriceCity/CreateSub
        [HttpGet]
        public ActionResult CreateSub(string level)
        {
            var priceCities = db.PriceCities.OrderBy(p => p.Level).ToList();

            foreach (var item in priceCities)
            {
                item.Name = StringClass.ShowNameLevel(item.Name, item.Level);
            }

            ViewBag.PriceCity = new SelectList(priceCities, "Level", "Name", level);

            return View();
        }

        // POST: /PriceCity/PriceCityCreate
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PriceCityCreate(FormCollection collection, PriceCity pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"];
                pc.Ord = Int32.Parse(collection["Ord"]);
                pc.Price_ship = float.Parse(collection["Price_ship"]);

                string le = collection["PriceCity"];
                if (le.Length > 0)
                {
                    pc.Level = le + "00000";
                }
                else
                {
                    pc.Level = "00000";
                }
                db.PriceCities.Add(pc);
                db.SaveChanges();
                return RedirectToAction("PriceCityIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[PriceCityEdit]

        [HttpGet]
        public ActionResult PriceCityEdit(int id)
        {
            var priceCity = db.PriceCities.Find(id);

            var priceCities = db.PriceCities.OrderBy(p => p.Level).ToList();

            foreach (var item in priceCities)
            {
                item.Name = StringClass.ShowNameLevel(item.Name, item.Level);
            }

            if (priceCity.Level.Length > 5)
            {
                string parentLevel = priceCity.Level.Substring(0, priceCity.Level.Length - 5);
                ViewBag.PriceCity = new SelectList(priceCities, "Level", "Name", parentLevel);
            }
            else
            {
                ViewBag.PriceCity = new SelectList(priceCities, "Level", "Name");
            }

            return View(priceCity);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PriceCityEdit(FormCollection collection, PriceCity pc)
        {
            if (Request.Cookies["Username"] != null)
            {

                pc.Name = collection["Name"].Replace(".","");
                pc.Ord = Int32.Parse(collection["Ord"]);
                pc.Price_ship = float.Parse(collection["Price_ship"]);
                string parentLevel = collection["PriceCity"];
                pc.Level = parentLevel + "00000";

                db.sp_PriceCity_Update(pc.Id, pc.Name, pc.Level, pc.Ord, pc.Price_ship);
                db.SaveChanges();

                return RedirectToAction("PriceCityIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion

        #region[Ajax Change PriceCity]

        // AJAX: /PriceCity/ChangePriceCity
        [HttpPost]
        public ActionResult ChangePriceCity(int id, string ord, string priceship)
        {
            var results = "";
            var pricecity = db.PriceCities.Find(id);
            if (pricecity != null)
            {
                if (ord != null)
                {
                    pricecity.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }

                if (priceship != null)
                {
                    pricecity.Price_ship = float.Parse(priceship);
                    results = "Cước vận chuyển đã được thay đổi.";
                }
            }
            db.Entry(pricecity).State = EntityState.Modified;
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
                                var Del = (from emp in db.PriceCities where emp.Id == id select emp).SingleOrDefault();
                                db.PriceCities.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("PriceCityIndex");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["PriceCityName"].Length > 0)
                    {
                        Session["PriceCityName"] = collection["PriceCityName"];
                    }
                    return RedirectToAction("PriceCityIndex");
                }
                else
                {
                    return RedirectToAction("PriceCityIndex");
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Autocomplete PriceCity Name]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var priceCityNames = from p in db.PriceCities
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
