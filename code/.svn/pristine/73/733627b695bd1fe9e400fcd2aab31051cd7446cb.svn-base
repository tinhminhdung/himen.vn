using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace onsoft
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");         
            routes.MapRoute( name: "Home", url: "", defaults: new { controller = "Home", action = "Index" });
            routes.MapRoute("HomePage", "{controller}/{action}/{page}", new { controller = "Home", action = "Index", page = UrlParameter.Optional }, new { controller = "^H.*", action = "^Index$" });
            routes.MapRoute("Product", "san-pham/{*catchall}", new { controller = "Default", action = "Album", tag = UrlParameter.Optional }, new { controller = "^D.*", action = "^Album$" });
            routes.MapRoute("ProductItem", "danh-muc/{tag}/{*catchall}", new { controller = "Default", action = "Albums", tag = UrlParameter.Optional }, new { controller = "^D.*", action = "^Albums$" });
            routes.MapRoute("ProductItem_r", "phu-dat-mart/{tag}/{r}/{*catchall}", new { controller = "Default", action = "Albums", tag = UrlParameter.Optional }, new { controller = "^D.*", action = "^Albums$" });
            routes.MapRoute("ProductItem_m", "mua-nhieu/{tag}/{r}/{*catchall}", new { controller = "Default", action = "muanhieu", tag = UrlParameter.Optional }, new { controller = "^D.*", action = "^muanhieu$" });
            routes.MapRoute("ProductItempage", "danh-muc/{tag}/{page}/{*catchall}", new { controller = "Default", action = "Albums", tag = UrlParameter.Optional }, new { controller = "^D.*", action = "^Albums$" });
            routes.MapRoute( name: "page", url: "gioi-thieu/{tag}", defaults: new { controller = "Default", action = "ilovestylepage" });
            routes.MapRoute(name: "buy", url: "Home/checkout", defaults: new { controller = "Home", action = "checkout" });
            routes.MapRoute(name: "Gencode", url: "Gen/Gencode", defaults: new { controller = "Gen", action = "Gencode" });
            routes.MapRoute(name: "cs", url: "Ps", defaults: new { controller = "My_Partialview", action = "SoftCS" });
            routes.MapRoute(name: "bando", url: "bando", defaults: new { controller = "Home", action = "bando" });
            #region[Admin]
            routes.MapRoute("Admin", "Admins/admins/{*catchall}", new { controller = "Admins", action = "admins", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^admins$" });
            routes.MapRoute("AdminDefault", "Admins/Default/{*catchall}", new { controller = "Admins", action = "Default", tag = UrlParameter.Optional }, new { controller = "^A.*", action = "^Default$" });
            #endregion
            routes.MapRoute("Detail", "thong-tin/{tag}/{*catchall}", new { controller = "Home", action = "Detail", tag = UrlParameter.Optional }, new { controller = "^H.*", action = "^Detail$" });
            routes.MapRoute("tintuc", "tin-tuc/{tag}/{*catchall}", new { controller = "NewsView", action = "List", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^List$" });
            routes.MapRoute("tintucpage", "tin-tuc/{tag}/{page}/{*catchall}", new { controller = "NewsView", action = "List", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^List$" });
            routes.MapRoute("danhmuctin", "danh-muc-tin/{tag}/{*catchall}", new { controller = "NewsView", action = "ListNews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^ListNews$" });
            routes.MapRoute("danhmuctinpage", "danh-muc-tin/{tag}/{page}/{*catchall}", new { controller = "NewsView", action = "ListNews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^ListNews$" });
            routes.MapRoute("NewsDetail", "new/{tag}/{*catchall}", new { controller = "NewsView", action = "NewsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^NewsDetail$" });
            routes.MapRoute( name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}