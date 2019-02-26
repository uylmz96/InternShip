using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
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
            InternShip model = ctx.InternShips.FirstOrDefault(x => x.InternShipID == id);

            return View(model);
        }
        //POST: InternshipAdd
        public ActionResult InternshipAdd(InternShip internship)
        {
            try
            {
                ctx.Set<InternShip>().Add(internship);
                ctx.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("InternshipAdd");
            }

        }
        //POST: InternshipUpdate
        public ActionResult InternshipUpdate(InternShip internship)
        {
            InternShip updatedInternship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == internship.InternShipID);
            try
            {                
                if (updatedInternship != null)
                {
                    internship.MapTo<InternShip>(updatedInternship);
                    TempData["JsFunc"] = "success();";
                    ctx.SaveChanges();
                }
                else
                {
                    TempData["JsFunc"] = "warning();";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("InternshipAdd");
            }

        }
        //GET: InternshipDelete
        public ActionResult InternshipDelete(int id)
        {
            try
            {
                InternShip updatedInternship = ctx.InternShips.FirstOrDefault(x => x.InternShipID == id);
                if (updatedInternship != null)
                {
                    updatedInternship.DelDate = DateTime.Now;
                    TempData["JsFunc"] = "success();";
                    ctx.SaveChanges();
                }
                else
                {
                    TempData["JsFunc"] = "warning();";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("InternshipAdd");
            }
        }
    }
}