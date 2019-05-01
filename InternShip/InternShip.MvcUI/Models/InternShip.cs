using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class InternShip
    {
        public InternShip()
        {
            this.Documents = new List<Document>();
            this.InternShipResults = new List<InternShipResult>();
            this.PreInternships = new List<PreInternship>();
        }

        public int InternShipID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string AdviserID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> City { get; set; }
        public string Time { get; set; }
        public Nullable<System.DateTime> CrtDate { get; set; }
        public Nullable<System.DateTime> DelDate { get; set; }
        public virtual City City1 { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<InternShipResult> InternShipResults { get; set; }
        public virtual ICollection<PreInternship> PreInternships { get; set; }
    }
}
