using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Admin/Product
        public ActionResult Index()
        {
            var models = db.Products.OrderByDescending(p => p.DateOfManufacture).ToList();
            return View(models);
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
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
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listmanufacturers = new SelectList(db.Manufacturers, "manufacturerID", "manufacturerName", product.ManufacturerID);
            ViewBag.listTypes = new SelectList(db.TypeProducts, "typeID", "typeName", product.TypeID);
            return View(product);
        }
        public ActionResult Details(int id)
        {
            var model = db.Products.Find(id);
            return View(model);
        }


        public ActionResult Delete(int id)
        {
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
                return RedirectToAction("Delete", "Product");
            }
            else
            {
                db.Products.Remove(item);
                db.SaveChanges();
                TempData["check"] = "Bạn đã xóa  sản phẩm thành công";
                return RedirectToAction("Index", "Product");
            }
        }



        public ActionResult Edit(int id)
        {

            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var model = db.Products.Find(id);
            ViewBag.types = new SelectList(db.TypeProducts.ToList(), "typeID", "typeName");
            ViewBag.manus = new SelectList(db.Manufacturers.ToList(), "manufacturerID", "manufacturerName");
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(Product product, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                product.Images = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + product.Images);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            ViewBag.listmanufacturers = new SelectList(db.Manufacturers, "manufacturerID", "manufacturerName", product.ManufacturerID);
            ViewBag.listTypes = new SelectList(db.TypeProducts, "typeID", "typeName", product.TypeID);

            return View(product);
        }
    }
}