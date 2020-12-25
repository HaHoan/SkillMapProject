using SkillMapProject.Helper;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class SkillController : Controller
    {
        [HttpGet]
        public ActionResult ListSkill()
        {
            if (SessionHelper.IsLogIn())
            {
                return View();
            }
            else return RedirectToAction("Index", "Login");
        }

        public JsonResult GetListSkill()
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var list = db.Skills.Include("SkillLevels").Where(m => m.Removed == 0).OrderByDescending(m => m.UpdateTime).ToList();
                if (list != null && list.Count > 0)
                {
                    return Json(new
                    {
                        body = Utils.ConvertViewToString("~/Views/Skill/_AllSkill.cshtml", list, ViewData, ControllerContext),
                    },
                        JsonRequestBehavior.AllowGet);
                }
                else return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DeleteSkill(int ID)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { code = RESULT.ERROR, message = "Phiên đăng nhập hết hạn! Cần đăng nhập lại" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var skill = db.Skills.Where(m => m.ID == ID).FirstOrDefault();
                if (skill != null)
                {
                    skill.Removed = 1;
                    db.SaveChanges();
                    return Json(new
                    {
                        code = RESULT.SUCCESS,
                    },
                        JsonRequestBehavior.AllowGet);
                }
                else return Json(new { code = RESULT.ERROR, message = "Có lỗi xảy ra!" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult AddSkill(string Name, string Detail, int ID, string levels)
        {
            if (SessionHelper.IsLogIn())
            {
                var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);

                ResultInfo result = null;
                if (ID != 0)
                {
                    result = EditSkill(new Skill() { Name = Name, Dept = user.Dept, Detail = Detail, ID = ID }, levels);
                }
                else
                {
                    result = AddNewSkill(new Skill() { Name = Name, Dept = user.Dept, Detail = Detail }, levels);
                }
                if (result != null && result.code == RESULT.SUCCESS)
                {
                    return Json(new { code = RESULT.SUCCESS }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = RESULT.ERROR, message = result.message }, JsonRequestBehavior.AllowGet);
                }

            }
            else return Json(new { code = RESULT.ERROR, message = "Phiên đăng nhập hết hạn! Cần đăng nhập lại" }, JsonRequestBehavior.AllowGet);
        }

        private ResultInfo AddNewSkill(Skill skill, string levels)
        {
            try
            {
                var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                skill.Creator = user.ID;
                skill.CreateTime = DateTime.Now;
                skill.Updator = user.ID;
                skill.UpdateTime = DateTime.Now;

                using (var db = new UMC_SKILLEntities())
                {
                    db.Skills.Add(skill);
                    db.SaveChanges();
                    var levelsArr = levels.Split(',');
                    foreach (var level in levelsArr)
                    {
                        var skillLevel = new SkillLevel();
                        skillLevel.Name = level;
                        skillLevel.SkillID = skill.ID;
                        db.SkillLevels.Add(skillLevel);
                        db.SaveChanges();
                    }
                    return new ResultInfo() { code = RESULT.SUCCESS };
                }
            }
            catch (Exception)
            {

                return new ResultInfo() { code = RESULT.ERROR, message = "Có lỗi xảy ra khi thêm vào DB" };

            }

        }

        private ResultInfo EditSkill(Skill skill, string levels)
        {
            try
            {
                var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                skill.Updator = user.ID;
                skill.UpdateTime = DateTime.Now;

                using (var db = new UMC_SKILLEntities())
                {
                    var skillInDb = db.Skills.Include("SkillLevels").Where(m => m.ID == skill.ID).FirstOrDefault();
                    if (skillInDb == null)
                    {
                        return new ResultInfo() { code = RESULT.ERROR, message = "Không tồn tại kĩ năng " + skill.ID };
                    }
                    skillInDb.Name = skill.Name;
                    skillInDb.Dept = skill.Dept;
                    skill.Detail = skill.Detail;
                    skillInDb.Updator = user.ID;
                    skillInDb.UpdateTime = DateTime.Now;
                    db.SaveChanges();
                    var levelsArr = levels.Split(',');
                    int i = 0;
                    foreach (var level in skillInDb.SkillLevels)
                    {
                        var skillLevel = db.SkillLevels.Where(m => m.SkillID == skill.ID && m.Name == level.Name).FirstOrDefault();
                        skillLevel.Name = levelsArr[i];
                        db.SaveChanges();
                        i++;
                    }
                    return new ResultInfo() { code = RESULT.SUCCESS };
                }
            }
            catch (Exception e)
            {

                return new ResultInfo() { code = RESULT.ERROR, message = "Có lỗi xảy ra khi thêm vào DB" };
            }

        }

    }
}