using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class PreInternship
    {
        public int PreInternshipID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> InternshipID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyMail { get; set; }
        public string Department { get; set; }
        public string Activity { get; set; }
        public string Tech { get; set; }
        public string Subject { get; set; }
        public string EmployeeDesc { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CrtDate { get; set; }
        public Nullable<System.DateTime> DelDate { get; set; }
        public string StudentNumber { get; set; }
        public string Time { get; set; }
        public Nullable<int> City { get; set; }
        public virtual City City1 { get; set; }
        public virtual InternShip InternShip { get; set; }
        public virtual Student Student { get; set; }
    }
}
