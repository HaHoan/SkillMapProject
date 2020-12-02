using SkillMapProject.Helper;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Member member)
        {
            using (var db = new UMC_SKILLEntities())
            {
                var user = db.Members.Where(m => m.Code == member.Code && m.Pass == member.Pass).FirstOrDefault();
                if (user != null)
                {
                    SessionHelper.Set(Constant.SESSION_LOGIN, user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Kiểm tra lại Code hoặc Password!");
                    return View("Index");
                }
            }
        }
        public ActionResult Logout()
        {
            if (SessionHelper.Get<Member>(Constant.SESSION_LOGIN) != null)
            {
                SessionHelper.Remove(Constant.SESSION_LOGIN);
            }
            return View("Index");
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}