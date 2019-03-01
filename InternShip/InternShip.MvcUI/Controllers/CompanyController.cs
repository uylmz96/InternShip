using InternShip.MvcUI.App_Classes.DTO;
using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using InternShip.MvcUI.App_Classes;

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
            context.Set<Company>().Add(company);
            TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
            return RedirectToAction("Index");
        }

        //POST: CompanyUpdate
        [HttpPost]
        public ActionResult CompanyUpdate(Company company)
        {
            Company updatedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == company.CompanyID);
            if (updatedCompany != null)
            {
                company.MapTo<Company>(updatedCompany);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Company", new { id = updatedCompany.CompanyID });
            }
        }

        //GET: CompanyDelete 
        [HttpGet]
        public ActionResult CompanyDelete(int id)
        {
            Company updatedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            if (updatedCompany != null)
            {
                updatedCompany.DelDate = DateTime.Now;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Company", new { id = updatedCompany.CompanyID });
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
            Company updatedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            if (updatedCompany != null)
            {
                updatedCompany.IsBlackCompany = true;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Company", new { id = updatedCompany.CompanyID });
            }
        }

        //GET: RemoveBlackList
        public ActionResult RemoveBlackList(int id)
        {
            Company updatedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            if (updatedCompany != null)
            {
                updatedCompany.IsBlackCompany = false;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Company", new { id = updatedCompany.CompanyID });
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

        //GET: GetCompanyName
        public JsonResult AutoCompleteName(string search)
        {
            int id = Convert.ToInt32(search);
            List<autocomplete> allsearch = context.Companies.Where(x => x.CompanyID == id & x.DelDate == null).Select(x => new autocomplete
            {
                id = x.CompanyID.ToString(),
                value = x.CompanyName
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}