using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternShip.MvcUI.Controllers
{
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
            Student student = context.Students.SingleOrDefault(x=>x.StudentID==id);
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View(student);
        }

        //POST: StudentAdd
        [HttpPost]
        public ActionResult StudentAdd(Student student)
        {            
            try
            {
                context.Set<Student>().Add(student);
                context.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }

        }

        //POST: StudentUpdate
        [HttpPost]
        public ActionResult StudentUpdate(Student student)
        {
            try
            {
                Student updatedStudent = context.Students.SingleOrDefault(x => x.StudentID == student.StudentID);
                if (updatedStudent != null)
                {
                    student.MapTo<Student>(updatedStudent);
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

        //POST: StudentDelete
        [HttpGet]
        public ActionResult StudentDelete(int id)
        {           
            try
            {
                Student deletedStudent = context.Students.SingleOrDefault(x => x.StudentID == id);
                if (deletedStudent != null)
                    deletedStudent.DelDate = DateTime.Now;
                context.SaveChanges();
                TempData["JsFunc"] = "success();";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("Index");
            }
        }        

        //GET: GetStudent Autocomplete
        [HttpGet]
        public JsonResult GetStudent(string query)
        {
            var list = context.Students.ToList();

            var autoCompleteList = new List<autocomplete>();
            foreach (Student item in list)
            {
                autoCompleteList.Add(new autocomplete
                {
                    id = item.StudentID.ToString(),
                    value = String.Format("{0} - {1} {2}",item.StudentNumber,item.Name,item.Surname)
                });
            }
            return Json(autoCompleteList, JsonRequestBehavior.AllowGet);
        }
    }
}