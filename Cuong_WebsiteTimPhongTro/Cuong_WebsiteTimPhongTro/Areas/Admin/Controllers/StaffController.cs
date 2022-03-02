using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;

namespace Cuong_WebsiteTimPhongTro.Areas.Admin.Controllers
{
    public class StaffController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: Admin/Staff
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var models = db.Staffs.ToList();
            return View(models);
        }

        public ActionResult Create()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");

            return View();
        }
        [HttpPost]

        public ActionResult Create(Staff staff, HttpPostedFileBase Images)
        {

            if (Images != null && Images.ContentLength > 0)
            {
                staff.image = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + staff.image);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                staff.password = Encode.EncodeMD5(staff.password);
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        public ActionResult Details(int id)
        {
            var model = db.Staffs.Find(id);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = db.Staffs.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var item = db.Staffs.Find(id);
            db.Staffs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Staff");
        }

        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");

            var model = db.Staffs.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Staff staff, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                staff.image = Images.FileName;
                string urlImage = Server.MapPath("~/Content/Images/" + staff.image);
                Images.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                staff.password = Encode.EncodeMD5(staff.password);
                db.SaveChanges();
                return RedirectToAction("Index", "Staff");
            }
            return View(staff);
        }
    }
}