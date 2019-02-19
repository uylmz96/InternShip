using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class RefusalReason
    {
        public RefusalReason()
        {
            this.InternShipResults = new List<InternShipResult>();
        }

        public int ReasonID { get; set; }
        public string Reason { get; set; }
        public string Desc { get; set; }
        public Nullable<System.DateTime> CrtDate { get; set; }
        public Nullable<System.DateTime> DelDate { get; set; }
        public virtual ICollection<InternShipResult> InternShipResults { get; set; }
    }
}
