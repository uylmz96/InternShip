using InternShip.MvcUI.App_Classes.DTO;
using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace InternShip.MvcUI.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Company
        public ActionResult Index()
        {
            ViewBag.Companies = context.Companies.Where(x => x.DelDate == null).ToList();
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //GET: Company
        public ActionResult Company(int id)
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            Company company = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            return View(company);
        }

        //POST: CompanyAdd
        [HttpPost]
        public ActionResult CompanyAdd(Company company)
        {
            try
            {
                context.Set<Company>().Add(company);
                context.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Company");
            }
        }

        //POST: CompanyUpdate
        [HttpPost]
        public ActionResult CompanyUpdate(Company company)
        {
            Company updatedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == company.CompanyID);
            try
            {
                if (updatedCompany != null)
                {
                    company.MapTo<Company>(updatedCompany);
                    context.SaveChanges();
                    TempData["JsFunc"] = "success();";
                }
                else
                {
                    TempData["JsFunc"] = "warning();";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Company", "Company");
            }
        }

        //GET: CompanyDelete
        [HttpGet]
        public ActionResult CompanyDelete(int id)
        {
            try
            {
                Company deletedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
                if (deletedCompany != null)
                {
                    deletedCompany.DelDate = DateTime.Now;
                }
                context.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }

        //GET: CompanyBlackList
        public ActionResult CompanyBlackList()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();

            ViewBag.Companies = context.Companies.Where(x => x.IsBlackCompany == true & x.DelDate == null).ToList();
            return View();
        }

        //GET: AddBlackList        
        public ActionResult AddBlackList(int id)
        {
            Company blackCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            if (blackCompany != null)
                blackCompany.IsBlackCompany = true;
            try
            {
                context.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }

        //GET: RemoveBlackList
        public ActionResult RemoveBlackList(int id)
        {
            Company blackCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            if (blackCompany != null)
                blackCompany.IsBlackCompany = false;
            try
            {
                context.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }

        //GET: GetCompany Autocomplete
        public JsonResult AutoComplete(string search)
        {
            List<autocomplete> allsearch = context.Companies.Where(x => x.CompanyName.Contains(search) & x.DelDate == null).Select(x => new autocomplete
            {
                id = x.CompanyID.ToString(),
                value = x.CompanyName
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}