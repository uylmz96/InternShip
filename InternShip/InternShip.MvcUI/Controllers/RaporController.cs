using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternShip.MvcUI.Controllers
{
    using App_Classes;
    using Models;
    [Authorize]
    public class RaporController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Rapor
        public ActionResult Index()
        {
            ViewBag.Company = context.Companies.Where(x => x.DelDate == null).ToList();
            ViewBag.Cities = context.Cities.OrderBy(x => x.CityName).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CheckInternships(Rapor fields)
        {

            //Tüm Listeyi getir
            List<InternShip> result = context.InternShips.ToList();
            DateTime nullDate = new DateTime(0001, 01, 01, 00, 00, 00); //ön yüzde tarih seçilmediği zaman bu tarih oluşuyor 

            //Rapor sınıfındaki fieldlardan null olmayanları where koşulunda kullan.
            if (fields.CompanyID != -1 & fields.CompanyID != null)
            {
                result = result.Where(x => x.CompanyID == fields.CompanyID).ToList();
            }
            if (fields.EndDate != nullDate)
            {
                result = result.Where(x => x.EndDate <= fields.EndDate).ToList();
            }
            if (fields.StartDate != nullDate)
            {
                result = result.Where(x => x.StartDate >= fields.StartDate).ToList();
            }
            if (fields.StudentID > 0 & fields.StudentID != null)
            {
                result = result.Where(x => x.StudentID == fields.StudentID).ToList();
            }
            if (fields.City > 0 && fields.City != null)
            {
                result = result.Where(x => x.City == fields.City).ToList();
            }
            if (fields.Keyword != null)
            {
                Frequance frequance = new Frequance();
                result = frequance.CheckKeywords(result, fields.Keyword);
            }

            TempData["PrintedInternships"] = result;
            return RedirectToAction("Print", "Rapor");

            //Listeleme ve yazdırma için vir action yazılacak.
        }

        public ActionResult Print()
        {
            ViewBag.Internships = TempData["PrintedInternships"];
            return View();
        }

    }
}