using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class Document
    {
        public int DocumentID { get; set; }
        public Nullable<int> InternShipID { get; set; }
        public string DocName { get; set; }
        public string Path { get; set; }
        public string Desc { get; set; }
        public Nullable<System.DateTime> CrtDate { get; set; }
        public Nullable<System.DateTime> DelDate { get; set; }
        public virtual InternShip InternShip { get; set; }
    }
}
