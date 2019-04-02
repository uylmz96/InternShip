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
    using System.Data;
    using System.Data.Entity.Core.EntityClient;
    using System.Reflection;
    [Authorize]
    public class StudentController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Index
        public ActionResult Index()
        {
            #region Öğrencilerin listelenmesi tamamlanan staj gün sayıları ile birlikte
            //Öğrencilerin toplam kabul edilen staj gün sayıları
            var ExtentedStudent = (from s in context.Students
                                   join i in context.InternShips on s.StudentID equals i.StudentID
                                   join r in context.InternShipResults on i.InternShipID equals r.InternShipID
                                   where s.DelDate == null & i.DelDate == null &(s.isGraduate == false || s.isGraduate == null)
                                   group r by s.StudentID into t
                                   select new
                                   {
                                       studentID = t.Key,
                                       sumAcceptedTime = t.Sum(x => x.AcceptedTime)
                                   }).ToList();

            //Tüm öğrenciler
            List<StudentExtentedModel> extentedModels = new List<StudentExtentedModel>();
            foreach (Student item in context.Students.Where(x => x.DelDate == null & (x.isGraduate ==false || x.isGraduate==null)).ToList())
            {
                StudentExtentedModel model = new StudentExtentedModel();
                item.MapTo(model);
                model.sumAcceptedTime = 0;
                extentedModels.Add(model);
            }

            //İkisini birleştirme işlemi
            foreach (var item in ExtentedStudent)
            {
                StudentExtentedModel model = extentedModels.FirstOrDefault(x => x.StudentID == item.studentID) as StudentExtentedModel;
                model.sumAcceptedTime = Convert.ToInt16(item.sumAcceptedTime);
            } 
            #endregion
            
            ViewBag.Students = extentedModels.OrderByDescending(x=>x.CrtDate);
            //  ViewBag.Students = context.Students.Where(x => x.DelDate == null).ToList();
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //GET:
        public ActionResult GraduateStudents()
        {
            #region Öğrencilerin listelenmesi tamamlanan staj gün sayıları ile birlikte
            //Öğrencilerin toplam kabul edilen staj gün sayıları
            var ExtentedStudent = (from s in context.Students
                                   join i in context.InternShips on s.StudentID equals i.StudentID
                                   join r in context.InternShipResults on i.InternShipID equals r.InternShipID
                                   where s.DelDate==null & i.DelDate==null & s.isGraduate==true
                                   group r by s.StudentID into t
                                   select new
                                   {
                                       studentID = t.Key,
                                       sumAcceptedTime = t.Sum(x => x.AcceptedTime)
                                   }).ToList();

            //Tüm öğrenciler
            List<StudentExtentedModel> extentedModels = new List<StudentExtentedModel>();
            foreach (Student item in context.Students.Where(x => x.DelDate == null & x.isGraduate == true).ToList())
            {
                StudentExtentedModel model = new StudentExtentedModel();
                item.MapTo(model);
                model.sumAcceptedTime = 0;
                extentedModels.Add(model);
            }

            //İkisini birleştirme işlemi
            foreach (var item in ExtentedStudent)
            {
                StudentExtentedModel model = extentedModels.FirstOrDefault(x => x.StudentID == item.studentID) as StudentExtentedModel;
                model.sumAcceptedTime = Convert.ToInt16(item.sumAcceptedTime);
            }
            #endregion

            ViewBag.Students = extentedModels.OrderByDescending(x => x.CrtDate);
            //  ViewBag.Students = context.Students.Where(x => x.DelDate == null).ToList();
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
            Student insertedStudent = context.Students.SingleOrDefault(x => x.StudentNumber == student.StudentNumber & x.DelDate == null);
            if (insertedStudent == null)
            {
                student.CrtDate = DateTime.Now;
                student.StudentPassword = student.StudentNumber;
                context.Set<Student>().Add(student);
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Öğrenci numarasına ait öğrenci zaten mevcut ');";
                return RedirectToAction("Student", new { id = insertedStudent.StudentID });
            }
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
                return RedirectToAction("Student", new { id = student.StudentID });
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
                return RedirectToAction("Student", new { id = id });
            }
        }

        //GET PasswordChange
        [AllowAnonymous]
        public ActionResult PasswordChange()
        {
            if (Session["studentNumber"] == null)//Öğrenci Girişi yapılmış mı
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        //POST: PasswordChage
        [AllowAnonymous][HttpPost]
        public ActionResult PasswordChange(string OldPassword, string Password, string PasswordAgain)
        {
            try
            {

                string studentNumber = Session["studentNumber"].ToString();                
                if (Password.Equals(PasswordAgain))
                {
                    Student _student = context.Students.FirstOrDefault(x => x.StudentNumber == studentNumber);
                    if (_student != null)
                    {

                        _student.StudentPassword = Password;
                        TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
                    }
                    else
                    {
                        TempData["JsFunc"] = "warning('Öğrenci bulunamadı tekrar giriş yapınız.');";
                    }
                }
                else
                {
                    TempData["JsFunc"] = "errorMessage('Girilen parolalar eşleşmemektedir.');";
                }
                return RedirectToAction("PasswordChange", "Student");
            }
            catch (Exception)
            {
                TempData["JsFunc"] = "error();";
                return RedirectToAction("PasswordChange", "Student");
            }
            
        }

        //GET:PasswordReset
        public ActionResult PasswordReset(int id)
        {
            Student updatedStudent = context.Students.FirstOrDefault(x=>x.StudentID==id);
            if (updatedStudent != null)
            {
                updatedStudent.StudentPassword = updatedStudent.StudentNumber;
                TempData["JsFunc"] = Result.isAppliedSaveChanges(context);
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Öğrenci bulunamadı.')";
            }
            return RedirectToAction("Index","Student");
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

        //GET: GetCompanyName
        public JsonResult AutoCompleteName(string search)
        {
            int id = Convert.ToInt32(search);
            List<autocomplete> allsearch = context.Students.Where(x => x.StudentID == id & x.DelDate == null).Select(x => new autocomplete
            {
                id = x.StudentID.ToString(),
                value = x.StudentNumber + " - " + x.Name + " " + x.Surname
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


    }
}