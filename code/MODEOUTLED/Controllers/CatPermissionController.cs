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
    public class CatPermissionController : Controller
    {
         wwwEntities db = new wwwEntities();

        #region[CatPermissionIndex]
        public ActionResult CatPermissionIndex(int? page)
        {

            var users = db.Users.Where(u => u.Active == true).ToList();

            //Dropdown List tìm kiếm theo user
            for (int i = 0; i <= users.Count; i++)
            {
                if ((Session["Users"] != null))
                {
                    if ((Int32.Parse(Session["Users"].ToString()) == i))
                    {
                        ViewBag.Users = new SelectList(users, "ID", "FullName", selectedValue: i);
                    }
                }
                else
                {
                    ViewBag.Users = new SelectList(users, "ID", "FullName");
                }
            }

            var all = db.v_Permission_GroupProduct.ToList();

            // Tìm theo userid
            if (Session["Users"] != null)
            {
                int userIdParam = Int32.Parse(Session["Users"].ToString());
                var alls = all.Where(pg => pg.UserID == userIdParam).OrderByDescending(pg => pg.ID).ToList();
                if (alls.Count != 0)
                {
                    all = alls;
                }
                
            }

            if (Session["Users"] != null) { Session["Users"] = null; }

            int pageSize = 10;
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

        #region[CatPermissionCreate]
        public ActionResult CatPermissionCreate()
        {
            return View();
        }
        #endregion

        #region[CatPermissionCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CatPermissionCreate(FormCollection collection, CatPermission catpermission)
        {
            if (Request.Cookies["Username"] != null)
            {
                catpermission.UserID = Int32.Parse(collection["UserID"]);
                catpermission.CatID = Int32.Parse(collection["CatID"]);
                catpermission.View = (collection["View"] == "false") ? false : true;
                catpermission.Full = (collection["Full"] == "false") ? false : true;

                db.CatPermissions.Add(catpermission);
                db.SaveChanges();
                return RedirectToAction("CatPermissionIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[CatPermissionEdit]
        public ActionResult CatPermissionEdit(int id)
        {
            var Edit = db.CatPermissions.First(c => c.ID == id);
            return View(Edit);
        }
        #endregion

        #region[CatPermissionEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CatPermissionEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var catpermission = db.CatPermissions.First(model => model.ID == id);

                catpermission.UserID = Int32.Parse(collection["UserID"]);
                catpermission.CatID = Int32.Parse(collection["CatID"]);
                catpermission.View = (collection["View"] == "false") ? false : true;
                catpermission.Full = (collection["Full"] == "false") ? false : true;

                db.SaveChanges();
                return RedirectToAction("CatPermissionIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[CatPermissionDelete]
        public ActionResult CatPermissionDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from catpermission in db.CatPermissions where catpermission.ID == id select catpermission).Single();
                db.CatPermissions.Remove(del);
                db.SaveChanges();

                List<CatPermission> catpermissions = db.CatPermissions.ToList();

                if ((catpermissions.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("CatPermissionIndex");
                    }
                }

                return RedirectToAction("CatPermissionIndex", new { page = page });
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
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from catpermission in db.CatPermissions where catpermission.ID == id select catpermission).SingleOrDefault();
                            db.CatPermissions.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("CatPermissionIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
        {
            int m = int.Parse(collect["mPage"]);
            int pagesize = int.Parse(collect["PageSize"]);

            List<CatPermission> catpermissions = db.CatPermissions.ToList();
            int lastpage = catpermissions.Count / pagesize;
            if (catpermissions.Count % pagesize > 0)
            {
                lastpage++;
            }
            //int lastPage = int.Parse(collect["LastPage"]);

            if (Request.Cookies["Username"] != null)
            {

                if (collect["btnDelete"] != null)
                {
                    //string str = "";
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.Contains("chk"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int id = Convert.ToInt32(key.Remove(0, 3));
                                var Del = (from del in db.CatPermissions where del.ID == id select del).SingleOrDefault();
                                db.CatPermissions.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("CatPermissionIndex");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("CatPermissionIndex", new { page = m });
                }
                else if (collect["Users"] != null)
                {
                    if (collect["Users"].Length > 0)
                    {
                        Session["Users"] = collect["Users"];
                    }
                    return RedirectToAction("CatPermissionIndex");
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("View"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.CatPermissions.Where(e => e.ID == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["ID" + id].Equals(""))
                                {
                                    Up.ID = Int32.Parse(collect["ID" + id]);
                                }

                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("CatPermissionIndex", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[AJAX]
        // AJAX: /CatPermission/ChangeView
        [HttpPost]
        public ActionResult ChangeView(int id)
        {
            var CatPermission = db.CatPermissions.Find(id);
            if (CatPermission != null)
            {
                CatPermission.View = CatPermission.View == true ? false : true;
            }
            db.Entry(CatPermission).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái đã được thay đổi.";


            return Json(results);
        }

        // AJAX: /CatPermission/ChangeFull
        [HttpPost]
        public ActionResult ChangeFull(int id)
        {
            var CatPermission = db.CatPermissions.Find(id);
            if (CatPermission != null)
            {
                CatPermission.Full = CatPermission.Full == true ? false : true;
            }
            db.Entry(CatPermission).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái đã được thay đổi.";


            return Json(results);
        }
        #endregion
    }
}
