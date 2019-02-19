using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class InternShipResult
    {
        public int ResultID { get; set; }
        public Nullable<int> InternShipID { get; set; }
        public Nullable<int> RefusalReasonID { get; set; }
        public Nullable<int> AcceptedTime { get; set; }
        public string Desc { get; set; }
        public virtual InternShip InternShip { get; set; }
        public virtual RefusalReason RefusalReason { get; set; }
    }
}
