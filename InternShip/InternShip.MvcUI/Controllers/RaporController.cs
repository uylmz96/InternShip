using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternShip.MvcUI.Controllers
{
    using App_Classes;
    using Models;
    using OfficeOpenXml;

    [Authorize]
    public class RaporController : Controller
    {
        InternShipContext context = new InternShipContext();
        // GET: Rapor
        public ActionResult Index()
        {
            ViewBag.Company = context.Companies.Where(x => x.DelDate == null).ToList();
            ViewBag.Cities = context.Cities.OrderBy(x => x.CityName).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CheckInternships(Rapor fields)
        {

            //Tüm Listeyi getir
            List<InternShip> result = context.InternShips.ToList();
            DateTime nullDate = new DateTime(0001, 01, 01, 00, 00, 00); //ön yüzde tarih seçilmediği zaman bu tarih oluşuyor 

            //Rapor sınıfındaki fieldlardan null olmayanları where koşulunda kullan.
            if (fields.CompanyID != -1 & fields.CompanyID != null)
            {
                result = result.Where(x => x.CompanyID == fields.CompanyID).ToList();
            }
            if (fields.EndDate != nullDate)
            {
                result = result.Where(x => x.EndDate <= fields.EndDate).ToList();
            }
            if (fields.StartDate != nullDate)
            {
                result = result.Where(x => x.StartDate >= fields.StartDate).ToList();
            }
            if (fields.StudentID > 0 & fields.StudentID != null)
            {
                result = result.Where(x => x.StudentID == fields.StudentID).ToList();
            }
            if (fields.City > 0 && fields.City != null)
            {
                result = result.Where(x => x.City == fields.City).ToList();
            }
            if (fields.Keyword != null)
            {
                Frequance frequance = new Frequance();
                result = frequance.CheckKeywords(result, fields.Keyword);
            }

            TempData["PrintedInternships"] = result;
            if (result != null)
                ExportExcell(result);
            return RedirectToAction("Print", "Rapor");

            //Listeleme ve yazdırma için vir action yazılacak.
        }

        public ActionResult Print()
        {
            ViewBag.Internships = TempData["PrintedInternships"];
            return View();
        }

        public void ExportExcell(List<InternShip> result)
        {
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Rapor");

            ws.Cells["A1"].Value = "Staj ID";
            ws.Cells["B1"].Value = "Öğrenci Numarası";
            ws.Cells["C1"].Value = "Öğrenci Adı";
            ws.Cells["D1"].Value = "Öğrenci Soyadı";
            ws.Cells["E1"].Value = "Danışman";
            ws.Cells["F1"].Value = "Şirket Adı";
            ws.Cells["G1"].Value = "Şehir";
            ws.Cells["H1"].Value = "Başlangıç Tarihi";
            ws.Cells["I1"].Value = "Bitiş Tarihi";
            ws.Cells["J1"].Value = "Süre";
            ws.Cells["K1"].Value = "Sonuç";
            ws.Cells["L1"].Value = "Açıklama";
            ws.Cells["M1"].Value = "Kabul Edilen Süre";

            int rowStart = 2;
            foreach (InternShip item in result)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.InternShipID; //StajID
                if (item.Student != null)
                {
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.Student.StudentNumber; //Öğrenci Numarasu
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.Student.Name; //Öğrenci Adı
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.Student.Surname; //Öğrenci Soyadı 
                }
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.AdviserID; //Danışman
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Company.CompanyName; //Şirket Adı 
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.City1.CityName; //Şehir
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.StartDate; //Başlangıç Tarihi
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.EndDate;  //Bitiş Tarihi
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.Time; //Süre
                InternShipResult shipResult = context.InternShipResults.SingleOrDefault(x => x.InternShipID == item.InternShipID);
                if (shipResult != null)
                {
                    ws.Cells[string.Format("K{0}", rowStart)].Value = shipResult.RefusalReason.Reason; //Sonuç Gerekçesi
                    ws.Cells[string.Format("L{0}", rowStart)].Value = shipResult.Desc; //Açıklama
                    ws.Cells[string.Format("M{0}", rowStart)].Value = shipResult.AcceptedTime; //Kabul Edilen süre 
                }
                rowStart++;
                //ws.Cells[string.Format("N{0}", rowStart)].Value = item;
                //ws.Cells[string.Format("O{0}", rowStart)].Value = item;
                //ws.Cells[string.Format("P{0}", rowStart)].Value = item;
                //ws.Cells[string.Format("Q{0}", rowStart)].Value = item;
                //ws.Cells[string.Format("R{0}", rowStart)].Value = item;
                //ws.Cells[string.Format("S{0}", rowStart)].Value = item;
                //ws.Cells[string.Format("T{0}", rowStart)].Value = item;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attacment:filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }
}