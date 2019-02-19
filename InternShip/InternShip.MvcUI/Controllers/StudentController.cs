using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternShip.MvcUI.Controllers
{
    using Models;
    using System.Reflection;

    public class StudentController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Index
        public ActionResult Index()
        {
            ViewBag.Students = context.Students.Where(x => x.DelDate == null).ToList();
            return View();
        }

        //GET: StudentAdd
        public ActionResult StudentAdd()
        {
            return View();
        }

        //POST: StudentAdd
        [HttpPost]
        public ActionResult StudentAdd(Student student)
        {
            context.Set<Student>().Add(student);
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

        //POST: StudentDelete
        [HttpPost]
        public ActionResult StudentDelete(int StudentID)
        {
            Student deletedStudent = context.Students.SingleOrDefault(x => x.StudentID == StudentID);
            if (deletedStudent != null)
                deletedStudent.DelDate = DateTime.Now;
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

        //GET: StudentUpdate
        public ActionResult StudentUpdate(int id)
        {
            Student student = context.Students.SingleOrDefault(x => x.StudentID == id);
            return View(student);
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
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }

        }
    }
}