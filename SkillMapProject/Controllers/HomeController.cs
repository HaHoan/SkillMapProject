using SkillMapProject.Helper;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (SessionHelper.Get<Member>(Constant.SESSION_LOGIN) == null)
            {
                return RedirectToAction("Index", "Login");
            } 
            return View();
        }

    }
}