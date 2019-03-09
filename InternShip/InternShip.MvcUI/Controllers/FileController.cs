using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using InternShip.MvcUI.App_Classes.DTO;
    using Models;
    using System.IO;
    using static System.Net.Mime.MediaTypeNames;

    public class FileController : Controller
    {
        InternShipContext context = new InternShipContext();
        public ActionResult FileUpload(int id)
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            if (Session["studentNumber"] != null)
            {
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
            else
            {
                ViewBag.Internships = null;
                //ViewBag.JsFunc = "errorMessage('Öğrenci girişi yapılmamış. Lütfen giriş yapınız.')";
                return RedirectToAction("StudentLogin", "Login");
            }
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, int internshipID)
        {
            //Staj notlandırılmışsa belge yüklenemez.
            InternShipResult _result = context.InternShipResults.FirstOrDefault(x => x.InternShipID == internshipID);
            if (_result != null)
            {
                TempData["JsFunc"] = "warningMessage('Notlandırılmış staja belge yüklenemez.')";
                return RedirectToAction("InternShipForStudent", "Home");
            }

            //Belge Yükleme işlemi
            string studentNumber = Session["studentNumber"].ToString();
            if (file != null && file.ContentLength > 0 & file.ContentLength < 4000000)
                try
                {
                    string folder = Path.Combine(Server.MapPath("/Documents/" + internshipID)); //Folder Path
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder); // Öğrencinin Stajı için klasör oluştur.

                    string path = Path.Combine(folder, Path.GetFileName(studentNumber + "_" + file.FileName));
                    file.SaveAs(path);
                    TempData["JsFunc"] = "success();";
                    return RedirectToAction("InternShipForStudent", "Home");
                }
                catch (Exception ex)
                {
                    TempData["JsFunc"] = "errorMessage('ERROR: " + ex.Message.ToString() + "')";
                    return RedirectToAction("FileUpload", new { id = internshipID });
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

        [HttpPost]
        public JsonResult FileDownload(int id)
        {
            string[] files = null;
            List<autocomplete> allsearch = new List<autocomplete>();
            string fileNames="",folder = Server.MapPath("/Documents/" + id); //Path.Combine(Server.MapPath("/Documents/" + id)); //Folder Path
            if (Directory.Exists(folder))
            {
                files = Directory.GetFiles(folder);
                foreach (string item in files)
                {
                    allsearch.Add(new autocomplete
                    {
                        //id = Request.Url.Authority+"/Documents/" +id+"/",
                        id="/Documents/"+id+"/",
                        value = item
                    });
                }
            }
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
