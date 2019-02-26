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
            // id=1?Ekleme:Güncelleme
            RefusalReason reason = ctx.RefusalReasons.SingleOrDefault(x => x.ReasonID == id);
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View(reason);
        }

        //POST: ReasonAdd
        [HttpPost]
        public ActionResult ReasonAdd(string Reason,string Desc)
        {
            try
            {
                RefusalReason _reason = new RefusalReason();
                _reason.Reason = Reason;
                _reason.Desc = Desc;
                ctx.Set<RefusalReason>().Add(_reason);
                // ctx.Set<RefusalReason>().Add(reason);
                ctx.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");

            }

        }

        //POST: ReasonUpdate
        [HttpPost]
        public ActionResult ReasonUpdate(int ReasonID, string Reason, string Desc)
        {
            try
            {
                RefusalReason updatedReason = ctx.RefusalReasons.FirstOrDefault(x => x.ReasonID == ReasonID);
                if (updatedReason != null)
                {
                    updatedReason.Reason = Reason;
                    updatedReason.Desc = Desc;
                }
                ctx.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }

        //GET: ReasonUDelete
        [HttpGet]
        public ActionResult ReasonDelete(int id)
        {
            try
            {
                RefusalReason deletedReason = ctx.RefusalReasons.FirstOrDefault(x => x.ReasonID == id);
                if (deletedReason != null)
                {
                    deletedReason.DelDate = DateTime.Now;
                }
                ctx.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }
    }
}