using SkillMapProject.Helper;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkillMapProject.Controllers
{
    public class HomeController : Controller
    {
        private void UpdateData()
        {
            var gaDB = new GA_UMCEntities();
            var db = new UMC_SKILLEntities();
            var listCheckEyes = gaDB.sp_Get_All_CheckEye(null, null).ToList();
            foreach (var checkEye in listCheckEyes)
            {
                var staff = db.Members.Where(m => m.Code == checkEye.StaffCode).FirstOrDefault();
                if (staff != null)
                {
                    var cerInDb = db.Certifications.Where(m => m.UserID == staff.ID && m.SkillID == 17).FirstOrDefault();
                    if (cerInDb == null)
                    {
                        cerInDb = new Certification();
                        cerInDb.UserID = staff.ID;
                        cerInDb.Code = staff.Code;
                        cerInDb.Updator = 2309;
                        cerInDb.UpdateTime = DateTime.Now;
                        cerInDb.NguoiChamDiem = 2309;
                        cerInDb.Mark = "100";
                        cerInDb.Note = "";
                        cerInDb.Result = "PASS";
                        cerInDb.NgayThiXacNhan = checkEye.NgayThi;
                        cerInDb.NgayThiThucTe = checkEye.NgayThiThucTe;
                        cerInDb.TypeSkill = "Skill";
                        cerInDb.SkillID = 17;
                        cerInDb.CapDo = checkEye.CapDo;
                        cerInDb.NgayCap = checkEye.NgayCap;
                        cerInDb.NangCap = checkEye.NangCap;
                        cerInDb.NgayNangCap = checkEye.NgayNangCap;
                        cerInDb.CNNguoiDaoTao = checkEye.CNNguoiDaoTao;
                        cerInDb.NgayCNNguoiDaoTao = checkEye.NgayCNNguoiDaoTao;
                        db.Certifications.Add(cerInDb);
                    }
                    else
                    {
                        cerInDb.Updator = 2309;
                        cerInDb.UpdateTime = DateTime.Now;
                        cerInDb.NgayThiXacNhan = checkEye.NgayThi;
                        cerInDb.NgayThiThucTe = checkEye.NgayThiThucTe;
                        cerInDb.CapDo = checkEye.CapDo;
                        cerInDb.NgayCap = checkEye.NgayCap;
                        cerInDb.NangCap = checkEye.NangCap;
                        cerInDb.NgayNangCap = checkEye.NgayNangCap;
                        cerInDb.CNNguoiDaoTao = checkEye.CNNguoiDaoTao;
                        cerInDb.NgayCNNguoiDaoTao = checkEye.NgayCNNguoiDaoTao;
                    }
                    db.SaveChanges();
                }
            }
            var listHan = gaDB.sp_Get_All_Check_Solders(null).ToList();
            foreach (var han in listHan)
            {
                var staff = db.Members.Where(m => m.Code == han.StaffCode).FirstOrDefault();
                if (staff != null)
                {
                    var cerInDb = db.Certifications.Where(m => m.UserID == staff.ID && m.SkillID == 16).FirstOrDefault();
                    if (cerInDb == null)
                    {
                        cerInDb = new Certification();
                        cerInDb.UserID = staff.ID;
                        cerInDb.Code = staff.Code;
                        cerInDb.Updator = 2309;
                        cerInDb.UpdateTime = DateTime.Now;
                        cerInDb.NguoiChamDiem = 2309;
                        cerInDb.Mark = "100";
                        cerInDb.Note = "";
                        cerInDb.Result = "PASS";
                        cerInDb.NgayThiXacNhan = han.NgayThiXacNhan;
                        cerInDb.NgayThiThucTe = han.NgayThiThucTe;
                        cerInDb.TypeSkill = "Skill";
                        cerInDb.SkillID = 16;
                        cerInDb.CapDo = han.CapDoHan;
                        cerInDb.NgayCap = han.NgayCap;
                        cerInDb.NangCap = han.NangCapDo;
                        cerInDb.NgayNangCap = han.NgayNangCap;
                        cerInDb.CNNguoiDaoTao = han.CNNguoiDaoTao;
                        cerInDb.NgayCNNguoiDaoTao = han.NgayCNNguoiDaoTao;
                        db.Certifications.Add(cerInDb);
                    }
                    else
                    {
                        cerInDb.Updator = 2309;
                        cerInDb.UpdateTime = DateTime.Now;
                        cerInDb.NgayThiXacNhan = han.NgayThiXacNhan;
                        cerInDb.NgayThiThucTe = han.NgayThiThucTe;
                        cerInDb.CapDo = han.CapDoHan;
                        cerInDb.NgayCap = han.NgayCap;
                        cerInDb.NangCap = han.NangCapDo;
                        cerInDb.NgayNangCap = han.NgayNangCap;
                        cerInDb.CNNguoiDaoTao = han.CNNguoiDaoTao;
                        cerInDb.NgayCNNguoiDaoTao = han.NgayCNNguoiDaoTao;
                    }
                    db.SaveChanges();
                }
            }
        }
        public ActionResult Index()
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            //UpdateData();
            if (user != null)
            {
                using (var db = new UMC_SKILLEntities())
                {
                    var listAllUsers = db.Members.ToList();
                    if (Constant.SPECIAL_DEPT.Contains(user.Dept))
                    {
                        // nếu thuộc phòng ban edu thì được thêm cho tất cả nhân viên
                        ViewBag.Users = listAllUsers;
                    }
                    else
                    {
                        // còn không chỉ được thêm cho nhân viên phòng mình
                        ViewBag.Users = listAllUsers.Where(m => m.Dept == user.Dept).ToList();
                    }

                    // Chỉ được thêm những chứng chỉ của phòng ban mình
                    ViewBag.SkillLevels = db.Skills.Include("SkillLevels").Where(m => m.Removed == 0).ToList();
                    var userGroups = from p in listAllUsers
                                     group p.ID by p.Dept into g
                                     select new { Dept = g.Key, users = g.ToList() };
                    var listDept = new List<MS_Department>();
                    foreach (var dept in userGroups)
                    {
                        listDept.Add(new MS_Department()
                        {
                            DeptName = dept.Dept
                        });
                    }
                    ViewBag.Depts = listDept;
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
        public JsonResult GetSkills(List<Member> list)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var listCerByUser = new List<CertificateByUser>();
                foreach (var u in list)
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
                        Customer = u.Customer,
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
        public JsonResult GetSkillOfStaffListWithSearch(string search)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var listUser = new List<Member>();
                if (string.IsNullOrEmpty(search))
                {
                    listUser = db.Members.Where(m => m.Removed == 0).Take(10).ToList();
                }
                else
                {
                    listUser = db.Members.Where(m => m.Code == search && m.Removed == 0).ToList();
                }
                return GetSkills(listUser);
            }
        }
        public JsonResult UpdateNewStaff()
        {
            try
            {
                var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
                if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
                using (var db = new UMC_SKILLEntities())
                {
                    GA_UMCEntities context = new GA_UMCEntities();
                    object[] param =
                        {
                            new SqlParameter() { ParameterName = "@deptCode", Value = DBNull.Value, SqlDbType = SqlDbType.NVarChar},
                            new SqlParameter("@Out_Parameter", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            }
                        };
                    var reports = context.Database.SqlQuery<Employees>("EXEC [dbo].[sp_Get_All_Staff] @deptCode", param).ToList();
                    var list = db.Members.ToList();
                    foreach (var mem in list)
                    {
                        if (reports.Where(m => m.StaffCode == mem.Code).FirstOrDefault() == null)
                        {
                            var memInDb = db.Members.Where(m => m.Code == mem.Code).FirstOrDefault();
                            memInDb.Removed = 1;
                            db.SaveChanges();
                        }
                    }
                    foreach (var employee in reports)
                    {
                        var eInSkillMap = db.Members.Where(m => m.Code == employee.StaffCode).FirstOrDefault();
                        if (eInSkillMap == null)
                        {
                            var mem = new Member()
                            {
                                Code = employee.StaffCode,
                                Name = employee.FullName,
                                Dept = employee.DeptCode,
                                RoleID = 1,
                                Removed = 0,
                                Pass = "umcvn",
                                Pos = employee.PosName
                            };
                            if (employee.EntryDate is DateTime date)
                            {
                                mem.DateEnter = date;
                            }
                            db.Members.Add(mem);
                            db.SaveChanges();
                        }
                        else
                        {
                            bool isChanged = false;
                            if (eInSkillMap.Dept != employee.DeptCode)
                            {
                                eInSkillMap.Dept = employee.DeptCode;
                                isChanged = true;
                            }
                            if (eInSkillMap.Pos != employee.PosName)
                            {
                                eInSkillMap.Pos = employee.PosName;
                                isChanged = true;
                            }
                            if (isChanged)
                                db.SaveChanges();
                        }
                    }

                    return Json(new { body = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { body = "NG" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetSkillOfStaffListWithDept(string dept)
        {
            var user = SessionHelper.Get<Member>(Constant.SESSION_LOGIN);
            if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
            using (var db = new UMC_SKILLEntities())
            {
                var listUser = new List<Member>();
                if (string.IsNullOrEmpty(dept) || dept == Constant.DEPT_ALL)
                {
                    listUser = db.Members.Where(m => m.Removed == 0).Take(10).ToList();
                }
                else
                {
                    listUser = db.Members.Where(m => m.Dept == dept && m.Removed == 0).Take(200).ToList();
                }
                return GetSkills(listUser);
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
                            ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
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
                                    ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
                                    return View(cer);
                                }
                                else
                                {
                                    ModelState.AddModelError("Error", "Không tồn tại chứng chỉ này!");
                                    ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
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
                                            ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
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
                                            ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
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
                                            ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
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
                                    ViewBag.SkillLevels = db.SkillLevels.Where(m => m.SkillID == cer.SkillID).ToList();
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
                            ListSkills = skills,
                            Customer = u.Customer
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

        public async Task<ActionResult> Export(int SkillID = 0)
        {
            MemoryStream bufferStream = null;
            var db = new UMC_SKILLEntities();
            await Task.Run(async () =>
            {
               
                var list = db.Certifications.Include("Member").Include("Skill").Where(m => m.SkillID == SkillID).ToList();
              
                var stream = await ExportUtils.CreateExcelFile(null, list);
                // Tạo buffer memory strean để hứng file excel
                bufferStream = stream as MemoryStream;
                Console.WriteLine("task...");

            });
            Skill skill = db.Skills.Where(m => m.ID == SkillID).FirstOrDefault();
            
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename="+skill.Name+"-"+DateTime.Now.ToDateString()+".xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(bufferStream.ToArray());
            Console.WriteLine("done!");
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Home");
        }


    }


}
