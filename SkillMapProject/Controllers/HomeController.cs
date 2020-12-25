using SkillMapProject.Helper;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);

            if (user != null)
            {
                using (var db = new UMC_SKILLEntities())
                {
                    if (Constant.SPECIAL_DEPT.Contains(user.Dept))
                    {
                        // nếu thuộc phòng ban edu thì được thêm cho tất cả nhân viên
                        ViewBag.Users = db.Members.ToList();
                    }
                    else
                    {
                        // còn không chỉ được thêm cho nhân viên phòng mình
                        ViewBag.Users = db.Members.Where(m => m.Dept == user.Dept).ToList();
                    }

                    // Chỉ được thêm những chứng chỉ của phòng ban mình
                    ViewBag.SkillLevels = db.Skills.Include("SkillLevels").ToList();
                }
                return View();
            }
            else return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult AddCertificatesForStaff()
        {
            if (SessionHelper.IsLogIn())
            {
                return View();
            }
            else return RedirectToAction("Index", "Login");
        }

        public JsonResult GetSkillOfStaffList()
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {

                var listUser = db.Members.ToList();
                var listCerByUser = new List<CertificateByUser>();
                foreach (var u in listUser)
                {
                    var skills = db.Certifications.Include("Skill").Where(m => m.UserID == u.ID).ToList();
                    var allSkill = db.Skills.ToList();
                    Dictionary<Skill, Certification> dics = new Dictionary<Skill, Certification>();
                    var cerByUser = new CertificateByUser()
                    {
                        userID = u.ID,
                        Code = u.Code,
                        FullName = u.Name,
                        Dept = u.Dept,
                        ListSkills = skills
                    };
                    foreach(var skill in allSkill)
                    {
                        var cer = skills.Where(m => m.SkillID == skill.ID).FirstOrDefault();
                        dics.Add(skill, cer);
                    }
                    cerByUser.dics = dics;
                    listCerByUser.Add(cerByUser);
                }
                if (listCerByUser != null && listCerByUser.Count > 0)
                {
                    return Json(new
                    {
                        body = Utils.ConvertViewToString("~/Views/Home/_AllSkillOfStaff.cshtml", listCerByUser, ViewData, ControllerContext),
                    },
                        JsonRequestBehavior.AllowGet);
                }
                else return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult AddSkillForStaff(int ID = 0)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);

            if (user != null)
            {
                using (var db = new UMC_SKILLEntities())
                {
                    // Chỉ được thêm những chứng chỉ của phòng ban mình
                    ViewBag.Skills = db.Skills.Where(m => m.Dept == user.Dept).ToList();
                    if (ID == 0)
                    {
                        return View();
                    }
                    else
                    {
                        var cerInDb = db.Certifications.Where(m => m.ID == ID).FirstOrDefault();
                        GetHistories(cerInDb, db);
                        return View(cerInDb);
                    }

                }
            }
            else return RedirectToAction("Index", "Login");
        }

        private void GetHistories(Certification cerInDb, UMC_SKILLEntities db)
        {
            //var list = db.Histories.Include("Member").Include("Member1").Include("Member2").Where(m => m.UserID == cerInDb.UserID && m.SkillID == cerInDb.SkillID).ToList();
            //if (list != null && list.Count > 0) ViewBag.History = list;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSkillForStaff(Certification cer)
        {
            if (SessionHelper.IsLogIn())
            {
                try
                {
                    using (var db = new UMC_SKILLEntities())
                    {
                        var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                        var member = db.Members.Where(m => m.Code == cer.Code).FirstOrDefault();
                        if (member == null)
                        {
                            ModelState.AddModelError("Error", "Không tồn tại user " + cer.Code);
                            ViewBag.Skills = db.Skills.Where(m => m.Dept == user.Dept).ToList();
                            return View(cer);
                        }
                        else
                        {
                            cer.UserID = member.ID;
                            if (cer.ID == 0)
                            {
                                var cerInDb = db.Certifications.Where(m => m.Code == cer.Code && m.SkillID == cer.SkillID).FirstOrDefault();
                                if (cerInDb != null)
                                {
                                    ModelState.AddModelError("Error", "Nhân viên " + cer.Code + " Đã học môn này rồi!");
                                    ViewBag.Skills = db.Skills.Where(m => m.Dept == user.Dept).ToList();
                                    return View(cer);
                                }
                                cer.Updator = user.ID;
                                cer.UpdateTime = DateTime.Now;
                                cer.NguoiChamDiem = user.ID;
                                db.Certifications.Add(cer);
                                db.SaveChanges();
                            }
                            else
                            {
                                var cerInDb = db.Certifications.Where(m => m.ID == cer.ID).FirstOrDefault();
                                if (cerInDb != null)
                                {
                                    cerInDb.Updator = user.ID;
                                    cerInDb.UpdateTime = DateTime.Now;
                                    cerInDb.NguoiChamDiem = user.ID;
                                    cerInDb.Mark = cer.Mark;
                                    cerInDb.Note = cer.Note;
                                    cerInDb.Result = cer.Result;
                                    cerInDb.NgayThiXacNhan = cer.NgayThiXacNhan;
                                    cerInDb.NgayThiThucTe = cer.NgayThiThucTe;
                                    cerInDb.TypeSkill = cer.TypeSkill;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    ModelState.AddModelError("Error", "Không tồn tại chứng chỉ này cho nhân viên!");
                                    ViewBag.Skills = db.Skills.Where(m => m.Dept == user.Dept).ToList();
                                    return View(cer);
                                }
                            }

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", "Có lỗi xảy ra!");
                    return View(cer);
                }

            }
            else return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ThemKiNangChoNV(string userID, string CapDo, string NgayCap, string NgayThiXacNhan, string SkillID)
        {
            try
            {
                var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                if (user != null)
                {
                    using (var db = new UMC_SKILLEntities())
                    {

                        var cer = new Certification();
                        cer.UserID = int.Parse(userID);
                        cer.SkillID = int.Parse(SkillID);
                        var cerInDb = db.Certifications.Where(m => m.UserID == cer.UserID && m.SkillID == cer.SkillID).FirstOrDefault();
                        if (cerInDb != null)
                        {
                            return Json(new { body = "Đã thêm kĩ năng này cho " + cerInDb.Member.Name + " rồi!", }, JsonRequestBehavior.AllowGet);
                        }
                        cer.CapDo = CapDo;
                        cer.NgayCap = DateTime.Parse(NgayCap);
                        cer.NgayThiXacNhan = DateTime.Parse(NgayThiXacNhan);
                        cer.SoLanThi = 1;
                        cer.Mark = "100";
                        cer.Result = "PASS";
                        cer.Updator = user.ID;
                        cer.UpdateTime = DateTime.Now;
                        cer.Code = db.Members.Where(m => m.ID == cer.UserID).FirstOrDefault().Code;
                        cer.TypeSkill = "Skill";
                        db.Certifications.Add(cer);
                        db.SaveChanges();

                    }
                    return Json(new { body = "OK" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {

            }
            return Json(new { body = "NG" }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ViewSkillForStaff(int ID = 0)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);

            if (user != null)
            {
                if (ID != 0)
                {
                    using (var db = new UMC_SKILLEntities())
                    {
                        var u = db.Members.Where(m => m.ID == ID).FirstOrDefault();
                        var skills = db.Certifications.Include("Skill")
                           .Where(m => m.UserID == u.ID)
                           .ToList();
                        var cerByUser = new CertificateByUser()
                        {
                            userID = u.ID,
                            Code = u.Code,
                            FullName = u.Name,
                            Dept = u.Dept,
                            DateEnter = u.DateEnter.ToShortDateString(),
                            ListSkills = skills
                        };
                        if (cerByUser != null)
                        {
                            return View(cerByUser);
                        }

                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Index", "Login");
        }
    }
}