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
    using System.IO;
    using static System.Net.Mime.MediaTypeNames;
    [Authorize]
    public class FileController : Controller
    {
        InternShipContext context = new InternShipContext();
        [AllowAnonymous]
        public ActionResult FileUpload(int id)
        {
            if (Session["studentNumber"] == null)//Öğrenci Girişi yapılmış mı
            {
                ViewBag.Internships = null;
                TempData["JsFunc"] = "errorMessage('Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }

            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();

            InternShip _internship = context.InternShips.FirstOrDefault(x => x.InternShipID == id);
            if (_internship != null)
            {
                ViewBag.ID = id.ToString();
                return View();
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Staj Bulunamadı.')";
                return RedirectToAction("InternShipForStudent", "Home");
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, int internshipID)
        {
            #region Dosya Yüklenebilirlik Kontrolü
            ExtraData FileUpload = context.ExtraDatas.FirstOrDefault(x => x.DataType == "FileUpload");
            //Staj notlandırılmışsa belge yüklenemez.
            InternShipResult _result = context.InternShipResults.FirstOrDefault(x => x.InternShipID == internshipID);
            if (_result != null)
            {
                TempData["JsFunc"] = "warningMessage('Notlandırılmış staja belge yüklenemez.')";
                return RedirectToAction("InternShipForStudent", "Home");
            }
            if (FileUpload.Data == "close")
            {
                TempData["JsFunc"] = "warningMessage('Dosya yükleme süresi bitmiştir.')";
                return RedirectToAction("InternShipForStudent", "Home");
            }
            #endregion

            //Belge Yükleme işlemi


            string studentNumber = Session["studentNumber"].ToString();
            if (file != null && file.ContentLength > 0 & file.ContentLength < 4000000) // file<4Mb 
            {
                try
                {
                    string folder = Path.Combine(Server.MapPath("/Documents/" + internshipID)); //Folder Path
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder); // Öğrencinin Stajı için klasör oluştur.

                    string path = Path.Combine(folder, Path.GetFileName(studentNumber + "_" + file.FileName));
                    
                    file.SaveAs(path);
                    //Dosyanın kelimelerinin frekanslarını hesaplatıp kaydetme işlemi                  
                    Frequance frequance = new Frequance();
                    frequance.FileFrequance(path, file, internshipID);

                    TempData["JsFunc"] = "success();";
                    return RedirectToAction("InternShipForStudent", "Home");
                }
                catch (Exception ex)
                {
                    TempData["JsFunc"] = "errorMessage('ERROR: " + ex.Message.ToString() + "')";
                    return RedirectToAction("FileUpload", new { id = internshipID });
                }
            }
            else if (file.ContentLength > 4000000)
            {
                TempData["JsFunc"] = "warningMessage('Dosya Boyutu 4 MB ile sınırlandırılmıştır.')";
                return RedirectToAction("FileUpload", new { id = internshipID });
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Seçili dosya yok')";
                return RedirectToAction("FileUpload", new { id = internshipID });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult FileDownload(int id)
        {
            string[] files = null;
            List<autocomplete> allsearch = new List<autocomplete>();
            string fileNames = "", folder = Server.MapPath("/Documents/" + id); //Path.Combine(Server.MapPath("/Documents/" + id)); //Folder Path
            if (Directory.Exists(folder))
            {
                files = Directory.GetFiles(folder);
                foreach (string item in files)
                {
                    allsearch.Add(new autocomplete
                    {
                        //id = Request.Url.Authority+"/Documents/" +id+"/",
                        id = "/Documents/" + id + "/",
                        value = item
                    });
                }
            }
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public string FileUploadActivePasive()
        {
            string retValue = "Dosya Yükleme Açık";
            ExtraData FileUpload = context.ExtraDatas.FirstOrDefault(x => x.DataType == "FileUpload");
            if (FileUpload == null)//Yokda Açık olarak oluştur.
            {
                FileUpload = new ExtraData();
                FileUpload.DataType = "FileUpload";
                FileUpload.Data = "open";
                context.ExtraDatas.Add(FileUpload);
            }
            else if (FileUpload.Data == "open")
            {
                FileUpload.Data = "close";
                retValue = "Dosya Yükleme Kapalı";
            }
            else if (FileUpload.Data == "close")
            {
                FileUpload.Data = "open";
            }
            Result.isAppliedSaveChanges(context);
            return retValue;
        }

        public ActionResult EmptyInternshipFileUpload()
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();

            return View();
        }
        [HttpPost]
        public ActionResult EmptyInternshipFileUpload(HttpPostedFileBase file)
        {
            string folder = Path.Combine(Server.MapPath("/Documents/Imports/")); //Folder Path
            string path = Path.Combine(folder, Path.GetFileName("Staj-Dosyası.rar"));

            if (file.FileName.EndsWith("rar") || file.FileName.EndsWith("zip"))
            {
                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}
                file.SaveAs(path);
                TempData["JsFunc"] = "successMessage('Dosya başarılı şekilde değiştirilmiştir.');";
                return RedirectToAction("EmptyInternshipFileUpload");
            }
            else
            {
                TempData["JsFunc"] = "warningMessage('Dosya .rar yada .zip dosyası olmalıdır.');";
                return RedirectToAction("EmptyInternshipFileUpload");
            }

        }
    }
}
