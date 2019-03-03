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
            InternShip _internship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == _id);
            Company _company = ctx.Companies.FirstOrDefault(x => x.CompanyID == _internship.CompanyID);
            Student _student = ctx.Students.FirstOrDefault(x=>x.StudentID==_internship.StudentID);
            MembershipUser _adviser = Membership.GetUser(_internship.AdviserID);
            ViewBag.InternShip = _internship;
            ViewBag.Student = _student;
            ViewBag.Company = _company;
            return View(_adviser);
        }
    }
}