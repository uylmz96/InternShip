using InternShip.MvcUI.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace InternShip.MvcUI.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        //GET: PasswordChange
        public ActionResult PasswordChange()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }
        //POST: PasswordChange
        [HttpPost]
        public ActionResult PasswordChange(User u)
        {
            try
            {
                Page page = new Page();
                string username = page.User.Identity.Name;
                if (u.Password.Equals(u.PasswordAgain))
                {
                    MembershipUser _user = Membership.GetUser(username);
                    if (_user != null)
                    {
                        bool isChanged=_user.ChangePassword(u.OldPassword, u.Password);
                        if(isChanged)
                        TempData["JsFunc"] = "success();";
                        else
                            TempData["JsFunc"] = "errorMessage('Girilen eski şifre yanlış');";
                        return RedirectToAction("PasswordChange");
                    }
                    else
                    {
                        TempData["JsFunc"] = "warning();";
                        return RedirectToAction("PasswordChange");
                    }
                }
                else
                {
                    TempData["JsFunc"] = "errorMessage('Girilen parolalar eşleşmemektedir.');";
                    return View();
                }
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("PasswordChange");
            }

        }

        public ActionResult UpdateUser(string id)
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View(u);
        }
        //POST: UpdateUser
        [HttpPost]
        public ActionResult UpdateUser(User user)
        {
            string username=null;
            try
            {
                MembershipUser u = Membership.GetUser(user.UserName);
                if (u != null)
                {
                    u.Comment = user.Name + " " + user.Surname;
                    u.Email = user.Mail;
                    Membership.UpdateUser(u);
                    TempData["JsFunc"] = "success();";
                    return RedirectToAction("UpdateUser", "Personal", new { id = u.UserName});
                }
                else
                {
                    TempData["JsFunc"] = "warning();";
                    return RedirectToAction("UpdateUser", "Personal", new { id = u.UserName });
                }
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("UpdateUser", "Personal", new { id = username });
            }
        }
    }
}