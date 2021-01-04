using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class OlympicController : Controller
    {
        // GET: Olympic
        public ActionResult Index()
        {
            using (var db = new UMC_SKILLEntities())
            {
                var list = db.OLYMPICS.Include("Member").ToList();
                if (list != null)
                    return View(list);
                else return View(new List<OLYMPIC>());
            }
        }
    }
}