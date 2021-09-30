using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;
using onsoft.Models;

namespace MODEOUTLED.Controllers.Admins.Color
{
    public class ColorController : Controller
    {
        wwwEntities db = new wwwEntities();
        #region[PriceCityIndex]
        public ActionResult ColorIndex(int? page, string currentColorName)
        {
            if (Request.HttpMethod == "GET")
            {
                if (Session["ColorName"] != null)
                {
                    currentColorName = Session["ColorName"].ToString();
                    Session["ColorName"] = null;
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentColorName = currentColorName;

            var all = db.sp_Color_GetByAll().OrderBy(p => p.Ord).ToList();

            if (!String.IsNullOrEmpty(currentColorName))
            {
                all = all.Where(p => p.Name.ToUpper().Contains(currentColorName.ToUpper())).OrderBy(p => p.Ord).ToList();
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

        #region[ColorDelete]
        public ActionResult ColorDelete(int id)
        {
            var del = db.Colors.First(p => p.Id == id);

            db.Colors.Remove(del);
            db.SaveChanges();

            return RedirectToAction("ColorIndex", "Color");

        }
        #endregion

        #region[ColorCreate]

        // GET: /PriceCity/ColorCreate
        [HttpGet]
        public ActionResult ColorCreate()
        {
            return View();
        }

        // POST: /PriceCity/ ColorCreate
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ColorCreate(FormCollection collection, onsoft.Models.Color pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"];
                pc.Description = "";
                pc.Ord = Int32.Parse(collection["Ord"]);
                pc.Active = true;
                pc.Image = collection["Image"];
                pc.Default = (collection["Default"] == "false") ? false : true;
                db.Colors.Add(pc);
                db.SaveChanges();
                return RedirectToAction("ColorIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Edit]

        [HttpGet]
        public ActionResult ColorEdit(int id)
        {
            var vColor = db.Colors.Find(id);
            return View(vColor);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ColorEdit(FormCollection collection, onsoft.Models.Color pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"];
                pc.Description = "";
                pc.Ord = Int32.Parse(collection["Ord"]);
                pc.Image = collection["Image"];
                pc.Active = true;
                pc.Default = (collection["Default"] == "false") ? false : true;
                db.sp_Color_Update(pc.Id, pc.Name, pc.Image, pc.Description, pc.Ord, pc.Active, pc.Default);
                db.SaveChanges();
                return RedirectToAction("ColorIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion

        #region[Ajax Change Color]

        // AJAX: /PriceCity/ChangeColor
        [HttpPost]
        public ActionResult ChangeColor(int id, string ord, string name)
        {
            var results = "";
            var vColor = db.Colors.Find(id);
            if (vColor != null)
            {
                if (ord != null)
                {
                    vColor.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                if (name != null)
                {
                    vColor.Name = name;
                    results = "Tên màu sắc đã được thay đổi.";
                }
            }
            db.Entry(vColor).State = EntityState.Modified;
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
                                var Del = (from emp in db.Colors where emp.Id == id select emp).SingleOrDefault();
                                db.Colors.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("ColorIndex");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["ColorName"].Length > 0)
                    {
                        Session["ColorName"] = collection["ColorName"];
                    }
                    return RedirectToAction("ColorIndex");
                }
                else
                {
                    return RedirectToAction("ColorIndex");
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Autocomplete Color Name]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var ColorName = from p in db.Colors
                           select p.Name;
            string[] items = ColorName.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
