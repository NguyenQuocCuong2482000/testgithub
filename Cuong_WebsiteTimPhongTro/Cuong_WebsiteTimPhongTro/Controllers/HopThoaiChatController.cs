using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuong_WebsiteTimPhongTro.Models;
namespace Cuong_WebsiteTimPhongTro.Controllers
{

    public class HopThoaiChatController : Controller
    {
        public OnlineTimPhongTroEntities db = new OnlineTimPhongTroEntities();
        // GET: HopThoaiChat
        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }
    }
}