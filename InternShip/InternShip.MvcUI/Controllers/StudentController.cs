using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternShip.MvcUI.Controllers
{
    using InternShip.MvcUI.App_Classes;
    using InternShip.MvcUI.App_Classes.DTO;
    using Models;
    using System.Reflection;
    [Authorize]
    public class StudentController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Index
        public ActionResult Index()
        {
            ViewBag.Students = context.Students.Where(x => x.DelDate == null).ToList();
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }
        //GET: StudentAdd
        public ActionResult Student(int id)
        {
            Student student = context.Students.SingleOrDefault(x => x.StudentID == id);
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View(student);
        }
        //POST: StudentAdd
        [HttpPost]
        public ActionResult StudentAdd(Student student)
        {
            context.Set<Student>().Add(student);
            TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
            return RedirectToAction("Index");
        }
        //POST: StudentUpdate
        [HttpPost]
        public ActionResult StudentUpdate(Student student)
        {
            Student updatedStudent = context.Students.SingleOrDefault(x => x.StudentID == student.StudentID);
            if (updatedStudent != null)
            {
                student.MapTo<Student>(updatedStudent);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Student", new { id = updatedStudent.StudentID });
            }           
        }
        //POST: StudentDelete
        [HttpGet]
        public ActionResult StudentDelete(int id)
        {
            Student updatedStudent = context.Students.SingleOrDefault(x => x.StudentID == id);
            if (updatedStudent != null)
            {
                updatedStudent.DelDate = DateTime.Now;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Bilgilere Erişilemiyor.');";
                return RedirectToAction("Student", new { id = updatedStudent.StudentID });
            }
        }
        //GET: GetStudent Autocomplete
        [HttpGet]
        public JsonResult AutoComplete(string search)
        {
            List<autocomplete> allsearch = context.Students.Where(x => x.StudentNumber.Contains(search) & x.DelDate == null).Select(x => new autocomplete
            {
                id = x.StudentID.ToString(),
                value = x.StudentNumber + " - " + x.Name + " " + x.Surname
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}