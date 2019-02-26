using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternShip.MvcUI.App_Classes
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public string OldPassword { get; set; }
        public string Mail { get; set; }
    }
}