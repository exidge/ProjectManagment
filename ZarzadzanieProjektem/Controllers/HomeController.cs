using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZarzadzanieProjektem.Models;
using System.Diagnostics;
namespace ZarzadzanieProjektem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext userDB = new ApplicationDbContext();
            if (userDB.Users.Count() == 0)
            {
                Debug.WriteLine("Debug: Redirecting to first MasterUser creation when no user exist.");
                return RedirectToAction("RegisterFirstUser", "Account");
            }
            if (!Request.LogonUserIdentity.IsAuthenticated)
            {
                Debug.WriteLine("Debug: Redirecting to Login");
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Projects");
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}