using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternShip.MvcUI.Models
{
    public partial class ExtraData
    {
        [Key]
        public string DataType { get; set; }
        public string Data { get; set; }
    }
}