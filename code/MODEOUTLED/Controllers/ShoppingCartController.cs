using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class ShoppingCartController : Controller
    {
         wwwEntities db = new wwwEntities();

        public ActionResult Cart()
        {
            return View();
        }

    }
}
