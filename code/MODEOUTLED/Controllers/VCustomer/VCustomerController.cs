using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers.VCustomer
{
    public class VCustomerController : Controller
    {
        //
        // GET: /Members/
         wwwEntities db = new wwwEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var encryptPassword = StringClass.Encrypt(password);
            var cus = db.Customers.Where(c => c.Email.Equals(email) && c.Password.Equals(encryptPassword)).ToList();
            if (cus.Count > 0)
            {
                Session["User-Email"] = email;
                Session["Name"] = cus[0].Name;
                Session["uId"] = cus[0].Id.ToString();
                var result = "success";
                return Json(result);
            }
            else
            {
                var result = "E-mail hoặc Password không đúng, nhập lại..";
                return Json(result);
            }

        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult information()
        {
            string Chuoi = "";
            if (Session["Email"] != null)
            {
                string mail=Session["Email"].ToString();
                var cus = db.Customers.Where(c => c.Email == mail).ToList();
                if (cus.Count > 0)
                {
                    float diem = float.Parse(cus[0].Diem.ToString());
                    if (diem < 1000000)
                    {
                        Chuoi += "<a class=\"v0\" href=\"/Pages/quan_ly_don_hang\"><span>Chào,</span>" + cus[0].Name + "</a>";
                    }
                    else if (diem < 2000000)
                    {
                        Chuoi += "<a class=\"v1\" href=\"/Pages/quan_ly_don_hang\"><span>Chào,</span>" + cus[0].Name + "</a>";
                    }
                    else if (diem < 3000000)
                    {
                        Chuoi += "<a class=\"v2\" href=\"/Pages/quan_ly_don_hang\"><span>Chào,</span>" + cus[0].Name + "</a>";
                    }
                    else if (diem < 4000000)
                    {
                        Chuoi += "<a class=\"v3\" href=\"/Pages/quan_ly_don_hang\"><span>Chào,</span>" + cus[0].Name + "</a>";
                    }
                    else if (diem < 5000000)
                    {
                        Chuoi += "<a class=\"v4\" href=\"/Pages/quan_ly_don_hang\"><span>Chào,</span>" + cus[0].Name + "</a>";
                    }
                    else
                    {
                        Chuoi += "<a class=\"v5\" href=\"/Pages/quan_ly_don_hang\"><span>Chào,</span>" + cus[0].Name + "</a>";
                    }
                }
                ViewBag.name = Chuoi;
                return PartialView();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string name, string tel, string address)
        {
            var cus = db.Customers.Where(c => c.Email.Equals(email)).ToList();
            var result = "";
            if (cus.Count > 0)
            {
                result = "E-mail đã tồn tại..";
            }
            else
            {
                Customer customer = new Customer();
                customer.Email = email;
                customer.Password = StringClass.Encrypt(password);
                customer.Name = name;
                if (tel != null)
                {
                    customer.Tel = tel;
                }
                if (address != null)
                {
                    customer.Address = address;
                }
                customer.SDate = DateTime.Now;

                db.Entry(customer).State = System.Data.EntityState.Added;
                db.SaveChanges();

                Session["User-Email"] = email;
                result = "success";
            }

            return Json(result);
        }

    }
}
