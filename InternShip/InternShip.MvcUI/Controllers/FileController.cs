using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternShip.MvcUI.Controllers
{
    using Models;
    using System.IO;

    public class FileController : Controller
    {
        InternShipContext context = new InternShipContext();
        public ActionResult FileUpload(int id)
        {
            if (TempData["JsFunc"] != null)
                ViewBag.JsFunc = TempData["JsFunc"].ToString();
            if (Session["studentNumber"] != null)
            {
                InternShip _is = context.InternShips.FirstOrDefault(x => x.InternShipID == id);
                if (_is != null)
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
            string studentNumber = Session["studentNumber"].ToString();
            if (file != null && file.ContentLength > 0)
                try
                {
                    string[] names = file.FileName.Split('.');
                    if (names[names.Length - 1] != "rar" & names[names.Length - 1] != "zip")
                    {
                        TempData["JsFunc"] = "warningMessage('Yüklenen dosyanın uzantısı rar yada zip olmalıdır.')";
                        return RedirectToAction("FileUpload", new { id = internshipID });
                    }
                    string folder = Path.Combine(Server.MapPath("/Documents"));                    
                    string path = Path.Combine(folder,
                                              Path.GetFileName(studentNumber + "_" + internshipID + ".rar"));
                    file.SaveAs(path);
                    TempData["JsFunc"] = "success();";
                    return RedirectToAction("InternShipForStudent", "Home");
                }
                catch (Exception ex)
                {
                    TempData["JsFunc"] = "errorMessage('ERROR: " + ex.Message.ToString() + "')";
                    return RedirectToAction("FileUpload", new { id = internshipID });
                }
            else
            {
                TempData["JsFunc"] = "warningMessage('Seçili dosya yok')";
                return RedirectToAction("FileUpload", new { id = internshipID });
            }
        }       
    }
}
