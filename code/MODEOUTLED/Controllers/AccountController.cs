using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult AccountInfomation()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult ManaCart()
        {
            return View();
        }
        public ActionResult DetailManaCart()
        {
            return View();
        }
    }
}
