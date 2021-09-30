using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Objects;
using System.Data;


namespace MODEOUTLED.Controllers
{
    public class UserModuleController : Controller
    {

         wwwEntities db = new wwwEntities();

        #region[ModuleIndex]
        public ActionResult UserModuleIndex(string sortOrder, string sortName, int? page, string currentFilter, string searchString, string currentSllSupport)
        {

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
                if (Session["Support"] != null)
                {
                    currentSllSupport = Session["Support"].ToString();
                }
            }
            else
            {
                page = 1;
            }


            ViewBag.CurrentFilter = searchString;
            ViewBag.NumberSupport = currentSllSupport;


            #region Order
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";
            ViewBag.CurrentSortName = sortName;
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

            var all = db.UserModules.OrderByDescending(module => module.Order).ToList();

            switch (sortOrder)
            {
                case "ord desc":
                    all = all.OrderByDescending(p => p.Order).ToList();
                    break;
                case "ord asc":
                    all = all.OrderBy(p => p.Order).ToList();
                    break;
                default:
                    break;
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
            #endregion Order

            int pageSize;
            if (currentSllSupport != null)
                pageSize = Convert.ToInt32(currentSllSupport);
            else
                pageSize = 4;
            List<SelectListItem> sllNumberSupport = new List<SelectListItem>();
            //sllNumberSupport.Add(new SelectListItem(){Text="Số lượng hỗ trợ viên", Value="10", Selected=true});
            for (int i = 2; i < 9; i += 2)
            {
                sllNumberSupport.Add(new SelectListItem() { Text = i + " hỗ trợ viên", Value = i + "" });
            }

            ViewBag.sllSupport = sllNumberSupport;


            int pageNumber = (page ?? 1);

            // begin [get last page]
            if (page != null)
                ViewBag.mPage = (int)page;
            else
                ViewBag.mPage = 1;


            int lastPage = all.Count / pageSize;
            if (all.Count % pageSize > 0)
            {
                lastPage++;
            }
            ViewBag.LastPage = lastPage;

            ViewBag.PageSize = pageSize;
            //end [get last page]

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

        #region[UserModuleCreate]

        [HttpGet]
        public ActionResult UserModuleCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UserModuleCreate(FormCollection collection, UserModule userModule)
        {
            if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];
                var Ord = collection["Order"];
                var description = collection["Description"];
                var Active = collection["Active"];

                userModule.Name = Name;
                userModule.Order = Convert.ToInt32(Ord);
                userModule.Description = description;
                userModule.Active = (collection["Active"] == "false") ? false : true;
                        
                db.UserModules.Add(userModule);
                db.SaveChanges();

                

                return RedirectToAction("UserModuleIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion UserModuleCreate

        #region Edit
        [HttpGet]
        public ActionResult UserModuleEdit(int id)
        {
            var userModuleEdit = db.UserModules.SingleOrDefault(m => m.ID == id);
            return View(userModuleEdit);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UserModuleEdit(FormCollection collection, UserModule userModule)
        {
            if (Request.Cookies["Username"] != null)
            {
                userModule.Name = collection["Name"];
                userModule.Order = Convert.ToInt32(collection["Order"]);
                userModule.Description = collection["Description"];
                userModule.Active = (collection["Active"] == "false") ? false : true;

                db.Entry(userModule).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("UserModuleIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion Edit

        #region DeleteItem
        public ActionResult UserModuleDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var moduleDelete = db.UserModules.Where(m => m.ID == id).SingleOrDefault();

                db.UserModules.Remove(moduleDelete);
                db.SaveChanges();

                List<UserModule> uModule = db.UserModules.ToList();

                if ((uModule.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("UserModuleIndex");
                    }
                }
                return RedirectToAction("UserModuleIndex", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion DeleteItem

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect, string sllSupport)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (collect["btnDeleteAll"] != null)
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
                                var Del = db.UserModules.Where(sp => sp.ID == id).SingleOrDefault();
                                db.UserModules.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));

                            var Up = db.UserModules.Where(sp => sp.ID == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Order = int.Parse(collect["Ord" + id]);
                                }
                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }                    
                }
                return RedirectToAction("UserModuleIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[UserModuleActive]
        public ActionResult UserModuleActive(int id)
        {
            var userModule = db.UserModules.Find(id);
            if (userModule != null)
                userModule.Active = userModule.Active == true ? false : true;

            db.Entry(userModule).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            var result = "Trạng thái kích hoạt đã được thay đổi.";// userModule.Active;
            return Json(result);
        }
        #endregion

        #region[UpdateDirect]
        public ActionResult UpdateDirect(int id, int order)
        {
            var userModule = db.UserModules.Find(id);
            if (userModule != null)                
                userModule.Order = order;

            db.Entry(userModule).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            var result = "Thứ tự đã được thay đổi.";// userModule.Active;
            return Json(result);
        }
        #endregion

    }
}
