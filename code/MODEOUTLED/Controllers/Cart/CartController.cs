using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using MODEOUTLED.ViewModels;


namespace MODEOUTLED.Controllers.Cart
{
    public class CartController : Controller
    {
        wwwEntities db = new wwwEntities();
        [ChildActionOnly]
        public ActionResult CartIcon()
        {
            return PartialView();
        }

        #region[View gio hang Top]
        [HttpPost]
        public JsonResult command(string co, string color, string size)
        {
            if (color == null || color == "") { color = "0"; }
            if (size == null || size == "") { size = "0"; }
            string anh = "";
            var re = co.Replace("\":", ":").Replace(",\"", ":").Replace("{\"", ":").Replace("}", ":");
            var tr = re.Split(':');
            string chuoi = "";
            int count = 0;
            List<int> numCart = new List<int>();
            List<Product> obj = new List<Product>();
            chuoi += "<ul><li></li>";
            for (int i = 0; i < tr.Length; i++)
            {
                if (i == 0 || i == (tr.Length - 1))
                { }
                else if (i % 2 != 0)
                {
                    int idpd = int.Parse(tr[i]);
                    var list = db.Products.Where(m => m.Id == idpd).FirstOrDefault();
                    if (list != null)
                    {
                        chuoi += "<li class='proCart clearfix'>";
                        chuoi += "<img src='" + list.Image + "'/>";
                        chuoi += "<a href='/Home/Detail/" + list.Tag + "'>" + list.Name + "</a><p>Số lượng mua: " + tr[(i + 1)] + "</p>";
                        chuoi += "</li>";
                    }
                    obj.Add(list);
                    count += Convert.ToInt32(tr[(i + 1)]);
                    numCart.Add(Convert.ToInt32(tr[(i + 1)]));
                }
            }
            Session["proId"] = obj;
            chuoi += "<li><p class='pCartView'><a class='btn-view-cart' href='/Home/checkout'>Xem đầy đủ giỏ hàng (" + count + " sản phẩm)</a></p></li>";
            chuoi += "<ul>";
            Session["count"] = numCart;
            AddToCart(tr, color, size);
            string tong = "<a href='/Home/checkout' title='Giỏ hàng của bạn hiện tại có " + count.ToString() + " sản phẩm'>(" + count.ToString() + ")</a>";
            return Json(new { success = chuoi, html = tong });
        }
        #endregion
        #region[Them moi sp vao gio hang]
        int cartTotal;
        ShoppingCartViewModel shoppCart;
        public void AddToCart(Array tr, string color, string size)
        {
            if (color == null || color == "") { color = "0"; }
            if (size == null || size == "") { size = "0"; }
            string NameColor = "";
            if (color != "0")
            {
                int _color = int.Parse(color);
                var _ds = db.Colors.FirstOrDefault(s => s.Id == _color);
                if (_ds != null)
                {
                    NameColor = _ds.Name.ToString();
                }
                
            }

            string NameSize = "";

            if (size != "0")
            {
                int _size = int.Parse(size);
                var _ds = db.Sizes.FirstOrDefault(s => s.Id == _size);
                if (_ds != null)
                {
                    NameSize = _ds.Name.ToString();
                }
            }

            var soluong = (List<int>)Session["count"];//so luong sp co trong gio hang tuong ung voi tung sp trong Session["proId"]
            var data = (List<Product>)Session["proId"];//luu thong tin sp co trong gio hang
            for (int i = 0; i < data.Count; i++)
            {
                Product obj = data[i];
                if (obj != null)
                {
                    int gia = 0;
                    string gias = "0";
                    if (obj.PriceRetail != null)
                    {
                        gias = obj.PricePromotion.ToString();
                        gia = int.Parse(gias);
                    }
                    int tong = (soluong[i] * gia);
                    int flag = -2;
                    if (Session["ShoppingCart"] == null)
                    {
                        InitShoppingCartSession();
                    }
                    shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                    int dem = GetCartItem(shoppCart, obj.Id, soluong[i]);
                    if (dem == flag)
                    {
                        osCart cartItem;
                        cartItem = new osCart
                        {
                            productId = obj.Id,
                            productImage = obj.Image,
                            productName = obj.Name,
                            productTag = obj.Tag,
                            price = obj.PricePromotion.ToString(),
                            count = soluong[i],
                            proweight = int.Parse(obj.Weight.ToString()),
                            idsize = int.Parse(size),
                            idcolor = int.Parse(color),
                            namecolor = NameColor,
                            namesize = NameSize,
                            total = tong
                        };
                        shoppCart.CartItems.Add(cartItem);
                    }
                    else
                    {
                        if (dem != -1)
                        {
                            shoppCart.CartItems[i].count = soluong[i];
                            shoppCart.CartItems[i].total = Convert.ToInt32(shoppCart.CartItems[i].price) * shoppCart.CartItems[i].count;
                        }
                    }
                }
            }
            if (shoppCart != null)
            {
                if (shoppCart.CartItems.Count > 0)
                {
                    for (int k = 0; k < shoppCart.CartItems.Count; k++)
                    {
                        cartTotal += shoppCart.CartItems[k].total;
                    }
                }
                shoppCart.CartTotal = cartTotal;
                Session["ShoppingCart"] = shoppCart;
            }
        }
        #endregion
        #region[Tao moi gio hang]
        public void InitShoppingCartSession()
        {
            var shoppingCart = new ShoppingCartViewModel();
            Session["ShoppingCart"] = shoppingCart;
        }
        #endregion
        #region[Kiem tra sp ton tai trong gio hang]
        private int GetCartItem(ShoppingCartViewModel cart, int key, int count)
        {
            int found = -2;
            if (cart.CartItems.Count > 0)
            {
                for (int i = 0; i < cart.CartItems.Count; i++)
                {
                    if (cart.CartItems[i].productId == key && cart.CartItems[i].count == count)
                    {
                        found = -1;
                    }
                    else if (cart.CartItems[i].productId == key && cart.CartItems[i].count != count)
                    {
                        found = 0;
                    }
                }
            }
            return found;
        }
        #endregion
        #region[Xoa sp khoi gio hang]
        [HttpPost]
        public void RemoveFromCart(int id)
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                if (shoppCart.CartItems[i].productId == id)
                {
                    shoppCart.CartItems.RemoveAt(i);
                    break;
                }
            }
            if (shoppCart.CartItems.Count > 0)
            {
                for (int j = 0; j < shoppCart.CartItems.Count; j++)
                {
                    cartTotal += shoppCart.CartItems[j].total;
                }
                shoppCart.CartTotal = cartTotal;
            }
            else
            {
                shoppCart.CartTotal = 0;
            }

            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Cap nhat so luong trong gio hang]
        [HttpPost]
        public void UpdateCartCountItem(int id, int cartCount)
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                if (shoppCart.CartItems[i].productId == id)
                {
                    shoppCart.CartItems[i].count = cartCount;
                    shoppCart.CartItems[i].total = Convert.ToInt32(shoppCart.CartItems[i].price) * cartCount;
                    break;
                }
            }
            for (int j = 0; j < shoppCart.CartItems.Count; j++)
            {
                cartTotal += shoppCart.CartItems[j].total;
            }
            shoppCart.CartTotal = cartTotal;
            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Them vao gio hang Top]
        [HttpPost]
        public ActionResult UpdateTopCart(string co, int type, string color, string size)
        {
            string anh = "";
            var re = co.Replace("\":", ":").Replace(",\"", ":").Replace("{\"", ":").Replace("}", ":");
            var tr = re.Split(':');
            string chuoi = "";
            int count = 0;
            List<int> numCart = new List<int>();
            List<Product> obj = new List<Product>();

            //chuoi += "<ul>";
            //for (int i = 0; i < tr.Length; i++)
            //{
            //    if (i == 0 || i == (tr.Length - 1))
            //    { }
            //    else if (i % 2 != 0)
            //    {
            //        int idpd = int.Parse(tr[i]);
            //        var list = db.Products.Where(m => m.Id == idpd).FirstOrDefault();
            //        if (list != null)
            //        {
            //            chuoi += "<li class='proCart clearfix'>";
            //            chuoi += "<img src='" + list.Image + "'/>";
            //            chuoi += "<a href='/Home/Detail/" + list.Tag + "'>" + list.Name + "</a><p>Số lượng mua: " + tr[(i + 1)] + "</p>";
            //            chuoi += "</li>";
            //        }
            //        obj.Add(list);
            //        count += Convert.ToInt32(tr[(i + 1)]);
            //        numCart.Add(Convert.ToInt32(tr[(i + 1)]));
            //    }
            //}
            //Session["proId"] = obj;
            //chuoi += "<li><p class='pCartView'><a class='btn-view-cart' href='/Home/checkout'>Xem đầy đủ giỏ hàng (" + count + " sản phẩm)</a></p></li>";
            //chuoi += "<ul>";
            //Session["count"] = numCart;
            //AddToCart(tr, color, size);
            //string tong = "<a href='/Home/checkout' title='Giỏ hàng của bạn hiện tại có " + count.ToString() + " sản phẩm'>(" + count.ToString() + ")</a>";
            //return Json(new { success = chuoi, html = tong });

            chuoi += "<ul><li></li>";
            for (int i = 0; i < tr.Length; i++)
            {
                if (i == 0 || i == (tr.Length - 1))
                { }
                else if (i % 2 != 0)
                {
                    int idpd = int.Parse(tr[i]);
                    var list = db.Products.Where(m => m.Id == idpd).FirstOrDefault();
                    if (list != null)
                    {
                        chuoi += "<li class='proCart clearfix'>";
                        chuoi += "<img src='" + list.Image + "'/>";
                        chuoi += "<a href='/Home/Detail/" + list.Tag + "'>" + list.Name + "</a><p>Số lượng mua: " + tr[(i + 1)] + "</p>";
                        //chuoi += "<a id=\"btnAddCart\" href=\"javascript:;\" class=\"delete-cart\"><span id=\"txt\">Thêm vào giỏ</span></a>";
                        chuoi += "</li>";
                    }
                    obj.Add(list);
                    count += Convert.ToInt32(tr[(i + 1)]);
                    numCart.Add(Convert.ToInt32(tr[(i + 1)]));
                }
            }
            Session["proId"] = obj;
            chuoi += "<li><p class='pCartView'><a class='btn-view-cart' href='/Home/checkout'>Xem đầy đủ giỏ hàng (" + count + " sản phẩm)</a></p></li>";
            chuoi += "<ul>";
            Session["count"] = numCart;
            AddToCart(tr, color, size);
            string tong = "<a href='/Home/checkout' title='Giỏ hàng của bạn hiện tại có " + count.ToString() + " sản phẩm'>(" + count.ToString() + ")</a>";

            if (type == 0)
            {
                return Json(new { success = chuoi, html = tong });
            }
            else
            {
                return Redirect("/Home/checkout");
            }
        }
        #endregion

        #region[Cat chuoi text de hien thi]
        protected string FormatContentNews(string value, int count)
        {
            string _value = value;
            if (_value.Length >= count)
            {
                string ValueCut = _value.Substring(0, count - 3);
                string[] valuearray = ValueCut.Split(' ');
                string valuereturn = "";
                for (int i = 0; i < valuearray.Length - 1; i++)
                {
                    valuereturn = valuereturn + " " + valuearray[i];
                }
                return valuereturn;
            }
            else
            {
                return _value;
            }
        }
        #endregion
        #region[view chon lua dang nhap hoac mua hang luon k can tai khoan dk]
        public ActionResult notLogon()
        {
            return View();
        }
        #endregion
    }
}
