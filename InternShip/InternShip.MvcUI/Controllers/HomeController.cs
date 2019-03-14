using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using Models;

    public class HomeController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //GET: InternShipForStudent
        public ActionResult InternShipForStudent()
        {
            if (Session["studentNumber"] == null)//Öğrenci Girişi yapılmış mı
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }

            if (TempData["JsFunc"] != null) //Mesaj varmı
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            
            //Öğrenciye ait stajlar
            string number = Session["studentNumber"].ToString();
            Student _student = context.Students.SingleOrDefault(x => x.StudentNumber == number & x.DelDate == null);
            ViewBag.Internships = context.InternShips.Where(x => x.StudentID == _student.StudentID & x.DelDate == null).OrderByDescending(x => x.CrtDate).ToList();
            return View();


        }

    }
}