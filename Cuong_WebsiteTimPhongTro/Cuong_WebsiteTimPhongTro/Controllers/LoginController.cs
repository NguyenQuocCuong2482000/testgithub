using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;  

namespace Cuong_WebsiteTimPhongTro.Controllers
{
    public class LoginController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            string pass = Encode.EncodeMD5(password);
            var rs = db.Customers.Where(p => p.UserName.Equals(username) && p.PassWord.Equals(pass)).FirstOrDefault();
            if (rs != null)
            {
                Session["customer"] = rs.CustomerID;
                return RedirectToAction("Index", "Home");
            }
            else
                TempData["check"] = "Sai tên tài khoản hoặc mật khẩu!";
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Logout()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            Session["customer"] = null;
            return Redirect("/");
        }





        public ActionResult Register()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                customer.image = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + customer.image);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                customer.PassWord = Encode.EncodeMD5(customer.PassWord);
                db.Customers.Add(customer);
                TempData["check"] = "Sai tên tài khoản hoặc mật khẩu!";
                db.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
        }
    }
}