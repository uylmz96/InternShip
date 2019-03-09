using InternShip.MvcUI.App_Classes;
using InternShip.MvcUI.App_Classes.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;


namespace InternShip.MvcUI.Controllers
{
    using App_Classes;
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            MembershipUserCollection Users = Membership.GetAllUsers();
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View(Users);
        }

        //GET: User
        public ActionResult User(string id)
        {
            if (id == null)
            {
                return View();
            }
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            MembershipUser user = Membership.GetUser(id);
            return View(user);
        }

        //POST: UserAdd()
        [HttpPost]
        public ActionResult UserAdd(User user)
        {
            try
            {
                if (user.Password.Equals(user.PasswordAgain))
                {
                    Membership.CreateUser(user.UserName, user.Password, user.Mail);
                    MembershipUser u = Membership.GetUser(user.UserName);
                    u.Comment = user.Name + " " + user.Surname;
                    Membership.UpdateUser(u);
                    TempData["JsFunc"] = "success();";
                    return RedirectToAction("Index");
                }
                else
                {

                    ViewBag.Message = "Parolalar uyuşmamaktadır.";
                    TempData["JsFunc"] = "warning();";
                    return RedirectToAction("User", "User", new { id = user.UserName });
                }
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return View();
            }
        }

        //POST: UserUpdate
        [HttpPost]
        public ActionResult UserUpdate(User user)
        {
            try
            {
                MembershipUser u = Membership.GetUser(user.UserName);
                if (u != null)
                {
                    u.Comment = user.Name + " " + user.Surname;
                    u.Email = user.Mail;
                    Membership.UpdateUser(u);
                    TempData["JsFunc"] = "success();";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["JsFunc"] = "warning();";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return View();
            }
        }

        //POST: GetUserRoles
        [HttpPost]
        public string GetUserRoles(string Username)
        {
            /* [20.02.2019] Umut Yılmaz
             * Kullanıcının rollerini alt alta döndürüyor.
             */
            List<string> roles = Roles.GetRolesForUser(Username).ToList();
            string organizedRoles = "";
            if (roles.Count > 0)
            {
                foreach (string item in roles)
                {
                    organizedRoles += item + ",";
                }
                organizedRoles = organizedRoles.Substring(0, organizedRoles.Length - 1);
            }
            return organizedRoles;
        }

        //POST: RemoveUserRole
        [HttpGet]
        public ActionResult RemoveUserRole()
        {
            try
            {
                string Username, RoleName;
                Username = Request.QueryString["username"];
                RoleName = Request.QueryString["role"];
                if (Roles.RoleExists(RoleName) & Membership.GetUser(Username) != null)
                    Roles.RemoveUserFromRole(Username, RoleName);
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }

        //Autocomplete
        public JsonResult AutoComplete(string search)
        {
            List<autocomplete> allsearch = new List<autocomplete>();
            foreach (MembershipUser item in Membership.GetAllUsers())
            {
                allsearch.Add(new autocomplete
                {
                    id = item.UserName,
                    value = string.Format("{0} - [{1}]", item.Comment, item.UserName)
                });
            }
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [AllowAnonymous]
        //GET: ForgotPassword
        public ActionResult ForgotPassword()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        //POST: ForgotPassword
        public ActionResult ForgotPassword(User u)
        {
            MembershipUser user = null;
            if (u.UserName != null)
            {
                user = Membership.GetUser(u.UserName);
            }
            else if (u.Mail != null)
            {
                string _username = Membership.GetUserNameByEmail(u.UserName);
                user = Membership.GetUser(_username);
            }


            if (user != null)
            {
                string newPassword = user.ResetPassword();
                
                string body = "Yeni Şifreniz : " + 
                                newPassword + 
                                "<a href=http://" + Request.Url.Authority + "/Login/Login> Giriş Yap </a>";   

                TempData["JsFunc"] = Mail.sendMail(user.Email, "Şifre Sıfırlama", body);
                return RedirectToAction("Login", "Login");
            }
            else
            {
                TempData["JsFunc"] = "error();";
                return View();
            }
        }       

        //GET: ActivePasive
        //[HttpPost]
        public ActionResult ActivePasive(string id)
        {
            try
            {
                MembershipUser user = Membership.GetUser(id);
                if (user != null)
                {
                    bool isAdminUser = Roles.GetRolesForUser(user.UserName).Contains("Admin");
                    if (!isAdminUser)
                    {
                        if (user.IsApproved)
                        {
                            user.IsApproved = false;
                        }
                        else
                        {
                            user.IsApproved = true;
                        }
                        Membership.UpdateUser(user);
                    }
                }
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }

        }
    }
}