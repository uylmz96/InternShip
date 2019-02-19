using InternShip.MvcUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    public class CompanyController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Company
        public ActionResult Index()
        {
            ViewBag.Companies = context.Companies.Where(x => x.DelDate == null).ToList();
            return View();
        }

        //GET: CompanyAdd
        public ActionResult CompanyAdd()
        {
            return View();
        }

        //POST: CompanyAdd
        [HttpPost]
        public ActionResult CompanyAdd(Company company)
        {
            context.Set<Company>().Add(company);
            try
            {
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        //POST: CompanyDelete
        [HttpPost]
        public ActionResult CompanyDelete(int id)
        {
            Company deletedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            try
            {
                deletedCompany.DelDate = DateTime.Now;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        //GET: CompanyUpdate
        public ActionResult CompanyUpdate(int id)
        {
            Company company = context.Companies.SingleOrDefault(x => x.CompanyID == id);
            return View(company);
        }

        //POST: CompanyUpdate
        [HttpPost]
        public ActionResult CompanyUpdate(Company company)
        {
            try
            {
                Company updatedCompany = context.Companies.SingleOrDefault(x => x.CompanyID == company.CompanyID);
                if (updatedCompany != null)
                {
                    company.MapTo<Company>(updatedCompany);
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        //GET: BlackList
        public ActionResult CompanyBlackList()
        {
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
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
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
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

    }
}