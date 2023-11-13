﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DU_AN.Models;

namespace DU_AN.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCard
        DBQLHiglandEntities database = new DBQLHiglandEntities();      
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmtyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }

        //Tạo mới giỏ hàng
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        //Thêm product mới vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.SingleOrDefault(s => s.IDProduct == id); //lấy pro theo ID 
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        //Cập nhận lại số lượng sanr phẩm và tính lại tiền
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        //Xóa dòng sản phẩm trong giỏ hàng
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        //Tổng số lượng sản phẩm có trong giỏ hàng
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }

        //Xử lý số lượng tồn trong bảng Product
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro(); //Bảng hóa đơn sản phẩm
                _order.DateOrder = DateTime.Now;
                _order.AddressDelivery = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail(); //Lưu dòng sản phẩm vào bảng chi tiết hóa đơn
                    _order_detail.IDOrder = _order.IDOrder;
                    _order_detail.IDProduct = item._product.IDProduct;
                    _order_detail.UnitPrice = (double)item._product.PriceProduct;
                    _order_detail.Quantity = item._quantity;
                    database.OrderDetails.Add(_order_detail);

                    //xử lý cập nhập lại số lượng tồn trong bảng Product
                    foreach(var p in database.Products.Where(s=>s.IDProduct==_order_detail.IDProduct)) //Lấy ID Product đang có trong giỏ hàng
                    {
                        var update_quan_pro = p.QuantityProduct - item._quantity; //số lượng tồn mới=số lượng tồn-số lượng đã mua
                        p.QuantityProduct = update_quan_pro; //thực hiện cập nhập lại số lượng tồn cho cột Quantity của bảng Product
                    }
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Error checkout. Please check infomation of Customer...Thanks");
            }
        }

        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}