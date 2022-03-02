using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Areas.Admin.Controllers
{
    public class AdminFeedbackController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Admin/AdminFeedback
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var models = db.Feedbacks.OrderByDescending(p => p.FeedbackDate).ToList();
            return View(models);
        }

        public ActionResult Details(int id)
        {
            var model = db.Feedbacks.Find(id);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = db.Feedbacks.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var item = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "AdminFeedback");
        }

        public ActionResult Edit(int id)
        {
            var model = db.Feedbacks.Find(id); 
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(Feedback feedback, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                feedback.Images = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + feedback.Images);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                
                db.Entry(feedback).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "AdminFeedback");
            }
            

            return View(feedback);
        }
    }
}