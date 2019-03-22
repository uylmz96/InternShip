using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class Company
    {
        public Company()
        {
            this.InternShips = new List<InternShip>();
        }

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Desc { get; set; }
        public Nullable<bool> IsBlackCompany { get; set; }
        public Nullable<System.DateTime> CrtDate { get; set; }
        public Nullable<System.DateTime> DelDate { get; set; }
        public virtual ICollection<InternShip> InternShips { get; set; }
    }
}
