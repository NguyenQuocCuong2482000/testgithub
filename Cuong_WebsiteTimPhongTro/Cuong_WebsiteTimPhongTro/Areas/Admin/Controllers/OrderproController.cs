using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Areas.Admin.Controllers
{
    public class OrderproController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Admin/Orderpro
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var models = db.Orderproes.ToList();
            return View(models);
        }
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            ViewBag.od = db.Orderproes.Find(id);
            var models = db.OrderDetails.Where(p => p.OrderID == id).ToList();
            return View(models);
        }

        

        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var model = db.Orderproes.Find(id);
            model.StaffId = (int)Session["admin"];
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Orderpro orderpro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderpro).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Orderpro");
            }
            return View(orderpro);
        }
    }
}