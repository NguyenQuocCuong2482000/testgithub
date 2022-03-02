using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Controllers
{

    public class FeedbackController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Feedback
        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            if (Session["customer"] == null)
                return RedirectToAction("Index", "Login");
            int customerID = (int)Session["customer"];
            var models = db.Feedbacks.Where(p => p.CustomerID == customerID).OrderByDescending(p => p.FeedbackDate).ToList();
            return View(models);
        }

        public ActionResult Create()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Feedback feedback, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                feedback.Images = Images.FileName;
                string urlImage = Server.MapPath("~/Content/images/" + feedback.Images);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                feedback.CustomerID = (int)Session["customer"];
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(feedback);
        }

        public ActionResult Details(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var model = db.Feedbacks.Find(id);
            return View(model);
        }
    }
}