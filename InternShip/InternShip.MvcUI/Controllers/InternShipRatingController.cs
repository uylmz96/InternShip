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

    public class InternShipRatingController : Controller
    {

        InternShipContext context = new InternShipContext();
        // GET: InternShipRating
        public ActionResult Index()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"];
            string _adviser = User.Identity.Name;
            ViewBag.Internships = context.InternShips.Where(x => x.AdviserID == _adviser & x.DelDate==null).OrderByDescending(x => x.CrtDate).ToList();
            return View();
        }

        //GET: InternshipRating
        public ActionResult InternshipRating(int id)
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            
            InternShipResult model= context.InternShipResults.SingleOrDefault(x => x.InternShipID == id);
            ViewBag.Reasons = context.RefusalReasons.ToList();
            return View(model);
            //Staj Detay için 
            int _id = Convert.ToInt32(id);
            InternShip _internship = context.InternShips.FirstOrDefault(x => x.InternShipID == _id);
            Company _company = context.Companies.FirstOrDefault(x => x.CompanyID == _internship.CompanyID);
            Student _student = context.Students.FirstOrDefault(x => x.StudentID == _internship.StudentID);
            MembershipUser _adviser = Membership.GetUser(_internship.AdviserID);
            ViewBag.InternShip = _internship;
            ViewBag.Student = _student;
            ViewBag.Company = _company;
            return View(_adviser);
        }

        //POST:InternshipRatingAdd
        public ActionResult InternshipRatingAdd(InternShipResult rating)
        {
            context.Set<InternShipResult>().Add(rating);
            TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
            return RedirectToAction("Index");
        }

        //POST:InternshipRatingUpdate
        public ActionResult InternshipRatingUpdate(InternShipResult rating)
        {

            InternShipResult updatedRating = context.InternShipResults.SingleOrDefault(x => x.ResultID == rating.ResultID);
            if (updatedRating != null)
            {
                rating.MapTo<InternShipResult>(updatedRating);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Company", new { id = updatedRating.ResultID });
            }
        }

    }
}