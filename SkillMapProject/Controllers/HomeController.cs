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
                    ViewBag.SkillLevels = db.Skills.Include("SkillLevels").Where(m => m.Removed == 0).ToList();
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

        public JsonResult GetSkillOfStaffList(string search)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var listUser = new List<Member>();
                if (string.IsNullOrEmpty(search))
                {
                    listUser = db.Members.ToList();

                }
                else
                {
                    listUser = db.Members.Where(m => m.Code == search).ToList();

                }
                var listCerByUser = new List<CertificateByUser>();
                foreach (var u in listUser)
                {
                    var skills = db.Certifications.Include("Skill").Where(m => m.UserID == u.ID && m.Skill.Removed == 0).ToList();
                    var allSkill = db.Skills.Where(m => m.Removed == 0).ToList();
                    Dictionary<Skill, Certification> dics = new Dictionary<Skill, Certification>();
                    var cerByUser = new CertificateByUser()
                    {
                        userID = u.ID,
                        Code = u.Code,
                        FullName = u.Name,
                        Dept = u.Dept,
                        ListSkills = skills
                    };
                    foreach (var skill in allSkill)
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
        public ActionResult SuaChungChiChoNV(int ID = 0)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);

            if (user != null)
            {
                using (var db = new UMC_SKILLEntities())
                {
                    // Chỉ được thêm những chứng chỉ của phòng ban mình
                    ViewBag.Skills = db.Skills.Where(m => m.Removed == 0).ToList();
                    if (ID == 0)
                    {
                        return View();
                    }
                    else
                    {
                        var cerInDb = db.Certifications.Include("Member").Include("Member1").Where(m => m.ID == ID).FirstOrDefault();
                        ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cerInDb.SkillID).ToList();
                        // GetHistories(cerInDb, db);
                        return View(cerInDb);
                    }

                }
            }
            else return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaChungChiChoNV(Certification cer)
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
                            ViewBag.Skills = db.Skills.Where(m => m.Removed == 0).ToList();
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
                                    ViewBag.Skills = db.Skills.Where(m => m.Removed == 0).ToList();
                                    return View(cer);
                                }
                                else
                                {
                                    ModelState.AddModelError("Error", "Không tồn tại chứng chỉ này!");
                                    return View(cer);
                                }

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
                                    cerInDb.SkillID = cer.SkillID;
                                    if (cer.CapDo != null)
                                    {
                                        if (cer.NgayCap == null)
                                        {
                                            ModelState.AddModelError("Error", "Bạn cần chọn Ngày Cấp!");
                                            return View(cer);
                                        }
                                        cerInDb.CapDo = cer.CapDo;
                                        cerInDb.NgayCap = cer.NgayCap;
                                    }
                                    if (cer.NangCap != null)
                                    {
                                        if (cer.NgayNangCap == null)
                                        {
                                            ModelState.AddModelError("Error", "Bạn cần chọn Ngày Nâng Cấp!");
                                            return View(cer);
                                        }
                                        cerInDb.NangCap = cer.NangCap;
                                        cerInDb.NgayNangCap = cer.NgayNangCap;
                                    }
                                    if (cer.CNNguoiDaoTao != null)
                                    {
                                        if (cer.NgayCNNguoiDaoTao == null)
                                        {
                                            ModelState.AddModelError("Error", "Bạn cần chọn Ngày CN Người Đào tạo!");
                                            return View(cer);
                                        }
                                        cerInDb.CNNguoiDaoTao = cer.CNNguoiDaoTao;
                                        cerInDb.NgayCNNguoiDaoTao = cer.NgayCNNguoiDaoTao;
                                    }
                                    db.SaveChanges();
                                }
                                else
                                {
                                    ModelState.AddModelError("Error", "Không tồn tại chứng chỉ này cho nhân viên!");
                                    ViewBag.Skills = db.Skills.Where(m => m.Removed == 0).ToList();
                                    return View(cer);
                                }
                            }

                            return RedirectToAction("ViewSkillForStaff", new { ID = cer.UserID });

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
                           .Where(m => m.UserID == u.ID && m.Skill.Removed == 0)
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
        public JsonResult GetAllSubjectOfStaff(int ID)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var member = db.Members.Where(m => m.ID == ID).FirstOrDefault();
                if (member == null)
                {
                    return Json(new { code = RESULT.ERROR }, JsonRequestBehavior.AllowGet);
                }
                var list = db.SKILLMAPs.Include("MONHOC").Where(m => m.UserID == ID && m.MONHOC.LoaiMonHoc == LoaiHinhDaoTao.TOANCONGTY).OrderByDescending(m => m.NgayThamGia).ToList();
                var listMonHoc = db.MONHOCs.Where(m => m.Removed == 0 && m.LoaiMonHoc == LoaiHinhDaoTao.TOANCONGTY).ToList();

                var skillMapByUser = new SkillMapByUser()
                {
                    userID = ID,
                    Code = member.Code,
                    FullName = member.Name,
                    Dept = member.Dept,
                    DateEnter = member.DateEnter.ToShortDateString(),
                    ListSkillMaps = list
                };
                Dictionary<MONHOC, SKILLMAP> dics = new Dictionary<MONHOC, SKILLMAP>();

                foreach (var monhoc in listMonHoc)
                {
                    var skillMap = list.Where(m => m.MaBoMon == monhoc.MaBoMon).FirstOrDefault();
                    dics.Add(monhoc, skillMap);
                }
                skillMapByUser.dics = dics;

                return Json(new
                {
                    code = RESULT.SUCCESS,
                    message = Utils.ConvertViewToString("~/Views/Home/_SkillMap.cshtml", skillMapByUser, ViewData, ControllerContext),
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult AddSkillMap(string MaMonHoc, string NgayThamGia, int userID, string StaffCode, string GhiChu)
        {
            try
            {

                using (var db = new UMC_SKILLEntities())
                {
                    var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                    SKILLMAP skillmap = db.SKILLMAPs.Where(m => m.MaBoMon == MaMonHoc && m.UserID == userID).FirstOrDefault();
                    if (skillmap != null)
                    {
                        skillmap.GhiChu = GhiChu;
                        skillmap.NgayThamGia = DateTime.Parse(NgayThamGia);
                    }
                    else
                    {
                        skillmap = new SKILLMAP();
                        skillmap.MaBoMon = MaMonHoc;
                        skillmap.GhiChu = GhiChu;
                        skillmap.StaffCode = StaffCode;
                        skillmap.UserID = userID;
                        skillmap.NgayThamGia = DateTime.Parse(NgayThamGia);
                        db.SKILLMAPs.Add(skillmap);
                    }
                    db.SaveChanges();

                    var list = db.SKILLMAPs.Include("MONHOC").Where(m => m.UserID == userID && m.MONHOC.LoaiMonHoc == LoaiHinhDaoTao.TOANCONGTY).OrderByDescending(m => m.NgayThamGia).ToList();
                    var listMonHoc = db.MONHOCs.Where(m => m.Removed == 0 && m.LoaiMonHoc == LoaiHinhDaoTao.TOANCONGTY).ToList();

                    var skillMapByUser = new SkillMapByUser()
                    {
                        userID = userID,
                        Code = StaffCode,
                        FullName = "",
                        Dept = "",
                        DateEnter = "",
                        ListSkillMaps = list
                    };
                    Dictionary<MONHOC, SKILLMAP> dics = new Dictionary<MONHOC, SKILLMAP>();

                    foreach (var monhoc in listMonHoc)
                    {
                        var skillMap = list.Where(m => m.MaBoMon == monhoc.MaBoMon).FirstOrDefault();
                        dics.Add(monhoc, skillMap);
                    }
                    skillMapByUser.dics = dics;

                    return Json(new
                    {
                        code = RESULT.SUCCESS,
                        message = Utils.ConvertViewToString("~/Views/Home/_SkillMap.cshtml", skillMapByUser, ViewData, ControllerContext),
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = RESULT.ERROR, message = "Có lỗi xảy ra khi thêm vào DB" });
            }

        }

    }


}
