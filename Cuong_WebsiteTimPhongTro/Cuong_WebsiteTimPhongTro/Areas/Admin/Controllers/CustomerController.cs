using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Admin/Customer
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var models = db.Customers.ToList();
            return View(models);
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Customer customer, HttpPostedFileBase Images)
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
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Customer");
        }

        public ActionResult Details(int id)
        {
            var model = db.Customers.Find(id);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = db.Customers.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var item = db.Customers.Find(id);
            db.Customers.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }

        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var model = db.Customers.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Customer customer, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                customer.image = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + customer.image);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                customer.PassWord = Encode.EncodeMD5(customer.PassWord);
                db.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            return View(customer);
        }
    }
}