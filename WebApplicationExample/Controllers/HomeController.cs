using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationExample.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult AutoComplete(string prefix)
        {
            var emails = StaticCache.ContactsNameList.Search(prefix);
            return Json(emails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}