using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InternShip.MvcUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            List<string> roles = Roles.GetAllRoles().ToList();
            return View(roles);
        }

        //GET: Role
        public ActionResult Role(string id)
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return id == "-1" ? View(string.Empty) : View(id);
        }

        //POST: RoleAdd
        [HttpPost]
        public ActionResult RoleAdd(string RolName)
        {
            TempData["JsFunc"] = "success();";
            Roles.CreateRole(RolName);
            return RedirectToAction("Index");
        }

        //POST: RoleUpdate
        [HttpPost]
        public ActionResult RoleUpdate(string RolName)
        {
            //TEST Aşamasında
            return RedirectToAction("Index");
        }

        //POST: RoleDelete
        [HttpGet]
        public ActionResult RoleDelete(string id)
        {
            int count = Roles.GetUsersInRole(id).Count();
            if (count == 0)
            {
                bool result= Roles.DeleteRole(id);
                if (result)
                    TempData["JsFunc"] = "success();";
                else
                    TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warning();";
                return RedirectToAction("Index");
            }
           
        }

        //GET: AddUser2Role
        public ActionResult AddUser2Role()
        {
            ViewBag.Users = Membership.GetAllUsers();
            ViewBag.Roles = Roles.GetAllRoles();
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //POST: AddUser2Role
        [HttpPost]
        public ActionResult AddUser2Role(string Username, string Role)
        {
            try
            {
                Roles.AddUserToRole(Username, Role);
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index", "User");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index", "User");
            }
        }

    }
}