using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;
using System.Collections;
using System.Data;

namespace MODEOUTLED.Controllers.Admins.GroupNews
{
    public class GroupNewController : Controller
    {
         wwwEntities db = new wwwEntities();
        //organogoldvn.com
        #region[GroupNewIndexot]
        public ActionResult GroupNewIndexot(int? page)
        {

            var all = db.GroupNews.OrderBy(g => g.Level).ToList();

            int pageSize = 15;
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

            PagedList<GroupNew> GroupNews = (PagedList<GroupNew>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialGroupNewIndex", GroupNews);

            return View(GroupNews);

        }
        #endregion
        #region[GroupNewCreateot]
        public ActionResult GroupNewCreateot()
        {
            var cat = db.GroupProducts.Where(n => n.Active == true).OrderByDescending(n => n.Level).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
            if (GroupNews.Count == 0)
            {
                GroupNews.Add(new GroupNew { Level = "", Name = "" });
            }

            foreach (var item in GroupNews)
            {
                item.Name = onsoft.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }

            for (int i = 0; i < GroupNews.Count; i++)
            {
                ViewBag.GroupNew = new SelectList(GroupNews, "Id", "Name");
            }

            return View();
        }
        #endregion
        #region[GroupNewCreateot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupNewCreateot(FormCollection collection, GroupNew catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupNew"] != "0")
                {
                    if (collection["GroupNew"] != "")
                    {
                        parentId = Int32.Parse(collection["GroupNew"]);
                        string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                        level = parentLevel + "00000";
                    }

                }
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;

                string groid = collection["Cat"];
                if (groid != null && groid != "")
                {
                    catego.GrpID = int.Parse(groid);
                }
                else
                {
                    catego.GrpID = 0;
                }
                db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("GroupNewIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupNewEdit]
        public ActionResult GroupNewEdit(int id)
        {
            var Edit = db.GroupNews.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion

        #region[GroupNewEditot]
        public ActionResult GroupNewEditot(int id)
        {
            var GroupNew = db.GroupNews.Find(id);

            var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();

            foreach (var item in GroupNews)
            {
                item.Name = onsoft.Models.StringClass.ShowNameLevel(item.Name, item.Level);

            }

            for (int i = 0; i < GroupNews.Count; i++)
            {
                ViewBag.GroupNew = new SelectList(GroupNews, "Id", "Name", GroupNew.Level);
            }
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();
            ViewBag.Cat = new SelectList(cat, "Id", "Name", GroupNew.GrpID);
            return View(GroupNew);
        }
        #endregion

        #region[GroupNewEditot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupNewEditot(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var catego = db.GroupNews.Find(id);
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupNew"] != "")
                {
                    parentId = Int32.Parse(collection["GroupNew"]);
                    string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    level = parentLevel + "00000";
                }
                catego.Level = level;
                string name = collection["Name"];
                if (catego.Level.Length > 5)
                {
                    catego.Name = name.Substring(catego.Level.Length - 5, name.Length - (catego.Level.Length - 5));
                }
                else
                {
                    catego.Name = name;
                }
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                string groid = collection["Cat"];
                if (groid != null && groid != "")
                {
                    catego.GrpID = int.Parse(groid);
                }
                else
                {
                    catego.GrpID = 0;
                }

                db.Entry(catego).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GroupNewIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupNewDelete]
        public ActionResult GroupNewDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from categaa in db.GroupNews where categaa.Id == id select categaa).Single();
                db.GroupNews.Remove(del);
                db.SaveChanges();

                List<GroupNew> GroupNews = db.GroupNews.ToList();

                if ((GroupNews.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("GroupNewIndexot");
                    }
                }

                return RedirectToAction("GroupNewIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupNewActive]
        public ActionResult GroupNewActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from catego in db.GroupNews where catego.Id == id select catego).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return RedirectToAction("GroupNewIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupNewAddSubot]
        public ActionResult GroupNewAddSubot(int id)
        {
            var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
            foreach (var item in GroupNews)
            {
                item.Name = onsoft.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }
            for (int i = 0; i < GroupNews.Count; i++)
            {

                ViewBag.GroupNew = new SelectList(GroupNews, "Id", "Name", id);
            }
            return View();
        }
        #endregion

        #region[GroupNewAddSubot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupNewAddSubot(FormCollection collection, GroupNew catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupNew"] != "")
                {
                    parentId = Int32.Parse(collection["GroupNew"]);
                    string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    level = parentLevel + "00000";
                }
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;


                db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("GroupNewIndexot");
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
                            var Del = (from emp in db.GroupNews where emp.Id == id select emp).SingleOrDefault();
                            db.GroupNews.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("GroupNewIndex");
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

            List<GroupNew> GroupNews = db.GroupNews.ToList();
            int lastpage = GroupNews.Count / pagesize;
            if (GroupNews.Count % pagesize > 0)
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
                                var Del = (from del in db.GroupNews where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.GroupNews.Remove(Del);
                                db.SaveChanges();
                                //}
                                //else
                                //{
                                //    str += Del.Name + ",";
                                //    Session["DeletePro"] = "Sản phẩm " + str + "  vẫn còn trong kho! Không được xóa!";
                                //}
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("GroupNewIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("GroupNewIndexot", new { page = m });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.GroupNews.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }

                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("GroupNewIndexot", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Ajax Change GroupNew]

        // AJAX: /GroupNew/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var GroupNew = db.GroupNews.Find(id);
            if (GroupNew != null)
            {
                GroupNew.Active = GroupNew.Active == true ? false : true;
            }
            db.Entry(GroupNew).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }

        // AJAX: /GroupNew/ChangeGroupNew
        [HttpPost]
        public ActionResult ChangeGroupNew(int id, string ord, string name)
        {
            var results = "";
            var GroupNew = db.GroupNews.Find(id);
            if (GroupNew != null)
            {
                if (ord != null)
                {
                    GroupNew.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                else if (name != null)
                {
                    GroupNew.Name = name;
                    results = "Tên nhóm tin đã được thay đổi.";
                }
            }
            db.Entry(GroupNew).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        #endregion

    }
}
