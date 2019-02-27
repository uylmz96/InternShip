using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using App_Classes;
    using System.Web.Security;

    public class LoginController : Controller
    {
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
            return RedirectToAction("Login", "Login");
        }

    }
}