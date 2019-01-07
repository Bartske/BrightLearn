using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.DataModels
{
    public class User
    {
        public int ID { get; set; }
        public int LoginID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
    }
}