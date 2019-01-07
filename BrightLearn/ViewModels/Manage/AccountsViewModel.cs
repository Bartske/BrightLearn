using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Models;

namespace BrightLearn.ViewModels.Manage
{
    public class AccountsViewModel
    {
        public List<User> Accounts { get; set; }

        public string ErrorMessage { get; set; }
    }
}