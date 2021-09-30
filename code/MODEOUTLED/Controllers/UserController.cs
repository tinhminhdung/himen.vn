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
    public class UserController : Controller
    {
         wwwEntities db = new wwwEntities();

        #region[UserIndex]
        public ActionResult UserIndex(int? page)
        {
            var all = db.Users.OrderByDescending(u => u.ID).ToList();

            int pageSize = 3;
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

        #region[UserCreate]
        public ActionResult UserCreate()
        {
            return View();
        }
        #endregion

        #region[UserCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UserCreate(FormCollection collection, User user)
        {
            if (Request.Cookies["Username"] != null)
            {
                user.FullName = collection["FullName"];
                user.UserName = collection["UserName"];
                user.Password = collection["Password"];
                user.Email = collection["Email"];
                user.Permission = short.Parse(collection["Permission"]);
                user.Active = (collection["Active"] == "false") ? false : true;

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("UserIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[UserEdit]
        public ActionResult UserEdit(int id)
        {
            var Edit = db.Users.First(u => u.ID == id);
            return View(Edit);
        }
        #endregion

        #region[GroupProductEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupProductEdit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var user = db.Users.First(model => model.ID == id);

                user.FullName = collection["FullName"];
                user.UserName = collection["UserName"];
                user.Password = collection["Password"];
                user.Email = collection["Email"];
                user.Permission = short.Parse(collection["Permission"]);
                user.Active = (collection["Active"] == "false") ? false : true;

                db.SaveChanges();
                return RedirectToAction("UserIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[UserDelete]
        public ActionResult UserDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from user in db.Users where user.ID == id select user).Single();
                db.Users.Remove(del);
                db.SaveChanges();

                List<User> users = db.Users.ToList();

                if ((users.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("UserIndex");
                    }
                }

                return RedirectToAction("UserIndex", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[UserActive]
        public ActionResult UserActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from user in db.Users where user.ID == id select user).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return RedirectToAction("UserIndex");
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
                            var Del = (from user in db.Users where user.ID == id select user).SingleOrDefault();
                            db.Users.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("UserIndex");
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

            List<User> users = db.Users.ToList();
            int lastpage = users.Count / pagesize;
            if (users.Count % pagesize > 0)
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
                                var Del = (from del in db.Users where del.ID == id select del).SingleOrDefault();
                                db.Users.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("UserIndex");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("UserIndex", new { page = m });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.Users.Where(e => e.ID == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["FullName" + id].Equals(""))
                                {
                                    Up.FullName = collect["FullName" + id];
                                }
                                if (!collect["UserName" + id].Equals(""))
                                {
                                    Up.UserName = collect["UserName" + id];
                                }
                                if (!collect["Password" + id].Equals(""))
                                {
                                    Up.Password = collect["Password" + id];
                                }
                                if (!collect["Email" + id].Equals(""))
                                {
                                    Up.Email = collect["Email" + id];
                                }
                                if (!collect["Permission" + id].Equals(""))
                                {
                                    Up.Permission = short.Parse(collect["Permission" + id]);
                                }
                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("UserIndex", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Ajax Change User]
        // AJAX: /User/ChangePermission
        [HttpPost]
        public ActionResult ChangePermission(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                user.Permission = user.Permission == 1 ? short.Parse("0") : short.Parse("1");
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Quyền đã được thay đổi.";


            return Json(results);
        }


        // AJAX: /User/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                user.Active = user.Active == true ? false : true;
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";


            return Json(results);
        }

        // AJAX: /User/ChangeUser
        [HttpPost]
        public ActionResult ChangeUser(int id, string fullname, string username, string password, string email)
        {
            var results = "";
            var user = db.Users.Find(id);
            if (user != null)
            {
                if (fullname != null)
                {
                    if (fullname == "")
                    {
                        results = "Phải nhập tên đầy đủ.";
                    }
                    else
                    {
                        user.FullName = fullname;
                        results = "Tên đầy đủ đã được thay đổi.";
                    }

                }


                if (username != null)
                {
                    user.UserName = username;
                    results = "UserName đã được thay đổi.";
                }

                if (password != null)
                {
                    user.Password = password;
                    results = "Password đã được thay đổi.";
                }

                if (email != null)
                {
                    user.Email = email;
                    results = "Email đã được thay đổi.";
                }
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        #endregion
    }
}
