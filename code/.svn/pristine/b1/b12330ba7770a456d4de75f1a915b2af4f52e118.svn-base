using MODEOUTLED.ViewModels;
using onsoft.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onsoft.Controllers
{
    public class BaogiaController : Controller
    {
        wwwEntities db = new wwwEntities();

        #region[MenuIndex]
        public ActionResult DanhsachHienthi()
        {
            #region ShowDropDownList
            var module = db.GroupProducts.OrderBy(m => m.Level).ToList();
            List<SelectListItem> listpage = new List<SelectListItem>();
            listpage.Clear();
            for (int i = 0; i < module.Count; i++)
            {
                listpage.Add(new SelectListItem { Text = StringClass.ShowNameLevel(module[i].Name, module[i].Level), Value = module[i].Id.ToString() });
            }
            ViewBag.LinkModule = listpage;
            #endregion

            var Baogia = db.Baogias.OrderBy(m => m.ID).ToList();
            return View(Baogia);
        }
        #endregion

        public ActionResult ThemMoi()// show ra danh sách
        {
            #region ShowDropDownList
            var module = db.GroupProducts.OrderBy(m => m.Level).ToList();
            List<SelectListItem> listpage = new List<SelectListItem>();
            listpage.Clear();
            for (int i = 0; i < module.Count; i++)
            {
                listpage.Add(new SelectListItem { Text = StringClass.ShowNameLevel(module[i].Name, module[i].Level), Value = module[i].Id.ToString() });
            }
            ViewBag.LinkModule = listpage;
            #endregion

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(FormCollection collection, Baogia obj)// Thêm mới 1 bài viết
        {
            obj.Name = collection["Name"];
            obj.Phone = collection["Phone"];
            obj.Email = collection["Email"];
            obj.Nhomsp = int.Parse(collection["LinkModule"]);
            obj.Conten = collection["Detail"];
            obj.Status = (collection["Active"] == "false") ? 0 : 1;
            db.Entry(obj).State = EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("DanhsachHienthi");

        }

        public ActionResult Editbaogia(int id)//hàm sửa gọi ra giá trị của 1 ID khi sửa 1 bài viết
        {
            var Edit = db.Baogias.First(m => m.ID == id);
            #region Giữ 1 ID của nhóm sản phẩm
            ViewBag.LinkModule = new SelectList(showlistPagerodule(), "value", "text", Edit.Nhomsp);
            #endregion
            return View(Edit);
        }

        #region Sửa DropDownList
        public static List<DropDownList> showlistPagerodule()// Load lại khi sửa DropDownList -->hàm sửa gọi ra giá trị của 1 ID khi sửa 1 bài viết  
        {
            wwwEntities db = new wwwEntities();
            var listg = db.GroupProducts.OrderBy(m => m.Level).ToList();
            List<DropDownList> list = new List<DropDownList>();
            if (listg.Count > 0)
            {
                for (int i = 0; i < listg.Count; i++)
                {
                    list.Add(new DropDownList { value = listg[i].Id.ToString(), text = StringClass.ShowNameLevel(listg[i].Name, listg[i].Level) });
                }
            }
            return list;
        }
        #endregion

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditBaogia(int id, FormCollection collection)//Hàm sửa lại 1 bài và lưu
        {
            var obj = db.Baogias.Find(id);
            obj.Name = collection["Name"];
            obj.Phone = collection["Phone"];
            obj.Email = collection["Email"];
            obj.Nhomsp = int.Parse(collection["LinkModule"]);
            obj.Conten = collection["Detail"];
            obj.Status = (collection["Active"] == "false") ? 0 : 1;
            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhsachHienthi");
        }

        [HttpGet]
        public ActionResult DeleteBaogia(int? id)
        {
            Baogia model = db.Baogias.SingleOrDefault(n => n.ID == id);
            db.Baogias.Remove(model);
            db.SaveChanges();
            return RedirectToAction("DanhsachHienthi");
        }
    }
}
