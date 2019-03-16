using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternShip.MvcUI.Controllers
{
    using App_Classes;
    using Models;
    using System.Web.Security;

    public class LoginController : Controller
    {

        InternShipContext context = new InternShipContext();
        // GET: Login
        public ActionResult Login()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //POST: Login
        [HttpPost]
        public ActionResult Login(User user, string RememberMe)
        {
            bool haveUser = Membership.ValidateUser(user.UserName, user.Password);
            if (haveUser)
            {
                if (RememberMe == "on")
                    FormsAuthentication.RedirectFromLoginPage(user.UserName, true);
                else
                    FormsAuthentication.RedirectFromLoginPage(user.UserName, false);

                return RedirectToAction("Index", "Student");
            }
            else
            {
                MembershipUser _user = Membership.GetUser(user.UserName);
                if (!_user.IsApproved)
                    TempData["JsFunc"] = "errorMessage('Kullanıcı engellenmiş.');";
                else
                    TempData["JsFunc"] = "errorMessage('Kullanıcı adı yada şifre hatalı.');";


                return RedirectToAction("Login");
            }

        }

        //GET: Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            if (Session["studentNumber"] != null)
                Session.Remove("studentNumber");
            if (Session["studentName"] != null)
                Session.Remove("studentName");
            return RedirectToAction("Login", "Login");
        }

        //GET: StudentLogin
        public ActionResult StudentLogin()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //POST: StudentLogin
        [HttpPost]
        public ActionResult StudentLogin(string studentNumber)
        {
            Student student = context.Students.SingleOrDefault(x => x.StudentNumber == studentNumber & x.DelDate == null);
            if (student != null)
            {
                Session.Add("studentNumber", student.StudentNumber);
                Session.Add("studentName", string.Format(" {0} {1}", student.Name, student.Surname));
                return RedirectToAction("InternShipForStudent", "Home");
            }
            else
            {
                TempData["JsFunc"] = "errorMessage('Öğrenci bulunamadı. Yetkili ile görüşünüz.');";
                return RedirectToAction("StudentLogin");
            }
        }

    }
}