using InternShip.MvcUI.App_Classes;
using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    [Authorize]
    public class RefusalReasonController : Controller
    {
        InternShipContext ctx = new InternShipContext();
        // GET: RefusalReason
        public ActionResult Index()
        {
            ViewBag.RefusalReasons = ctx.RefusalReasons.Where(x => x.DelDate == null).ToList();
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //GET: Reason
        public ActionResult Reason(int id)
        {
            RefusalReason reason = ctx.RefusalReasons.SingleOrDefault(x => x.ReasonID == id);
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View(reason);
        }

        //POST: ReasonAdd
        [HttpPost]
        public ActionResult ReasonAdd(string Reason, string Desc)
        {

            RefusalReason _reason = new RefusalReason();
            _reason.CrtDate = DateTime.Now;
            _reason.Reason = Reason;
            _reason.Desc = Desc;
            ctx.Set<RefusalReason>().Add(_reason);
            TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
            return RedirectToAction("Index");
        }

        //POST: ReasonUpdate
        [HttpPost]
        public ActionResult ReasonUpdate(int ReasonID, string Reason, string Desc)
        {
            RefusalReason updatedReason = ctx.RefusalReasons.FirstOrDefault(x => x.ReasonID == ReasonID);
            if (updatedReason != null)
            {
                updatedReason.Reason = Reason;
                updatedReason.Desc = Desc;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Reason", new { id = ReasonID });
            }
        }

        //GET: ReasonUDelete
        [HttpGet]
        public ActionResult ReasonDelete(int id)
        {
            RefusalReason updatedReason = ctx.RefusalReasons.FirstOrDefault(x => x.ReasonID == id);
            if (updatedReason != null)
            {
                updatedReason.DelDate = DateTime.Now;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(ctx);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Index");
            }
        }
    }
}