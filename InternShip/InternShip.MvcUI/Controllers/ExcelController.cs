using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace InternShip.MvcUI.Controllers
{
    using Models;
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Student()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }
        [HttpPost]
        public ActionResult UploadStudentExcel(HttpPostedFileBase excelFile)
        {
            //Dosya kontrolü
            if (excelFile == null
            || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Lütfen dosya seçimi yapınız.";

                return View();
            }
            else
            {
                InternShipContext _context = new InternShipContext();
                Excel.Application application = null;
                List<Student> localList = null;
                //Dosyanın uzantısı xls ya da xlsx ise;
                try
                {

                    if (excelFile.FileName.EndsWith("xls")
                           || excelFile.FileName.EndsWith("xlsx"))
                    {
                        //Seçilen dosyanın nereye kaydedileceği belirtiliyor.
                        string path = Server.MapPath("~/Documents/Imports/Uploaded/" + excelFile.FileName);

                        //Dosya kontrol edilir, varsa silinir.
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        //Excel path altına kaydedilir.
                        excelFile.SaveAs(path);

                        //+Exceli açıyoruz
                        application = new Excel.Application();
                        Excel.Workbook workbook = application.Workbooks.Open(path);
                        Excel.Worksheet worksheet = workbook.ActiveSheet;
                        Excel.Range range = worksheet.UsedRange;
                        //-

                        //Verileri listeye atıp listele viewında göstereceğim, o yüzden modelimin
                        //tipinde liste değişkeni tanımlıyorum.
                        localList = new List<Student>();

                        //tüm verilerde dönüp ilgili veriyi ilgili modele atıyorum. sonrasında modeli
                        //listeye atıyorum.
                        for (int i = 2; i <= range.Rows.Count; i++)
                        {

                            //Excel den okuyup modele ekleme işlemi
                            string studentNumber = ((Excel.Range)range.Cells[i, 1]).Text;
                            Student _student = _context.Students.FirstOrDefault(x => x.StudentNumber == studentNumber);
                            if (_student == null)
                            {
                                _student = new Student();
                                _student.StudentNumber = ((Excel.Range)range.Cells[i, 1]).Text;
                                _student.Name = ((Excel.Range)range.Cells[i, 2]).Text;
                                _student.Surname = ((Excel.Range)range.Cells[i, 3]).Text;
                                _student.Mail = ((Excel.Range)range.Cells[i, 4]).Text;
                                _student.CrtDate = DateTime.Now;
                                _student.isGraduate = false;

                                if (((Excel.Range)range.Cells[i, 5]).Text != "")
                                    _student.Phone = ((Excel.Range)range.Cells[i, 5]).Text;
                                else
                                    _student.Phone = "(000) 000 0000";

                                if (((Excel.Range)range.Cells[i, 6]).Text != "")
                                    _student.EntryDate = ((Excel.Range)range.Cells[i, 6]).Text;
                                else
                                    _student.EntryDate = DateTime.Now;
                                localList.Add(_student);
                            }
                        }
                        application.Quit();

                        //Database Kaydetme
                        if (localList != null)
                        {
                            _context.Students.AddRange(localList);
                            _context.SaveChanges();
                            TempData["JsFunc"] = "success();";
                        }
                        return RedirectToAction("Student");
                    }
                    else
                    {
                        TempData["JsFunc"] = "errorMessage('Dosya tipiniz yanlış, lütfen '.xls' yada '.xlsx' uzantılı dosya yükleyiniz.');";
                        return RedirectToAction("Student");
                    }
                }
                catch (Exception ex)
                {
                    TempData["JsFunc"] = "errorMessage('Hatalı düzenlenmiş dosya lütfen kontol edip tekrar deneyiniz.');";
                    application.Quit();
                    return RedirectToAction("Student");
                }
            }
        }

        public ActionResult GraduateStudent()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult UploadGraduateStudentExcel(HttpPostedFileBase excelFile)
        {
            //Dosya kontrolü
            if (excelFile == null
            || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Lütfen dosya seçimi yapınız.";

                return View();
            }
            else
            {
                InternShipContext _context = new InternShipContext();
                Excel.Application application = null;
                List<Student> localList = null;
                //Dosyanın uzantısı xls ya da xlsx ise;
                try
                {

                    if (excelFile.FileName.EndsWith("xls")
                           || excelFile.FileName.EndsWith("xlsx"))
                    {
                        //Seçilen dosyanın nereye kaydedileceği belirtiliyor.
                        string path = Server.MapPath("~/Documents/Imports/Uploaded/" + excelFile.FileName);

                        //Dosya kontrol edilir, varsa silinir.
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        //Excel path altına kaydedilir.
                        excelFile.SaveAs(path);

                        //+Exceli açıyoruz
                        application = new Excel.Application();
                        Excel.Workbook workbook = application.Workbooks.Open(path);
                        Excel.Worksheet worksheet = workbook.ActiveSheet;
                        Excel.Range range = worksheet.UsedRange;
                        //-

                        //Verileri listeye atıp listele viewında göstereceğim, o yüzden modelimin
                        //tipinde liste değişkeni tanımlıyorum.
                        localList = new List<Student>();

                        //tüm verilerde dönüp ilgili veriyi ilgili modele atıyorum. sonrasında modeli
                        //listeye atıyorum.
                        for (int i = 2; i <= range.Rows.Count; i++)
                        {

                            //Excel den okuyup modele ekleme işlemi
                            string studentNumber = ((Excel.Range)range.Cells[i, 1]).Text;
                            if (studentNumber != "" & studentNumber != null)
                            {
                                Student _student = _context.Students.FirstOrDefault(x => x.StudentNumber == studentNumber);
                                _student.isGraduate = true;
                                _student.GraduateDate = DateTime.Now;
                                localList.Add(_student);
                            }
                        }
                        application.Quit();

                        //Database Kaydetme
                        if (localList != null)
                        {
                            _context.SaveChanges();
                            TempData["JsFunc"] = "success();";
                        }
                        return RedirectToAction("GraduateStudent");
                    }
                    else
                    {
                        TempData["JsFunc"] = "errorMessage('Dosya tipiniz yanlış, lütfen '.xls' yada '.xlsx' uzantılı dosya yükleyiniz.');";
                        return RedirectToAction("GraduateStudent");
                    }
                }
                catch (Exception ex)
                {
                    TempData["JsFunc"] = "errorMessage('Hatalı düzenlenmiş dosya lütfen kontol edip tekrar deneyiniz.');";
                    application.Quit();
                    return RedirectToAction("GraduateStudent");
                }
            }
        }


    }
}