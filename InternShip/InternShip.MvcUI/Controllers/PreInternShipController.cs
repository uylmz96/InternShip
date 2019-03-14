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

    public class PreInternShipController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: PreInternShip
        public ActionResult Index()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            if (Session["studentNumber"] != null)
            {
                string number = Session["studentNumber"].ToString();
                ViewBag.PreInternShips = context.PreInternships.Where(x => x.StudentNumber == number & x.DelDate == null).OrderByDescending(x => x.CrtDate).ToList();
                return View();
            }
            else
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Öğrenci girişi yapılmamış. Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }

        }

        //GET:PreInternShip
        public ActionResult PreInternShip(int id)
        {
            if (Session["studentNumber"] == null)//Öğrenci Girişi yapılmış mı
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }

            PreInternship model = context.PreInternships.FirstOrDefault(x => x.PreInternshipID == id);
            return View(model);
        }

        //POST: PreInternShipAdd
        [HttpPost]
        public ActionResult PreInternShipAdd(PreInternship internship)
        {
            internship.CrtDate = DateTime.Now;
            if (Session["studentNumber"] != null)
            {
                string number = Session["studentNumber"].ToString();
                Student student = context.Students.FirstOrDefault(x => x.StudentNumber == number);
                if (student != null)
                {
                    internship.StudentID = student.StudentID;
                    internship.StudentNumber = student.StudentNumber;
                }
                context.Set<PreInternship>().Add(internship);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Öğrenci girişi yapılmamış. Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }
        }

        //POST: PreInternShipUpdate
        [HttpPost]
        public ActionResult PreInternShipUpdate(PreInternship internship)
        {
            PreInternship updatedpreInternship = context.PreInternships.FirstOrDefault(x => x.PreInternshipID == internship.PreInternshipID);
            if (updatedpreInternship != null)
            {
                internship.MapTo<PreInternship>(updatedpreInternship);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("PreInternShip", new { id = internship.PreInternshipID });
            }
        }

        //GET: PreInternShipPrint
        public ActionResult PreInternShipPrint(int id)
        {
            if (Session["studentNumber"] == null)//Öğrenci Girişi yapılmış mı
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }

            PreInternship model = context.PreInternships.FirstOrDefault(x => x.PreInternshipID == id);
            if (model != null)
            {
                ViewBag.Student = context.Students.FirstOrDefault(x => x.StudentID == model.StudentID);

                if (model.InternshipID != null)
                {
                    InternShip intern = context.InternShips.FirstOrDefault(x => x.InternShipID == model.InternshipID);
                    MembershipUser adviser = Membership.GetUser(intern.AdviserID);
                    ViewBag.Adviser = adviser.Comment;
                }
                else
                {
                    ViewBag.Adviser = null;
                }
                return View(model);
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor. Lütfen Tekrar Deneyiniz.');";
                return RedirectToAction("Index");
            }
        }

    }
}