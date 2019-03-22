using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using InternShip.MvcUI.App_Classes;
    using InternShip.MvcUI.App_Classes.DTO;
    using Models;
    using System.Web.Security;

    [Authorize]
    public class InternShipController : Controller
    {
        InternShipContext ctx = new InternShipContext();
        // GET: InternShip
        public ActionResult Index()
        {
            ViewBag.Internships = ctx.InternShips.ToList().Where(x => x.DelDate == null);
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //GET: InternshipAdd
        public ActionResult Internship(int id)
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            ViewBag.Users = Membership.GetAllUsers();
            InternShip model = ctx.InternShips.SingleOrDefault(x => x.InternShipID == id);
            return View(model);
        }

        //POST: InternshipAdd
        public ActionResult InternshipAdd(InternShip internship)
        {
            internship.CrtDate = DateTime.Now;
            ctx.Set<InternShip>().Add(internship);
            TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
            return RedirectToAction("Index");
        }

        //POST: InternshipUpdate
        public ActionResult InternshipUpdate(InternShip internship)
        {
            InternShip updatedInternship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == internship.InternShipID);
            if (updatedInternship != null)
            {
                internship.MapTo<InternShip>(updatedInternship);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Internship", new { id = internship.InternShipID });
            }
        }

        //GET: InternshipDelete
        public ActionResult InternshipDelete(int id)
        {
            InternShip updatedInternship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == id);
            if (updatedInternship != null)
            {
                updatedInternship.DelDate = DateTime.Now;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Internship", new { id = id });
            }
        }

        //GET: Internship Detail
        public ActionResult IntershipDetail(string id)
        {
            int _id = Convert.ToInt32(id);
            if (_id < 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                InternShip _internship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == _id);
                Company _company = ctx.Companies.FirstOrDefault(x => x.CompanyID == _internship.CompanyID);
                Student _student = ctx.Students.FirstOrDefault(x => x.StudentID == _internship.StudentID);
                PreInternship _preInternship = ctx.PreInternships.FirstOrDefault(x => x.InternshipID == _internship.InternShipID);
                MembershipUser _adviser = null;
                if (_internship.AdviserID != null)
                    _adviser = Membership.GetUser(_internship.AdviserID);
                InternShipResult _result = ctx.InternShipResults.FirstOrDefault(x => x.InternShipID == _internship.InternShipID);

                ViewBag.InternShip = _internship;
                ViewBag.Student = _student;
                ViewBag.Company = _company;
                ViewBag.PreInternShip = _preInternship;
                ViewBag.Result = _result;
                return View(_adviser);

            }
        }

        //GET: Internship Detail For Student
        [AllowAnonymous]
        public ActionResult IntershipDetailForStudent(string id)
        {
            if (Session["studentNumber"] == null)//Öğrenci Girişi yapılmış mı
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }

            int _id = Convert.ToInt32(id);
            if (_id < 1)
            {
                return RedirectToAction("InternShipForStudent", "Home");
            }
            else
            {
                InternShip _internship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == _id);
                Company _company = ctx.Companies.FirstOrDefault(x => x.CompanyID == _internship.CompanyID);
                Student _student = ctx.Students.FirstOrDefault(x => x.StudentID == _internship.StudentID);
                PreInternship _preInternship = ctx.PreInternships.FirstOrDefault(x => x.InternshipID == _internship.InternShipID);
                MembershipUser _adviser = null;
                if (_internship.AdviserID != null)
                    _adviser = Membership.GetUser(_internship.AdviserID);
                InternShipResult _result = ctx.InternShipResults.FirstOrDefault(x => x.InternShipID == _internship.InternShipID);

                ViewBag.InternShip = _internship;
                ViewBag.Student = _student;
                ViewBag.Company = _company;
                ViewBag.PreInternShip = _preInternship;
                ViewBag.Result = _result;
                return View(_adviser);

            }
        }

        //GET: PreInternShipAccept
        public ActionResult PreInternShip()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();

            #region Advisers
            //Akademisyen olan kullanıcıların çekilmesi
            List<AdviserWithCount> Advisers = new List<AdviserWithCount>();
            foreach (string item in Roles.GetUsersInRole("Akademisyen"))
            {
                AdviserWithCount adviser = new AdviserWithCount();
                adviser.adviserID = Membership.GetUser(item).UserName;
                adviser.internshipCount = 0;
                Advisers.Add(adviser);
            }
            DateTime thisYear = new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 00);
            //Önceden internship tablosundaki kullanıcıların kaç tane staj ile ilgilendikleri

            var AdviserCounts = (from i in ctx.InternShips
                                 where i.DelDate == null & i.CrtDate > thisYear
                                 group i by i.AdviserID into t
                                 select new
                                 {
                                     adviserID = t.Key,
                                     sumInternShip = t.Count()
                                 }).ToList();
            //İkisinin birleştirilmesi
            foreach (var item in AdviserCounts)
            {
                AdviserWithCount a = Advisers.FirstOrDefault(x => x.adviserID == item.adviserID);
                if (a != null)
                {
                    a.internshipCount = item.sumInternShip;
                }
            }
            #endregion


            ViewBag.Advisers = Advisers;
            ViewBag.PreInternShips = ctx.PreInternships.Where(x => x.DelDate == null).OrderByDescending(x => x.CrtDate).OrderBy(x => x.InternshipID).ToList();
            return View();
        }

        //POST: PreInternShipAccept
        [HttpPost]
        public int PreInternShipAccept(int ID, string Adviser)
        {
            /* Eklenen bir ön başvuru formunu internship tablosuna ekleme ve şirket yoksa oluşturma
             * parametre olarak gelen adviser da internship e tanımlanıyor.
             */
            try
            {
                PreInternship _pre = ctx.PreInternships.FirstOrDefault(x => x.PreInternshipID == ID);
                int cmpID, internshipID;
                if (_pre != null & _pre.InternshipID == null)
                {
                    #region Company yoksa oluştur varsa id al          
                    Company _cmp = ctx.Companies.FirstOrDefault(x => x.DelDate == null & (x.CompanyName.Contains(_pre.CompanyName) || x.Mail.Contains(_pre.CompanyMail) || x.Phone.Contains(_pre.CompanyPhone)));
                    if (_cmp == null)
                    {
                        _cmp = new Company();
                        _cmp.CrtDate = DateTime.Now;
                        _cmp.Address = _pre.CompanyAddress;
                        _cmp.CompanyName = _pre.CompanyName;
                        _cmp.Mail = _pre.CompanyMail;
                        _cmp.Phone = _pre.CompanyPhone;
                        ctx.Set<Company>().Add(_cmp);
                    }
                    bool isCompanyAdd = Result.SaveChanges2(ctx);
                    cmpID = _cmp.CompanyID;
                    #endregion

                    #region InternShip Oluşturma
                    InternShip _internship = new InternShip();
                    _internship.CompanyID = cmpID;
                    _internship.EndDate = _pre.EndDate;
                    _internship.StartDate = _pre.StartDate;
                    _internship.StudentID = _pre.StudentID;
                    _internship.AdviserID = Adviser;
                    _internship.Time = _pre.Time;
                    _internship.CrtDate = DateTime.Now;
                    ctx.Set<InternShip>().Add(_internship);
                    bool isInternshipAdd = Result.SaveChanges2(ctx);
                    internshipID = _internship.InternShipID;
                    #endregion

                    #region PreInternship e eklenen intershipi tanımlama
                    _pre.InternshipID = internshipID;
                    ctx.Set<PreInternship>();
                    bool isPreAccepted = Result.SaveChanges2(ctx);
                    #endregion


                    if (isInternshipAdd & isPreAccepted)
                    {
                        Student _student = ctx.Students.FirstOrDefault(x => x.StudentID == _pre.StudentID);
                        if (_student.Mail != null)
                            Mail.sendMailUseThread(_student.Mail, "Danışman Atama", "Stajınızın danışman ataması yapılmıştır. Lütfen çıktı alarak danışmanınıza ve ilgili şirkete imzalatınız.");
                    }
                    return 1;
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        //POST: PreInternShipAcceptAuto
        public ActionResult PreInternShipAcceptAuto()
        {
            DateTime thisYear = new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 00);
            #region Bu yıl eklenen danışman atanmamaış Ön Başvurular
            var preInternships = (from pi in ctx.PreInternships
                                  where pi.CrtDate > thisYear & pi.DelDate == null & pi.InternshipID == null
                                  select new
                                  {
                                      pi.PreInternshipID
                                  }
                                ).ToList();
            #endregion

            foreach (var item in preInternships)
            {
                try
                {
                    PreInternship _pre = ctx.PreInternships.FirstOrDefault(x => x.PreInternshipID == item.PreInternshipID);
                    int cmpID, internshipID;
                    if (_pre != null & _pre.InternshipID == null)
                    {
                        #region Company yoksa oluştur varsa id al          
                        Company _cmp = ctx.Companies.FirstOrDefault(x => x.DelDate == null & (x.CompanyName.Contains(_pre.CompanyName) || x.Mail.Contains(_pre.CompanyMail) || x.Phone.Contains(_pre.CompanyPhone)));
                        if (_cmp == null)
                        {
                            _cmp = new Company();
                            _cmp.CrtDate = DateTime.Now;
                            _cmp.Address = _pre.CompanyAddress;
                            _cmp.CompanyName = _pre.CompanyName;
                            _cmp.Mail = _pre.CompanyMail;
                            _cmp.Phone = _pre.CompanyPhone;
                            ctx.Set<Company>().Add(_cmp);
                        }
                        bool isCompanyAdd = Result.SaveChanges2(ctx);
                        cmpID = _cmp.CompanyID;
                        #endregion

                        #region Atama yapılacak danışmanı çekme
                        string Adviser = GetAdviser()[0].adviserID;
                        #endregion

                        #region InternShip Oluşturma
                        InternShip _internship = new InternShip();
                        _internship.CompanyID = cmpID;
                        _internship.EndDate = _pre.EndDate;
                        _internship.StartDate = _pre.StartDate;
                        _internship.StudentID = _pre.StudentID;
                        _internship.AdviserID = Adviser;
                        _internship.Time = _pre.Time;
                        _internship.CrtDate = DateTime.Now;
                        ctx.Set<InternShip>().Add(_internship);
                        bool isInternshipAdd = Result.SaveChanges2(ctx);
                        internshipID = _internship.InternShipID;
                        #endregion

                        #region PreInternship e eklenen intershipi tanımlama
                        _pre.InternshipID = internshipID;
                        ctx.Set<PreInternship>();
                        bool isPreAccepted = Result.SaveChanges2(ctx);
                        #endregion

                        #region Öğrenciye mail gönderme işlemi
                        if (isInternshipAdd & isPreAccepted)
                        {
                            Student _student = ctx.Students.FirstOrDefault(x => x.StudentID == _pre.StudentID);
                            if (_student.Mail != null)
                                Mail.sendMailUseThread(_student.Mail, "Danışman Atama", "Stajınızın danışman ataması yapılmıştır. Lütfen çıktı alarak danışmanınıza ve ilgili şirkete cv bnmöimzalatınız.");

                             
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    TempData["JsFunc"] = "errorMessage('İşlem yarıda kesildi sayfayı yenileyip kontrol ediniz.');";
                    return RedirectToAction("PreInternShip");
                }
            }
            TempData["JsFunc"] = "success();";
            return RedirectToAction("PreInternShip");
        }

        //En Az staj atanmış danışmanı bulma
        private List<AdviserWithCount> GetAdviser()
        {
            #region Danışmanlar ve bu yıl atanmış staj sayıları
            DateTime thisYear = new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 00);
            //Akademisyen olan kullanıcıların çekilmesi
            List<AdviserWithCount> Advisers = new List<AdviserWithCount>();
            foreach (string item in Roles.GetUsersInRole("Akademisyen"))
            {
                AdviserWithCount adviser = new AdviserWithCount();
                adviser.adviserID = Membership.GetUser(item).UserName;
                adviser.internshipCount = 0;
                Advisers.Add(adviser);
            }

            //Önceden internship tablosundaki kullanıcıların kaç tane staj ile ilgilendikleri
            var AdviserCounts = (from i in ctx.InternShips
                                 where i.DelDate == null & i.CrtDate > thisYear
                                 group i by i.AdviserID into t
                                 select new
                                 {
                                     adviserID = t.Key,
                                     sumInternShip = t.Count()
                                 }).ToList();

            //İkisinin birleştirilmesi
            foreach (var item in AdviserCounts)
            {
                AdviserWithCount a = Advisers.FirstOrDefault(x => x.adviserID == item.adviserID);
                if (a != null)
                {
                    a.internshipCount = item.sumInternShip;
                }
            }
            Advisers = Advisers.OrderBy(x => x.internshipCount).ToList();
            #endregion

            return Advisers;
        }

        //GET: PreInternShipRemove
        public ActionResult PreInternShipRemove(int id)
        {
            InternShip _internShip = ctx.InternShips.FirstOrDefault(x => x.InternShipID == id);
            if (_internShip != null)
            {
                if (_internShip != null)
                    _internShip.DelDate = DateTime.Now;
                ctx.Set<InternShip>();
                TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
            }
            return RedirectToAction("PreInternShip");
        }


    }
}