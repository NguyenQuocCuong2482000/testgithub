using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Controllers
{
    public class CartController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            List<CartItem> list = (List<CartItem>)Session["cartSession"];
            return View(list);
        }
        public ActionResult AddItem(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var cart = Session["cartSession"];
            List<CartItem> list = new List<CartItem>();
            //cart is null
            if (cart == null)
            {
                Product product = db.Products.Where(x => x.ProductID == id).SingleOrDefault();
                CartItem item = new CartItem();
                item.product = product;
                item.quantity = 1;
                list.Add(item);
                Session["cartSession"] = list;
            }
            else //cart != null
            {
                list = (List<CartItem>)Session["cartSession"];
                if (list.Exists(x => x.product.ProductID == id)) //exist
                {
                    foreach (var a in list)
                    {
                        if (a.product.ProductID == id)
                            a.quantity = a.quantity + 1;
                    }
                    Session["cartSession"] = list;
                }
                else // not exist
                {
                    Product product = db.Products.Where(x => x.ProductID == id).SingleOrDefault();
                    CartItem item = new CartItem();
                    item.product = product;
                    item.quantity = 1;
                    list.Add(item);
                    Session["cartSession"] = list;
                }
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult UpdateItem(int id, int quantity)
        {
            List<CartItem> list = (List<CartItem>)Session["cartSession"];
            if (quantity != 0)
                list.Where(p => p.product.ProductID.Equals(id)).FirstOrDefault().quantity = quantity;
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult DeleteItem(int id)
        {
            List<CartItem> list = (List<CartItem>)Session["cartSession"];
            CartItem item = list.Where(p => p.product.ProductID.Equals(id)).FirstOrDefault();
            list.Remove(item);
            Session["cartSession"] = list;
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Order()
        {
            if (Session["customer"] == null)
                return RedirectToAction("Index", "Login");
            else
            {

                // Order product    
                Orderpro od = new Orderpro();
                od.OrderID = db.Orderproes.OrderByDescending(p => p.OrderID).First().OrderID + 1;
                od.OrderDate = DateTime.Now;
                od.CustomerID = (int)Session["customer"];
                od.Status = 0;
                db.Orderproes.Add(od);
                db.SaveChanges();  // từ trên xuống đây là lưu vào bảng Orderpro
                List<CartItem> list = (List<CartItem>)Session["cartSession"]; // trong cart nội dung được thể hiện theo danh sach
                string message = "";
                float sum = 0;
                foreach (CartItem item in list)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderID = od.OrderID;
                    orderDetail.ProductID = item.product.ProductID;
                    orderDetail.Quantity = item.quantity;
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                  
                    message += "<br/> <b>Mã ID phòng:</b> " + item.product.ProductID;
                    message += "<br/> <b>Tên:</b> " + item.product.ProductName;
                    message += "<br/> <b>Địa chỉ:</b> " + item.product.Address;
                    message += "<br/> <b>Diện tích:</b> " + item.product.DienTich;
                    message += "<br/> <b>Thông tin phòng:</b> " + item.product.Description;
                    message += "<br/> <b>số tháng thuê:</b> " + item.quantity;
                    message += "<br/> <b>Giá thuê 1 tháng:</b> " + item.product.Price ;
                    message += "<br/> <b>Tổng giá thuê:</b> " + String.Format("{0:0,0 VND}", item.quantity * item.product.Price);
                    sum += (float)(item.quantity * item.product.Price);
                    message += "<br/>";
                }
                //message += "<br> Total Money: " + String.Format("{0:0,0 VND}", sum);
                //Send to customer
                Customer ct = db.Customers.Find(od.CustomerID);
                SendEmail(ct.Email, "Truy cập Phòng trọ vip wa link: http://nguyenquoccuong-001-site1.ftempurl.com/", message);
                Session["cartSession"] = null;   // lưu xong xóa hết trong Ghim hoặc giỏ hàng
                return RedirectToAction("Index", "Home");
            }

        }



        public void SendEmail(string address, string subject, string message)
        {
            string email = "thanh2482000@gmail.com";
            string password = "Cuong@0918457267";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
    }
}