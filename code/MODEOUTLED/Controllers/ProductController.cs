using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using onsoft.Models;
using MODEOUTLED.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using PagedList;
using PagedList.Mvc;


namespace MODEOUTLED.Controllers
{
    public class ProductController : Controller
    {
        #region [ControlAdmin]
         wwwEntities db = new wwwEntities();         
        #region [GroupProduct]
        public ActionResult GroupProduct()
        {
            return View();
        }
        #endregion

        #region [DetailGroupProduct]
        public ActionResult DetailGroupProduct()
        {
            return View();
        }
        #endregion

        #region [DetailProduct]
        public ActionResult DetailProduct()
        {
            return View();
        }
        #endregion
        #region[ProductIndexot]
        public ActionResult ProductIndexot(string sortOrder, string sortName, string sortGroup, string sortPriceRetail, string sortPricePromotion, int? page, string currentProductCodeFilter, string productcode, string currentProductNameFilter, string productname, string currentCatProductFilter, string catproduct, string currentProductAmount)
        {

            if (Request.HttpMethod == "GET")
            {
                productcode = currentProductCodeFilter;
                productname = currentProductNameFilter;
                catproduct = currentCatProductFilter;
                if (Session["ProductAmount"] != null)
                {
                    currentProductAmount = Session["ProductAmount"].ToString();
                    Session["ProductAmount"] = null;
                }
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentProductCodeFilter = productcode;
            ViewBag.CurrentProductNameFilter = productname;
            ViewBag.CurrentCatProductFilter = catproduct;
            ViewBag.CurrentProductAmount = currentProductAmount;


            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

            // Sort Name (Truyền sortName khi phân trang)
            if (sortName != "")
            {
                ViewBag.CurrentSortName = sortName;
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            }
            // Sort Name (Truyền sortName khi sắp xếp)
            if (Session["sortName"] != null)
            {
                sortName = Session["sortName"].ToString();
                ViewBag.CurrentSortName = Session["sortName"].ToString();
                Session["sortName"] = null;
            }
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

            ViewBag.CurrentSortGroup = sortGroup;
            ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";

            ViewBag.CurrentSortPriceRetail = sortPriceRetail;
            ViewBag.SortPriceRetailParm = sortPriceRetail == "priceretail asc" ? "priceretail desc" : "priceretail asc";

            ViewBag.CurrentSortPricePromotion = sortPricePromotion;
            ViewBag.SortPricePromotionParm = sortPricePromotion == "pricepromotion asc" ? "pricepromotion desc" : "pricepromotion asc";

            var all = db.sp_Product_GetByAll_NameGroup().OrderByDescending(p => p.Id).ToList();

            #region[Tìm kiếm]

            // Tìm theo mã sản phẩm
            if (!String.IsNullOrEmpty(productcode))
            {
                if (productcode != "Mã sản phẩm")
                {
                    all = all.Where(p => p.Code.ToUpper().Contains(productcode.ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
            }

            // Tìm theo tên sản phẩm
            if (!String.IsNullOrEmpty(productname))
            {
                if (productname != "Tên sản phẩm")
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(productname.ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
            }

            // Tìm theo nhóm sản phẩm
            if (!String.IsNullOrEmpty(catproduct))
            {
                int catid = Int32.Parse(catproduct);
                all = all.Where(p => p.IdCategory == catid).OrderByDescending(p => p.Id).ToList();
            }

            if (Session["Model"] != null)
            {
                string code = Session["Model"].ToString().ToUpper();
                all = all.Where(p => p.Code.ToUpper().Contains(code)).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["Namesearch"] != null)
            {
                if (Session["Model"] != null)
                {
                    all = all.Where(p => p.Code.ToUpper().Contains(Session["Model"].ToString().ToUpper()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                else
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
            }
            if (Session["Cat"] != null)
            {
                if (Session["Namesearch"] != null)
                {
                    if (Session["Model"] != null)
                    {
                        all = all.Where(p => p.IdCategory == int.Parse(Session["Cat"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper()) && p.Code.ToUpper().Contains(Session["Model"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                    else
                    {
                        all = all.Where(p => p.IdCategory == int.Parse(Session["Cat"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                }
                else
                {
                    if (Session["Model"] != null)
                    {
                        all = all.Where(p => p.IdCategory == int.Parse(Session["Cat"].ToString()) && p.Code.ToUpper().Contains(Session["Model"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                    else
                    {
                        all = all.Where(p => p.IdCategory == int.Parse(Session["Cat"].ToString())).OrderByDescending(p => p.Id).ToList();
                    }
                }
                Session["Cat"] = null;
            }
            if (Session["Model"] != null) { Session["Model"] = null; }
            if (Session["Namesearch"] != null) { Session["Namesearch"] = null; }
            #endregion

            #region[Sắp xếp]
            switch (sortOrder)
            {
                case "ord desc":
                    all = all.OrderByDescending(p => p.Ord).ToList();
                    break;
                case "ord asc":
                    all = all.OrderBy(p => p.Ord).ToList();
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

            switch (sortGroup)
            {
                case "group desc":
                    all = all.OrderByDescending(p => p.Expr1).ToList();
                    break;
                case "group asc":
                    all = all.OrderBy(p => p.Expr1).ToList();
                    break;
                default:
                    break;
            }

            switch (sortPriceRetail)
            {
                case "priceretail desc":
                    all = all.OrderByDescending(p => p.PriceRetail).ToList();
                    break;
                case "priceretail asc":
                    all = all.OrderBy(p => p.PriceRetail).ToList();
                    break;
                default:
                    break;
            }

            switch (sortPricePromotion)
            {
                case "pricepromotion desc":
                    all = all.OrderByDescending(p => p.PricePromotion).ToList();
                    break;
                case "pricepromotion asc":
                    all = all.OrderBy(p => p.PricePromotion).ToList();
                    break;
                default:
                    break;
            }
            #endregion

            if (Session["DeletePro"] != null)
            {
                var a = Session["DeletePro"].ToString();
                ViewBag.DelErr = "<p class='require'>" + a + "</p>";
                Session["DeletePro"] = null;
            }

            int pageSize = 20;
            if (currentProductAmount != null && currentProductAmount != "")
            {
                pageSize = Convert.ToInt32(currentProductAmount);
            }
            int pageNumber = (page ?? 1);

            #region[Get Last Page]
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
            #endregion

            #region[view drop search]
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            #endregion

            #region[Số sản phẩm hiển thị trên trang]
            // Số sản phẩm hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Sản phẩm / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Sản phẩm / trang", Value = i + "" });
                }
            }
            ViewBag.ddlProductAmount = items;
            #endregion

            ViewBag.TotalProduct = db.Products.Count();

            // Phân trang
            PagedList<sp_Product_GetByAll_NameGroup_Result> products = (PagedList<sp_Product_GetByAll_NameGroup_Result>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialProductIndex", products);

            return View(products);
        }
        #endregion

        #region[Search]
        [HttpPost]
        public ActionResult Search(FormCollection collect)
        {
            string url = "";
            var Cat = collect["Cat"];
            var Name = collect["Name"];
            if (Cat != "" && Name == "")
            {
                url = "/Product/ProductIndex?Cat=" + Cat + "";
            }
            else if (Cat == "" && Name != "")
            {
                url = "/Product/ProductIndex?Name=" + Name + "";
            }
            else if (Cat != "" && Name != "")
            {
                url = "/Product/ProductIndex?Cat=" + Cat + "&Name=" + Name + "";
            }
            else
            {
                url = "/Product/ProductIndex";
            }
            return Redirect(url);
        }
        #endregion

        #region[ProductCreateot]
        public ActionResult ProductCreateot()
        {

            string chuoi = "";
            var cat = db.GroupProducts.Where(n => n.Active == true).OrderByDescending(n=>n.Level).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            return View();
        }
        #endregion

        #region[ProductCreate]
        public ActionResult ProductCreate()
        {

            string chuoi = "";
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }

            chuoi += " <tr>";
            chuoi += "    <td>Giá nhập</td>";
            chuoi += "    <td><input type=\"text\" value=\"0\" id=\"PriceImport\" name=\"PriceImport\" /></td>";
            chuoi += " </tr>";
            chuoi += " <tr>";
            chuoi += "    <td>Giá bán lẻ</td>";
            chuoi += "    <td><input type=\"text\" value=\"0\" id=\"PriceExport_L\" name=\"PriceExport_L\" /></td>";
            chuoi += " </tr>";
            chuoi += " <tr>";
            chuoi += "     <td>Giá bán buôn</td>";
            chuoi += "   <td><input type=\"text\" value=\"0\" id=\"PriceExport_S\" name=\"PriceExport_S\" /></td>";
            chuoi += " </tr>";
            chuoi += "<tr>";
            chuoi += "    <td>Giá khuyến mãi</td>";
            chuoi += "  <td><input type=\"text\" value=\"0\" id=\"PricePromotion\" name=\"PricePromotion\" /></td>";
            chuoi += " </tr>";
            chuoi += " <tr>";
            chuoi += "   <td>Ngày bắt đầu</td>";
            chuoi += "   <td><input type=\"text\" id=\"SDate\" name=\"SDate\" /></td>";
            chuoi += " </tr>";
            chuoi += " <tr>";
            chuoi += "    <td>Ngày kết thúc</td>";
            chuoi += "   <td><input type=\"text\" id=\"EDate\" name=\"EDate\" /> </td>";
            chuoi += " </tr>";
            ViewBag.Gia = chuoi;
            return View();
        }
        #endregion

        #region[ProductCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductCreate(FormCollection collection, Product pro, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                pro.Name = collection["Name"];
                pro.Tag = StringClass.NameToTag(collection["Name"]);
                pro.Image = collection["Image"];
                pro.Content = collection["Content"];
                pro.Baohanh = collection["Baohanh"];
                pro.Noibat = collection["Noibat"];
                pro.Giamgia = collection["Giamgia"];
                pro.Code = collection["Code"];
                if (pro.Code == null || pro.Code == "")
                {
                    pro.Code = "PDM" + DateTime.Now.Millisecond.ToString();
                }
                string sPriceRetail = collection["PriceRetail"].ToString();
                string sPricePromotion = collection["PricePromotion"].ToString();
                pro.PriceRetail = Convert.ToDouble(StringClass.NumberStr(sPriceRetail));
                pro.PricePromotion = Convert.ToDouble(StringClass.NumberStr(sPricePromotion));
                pro.Detail = collection["Detail"];
                pro.Date = Convert.ToDateTime(DateTime.Now.ToString());
                int cat = Convert.ToInt32(collection["Cat"]);
                //var grp = db.GroupProducts.Where(m => m.Id == cat).FirstOrDefault();
                //if (grp.Level.Length > 5)
                //{
                //    var grp2 = db.GroupProducts.Where(m => m.Level == grp.Level.Substring(0, 5)).FirstOrDefault();
                //    pro.IdCategory = Convert.ToInt32(grp2.Id);
                //}
                //else
                //{
                //    pro.IdCategory = Convert.ToInt32(grp.Id);
                //}
                pro.IdCategory = cat;
                if (collection["CatL2"] != null && collection["CatL2"] != "")
                {
                    pro.IdCategory2 = int.Parse(collection["CatL2"]);
                }
                else { pro.IdCategory2 = null; }
                pro.Keyword = collection["Keyword"];
                pro.Description = collection["Description"];
                if (collection["Weight"] != "")
                {
                    pro.Weight = Int32.Parse(collection["Weight"]);
                }
                else
                {
                    pro.Weight = 0;
                }
                pro.Num = Int32.Parse(collection["Num"]);
                pro.Title = collection["Title"];
                pro.Ord = Convert.ToInt32(collection["Ord"]);
                pro.Priority = (collection["Priority"] == "false") ? false : true;
                pro.Index = (collection["Index"] == "false") ? false : true;
                pro.Active = (collection["Active"] == "false") ? false : true;
                pro.Status = (collection["Status"] == "false") ? short.Parse("0") : short.Parse("1");
                pro.View = 0;//so luot xem
                pro.Count = 0;//so luong don hang da dat nhung chua giao hang
                pro.SpTon = 0;//sp con ton trong kho
                pro.Like = 0;
                db.Products.Add(pro);
                db.SaveChanges();
                var product = db.Products.OrderByDescending(m => m.Id).FirstOrDefault();
                #region[Lưu vào bảng màu sản phẩm]
                db.sp_ProColor_Delete_Proid(product.Id);
                db.SaveChanges();
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.Contains("color"))
                    {
                        if (key.Contains("colorAll"))
                        {
                            #region[Trường hợp chọn tất cả]
                            var Color = db.Colors.ToList();
                            if (Color.Count > 0)
                            {
                                for (int i = 0; i < Color.Count; i++)
                                {
                                    int cId = Color[i].Id;
                                    ProColor obj = new ProColor();
                                    obj.IdPro = product.Id;
                                    obj.IdColor = cId;
                                    db.ProColors.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                            #endregion
                        }
                        //else if (key.Contains("color0"))
                        //{
                        //    #region[Trường hợp chọn màu mặc định theo ảnh]
                        //    ProColor obj = new ProColor();
                        //    obj.IdPro = product.Id;
                        //    obj.IdColor = 0;
                        //    db.ProColors.Add(obj);
                        //    db.SaveChanges();
                        //    #endregion
                        //}
                        else
                        {
                            #region[Trường hợp chọn lẻ]
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int id = Convert.ToInt32(key.Remove(0, 5));
                                ProColor obj = new ProColor();
                                obj.IdPro = product.Id;
                                obj.IdColor = id;
                                db.ProColors.Add(obj);
                                db.SaveChanges();
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region[Lưu vào bảng kích thươc sản phẩm]
                db.sp_ProSize_Delete_Proid(product.Id);
                db.SaveChanges();
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.Contains("size"))
                    {
                        if (key.Contains("sizeAll"))
                        {
                            #region[Trường hợp chọn tất cả]
                            var Size = db.Sizes.ToList();
                            if (Size.Count > 0)
                            {
                                for (int i = 0; i < Size.Count; i++)
                                {
                                    int sId = Size[i].Id;
                                    ProSize obj = new ProSize();
                                    obj.IdPro = product.Id;
                                    obj.IdSize = sId;
                                    db.ProSizes.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region[Trường hợp chọn lẻ]
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int id = Convert.ToInt32(key.Remove(0, 4));
                                ProSize obj = new ProSize();
                                obj.IdPro = product.Id;
                                obj.IdSize = id;
                                db.ProSizes.Add(obj);
                                db.SaveChanges();
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                return RedirectToAction("ProductIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[ProductEditot]
        public ActionResult ProductEditot(int id)
        {
            string chuoi = "";
            Session["curProId"] = id;
            var Edit = db.Products.First(m => m.Id == id);
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();            
            ViewBag.Cat = new SelectList(cat, "Id", "Name", Edit.IdCategory);

            return View(Edit);
        }
        #endregion

        #region[ProductEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductEdit(int id, FormCollection collection, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                //var pro = db.Products.First(m => m.Id == id);
                string Name = collection["Name"];
                string Tag = StringClass.NameToTag(collection["Name"]);
                string Image = collection["Image"];
                string Content = collection["Content"];
                string Detail = collection["Detail"];
                string Baohanh = collection["Baohanh"];
                string Noibat = collection["Noibat"];
                string Giamgia = collection["Giamgia"];
                DateTime Date = Convert.ToDateTime(DateTime.Now.ToString());
                int cId = Convert.ToInt32(collection["Cat"]);
                var grp = db.GroupProducts.Where(m => m.Id == cId).FirstOrDefault();
                var pro = db.Products.Where(m => m.Id == id).FirstOrDefault();
                int IdCategory = 0;
                IdCategory = cId;
                //if (grp.Level.Length > 5)
                //{
                //    var grp2 = db.GroupProducts.Where(m => m.Level == grp.Level.Substring(0, 5)).FirstOrDefault();
                //    IdCategory = Convert.ToInt32(grp2.Id);
                //}
                //else
                //{
                //    IdCategory = Convert.ToInt32(grp.Id);
                //}
                //if (collection["CatL2"] != null && collection["CatL2"] != "")
                //{
                //    IdCategory2 = int.Parse(collection["CatL2"]);
                //}
                int Weight = Int32.Parse(collection["Weight"]);
                int Num = Int32.Parse(collection["Num"]);
                string Keyword = collection["Keyword"];
                string Description = collection["Description"];
                string Title = collection["Title"];
                int Ord = Convert.ToInt32(collection["Ord"]);
                bool Priority = (collection["Priority"] == "false") ? false : true;
                bool Index = (collection["Index"] == "false") ? false : true;
                bool Active = (collection["Active"] == "false") ? false : true;
                Double PriceRetail = Double.Parse(StringClass.NumberStr(collection["PriceRetail"].ToString()));
                Double PricePromotion = Double.Parse(StringClass.NumberStr(collection["PricePromotion"].ToString()));
                short Status = (collection["Status"] == "false") ? short.Parse("0") : short.Parse("1");
                db.sp_Product_Update(id, Name, Tag, Image, Description, Weight, "0", Status, Detail, 0, 0, pro.View, 0, IdCategory, Date, Index, Priority, Keyword, Title, Content, Ord, Active, PriceRetail, PricePromotion, Num, Noibat, Baohanh, Giamgia);
                db.SaveChanges();
                #region[Lưu vào bảng màu sản phẩm]
                db.sp_ProColor_Delete_Proid(id);
                db.SaveChanges();
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.Contains("color"))
                    {
                        if (key.Contains("colorAll"))
                        {
                            #region[Trường hợp chọn tất cả]
                            var Color = db.Colors.ToList();
                            if (Color.Count > 0)
                            {
                                for (int i = 0; i < Color.Count; i++)
                                {
                                    int coId = Color[i].Id;
                                    ProColor obj = new ProColor();
                                    obj.IdPro = id;
                                    obj.IdColor = coId;
                                    db.ProColors.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region[Trường hợp chọn lẻ]
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                string t = key;
                                int coId = Convert.ToInt32(key.Remove(0, 5));
                                ProColor obj = new ProColor();
                                obj.IdPro = id;
                                obj.IdColor = coId;
                                db.ProColors.Add(obj);
                                db.SaveChanges();
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region[Lưu vào bảng kích thươc sản phẩm]
                db.sp_ProSize_Delete_Proid(id);
                db.SaveChanges();
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.Contains("size"))
                    {
                        if (key.Contains("sizeAll"))
                        {
                            #region[Trường hợp chọn tất cả]
                            var Size = db.Sizes.ToList();
                            if (Size.Count > 0)
                            {
                                for (int i = 0; i < Size.Count; i++)
                                {
                                    int sId = Size[i].Id;
                                    ProSize obj = new ProSize();
                                    obj.IdPro = id;
                                    obj.IdSize = sId;
                                    db.ProSizes.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region[Trường hợp chọn lẻ]
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int sizeid = Convert.ToInt32(key.Remove(0, 4));
                                ProSize obj = new ProSize();
                                obj.IdPro = id;
                                obj.IdSize = sizeid;
                                db.ProSizes.Add(obj);
                                db.SaveChanges();
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                Session["curProId"] = "0";
                return RedirectToAction("ProductIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[ProductActive]
        public ActionResult ProductActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var list = db.Products.First(m => m.Id == id);
                var pro = db.Products.First(m => m.Id == id);
                if (list.Active == true)
                {
                    pro.Active = false;
                }
                else
                {
                    pro.Active = true;
                }
                db.SaveChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[ProductDelete]
        public ActionResult ProductDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.Products.First(p => p.Id == id);
                //if (del.SpTon == 0)
                //{
                #region [Xoa Anh]
                var delImg = db.ProImages.AsEnumerable().Where(m => m.IdPro == id).ToList();
                if (delImg != null)
                {
                    for (int i = 0; i < delImg.Count; i++)
                    {
                        db.ProImages.Remove(delImg[i]);
                        db.SaveChanges();
                    }
                }
                #endregion
                #region [Xoa Color]
                var delcolor = db.ProColors.AsEnumerable().Where(m => m.IdPro == id).ToList();
                if (delcolor != null)
                {
                    for (int i = 0; i < delcolor.Count; i++)
                    {
                        db.ProColors.Remove(delcolor[i]);
                        db.SaveChanges();
                    }
                }
                #endregion
                #region [Xoa Size]
                var delsize = db.ProSizes.AsEnumerable().Where(m => m.IdPro == id).ToList();
                if (delsize != null)
                {
                    for (int i = 0; i < delsize.Count; i++)
                    {
                        db.ProSizes.Remove(delsize[i]);
                        db.SaveChanges();
                    }
                }
                #endregion
                db.Products.Remove(del);
                db.SaveChanges();
                //}
                //else
                //{
                //    Session["DeletePro"] = "Sản phẩm " + del.Name + "  vẫn còn trong kho! Không được xóa!";
                //}

                List<Product> products = db.Products.ToList();

                if ((products.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("ProductIndexot");
                    }
                }

                return RedirectToAction("ProductIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[MultiCommand]
        public ActionResult MultiCommand(FormCollection collect)
        {
            int m = int.Parse(collect["mPage"]);
            int pagesize = int.Parse(collect["PageSize"]);

            List<Product> products = db.Products.ToList();
            int lastpage = products.Count / pagesize;
            if (products.Count % pagesize > 0)
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
                            if (key.Contains("chkStatus") || key.Contains("chkIndex") || key.Contains("chkActive"))
                            {

                            }
                            else
                            {
                                checkbox = Request.Form["" + key];
                                if (checkbox != "false")
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 3));
                                    var Del = (from del in db.Products where del.Id == id select del).SingleOrDefault();
                                    var Delorder = (from deloder in db.OrderDetails where deloder.IdPro == id select deloder).ToList();
                                    for (int l = 0; l < Delorder.Count; l++)
                                    {
                                        int idp = Delorder[l].Id;
                                        var orldel = (from orldels in db.OrderDetails where orldels.Id == idp select orldels).SingleOrDefault();
                                        db.OrderDetails.Remove(orldel);
                                        db.SaveChanges();
                                    }
                                    //if (Del.SpTon == 0)
                                    //{
                                    db.Products.Remove(Del);
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
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("ProductIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("ProductIndexot", new { page = m });
                }
                else if (collect["ddlProductAmount"] != null)
                {
                    if (collect["ddlProductAmount"].Length > 0)
                    {
                        Session["ProductAmount"] = collect["ddlProductAmount"];
                    }
                    return RedirectToAction("ProductIndexot");
                }
                #region[Import]
                //else if (true)
                //{
                //    foreach (string key in Request.Form)
                //    {
                //        var checkbox = "";
                //        if (key.StartsWith("chk"))
                //        {
                //            checkbox = Request.Form["" + key];
                //            if (checkbox != "false")
                //            {
                //                if (Session["AddProductImport"] == null)
                //                {
                //                    list = new List<Product>();
                //                    int id = Convert.ToInt32(key.Remove(0, 3));
                //                    var imp = (from im in db.Products where im.Id == id select im).SingleOrDefault();
                //                    list.Add(imp);
                //                }
                //                else
                //                {
                //                    list = (List<Product>)Session["AddProductImport"];
                //                    int id = Convert.ToInt32(key.Remove(0, 3));
                //                    var imp = (from im in db.Products where im.Id == id select im).SingleOrDefault();
                //                    list.Add(imp);
                //                }
                //                Session["AddProductImport"] = list;
                //            }
                //        }
                //    }
                //    return Redirect("/Import/ImportCreate");
                //}
                //else//command == AddExport
                //{
                //    foreach (string key in Request.Form)
                //    {
                //        var checkbox = "";
                //        if (key.StartsWith("chk"))
                //        {
                //            checkbox = Request.Form["" + key];
                //            if (checkbox != "false")
                //            {
                //                if (Session["AddProductExport"] == null)
                //                {
                //                    list = new List<Product>();
                //                    int id = Convert.ToInt32(key.Remove(0, 3));
                //                    var imp = (from im in db.Products where im.Id == id select im).SingleOrDefault();
                //                    list.Add(imp);
                //                }
                //                else
                //                {
                //                    list = (List<Product>)Session["AddProductExport"];
                //                    int id = Convert.ToInt32(key.Remove(0, 3));
                //                    var imp = (from im in db.Products where im.Id == id select im).SingleOrDefault();
                //                    list.Add(imp);
                //                }
                //                Session["AddProductExport"] = list;
                //            }
                //        }
                //    }
                //    return Redirect("/Export/ExportCreate");
                //}
                #endregion

                else if (collect["btnSearch"] != null)
                {
                    string Model = collect["txtModel"];
                    string Namesearch = collect["txtSearch"];
                    string cat = collect["Cat"];
                    if (Model.Length > 0)
                    {
                        Session["Model"] = Model;
                    }
                    if (Namesearch.Length > 0)
                    {
                        Session["Namesearch"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["Cat"] = cat;
                    }
                    return RedirectToAction("ProductIndexot", new { page = m });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.Products.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }
                                if (!collect["CodeSp" + id].Equals(""))
                                {
                                    Up.Code = collect["CodeSp" + id];
                                }
                                if (!collect["Retail" + id].Equals(""))
                                {
                                    Up.PriceRetail = Double.Parse(collect["Retail" + id]);
                                }
                                if (!collect["Promotion" + id].Equals(""))
                                {
                                    Up.PricePromotion = Double.Parse(collect["Promotion" + id]);
                                }
                                if (!collect["chkIndex" + id].Equals(""))
                                {
                                    bool Index = (collect["chkIndex" + id] == "false") ? false : true;
                                    Up.Index = Index;
                                }
                                if (!collect["chkActive" + id].Equals(""))
                                {
                                    bool Active = (collect["chkActive" + id] == "false") ? false : true;
                                    Up.Active = Active;
                                }
                                if (!collect["chkStatus" + id].Equals(""))
                                {
                                    short Active = (collect["chkStatus" + id] == "false") ? short.Parse("0") : short.Parse("1");
                                    Up.Status = Active;
                                }
                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("ProductIndexot", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[DropDownCheckboxColor]
        public ActionResult _DropDownCheckboxColor()
        {
            int proId = Session["curProId"] != null ? int.Parse(Session["curProId"].ToString()) : 0;
            var lstColor = db.ProColors.Where(s => s.IdPro == proId).Select(s => s.IdColor).ToArray();
            int lengthColor = db.Colors.Count();
            string chuoi = "";
            var Color = db.Colors.ToList();
            if (Color.Count > 0)
            {
                if (lstColor.Length == lengthColor)
                {
                    chuoi += "<input type=\"checkbox\" name=\"colorAll\" id=\"colorAll\" checked='checked' value=\"Tất cả\" /><span style=\"margin-right: 10px\">Tất cả</span>";
                    for (int i = 0; i < Color.Count; i++)
                    {
                        int cId = Color[i].Id;
                        chuoi += "<input type=\"checkbox\" name=\"color" + cId + "\" id=\"color" + cId + "\" value=\"" + Color[i].Name + "\" /><span style=\"margin-right: 10px\">" + Color[i].Name + "</span>";
                    }
                }
                else
                {
                    chuoi += "<input type=\"checkbox\" name=\"colorAll\" id=\"colorAll\" value=\"Tất cả\" /><span style=\"margin-right: 10px\">Tất cả</span>";
                    for (int i = 0; i < Color.Count; i++)
                    {
                        int cId = Color[i].Id;
                        if (lstColor.Contains(cId))
                        {
                            chuoi += "<input type=\"checkbox\" name=\"color" + cId + "\" checked='checked' id=\"color" + cId + "\" value=\"" + Color[i].Name + "\" /><span style=\"margin-right: 10px\">" + Color[i].Name + "</span>";
                        }
                        else
                        {
                            chuoi += "<input type=\"checkbox\" name=\"color" + cId + "\" id=\"color" + cId + "\" value=\"" + Color[i].Name + "\" /><span style=\"margin-right: 10px\">" + Color[i].Name + "</span>";
                        }                        
                    }
                }                                
            }
            ViewBag.Color = chuoi;
            return PartialView();
        }
        #endregion

        #region[DropDownCheckboxSize]
        public ActionResult _DropDownCheckboxSize()
        {
            string chuoi = "";
            int proId = Session["curProId"] != null ? int.Parse(Session["curProId"].ToString()) : 0;
            var lstSize = db.ProSizes.Where(s => s.IdPro == proId).Select(s => s.IdSize).ToArray();
            int lengthSize = db.Sizes.Count();
            var size = db.Sizes.ToList();
            if (size.Count > 0)
            {
                if (lstSize.Length == lengthSize)
                {
                    chuoi += "<input type=\"checkbox\" name=\"sizeAll\" id=\"sizeAll\" checked='checked' title=\"Tất cả\" /><span style=\"margin-right: 10px\">Tất cả</span>";
                    for (int i = 0; i < size.Count; i++)
                    {
                        int cId = size[i].Id;
                        chuoi += "<input type=\"checkbox\" name=\"size" + cId + "\" id=\"size" + cId + "\" title=\"" + size[i].Name + "\" /><span  style=\"margin-right: 10px\">" + size[i].Name + "</span>";
                    }
                }
                else
                {
                    chuoi += "<input type=\"checkbox\" name=\"sizeAll\" id=\"sizeAll\" title=\"Tất cả\" /><span style=\"margin-right: 10px\">Tất cả</span>";
                    for (int i = 0; i < size.Count; i++)
                    {
                        int cId = size[i].Id;
                        if (lstSize.Contains(cId))
                        {
                            chuoi += "<input type=\"checkbox\" name=\"size" + cId + "\" checked='checked' id=\"size" + cId + "\" title=\"" + size[i].Name + "\" /><span  style=\"margin-right: 10px\">" + size[i].Name + "</span>";
                        }
                        else
                        {
                            chuoi += "<input type=\"checkbox\" name=\"size" + cId + "\" id=\"size" + cId + "\" title=\"" + size[i].Name + "\" /><span  style=\"margin-right: 10px\">" + size[i].Name + "</span>";
                        }                        
                    }
                }
                
            }
            ViewBag.Size = chuoi;
            return PartialView();
        }
        #endregion

        #region[ProductPriceView]
        public ActionResult ProductPriceView(int id)
        {
            var pro = db.Products.Where(m => m.Id == id).FirstOrDefault();
            var proImg = db.ProImages.Where(m => m.IdPro == id && m.Image.LastIndexOf("_noz") > 0).FirstOrDefault();
            if (proImg == null) { proImg = db.ProImages.Where(m => m.IdPro == id && m.Image.LastIndexOf("_huge") > 0).FirstOrDefault(); }
            if (proImg == null) { proImg = db.ProImages.Where(m => m.IdPro == id && m.Image.LastIndexOf("_small") > 0).FirstOrDefault(); }
            if (proImg == null) { proImg = db.ProImages.Where(m => m.IdPro == id && m.Image.LastIndexOf("_big") > 0).FirstOrDefault(); }
            var proPrice = db.ProPrices.Where(m => m.IdPro == id).OrderByDescending(m => m.Date).ToList();
            string chuoi = "";
            chuoi += "<h2>Xem thông tin sản phẩm</h2>";
            chuoi += "<div class=\"viewInfo\">";
            chuoi += "<div>";
            chuoi += "<p>Tên sản phẩm</p>";
            chuoi += "<p>Giá bán lẻ hiện tại</p>";
            chuoi += "<p>Giá bán sỉ hiện tại</p>";
            chuoi += "<p>Giá nhập hàng mới nhất</p>";
            chuoi += "<p>Giá khuyến mãi hiện tại</p>";
            chuoi += "<p>Ngày bắt đầu áp dụng</p>";
            chuoi += "<p>Ngày kết thúc áp dụng</p>";
            chuoi += "</div>";
            chuoi += "<div>";
            chuoi += "<p>" + pro.Name + "</p>";
            chuoi += "<p>" + StringClass.Format_Price(proPrice[0].PriceExport_L) + " VNĐ</p>";
            chuoi += "<p>" + StringClass.Format_Price(proPrice[0].PriceExport_S) + " VNĐ</p>";
            chuoi += "<p>" + StringClass.Format_Price(proPrice[0].PriceImport) + " VNĐ</p>";
            if (proPrice[0].PricePromotion != null)
            {
                chuoi += "<p>" + StringClass.Format_Price(proPrice[0].PricePromotion.ToString()) + " VNĐ</p>";
                chuoi += "<p>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[0].SDate.ToString()) + "</p>";
                chuoi += "<p>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[0].EDate.ToString()) + "</p>";
            }
            else
            {
                chuoi += "<p>0 VNĐ</p>";
                chuoi += "<p>Không khuyến mãi</p>";
                chuoi += "<p>Không khuyến mãi</p>";
            }
            chuoi += "</div>";
            chuoi += "<div>";
            if (proImg != null)
            {
                chuoi += "<img src=\"" + proImg.Image + "\" style=\"width:100px; height:100px; margin-left: 50px;\" />";
            }
            else if (proImg == null && pro.Image != "")
            {
                chuoi += "<img src=\"" + pro.Image + "\" style=\"width:100px; height:100px; margin-left: 50px;\" />";
            }
            else { chuoi += "<img src=\"/Images/no-image.png\" style=\"width:100px; height:100px; margin-left: 50px;\" />"; }
            chuoi += "</div>";
            chuoi += "</div>";
            chuoi += "<div class=\"clearfix\"></div>";
            if (proPrice.Count > 0)
            {
                chuoi += "<div class=\"divShowHideHistory\">";
                chuoi += "<div class=\"showHideHistory\">Hiển thị thông tin lịch sử giá</div>";
                chuoi += "<div id=\"divHistory\">";
                chuoi += "<table border=\"1\">";
                chuoi += "<tr>";
                chuoi += "<th>STT</th>";
                chuoi += "<th>Giá nhập</th>";
                chuoi += "<th>Giá bán bán sỉ</th>";
                chuoi += "<th>Giá bán lẻ</th>";
                chuoi += "<th>Giá khuyến mãi</th>";
                chuoi += "<th>Ngày bắt đầu</th>";
                chuoi += "<th>Ngày kết thúc</th>";
                chuoi += "<th>Ngày nhập</th>";
                chuoi += "<th>Chức năng</th>";
                chuoi += "</tr>";
                for (int i = 0; i < proPrice.Count; i++)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + (i + 1) + "</td>";
                    chuoi += "<td>" + StringClass.Format_Price(proPrice[i].PriceImport) + " VNĐ</td>";
                    chuoi += "<td>" + StringClass.Format_Price(proPrice[i].PriceExport_S) + " VNĐ</td>";
                    chuoi += "<td>" + StringClass.Format_Price(proPrice[i].PriceExport_L) + " VNĐ</td>";
                    if (proPrice[i].PricePromotion != null)
                    {
                        chuoi += "<td>" + proPrice[i].PricePromotion + "</td>";
                        chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[i].SDate.ToString()) + "</td>";
                        chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[i].EDate.ToString()) + "</td>";
                    }
                    else
                    {
                        chuoi += "<td>0 VNĐ</td>";
                        chuoi += "<td>Không khuyến mãi</td>";
                        chuoi += "<td>Không khuyến mãi</td>";
                    }
                    chuoi += "<td>" + DateTimeClass.ConvertDateTimeddMMyyyy(proPrice[i].Date.ToString()) + "</td>";
                    chuoi += "<td><a href='/Product/ProductPriceEdit/" + proPrice[i].Id + "' title='Chỉnh sửa khung giá' class='edit'>Sửa</a>";
                    if (Request.Cookies["Username"] != null)
                    {
                        chuoi += "<a href='/Product/ProductPriceDelete/" + proPrice[i].Id + "' title='Xóa' class='vdel'>Xóa</a>";
                    }
                    else
                    {
                        chuoi += "<p class='vdel' onclick='AlertErr()'>Xóa</p>";
                    }
                    chuoi += "</td>";
                    chuoi += "</tr>";
                }
                chuoi += "</table>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<a href=\"/Product/ProductPriceCreate/" + pro.Id + "\">Thêm khung giá mới</a>";
            }
            ViewBag.View = chuoi;
            return View();
        }
        #endregion

        #region[ProductPriceCreate]
        public ActionResult ProductPriceCreate(int id)
        {
            var imp = db.ImportDetails.Where(m => m.IdPro == id).OrderByDescending(m => m.Id).FirstOrDefault();
            if (imp != null)
            {
                ViewBag.Imp = imp.Price;
            }
            return View();
        }
        #endregion

        #region[ProductPriceCreate]
        [HttpPost]
        public ActionResult ProductPriceCreate(FormCollection collect, ProPrice pp, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                pp.IdPro = id;
                pp.PriceExport_L = collect["PriceExport_L"];
                pp.PriceExport_S = collect["PriceExport_S"];
                pp.PriceImport = collect["PriceImport"];
                pp.PricePromotion = collect["PricePromotion"];
                pp.Ord = 1;
                var datebegin = collect["SDate"];
                var dateend = collect["EDate"];
                string dateInputBegin = "";
                string dateInputEnd = "";
                DateTime myDateBegin;
                DateTime myDateEnd;
                string[] dateArrBegin = datebegin.Split('/');
                string[] dateArrEnd = dateend.Split('/');
                if (dateArrBegin.Length == 3) dateInputBegin = dateArrBegin[2] + "-" + dateArrBegin[1] + "-" + dateArrBegin[0];
                if (dateArrEnd.Length == 3) dateInputEnd = dateArrEnd[2] + "-" + dateArrEnd[1] + "-" + dateArrEnd[0];
                if (DateTime.TryParse(dateInputBegin, out myDateBegin)) pp.SDate = myDateBegin;
                if (DateTime.TryParse(dateInputEnd, out myDateEnd)) pp.EDate = myDateEnd;
                pp.Date = DateTime.Now;
                db.ProPrices.Add(pp);
                db.SaveChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/admins/admins");
            }
        }
        #endregion

        #region[ProductPriceEdit]
        public ActionResult ProductPriceEdit(int id)
        {
            var edit = db.ProPrices.Where(m => m.Id == id).FirstOrDefault();
            return View(edit);
        }
        #endregion

        #region[ProductPriceEdit]
        [HttpPost]
        public ActionResult ProductPriceEdit(int id, FormCollection collect)
        {
            if (Request.Cookies["Username"] != null)
            {
                var edit = db.ProPrices.Where(m => m.Id == id).FirstOrDefault();
                edit.PriceExport_L = collect["PriceExport_L"];
                edit.PriceExport_S = collect["PriceExport_S"];
                edit.PriceImport = collect["PriceImport"];
                edit.PricePromotion = collect["PricePromotion"];
                edit.Ord = 1;
                edit.Date = DateTime.Now;
                var datebegin = collect["SDate"];
                var dateend = collect["EDate"];
                if (datebegin != "" && dateend != "")
                {
                    string dateInputBegin = "";
                    string dateInputEnd = "";
                    DateTime myDateBegin;
                    DateTime myDateEnd;
                    string[] dateArrBegin = datebegin.Split('/');
                    string[] dateArrEnd = dateend.Split('/');
                    if (dateArrBegin.Length == 3) dateInputBegin = dateArrBegin[2] + "-" + dateArrBegin[1] + "-" + dateArrBegin[0];
                    if (dateArrEnd.Length == 3) dateInputEnd = dateArrEnd[2] + "-" + dateArrEnd[1] + "-" + dateArrEnd[0];
                    if (DateTime.TryParse(dateInputBegin, out myDateBegin)) edit.SDate = myDateBegin;
                    if (DateTime.TryParse(dateInputEnd, out myDateEnd)) edit.EDate = myDateEnd;
                }
                db.SaveChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/admins/admins");
            }
        }
        #endregion

        #region[ProductPriceDelete]
        public ActionResult ProductPriceDelete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.ProPrices.Where(m => m.Id == id).FirstOrDefault();
                db.ProPrices.Remove(del);
                db.SaveChanges();
                return RedirectToAction("ProductIndex");
            }
            else
            {
                return Redirect("/admins/admins");
            }
        }
        #endregion

        

        #region[Xem anh]

        public ActionResult ProductViewImgot(int id)
        {
            var chuoiA = "";
            var chuoiB = "";
            var chuoiC = "";
            var chuoiD = "";
            var chuoiE = "";
            var list = db.ProImages.Where(m => m.IdPro == id).ToList();
            chuoiA += "<h5>Ảnh lớn nhất (_huge):</h5>";
            chuoiB += "<h5>Ảnh lớn (_big):</h5>";
            chuoiC += "<h5>Ảnh vừa (_noz):</h5>";
            chuoiD += "<h5>Ảnh nhỏ (_small):</h5>";
            chuoiE += "";
            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i].Image.IndexOf("_huge");
                var b = list[i].Image.IndexOf("_big");
                var c = list[i].Image.IndexOf("_noz");
                var d = list[i].Image.IndexOf("_small");
                #region[View anh _huge]
                if (a > 0)
                {
                    chuoiA += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiA += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiA += "</div></div>";
                    //chuoiA += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh _big]
                else if (b > 0)
                {
                    chuoiB += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiB += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiB += "</div></div>";
                    //chuoiB += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh _noz]
                else if (c > 0)
                {
                    chuoiC += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiC += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiC += "</div></div>";
                    //chuoiC += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh _small]
                else if (d > 0)
                {
                    chuoiD += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiD += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiD += "</div></div>";
                    //chuoiD += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh other - anh ten k co duoi nhu tren]
                else
                {
                    chuoiE += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImgot/" + list[i].Id + "\">Sửa</a>";
                    chuoiE += "<a title=\"Bạn chắc chắn muốn xóa ảnh này?\" class=\"yesno\" href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiE += "</div></div>";
                    //chuoiE += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
            }

            ViewBag.AddImage = "<a title=\"Thêm ảnh mới\" class=\"toolbar btn btn-info\" href=\"/Product/ProductAddImgot?id=" + id + "\"><i class=\"icon-plus\"></i>&nbsp;Thêm mới</a>";

            ViewBag.ViewA = chuoiA;
            ViewBag.ViewB = chuoiB;
            ViewBag.ViewC = chuoiC;
            ViewBag.ViewD = chuoiD;
            ViewBag.ViewE = chuoiE;
            return View();
        }

        public ActionResult ProductViewImg(int id)
        {
            var chuoiA = "";
            var chuoiB = "";
            var chuoiC = "";
            var chuoiD = "";
            var chuoiE = "";
            var list = db.ProImages.Where(m => m.IdPro == id).ToList();
            chuoiA += "<h4>Ảnh lớn nhất (_huge):</h4>";
            chuoiB += "<h4>Ảnh lớn (_big):</h4>";
            chuoiC += "<h4>Ảnh vừa (_noz):</h4>";
            chuoiD += "<h4>Ảnh nhỏ (_small):</h4>";
            chuoiE += "<h4>Ảnh khác</h4>";
            for (int i = 0; i < list.Count; i++)
            {
                var a = list[i].Image.IndexOf("_huge");
                var b = list[i].Image.IndexOf("_big");
                var c = list[i].Image.IndexOf("_noz");
                var d = list[i].Image.IndexOf("_small");
                #region[View anh _huge]
                if (a > 0)
                {
                    chuoiA += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiA += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiA += "</div></div>";
                    //chuoiA += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh _big]
                else if (b > 0)
                {
                    chuoiB += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiB += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiB += "</div></div>";
                    //chuoiB += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh _noz]
                else if (c > 0)
                {
                    chuoiC += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiC += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiC += "</div></div>";
                    //chuoiC += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh _small]
                else if (d > 0)
                {
                    chuoiD += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiD += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiD += "</div></div>";
                    //chuoiD += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
                #region[View anh other - anh ten k co duoi nhu tren]
                else
                {
                    chuoiE += "<div class=\"imgView\"><img src=\"" + list[i].Image + "\" /><div class=\"funcImg\"><a href=\"/Product/ProductEditImg/" + list[i].Id + "\">Sửa</a>";
                    chuoiE += "<a href=\"/Product/ProductDelImg/" + list[i].Id + "\">Xóa</a>";
                    chuoiE += "</div></div>";
                    //chuoiE += "<a href='/Products/ProductEditImg/" + list[i].Id + "' class='editImg'><img src='" + list[i].Image + "' id='fileImg' name='fileImg'/></a>";
                }
                #endregion
            }
            chuoiE += "<div class='clearfix'></div><a href='/Product/ProductAddImg/" + id + "' class='addImg'>Thêm ảnh mới</a>";
            ViewBag.ViewA = chuoiA;
            ViewBag.ViewB = chuoiB;
            ViewBag.ViewC = chuoiC;
            ViewBag.ViewD = chuoiD;
            ViewBag.ViewE = chuoiE;
            return View();
        }
        #endregion

        #region[Sua anh]
        public ActionResult ProductEditImg(int id)
        {
            var list = db.ProImages.Where(m => m.Id == id).FirstOrDefault();
            return View(list);
        }

        public ActionResult ProductEditImgot(int id)
        {
            var list = db.ProImages.Where(m => m.Id == id).FirstOrDefault();
            ViewBag.ProId = id;
            return View(list);
        }
        #endregion

        #region[Sua anh]
        [HttpPost]
        public ActionResult ProductEditImg(int id, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (fileImg.ContentLength > 0)
                {
                    String FileExtn = System.IO.Path.GetExtension(fileImg.FileName).ToLower();
                    if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                    {
                        ViewBag.error = "Only jpg, gif and png files are allowed!";
                    }
                    else
                    {
                        var u = fileImg.FileName;
                        List<string> sizeImg = new List<string>();
                        sizeImg.Add("_huge");
                        sizeImg.Add("_big");
                        sizeImg.Add("_noz");
                        sizeImg.Add("_small");
                        string co = "";
                        string kco = "";
                        for (int i = 0; i < sizeImg.Count; i++)
                        {
                            var a = u.LastIndexOf(sizeImg[i]);
                            if (a > 0)
                            {
                                co = u.Substring(0, a);
                                kco = sizeImg[i];
                                break;
                            }
                        }
                        var fileName = String.Format("{0}" + kco + ".jpg", Guid.NewGuid().ToString());
                        var imagePath = Path.Combine(Server.MapPath(Url.Content("/Uploads")), fileName);
                        fileImg.SaveAs(imagePath);
                        var ProImg = db.ProImages.First(m => m.Id == id);
                        var im = ProImg.Image.IndexOf("/Uploads/");
                        if (im > -1)
                        {
                            var anh = ProImg.Image.Substring(im);
                            var d = Request.PhysicalApplicationPath + anh;
                            System.IO.File.Delete(d);
                        }
                        ProImg.Image = "/Uploads/" + fileName;
                        //ProImg.Date = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("ProductIndexot");
            }
            else
            {
                return Redirect("/admins/admins");
            }
        }
        #endregion

        #region[Them anh]
        public ActionResult ProductAddImgot(int id)
        {
            ViewBag.ProId = id;
            return View();
        }
        #endregion

        #region[Them anh]
        [HttpPost]
        public ActionResult ProductAddImg(IEnumerable<HttpPostedFileBase> fileImg, int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                foreach (var file in fileImg)
                {
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
                            ProImage img = new ProImage();
                            var Filename = Path.GetFileName(file.FileName);
                            List<string> sizeImg = new List<string>();
                            sizeImg.Add("_huge");
                            sizeImg.Add("_big");
                            sizeImg.Add("_noz");
                            sizeImg.Add("_small");
                            string co = "";
                            string kco = "";
                            for (int i = 0; i < sizeImg.Count; i++)
                            {
                                var a = Filename.LastIndexOf(sizeImg[i]);
                                if (a > 0)
                                {
                                    co = Filename.Substring(0, a);
                                    kco = sizeImg[i];
                                    break;
                                }
                            }
                            var fileName = String.Format("{0}" + kco + ".jpg", Guid.NewGuid().ToString());
                            //String imgPath = String.Format("Uploads/{0}{1}", file.FileName, FileExtn);
                            //file.Save(String.Format("{0}{1}", Server.MapPath("~"), imgPath), Img.RawFormat);
                            var path = Path.Combine(Server.MapPath(Url.Content("/Uploads")), fileName);
                            file.SaveAs(path);
                            img.IdPro = id;
                            img.Image = "/Uploads/" + fileName;
                            //img.Date = DateTime.Now;
                            db.ProImages.Add(img);
                            db.SaveChanges();
                        }
                    }
                    var fd = file;
                }
                return RedirectToAction("ProductIndexot");
            }
            else
            { return Redirect("/admins/admins"); }
        }
        #endregion

        #region [Xoa anh]
        public ActionResult ProductDelImg(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var pro = db.ProImages.Where(m => m.Id == id).FirstOrDefault();
                var a = Request.PhysicalApplicationPath + pro.Image;
                System.IO.File.Delete(a);
                db.ProImages.Remove(pro);
                db.SaveChanges();
                return RedirectToAction("ProductIndexot");
            }
            else
            {
                return Redirect("/admins/admins");
            }
        }
        #endregion

        #region[ProductAddMultipleot]
        public ActionResult ProductAddMultipleot()
        {
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            return View();
        }
        #endregion

        #region[ProductAddMultiple]
        public ActionResult ProductAddMultiple()
        {
            var cat = db.GroupProducts.Where(n => n.Active == true).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.Cat = new SelectList(cat, "Id", "Name");
            }
            return View();
        }
        #endregion

        #region[ProductAddMultiple]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductAddMultiple(HttpPostedFileBase fileExcel, IEnumerable<HttpPostedFileBase> ImageUrl, FormCollection conllection)
        {
            foreach (var item in ImageUrl)
            {
                // Lưu ảnh lên server
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/Images"), System.IO.Path.GetFileName(item.FileName));
                item.SaveAs(path);
            }
            if (fileExcel != null)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/FileUpload"), System.IO.Path.GetFileName(fileExcel.FileName));
                fileExcel.SaveAs(path);
                //Declare variables to hold refernces to Excel objects.
                Workbook workBook;
                SharedStringTable sharedStrings;
                IEnumerable<Sheet> workSheets;
                WorksheetPart productSheet;

                //Declare helper variables.
                string productID;

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, true))
                {
                    //References to the workbook and Shared String Table.
                    workBook = document.WorkbookPart.Workbook;
                    workSheets = workBook.Descendants<Sheet>();
                    sharedStrings = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

                    //Reference to Excel Worksheet with Product data.
                    productID = workSheets.First().Id;
                    productSheet = (WorksheetPart)document.WorkbookPart.GetPartById(productID);

                    //Load product data to business object.
                    SaveProducts(productSheet.Worksheet, sharedStrings, conllection["Cat"].ToString());

                    document.Close();

                    System.IO.File.Delete(path);

                    return RedirectToAction("ProductIndexot");
                }
            }

            return View();
        }
        #endregion

        #region[Save Product]
        // Save Product
        private void SaveProducts(Worksheet worksheet, SharedStringTable sharedString, string cat)
        {
            
            //Initialize the product list.
            List<Product> result = new List<Product>();

            //LINQ query to skip first row with column names.
            IEnumerable<Row> dataRows = from row in worksheet.Descendants<Row>()
                                        where row.RowIndex > 1
                                        select row;

             wwwEntities db = new wwwEntities();
            List<Product> productsdb = db.Products.ToList();
            List<GroupProduct> categories = db.GroupProducts.ToList();


            foreach (Row row in dataRows)
            {

                IEnumerable<String> textValues = from cell in row.Descendants<Cell>()
                                                 where cell.CellValue != null
                                                 select (cell.DataType != null && cell.DataType.HasValue && cell.DataType == CellValues.SharedString ? sharedString.ChildElements[int.Parse(cell.CellValue.InnerText)].InnerText : cell.CellValue.InnerText);

                //Check to verify the row contained data.
                if (textValues.Count() > 0)
                {
                    //Create a product and add it to the list.
                    var textArray = textValues.ToArray();
                    Product product = new Product();

                    int leng = textArray.Length;
                    product.IdCategory = Convert.ToInt32(cat);
                    product.Name = textArray[1];
                    product.Tag = StringClass.NameToTag(textArray[1]);
                    product.Description = textArray[1];
                    product.Content = textArray[1];
                    product.Active = true;
                    product.Ord = 1;
                    product.Image = textArray[5];
                    if (textArray[2] != null && textArray[2] != "")
                    {
                        product.SpTon = int.Parse(textArray[2]);
                    }
                    else
                    {
                        product.SpTon = 0;
                    }
                    product.Priority = true;
                    product.Index = true;
                    product.Code = textArray[0];
                    if (textArray[4] != null && textArray[4] != "")
                    {
                        product.PricePromotion = Double.Parse(textArray[4]);
                    }
                    else
                    {
                        product.PricePromotion = 0;
                    }
                    if (textArray[3] != null && textArray[3] != "")
                    {
                        product.PriceRetail = Double.Parse(textArray[3]);
                    }
                    else
                    {
                        product.PriceRetail = 0;
                    }
                    if (textArray[2] != null && textArray[2] != "")
                    {
                        product.Num = int.Parse(textArray[2]);
                    }
                    else
                    {
                        product.Num = 0;
                    }
                    if (textArray[6] != null && textArray[6] != "")
                    {
                        product.Weight = int.Parse(textArray[6]);
                    }
                    else
                    {
                        product.Weight = 0;
                    }
                    result.Add(product);
                    db.Products.Add(product);


                }
                else
                {
                    //If no cells, then you have reached the end of the table.
                    break;
                }

            }
            db.SaveChanges();

        }
        #endregion

        #region[Ajax Autocomplete for search]
        // Autocomplete for search productcode 
        [HttpGet]
        public ActionResult ProductAutocomplete(string term)
        {
            var productCodes = from p in db.Products
                               select p.Code;
            string[] items = productCodes.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region[Ajax Change Product]

        // AJAX: /Product/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.Active = product.Active == true ? false : true;
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }

        // AJAX: /Product/ChangeIndex
        [HttpPost]
        public ActionResult ChangeIndex(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.Index = product.Index == true ? false : true;
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái đã được thay đổi.";
            return Json(results);
        }
        // AJAX: /Product/ChangeStatus
        [HttpPost]
        public ActionResult ChangeStatus(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.Status = product.Status == short.Parse("1") ? short.Parse("0") : short.Parse("1");
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái đã được thay đổi.";
            return Json(results);
        }

        // AJAX: /Product/ChangeProduct
        [HttpPost]
        public ActionResult ChangeProduct(int id, string ord, string pricePromotion, string priceRetail, string code, string name, string num, string Weight)
        {
            var results = "";
            var product = db.Products.Find(id);
            if (product != null)
            {
                if (ord != null)
                {
                    product.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }

                if (pricePromotion != null)
                {
                    product.PricePromotion = Int32.Parse(pricePromotion);
                    results = "Giá bán buôn đã được thay đổi.";
                }

                if (priceRetail != null)
                {
                    product.PriceRetail = Int32.Parse(priceRetail);
                    results = "Giá bán đã được thay đổi.";
                }

                if (code != null)
                {
                    product.Code = code;
                    results = "Mã sản phẩm đã được thay đổi.";
                }

                if (name != null)
                {
                    product.Name = name;
                    results = "Tên sản phẩm đã được thay đổi.";
                }

                if (num != null)
                {
                    product.Num = Int32.Parse(num);
                    results = "Số lượng sản phẩm đã được thay đổi.";
                }
                if (Weight != null)
                {
                    product.Weight = Int32.Parse(Weight);
                    results = "Trọng lượng sản phẩm đã được thay đổi.";
                }

            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }

        #endregion

        #region[Ajax LoadProductAmount]

        // AJAX: /Product/LoadProductAmount
        [HttpPost]
        public ActionResult LoadProductAmount(string productAmount, string sortName)
        {

            if (productAmount != null)
            {
                Session["ProductAmount"] = productAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("ProductIndexot");
        }

        #endregion

        #region[Ajax Sort Product]

        // AJAX: /Product/SortByName
        [HttpPost]
        public ActionResult SortByName(string sortName, string currentProductAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentProductAmount != null)
                {
                    Session["ProductAmount"] = currentProductAmount;
                }
            }
            return RedirectToAction("ProductIndexot");
        }

        #endregion


        #endregion
    }
}
