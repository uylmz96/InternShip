using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using InternShip.MvcUI.App_Classes;
    using Models;
    [Authorize]
    public class MailController : Controller
    {
        InternShipContext _context = new InternShipContext();
        // GET: Mail
        public ActionResult Index()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            List<ExtraData> model = _context.ExtraDatas.Where(x => x.DataType == "MailAddress" || x.DataType == "MailPassword").ToList();
            return View(model);
        }
        public ActionResult MailChange(string Mail,string Password)
        {
            ExtraData MailAddress= _context.ExtraDatas.FirstOrDefault(x => x.DataType == "MailAddress");
            ExtraData MailPassword = _context.ExtraDatas.FirstOrDefault(x => x.DataType == "MailPassword");
            if(MailAddress !=null & MailPassword != null) //varsa güncelle
            {
                MailAddress.Data = Mail;
                MailPassword.Data = Password;
                
            }
            else if(MailAddress == null & MailPassword == null) //yoksa oluştur.
            {
                //Mail Adresi
                MailAddress = new ExtraData();
                MailAddress.DataType = "MailAddress";
                MailAddress.Data = "meubilgisayarmuhstaj@gmail.com";
                //Mail Şifresi
                MailPassword = new ExtraData();
                MailPassword.DataType = "MailPassword";
                MailPassword.Data = "aQW75TETvyAm][{@";

            }

            TempData["JsFunc"] = Result.isAppliedSaveChanges(_context);
            return RedirectToAction("Index");
        }
    }
}