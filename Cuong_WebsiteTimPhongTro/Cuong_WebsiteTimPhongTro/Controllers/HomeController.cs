using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;
using PagedList;

namespace Cuong_WebsiteTimPhongTro.Controllers
{
    public class HomeController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();

// phân trang
        public IEnumerable<Product> AllListPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ProductName.Contains(searchString) || x.Address.Contains(searchString) || x.DienTich.Contains(searchString));
            }

            return model.OrderByDescending(x => x.DateOfManufacture).ToPagedList(page, pageSize);
        }

        public IEnumerable<Product> AllListPagingByType(int page, int pageSize, string typeId)
        {
            return db.Products.OrderByDescending(x => x.DateOfManufacture).Where(x => x.TypeID.Equals(typeId)).ToPagedList(page, pageSize);
        }

        public ActionResult Index(string searchString, string id, int page = 1, int pageSize = 9)
        {

            ViewBag.menu = db.TypeProducts.ToList();

            if (id == null)
            {
                var models = AllListPaging(searchString, page, pageSize);
                ViewBag.id = null;
                ViewBag.SearchString = searchString;
                return View(models);
            }
            else
            {
                var models = AllListPagingByType(page, pageSize, id);
                ViewBag.id = id;
                return View(models);
            }
        }

        // từ đây lên trên là phân trang

        public ActionResult Details(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var model = db.Products.Find(id);
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            if (Session["customer"] == null)
                return RedirectToAction("Index", "Login");
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
           
            if (ModelState.IsValid && product != null)
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
        public ActionResult HistoryOrder()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            int cusID = (int)Session["customer"];
            var models = db.Orderproes.Where(p => p.CustomerID == cusID).OrderByDescending(p => p.OrderDate).ToList();
            return View(models);
        }

        public ActionResult DetailsOrder(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            ViewBag.od = db.Orderproes.Find(id);
            var models = db.OrderDetails.Where(p => p.OrderID == id).ToList();
            return View(models);
        }

        

    }
}