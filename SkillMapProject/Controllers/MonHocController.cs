using SkillMapProject.Helper;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class MonHocController : Controller
    {
        // GET: MonHoc
        public ActionResult Index()
        {
            try
            {
                using (var db = new UMC_SKILLEntities())
                {
                    var list = db.MONHOCs.ToList();

                    using (var dbGA = new GA_UMCEntities())
                    {
                        ViewBag.Depts = dbGA.MS_Department.ToList();
                    }
                    if (list != null)
                        return View(list);
                    else return View(new List<MONHOC>());
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return View(new List<MONHOC>());
            }
        }
        public JsonResult AddMonHoc(string MaMonHoc, string TenMonHoc, string loaihinh, string dept)
        {
            try
            {
                var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                MONHOC monhoc = new MONHOC();
                monhoc.MaBoMon = MaMonHoc;
                monhoc.TenBoMon = TenMonHoc;
                monhoc.LoaiMonHoc = loaihinh;
                monhoc.Dept = dept;
                monhoc.CreateBy = user.ID;
                monhoc.CreateDate = DateTime.Now;
                monhoc.ModifyBy = user.ID;
                monhoc.ModifyDate = DateTime.Now;

                using (var db = new UMC_SKILLEntities())
                {
                    if (db.MONHOCs.Where(m => m.MaBoMon == monhoc.MaBoMon).FirstOrDefault() != null)
                    {
                        return Json(new { code = RESULT.ERROR, message = "Đã có mã bộ môn này rồi" }, JsonRequestBehavior.AllowGet);
                    }
                    db.MONHOCs.Add(monhoc);
                    db.SaveChanges();
                     var listMonHoc = db.MONHOCs.OrderByDescending(m => m.ModifyDate).ToList();
                    var message = Utils.ConvertViewToString("~/Views/MonHoc/_ListMonHoc.cshtml", listMonHoc, ViewData, ControllerContext);
                    return Json(new { code = RESULT.SUCCESS, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = RESULT.ERROR, message = "Có lỗi xảy ra khi thêm vào DB" });
            }

        }
    }
}