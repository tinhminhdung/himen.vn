using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;
using onsoft.Models;

namespace MODEOUTLED.Controllers.Admins.Size
{
    public class SizeController : Controller
    {
         wwwEntities db = new wwwEntities();
        #region[PriceCityIndex]
        public ActionResult SizeIndex(int? page, string currentSizeName)
        {
            if (Request.HttpMethod == "GET")
            {
                if (Session["SizeName"] != null)
                {
                    currentSizeName = Session["SizeName"].ToString();
                    Session["SizeName"] = null;
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentSizeName = currentSizeName;

            var all = db.sp_Size_GetByAll().OrderBy(p => p.Ord).ToList();

            if (!String.IsNullOrEmpty(currentSizeName))
            {
                all = all.Where(p => p.Name.ToUpper().Contains(currentSizeName.ToUpper())).OrderBy(p => p.Ord).ToList();
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

        #region[SizeDelete]
        public ActionResult SizeDelete(int id)
        {
            var del = db.Sizes.First(p => p.Id == id);

            db.Sizes.Remove(del);
            db.SaveChanges();

            return RedirectToAction("SizeIndex", "Size");

        }
        #endregion

        #region[SizeCreate]

        // GET: /PriceCity/SizeCreate
        [HttpGet]
        public ActionResult SizeCreate()
        {
            return View();
        }

        // POST: /PriceCity/ SizeCreate
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SizeCreate(FormCollection collection, onsoft.Models.Size pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"];
                pc.Description = "";
                pc.Ord = Int32.Parse(collection["Ord"]);
                pc.Active = true;
                pc.Default = (collection["Default"] == "false") ? false : true;
                db.Sizes.Add(pc);
                db.SaveChanges();
                return RedirectToAction("SizeIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Edit]

        [HttpGet]
        public ActionResult SizeEdit(int id)
        {
            var vSize = db.Sizes.Find(id);
            return View(vSize);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SizeEdit(FormCollection collection, onsoft.Models.Size pc)
        {
            if (Request.Cookies["Username"] != null)
            {
                pc.Name = collection["Name"];
                pc.Description = "";
                pc.Ord = Int32.Parse(collection["Ord"]);
                pc.Active = true;
                pc.Default = (collection["Default"] == "false") ? false : true;
                db.sp_Size_Update(pc.Id, pc.Name, pc.Description, pc.Ord, pc.Active, pc.Default);
                db.SaveChanges();
                return RedirectToAction("SizeIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion

        #region[Ajax Change Size]

        // AJAX: /PriceCity/ChangeSize
        [HttpPost]
        public ActionResult ChangeSize(int id, string ord, string name)
        {
            var results = "";
            var vSize = db.Sizes.Find(id);
            if (vSize != null)
            {
                if (ord != null)
                {
                    vSize.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                if (name != null)
                {
                    vSize.Name = name;
                    results = "Tên kích thước đã được thay đổi.";
                }
            }
            db.Entry(vSize).State = EntityState.Modified;
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
                                var Del = (from emp in db.Sizes where emp.Id == id select emp).SingleOrDefault();
                                db.Sizes.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("SizeIndex");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["SizeName"].Length > 0)
                    {
                        Session["SizeName"] = collection["SizeName"];
                    }
                    return RedirectToAction("SizeIndex");
                }
                else
                {
                    return RedirectToAction("SizeIndex");
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Autocomplete Size Name]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var SizeName = from p in db.Sizes
                                 select p.Name;
            string[] items = SizeName.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
