using System;
using System.Collections.Generic;

namespace InternShip.MvcUI.Models
{
    public partial class City
    {
        public City()
        {
            this.InternShips = new List<InternShip>();
            this.PreInternships = new List<PreInternship>();
        }

        public int id { get; set; }
        public string CityName { get; set; }
        public virtual ICollection<InternShip> InternShips { get; set; }
        public virtual ICollection<PreInternship> PreInternships { get; set; }
    }
}
