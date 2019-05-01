using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternShip.MvcUI.App_Classes
{
    public class Rapor
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StudentID { get; set; }
        public string Keyword { get; set; }
        public int City { get; set; }
        public int CompanyID { get; set; }
    }
}