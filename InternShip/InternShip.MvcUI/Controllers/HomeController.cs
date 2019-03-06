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
        public ActionResult Index(string id)
        {
            return View();
        }

        //GET: InternShipForStudent
        public ActionResult InternShipForStudent()
        {
            //Öğrenciye ait stajlar
            if (Session["studentNumber"] != null)
            {
                string number = Session["studentNumber"].ToString();
                Student _student = context.Students.SingleOrDefault(x => x.StudentNumber == number & x.DelDate == null);
                ViewBag.Internships = context.InternShips.Where(x => x.StudentID == _student.StudentID & x.DelDate == null).OrderByDescending(x => x.CrtDate).ToList();
                return View();
            }
            else
            {
                ViewBag.Internships = null;
                ViewBag.JsFunc = "errorMessage('Öğrenci girişi yapılmamış. Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin","Login");
            }
            
        }
    }
}