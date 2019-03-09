using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using InternShip.MvcUI.App_Classes;
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
            if (_id <1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                InternShip _internship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == _id);
                Company _company = ctx.Companies.FirstOrDefault(x => x.CompanyID == _internship.CompanyID);
                Student _student = ctx.Students.FirstOrDefault(x => x.StudentID == _internship.StudentID);
                PreInternship _preInternship = ctx.PreInternships.FirstOrDefault(x => x.InternshipID == _internship.InternShipID);
                MembershipUser _adviser = Membership.GetUser(_internship.AdviserID);
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
            int _id = Convert.ToInt32(id);
            InternShip _internship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == _id);
            Company _company = ctx.Companies.FirstOrDefault(x => x.CompanyID == _internship.CompanyID);
            Student _student = ctx.Students.FirstOrDefault(x => x.StudentID == _internship.StudentID);
            PreInternship _preInternship = ctx.PreInternships.FirstOrDefault(x => x.InternshipID == _internship.InternShipID);
            MembershipUser _adviser = Membership.GetUser(_internship.AdviserID);
            InternShipResult _result = ctx.InternShipResults.FirstOrDefault(x => x.InternShipID == _internship.InternShipID);
            ViewBag.InternShip = _internship;
            ViewBag.Student = _student;
            ViewBag.Company = _company;
            ViewBag.PreInternShip = _preInternship;
            ViewBag.Result = _result;
            return View(_adviser);
        }

        //GET: PreInternShipAccept
        public ActionResult PreInternShip()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            List<MembershipUser> Advisers = new List<MembershipUser>();
            foreach (string item in Roles.GetUsersInRole("Akademisyen"))
            {
                Advisers.Add(Membership.GetUser(item));
            }
            ViewBag.Advisers = Advisers;
            ViewBag.PreInternShips = ctx.PreInternships.Where(x => x.DelDate == null).OrderBy(x => x.StudentID).OrderByDescending(x => x.CrtDate).ToList();
            return View();
        }

        //GET: PreInternShipAccept
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
                    Company _cmp = ctx.Companies.FirstOrDefault(x => x.DelDate == null & (x.CompanyName.Contains(_pre.CompanyName) || x.Address.Contains(_pre.CompanyAddress) || x.Mail.Contains(_pre.CompanyMail) || x.Phone.Contains(_pre.CompanyPhone)));
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