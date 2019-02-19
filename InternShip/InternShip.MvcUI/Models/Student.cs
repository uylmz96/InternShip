using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class Student
    {
        public Student()
        {
            this.InternShips = new List<InternShip>();
        }

        public int StudentID { get; set; }
        public string StudentNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<bool> isGraduate { get; set; }
        public Nullable<System.DateTime> GraduateDate { get; set; }
        public Nullable<System.DateTime> CrtDate { get; set; }
        public Nullable<System.DateTime> DelDate { get; set; }
        public virtual ICollection<InternShip> InternShips { get; set; }
    }
}
