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
namespace MODEOUTLED.Controllers
{
    public class GroupProductController : Controller
    {
         wwwEntities db = new wwwEntities();

        #region[GroupProductIndexot]
        public ActionResult GroupProductIndexot(int? page)
        {

            var all = db.GroupProducts.OrderBy(g => g.Level).ToList();

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

            PagedList<GroupProduct> groupproducts = (PagedList<GroupProduct>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialGroupProductIndex", groupproducts);

            return View(groupproducts);

        }
        #endregion

        #region[GroupProductIndex]
        public ActionResult GroupProductIndex()
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
            var all = db.GroupProducts.ToList();
            var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
            //var pages = db.sp_GroupProduct_Phantrang(page, productize, "", "[Level] asc").ToList();
            var url = Request.Path;
            numOfNews = all.Count;
            if (numOfNews > 0)
            {
                ViewBag.Pager = onsoft.Models.Phantrang.PhanTrang(pagesize, curpage, numOfNews, url);
            }
            else
            {
                ViewBag.Pager = "";
            }
            return View(pages);
        }
        #endregion

        #region[GroupProductCreateot]
        public ActionResult GroupProductCreateot()
        {
            var groupProducts = db.GroupProducts.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
            if (groupProducts.Count == 0)
            {
                groupProducts.Add(new GroupProduct { Level = "", Name = "" });
            }

            foreach (var item in groupProducts)
            {
                item.Name = onsoft.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }

            for (int i = 0; i < groupProducts.Count; i++)
            {
                ViewBag.GroupProduct = new SelectList(groupProducts, "Id", "Name");
            }

            return View();
        }
        #endregion

        #region[GroupProductCreate]
        public ActionResult GroupProductCreate()
        {
            return View();
        }
        #endregion

        #region[GroupProductCreateot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupProductCreateot(FormCollection collection, GroupProduct catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupProduct"] != "0")
                {
                    if (collection["GroupProduct"] != "")
                    {
                        parentId = Int32.Parse(collection["GroupProduct"]);
                        string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                        level = parentLevel + "00000";
                    }

                }
                catego.Level = level;
                catego.ParentId = parentId;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.Icon = collection["Icon"];
                catego.Images = collection["Images"];
                catego.BaoHanh = collection["Baohanh"];
                catego.Detail = collection["Detail"];

                db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("GroupProductIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupProductEdit]
        public ActionResult GroupProductEdit(int id)
        {
            var Edit = db.GroupProducts.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion

        #region[GroupProductEditot]
        public ActionResult GroupProductEditot(int id)
        {
            var groupproduct = db.GroupProducts.Find(id);

            var groupProducts = db.GroupProducts.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();

            foreach (var item in groupProducts)
            {
                item.Name = onsoft.Models.StringClass.ShowNameLevel(item.Name, item.Level);

            }

            for (int i = 0; i < groupProducts.Count; i++)
            {
                ViewBag.GroupProduct = new SelectList(groupProducts, "Id", "Name", groupproduct.ParentId);
            }

            return View(groupproduct);
        }
        #endregion

        #region[GroupProductEditot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupProductEditot(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var catego = db.GroupProducts.Find(id);
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupProduct"] != "")
                {
                    parentId = Int32.Parse(collection["GroupProduct"]);
                    string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    level = parentLevel + "00000";
                }
                catego.Level = level;
                catego.ParentId = parentId;
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
                catego.Icon = collection["Icon"];
                catego.Images = collection["Images"];
                catego.BaoHanh = collection["Baohanh"];
                catego.Detail = collection["Detail"];

                db.Entry(catego).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GroupProductIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupProductDelete]
        public ActionResult GroupProductDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from categaa in db.GroupProducts where categaa.Id == id select categaa).Single();
                db.GroupProducts.Remove(del);
                db.SaveChanges();

                List<GroupProduct> groupProducts = db.GroupProducts.ToList();

                if ((groupProducts.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("GroupProductIndexot");
                    }
                }

                return RedirectToAction("GroupProductIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupProductActive]
        public ActionResult GroupProductActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var act = (from catego in db.GroupProducts where catego.Id == id select catego).Single();
                if (act.Active == true)
                {
                    act.Active = false;
                }
                else { act.Active = true; }
                db.SaveChanges();
                return RedirectToAction("GroupProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupProductAddSubot]
        public ActionResult GroupProductAddSubot(int id)
        {
            var groupProducts = db.GroupProducts.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
            foreach (var item in groupProducts)
            {
                item.Name = onsoft.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }
            for (int i = 0; i < groupProducts.Count; i++)
            {

                ViewBag.GroupProduct = new SelectList(groupProducts, "Id", "Name", id);
            }
            return View();
        }
        #endregion

        #region[GroupProductAddSubot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupProductAddSubot(FormCollection collection, GroupProduct catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupProduct"] != "")
                {
                    parentId = Int32.Parse(collection["GroupProduct"]);
                    string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    level = parentLevel + "00000";
                }
                catego.Level = level;
                catego.ParentId = parentId;
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
                return RedirectToAction("GroupProductIndexot");
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
                            var Del = (from emp in db.GroupProducts where emp.Id == id select emp).SingleOrDefault();
                            db.GroupProducts.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("GroupProductIndex");
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

            List<GroupProduct> groupProducts = db.GroupProducts.ToList();
            int lastpage = groupProducts.Count / pagesize;
            if (groupProducts.Count % pagesize > 0)
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
                                var Del = (from del in db.GroupProducts where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.GroupProducts.Remove(Del);
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
                            return RedirectToAction("GroupProductIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("GroupProductIndexot", new { page = m });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.GroupProducts.Where(e => e.Id == id).FirstOrDefault();

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
                    return RedirectToAction("GroupProductIndexot", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[Ajax Change GroupProduct]

        // AJAX: /GroupProduct/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var groupproduct = db.GroupProducts.Find(id);
            if (groupproduct != null)
            {
                groupproduct.Active = groupproduct.Active == true ? false : true;
            }
            db.Entry(groupproduct).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }

        // AJAX: /GroupProduct/ChangeGroupProduct
        [HttpPost]
        public ActionResult ChangeGroupProduct(int id, string ord, string name)
        {
            var results = "";
            var groupproduct = db.GroupProducts.Find(id);
            if (groupproduct != null)
            {
                if (ord != null)
                {
                    groupproduct.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                else if(name != null)
                {
                    groupproduct.Name = name;
                    results = "Tên nhóm sản phẩm đã được thay đổi.";
                }
            }
            db.Entry(groupproduct).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        #endregion
    }
}
