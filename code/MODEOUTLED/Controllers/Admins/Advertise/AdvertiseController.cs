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
using System.IO;


namespace MODEOUTLED.Controllers.Admins.Advertise
{
    public class AdvertiseController : Controller
    {
        wwwEntities db = new wwwEntities();

        #region[AdvertiseIndex]
        public ActionResult AdvertiseIndex(string sortOrder, string sortName, int? page, string posstionSearch, string sortPos, string getSortNameClass, string getSortOrderClass)
        {
            var all = db.Advertises.OrderBy(a => a.Position).ToList();

            #region Controls SelectListItem
            List<SelectListItem> sllTar = new List<SelectListItem>();
            List<SelectListItem> sllPos = new List<SelectListItem>();
            sllTar = CreateTarget("_blank", "_seft", "_parient", "_top");
            sllPos = CreateSSL("Logo trang","Quảng cáo Popup", "Quảng cáo Slide", "Quảng cáo bên trái");
            ViewBag.sllTarget = sllTar;
            ViewBag.sllPossition = sllPos;
            #endregion SelectListItem

            int pageSize = 15;

            int pageNumber = (page ?? 1);

            #region GetLastPage
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
            #endregion GetLastPage

            #region Order
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ordAsc" ? "ordDesc" : "ordAsc";
            ViewBag.CurrentSortName = sortName;
            ViewBag.SortNameParm = sortName == "nameAsc" ? "nameDesc" : "nameAsc";
            
            switch (sortOrder)
            {
                case "ordDesc":
                    all = db.Advertises.OrderByDescending(a => a.Orders).ToList();
                    break;
                case "ordAsc":
                    all = db.Advertises.OrderBy(a => a.Orders).ToList();
                    break;
                default:
                    break;
            }

            switch (sortName)
            {
                case "nameDesc":
                    all = db.Advertises.OrderByDescending(a => a.Name).ToList();
                    break;
                case "nameAsc":
                    all = db.Advertises.OrderBy(a => a.Name).ToList();
                    break;
                default:
                    break;
            }
            #endregion Order

            PagedList<onsoft.Models.Advertise> adv = (PagedList<onsoft.Models.Advertise>)all.ToPagedList(pageNumber, pageSize);

            /// Kiem tra co tim kiem theo vi tri va phan trang theo vi tri khong
            if ((String.IsNullOrEmpty(posstionSearch) || posstionSearch == "") && (sortPos == null))
            {
                if (Request.IsAjaxRequest())
                    return PartialView("_AdvertiseData", adv);
                else
                    return View(adv);
            }
            else /// Truong hop co tim kiem theo vi tri
            {
                int pos;
                /// Kiem tra xem co phan trang khong
                if (sortPos != null) /// Phan trang
                {
                    ViewBag.currentPos = sortPos;                    
                    pos = Convert.ToInt32(sortPos);
                    if (sortName == "nameAsc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderBy(a => a.Name).ToList();
                    else if (sortName == "nameDesc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderByDescending(a => a.Name).ToList();
                    else if (sortOrder == "ordDesc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderByDescending(a => a.Orders).ToList();
                    else if (sortOrder == "ordAsc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderBy(a => a.Orders).ToList();
                    else
                        all = db.Advertises.Where(a => a.Position == pos).OrderByDescending(a => a.Id).ToList();
                }
                else /// ko phan trang
                {
                    ViewBag.currentPos = posstionSearch;
                    pos = Convert.ToInt32(posstionSearch);
                    if (sortName == "nameAsc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderBy(a => a.Name).ToList();
                    else if (sortName == "nameDesc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderByDescending(a => a.Name).ToList();
                    else if (sortOrder == "ordDesc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderByDescending(a => a.Orders).ToList();
                    else if (sortOrder == "ordAsc")
                        all = db.Advertises.Where(a => a.Position == pos).OrderBy(a => a.Orders).ToList();
                    else
                        all = db.Advertises.Where(a => a.Position == pos).OrderByDescending(a => a.Id).ToList();
                }

                sllPos[pos].Selected = true;

                adv = (PagedList<onsoft.Models.Advertise>)all.ToPagedList(pageNumber, pageSize);

                if (Request.IsAjaxRequest())
                    return PartialView("_AdvertiseData", adv);
                else
                    return View(adv);
            }





        }
        #endregion

        #region[AdvertiseCreate]

        [HttpGet]
        public ActionResult AdvertiseCreate()
        {
            var cat = db.GroupProducts.Where(n => n.Active == true).OrderByDescending(n => n.Level).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            ViewBag.sllPossition = CreateSSL("Logo trang","Quảng cáo Popup", "Quảng cáo Slide", "Quảng cáo bên trái");
            ViewBag.sllTarget = CreateTarget("_blank", "_seft", "_parient", "_top");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AdvertiseCreate(FormCollection collection, onsoft.Models.Advertise advertise)
        {
            if (Request.Cookies["Username"] != null)
            {
                advertise.Name = collection["Name"]; ;
                advertise.Image = collection["Picture"];
                advertise.Width = Convert.ToInt32(collection["Width"]);
                advertise.Height = Convert.ToInt32(collection["Height"]);
                advertise.Orders = Convert.ToInt32(collection["Order"]);
                advertise.Link = collection["Link"];
                advertise.Click = Convert.ToInt32(collection["Click"]);
                advertise.Active = (collection["Active"] == "false") ? false : true;

                advertise.Position = Convert.ToInt32(collection["sllPossition"]);
                advertise.Description = collection["Description"];
                string groid = collection["Cat"];
                if (groid != null && groid != "")
                {
                    advertise.GrpID = int.Parse(groid);
                }
                else
                {
                    advertise.GrpID = 0;
                }
                if (Convert.ToInt32(collection["sllTarget"]) == 0)
                {
                    advertise.Target = "_blank";
                }
                else if (Convert.ToInt32(collection["sllTarget"]) == 1)
                {
                    advertise.Target = "_self";
                }
                else if (Convert.ToInt32(collection["sllTarget"]) == 2)
                {
                    advertise.Target = "_Parient";
                }
                else if (Convert.ToInt32(collection["sllTarget"]) == 3)
                {
                    advertise.Target = "_top";
                }

                advertise.Click = 0;

                db.Advertises.Add(advertise);
                db.SaveChanges();
                return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion UserModuleCreate

        #region Edit
        [HttpGet]
        public ActionResult AdvertiseEdit(int id)
        {
            var advertiseEdit = db.Advertises.SingleOrDefault(m => m.Id == id);
            ViewBag.sllPossition = CreateSSL("Logo trang","Quảng cáo Popup", "Quảng cáo Slide", "Quảng cáo bên trái");
            ViewBag.sllTarget = CreateTarget("_blank", "_seft", "_parient", "_top");
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();
            ViewBag.Cat = new SelectList(cat, "Id", "Name", advertiseEdit.GrpID);
            return View(advertiseEdit);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AdvertiseEdit(FormCollection collection, onsoft.Models.Advertise advertise)
        {
            if (Request.Cookies["Username"] != null)
            {
                advertise.Name = collection["Name"];
                advertise.Image = collection["Image"];
                advertise.Height = Convert.ToInt32(collection["Height"]);
                advertise.Width = Convert.ToInt32(collection["Width"]);
                advertise.Position = Convert.ToInt32(collection["sllPossition"]);
                advertise.Link = collection["Link"];
                advertise.Target = collection["sllTarget"];
                advertise.Orders = Convert.ToInt32(collection["Order"]);
                advertise.Active = (collection["Active"] == "false") ? false : true;
                advertise.Description = collection["Description"];
                string groid = collection["Cat"];
                if (groid != null && groid != "")
                {
                    advertise.GrpID = int.Parse(groid);
                }
                else
                {
                    advertise.GrpID = 0;
                }
                db.Entry(advertise).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

        #endregion Edit

        #region DeleteItem
        public ActionResult AdvertiseDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var aDelete = db.Advertises.Where(m => m.Id == id).SingleOrDefault();

                db.Advertises.Remove(aDelete);
                db.SaveChanges();

                List<onsoft.Models.Advertise> adv = db.Advertises.ToList();

                if ((adv.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("AdvertiseIndex");
                    }
                }
                return RedirectToAction("AdvertiseIndex", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion DeleteItem

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
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
                                var Del = db.Advertises.Where(sp => sp.Id == id).SingleOrDefault();
                                db.Advertises.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else if (collect["sllPossition"] != null)
                {
                    //string i = collect["sllPossition"];
                    Session["sllPossition"] = collect["sllPossition"];
                    return RedirectToAction("AdvertiseIndex", "Advertise");
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
                return RedirectToAction("AdvertiseIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[AdvertiseActive]
        public ActionResult AdvertiseActive(int id)
        {
            var advertise = db.Advertises.Find(id);
            if (advertise != null)
                advertise.Active = advertise.Active == true ? false : true;

            db.Entry(advertise).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            var result = "Trạng thái kích hoạt đã được thay đổi.";// userModule.Active;
            return Json(result);
        }
        #endregion

        #region[UpdateDirect]
        public ActionResult UpdateDirect(int id, string order, string width, string height, string name, string image)
        {
            var advertise = db.Advertises.Find(id);
            var result = string.Empty;
            if (advertise != null)
            {
                if (order != null)
                {
                    advertise.Orders = Convert.ToInt32(order);
                    result = "Thứ tự đã được thay đổi.";
                }
                else if (width != null)
                {
                    advertise.Width = Convert.ToInt32(width);
                    result = "Độ rộng quảng cáo đã được thay đổi.";
                }
                else if (height != null)
                {
                    advertise.Height = Convert.ToInt32(height);
                    result = "Chiều cao quảng cáo đã được thay đổi.";
                }
                else if (name != null)
                {
                    advertise.Name = name;
                    result = "Tên quảng cáo đã được thay đổi.";
                }
                else if (image != null)
                {
                    advertise.Image = image;
                    result = "Ảnh quảng cáo đã được thay đổi.";
                }
                else
                {
                    advertise.Active = advertise.Active == true ? false : true;
                    result = "Trạng thái quảng cáo đã được thay đổi.";
                }
            }

            db.Entry(advertise).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            //var result = "Dữ liệu đã được thay đổi.";// userModule.Active;
            return Json(result);
        }
        #endregion

        #region OtherMethods

        public List<SelectListItem> CreateSSL(string v0, string v1, string v2, string v3)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v0 });
            sll.Add(new SelectListItem { Value = "1", Text = v1 });
            sll.Add(new SelectListItem { Value = "2", Text = v2 });
            sll.Add(new SelectListItem { Value = "3", Text = v3 });
            return sll;
        }


        public List<SelectListItem> CreateTarget(string v1, string v2, string v3, string v4)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1 });
            sll.Add(new SelectListItem { Value = "1", Text = v2 });
            sll.Add(new SelectListItem { Value = "2", Text = v3 });
            sll.Add(new SelectListItem { Value = "3", Text = v4 });
            return sll;
        }

        

        #region Target
        public List<SelectListItem> Target()
        {
            List<SelectListItem> Target = new List<SelectListItem>();
            Target.Add(new SelectListItem { Value = "0", Text = "_blank" });
            Target.Add(new SelectListItem { Value = "1", Text = "_seft" });
            Target.Add(new SelectListItem { Value = "2", Text = "_parient" });
            Target.Add(new SelectListItem { Value = "3", Text = "_top" });
            return Target;
        }
        #endregion Target


        #region Possition
        public List<SelectListItem> Possition()
        {
            List<SelectListItem> Possition = new List<SelectListItem>();
            Possition.Add(new SelectListItem { Value = "0", Text = "Vị trí logo" });
            Possition.Add(new SelectListItem { Value = "1", Text = "Vị trí Popup" });
            Possition.Add(new SelectListItem { Value = "2", Text = "Quảng cáo Slide" });
            Possition.Add(new SelectListItem { Value = "3", Text = "Quảng cáo bên trái" });
            return Possition;
        }
        #endregion Possition
        #endregion

        #region AddMultileAdvertise

        #region[AddMutilImage]
        public ActionResult AddMultilImage()
        {
            ViewBag.sllPossition = CreateSSL("Logo trang","Quảng cáo Popup", "Quảng cáo Slide", "Quảng cáo bên trái");
            return View();
        }
        #endregion


        #region[AddMultilImage]
        [HttpPost]
        public ActionResult AddMultilImage(IEnumerable<HttpPostedFileBase> fileImg, FormCollection aForm)
        {
            if (Request.Cookies["Username"] != null)
            {
                int ID;
                var tmp = db.Advertises.Select(a => (int?)a.Id).Max();
                //if (tmp == null)
                //    tmp = 1;
                foreach (var file in fileImg)
                {
                    if (tmp == null)
                    { ID = 1; tmp = 1; }
                    else
                        ID = db.Advertises.Select(a => a.Id).Max();

                    if (file.ContentLength > 0)
                    {
                        //var b = (from k in db.ProImages select k.Id).Max();
                        var ab = Request.Files["fileImg"];
                        String FileExtn = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                        {
                            ViewBag.error = "Only jpg, gif and png files are allowed!";
                        }
                        else
                        {
                            onsoft.Models.Advertise aImage = new onsoft.Models.Advertise();
                            var Filename = Path.GetFileName(file.FileName);
                            //List<string> sizeImg = new List<string>();
                            //sizeImg.Add("_huge");
                            //sizeImg.Add("_big");
                            //sizeImg.Add("_noz");
                            //sizeImg.Add("_small");
                            //string co = "";
                            //string kco = "";
                            //for (int i = 0; i < sizeImg.Count; i++)
                            //{
                            //    var a = Filename.LastIndexOf(sizeImg[i]);
                            //    if (a > 0)
                            //    {
                            //        co = Filename.Substring(0, a);
                            //        kco = sizeImg[i];
                            //        break;
                            //    }
                            //}
                            //var fileName = String.Format("{0}" + kco + ".jpg", Guid.NewGuid().ToString());
                            //String imgPath = String.Format("Uploads/{0}{1}", file.FileName, FileExtn);
                            //file.Save(String.Format("{0}{1}", Server.MapPath("~"), imgPath), Img.RawFormat);
                            var path = Path.Combine(Server.MapPath(Url.Content("/Uploads")), Filename);
                            file.SaveAs(path);

                            if (Convert.ToInt32(aForm["sllPossition"]) == 0)
                                aImage.Position = 0;
                            else if (Convert.ToInt32(aForm["sllPossition"]) == 1)
                                aImage.Position = 1;
                            else if (Convert.ToInt32(aForm["sllPossition"]) == 2)
                                aImage.Position = 2;
                            else if (Convert.ToInt32(aForm["sllPossition"]) == 3)
                                aImage.Position = 3;

                            aImage.Id = ID + 1;
                            aImage.Name = "QC thứ " + ID;
                            aImage.Orders = 0;
                            aImage.Click = 0;
                            aImage.Target = "_top";
                            aImage.Width = 0;
                            aImage.Height = 0;
                            //aImage.Position = 0;
                            aImage.Image = "/Uploads/" + Filename;
                            //img.Date = DateTime.Now;
                            db.Advertises.Add(aImage);
                            db.SaveChanges();
                        }
                    }
                    var fd = file;
                }
                return RedirectToAction("AdvertiseIndex");
            }
            else
            { return Redirect("/admins/admins"); }
        }
        #endregion

        #endregion AddMultileAdvertise

        [HttpPost]
        public ActionResult Search(string searchString, int? page)
        {
            if (Request.Cookies["Username"] != null)
            {
                int pageSize = 4;
                int pageNumber = (page ?? 1);

                PagedList<onsoft.Models.Advertise> adv1 = (PagedList<onsoft.Models.Advertise>)db.Advertises.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())).OrderByDescending(a => a.Name).ToPagedList(pageNumber, pageSize);

                if (adv1.Count > 0)
                    return PartialView("_AdvertiseData", adv1);
                else
                    return PartialView("ErrorSearch");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }

    }
}
