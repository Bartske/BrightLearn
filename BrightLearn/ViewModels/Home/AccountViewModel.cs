using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightLearn.ViewModels.Home
{
    public class AccountViewModel
    {
        public User user { get; set; }

        public string ValidationErrorMessage;

        public PasswordViewModel passwordViewModel { get; set; }
    }
}