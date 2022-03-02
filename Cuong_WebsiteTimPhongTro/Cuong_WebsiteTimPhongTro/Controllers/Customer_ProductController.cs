using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Controllers
{
    public class Customer_ProductController : Controller
    {
        // GET: Customer_Product
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        
        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            if (Session["customer"] == null)
                return RedirectToAction("Index", "Login");
            int customerID = (int)Session["customer"];
            var models = db.Products.Where(p => p.CustomerID == customerID).OrderByDescending(p => p.DateOfManufacture).ToList();
            return View(models);
        }

        public ActionResult Create()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            ViewBag.types = new SelectList(db.TypeProducts.ToList(), "typeID", "typeName");
            ViewBag.manus = new SelectList(db.Manufacturers.ToList(), "manufacturerID", "manufacturerName");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product, HttpPostedFileBase Images, HttpPostedFileBase Images1, HttpPostedFileBase Images2, HttpPostedFileBase Images3)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                product.Images = Images.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images);
                Images.SaveAs(urlImage);
            }
            if (Images1 != null && Images1.ContentLength > 0)
            {
                product.Images1 = Images1.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images1);
                Images1.SaveAs(urlImage);
            }
            if (Images2 != null && Images2.ContentLength > 0)
            {
                product.Images2 = Images2.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images2);
                Images2.SaveAs(urlImage);
            }
            if (Images3 != null && Images3.ContentLength > 0)
            {
                product.Images3 = Images3.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images3);
                Images3.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                product.CustomerID = (int)Session["customer"];
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listManufacturers = new SelectList(db.Manufacturers, "manufacturerID", "manufacturerName", product.ManufacturerID);
            ViewBag.listTypes = new SelectList(db.TypeProducts, "typeID", "typeName", product.TypeID);
            return View(product);
        }
        public ActionResult Details(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var model = db.Products.Find(id);
            return View(model);
        }


        public ActionResult Delete(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var model = db.Products.Find(id);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var item = db.Products.Find(id);
            var chitiet = db.OrderDetails.ToList();
            int checkx = 0;
            foreach (OrderDetail i in chitiet)
            {
                if (i.ProductID == id)
                {
                    checkx++;

                }
            }
            if (checkx > 0)
            {
                TempData["check"] = "Không xóa được sản phẩm do sản phẩm đã được order, vui lòng kiểm tra lại";
                return RedirectToAction("Delete", "Customer_Product");
            }
            else
            {
                db.Products.Remove(item);
                db.SaveChanges();
                TempData["check"] = "Bạn đã xóa  sản phẩm thành công";
                return RedirectToAction("Index", "Customer_Product");
            }
        }



        public ActionResult Edit(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            
            var model = db.Products.Find(id);
            ViewBag.types = new SelectList(db.TypeProducts.ToList(), "typeID", "typeName");
            ViewBag.manus = new SelectList(db.Manufacturers.ToList(), "manufacturerID", "manufacturerName");
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(Product product, HttpPostedFileBase Images, HttpPostedFileBase Images1, HttpPostedFileBase Images2, HttpPostedFileBase Images3)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                product.Images = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + product.Images);
                Images.SaveAs(urlImage);
            }
            if (Images1 != null && Images1.ContentLength > 0)
            {
                product.Images1 = Images1.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images1);
                Images1.SaveAs(urlImage);
            }
            if (Images2 != null && Images2.ContentLength > 0)
            {
                product.Images2 = Images2.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images2);
                Images2.SaveAs(urlImage);
            }
            if (Images3 != null && Images3.ContentLength > 0)
            {
                product.Images3 = Images3.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + product.Images3);
                Images3.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                product.CustomerID = (int)Session["customer"];
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "Customer_Product");
            }
            ViewBag.listmanufacturers = new SelectList(db.Manufacturers, "manufacturerID", "manufacturerName", product.ManufacturerID);
            ViewBag.listTypes = new SelectList(db.TypeProducts, "typeID", "typeName", product.TypeID);

            return View(product);
        }

        public ActionResult EditProfile(int id)
        {
            
            ViewBag.menu = db.TypeProducts.ToList();
            id = (int)Session["customer"];
            var models = db.Customers.Find(id);
            return View(models);
        }
        [HttpPost]
        public ActionResult EditProfile(Customer customer, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                customer.image = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + customer.image);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                customer.PassWord = Encode.EncodeMD5(customer.PassWord);
                db.SaveChanges();
                return RedirectToAction("Index", "Customer_Product");
            }
            return View(customer);
        }
    }
}