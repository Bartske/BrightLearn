using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Models.DataModels;

namespace DAL
{
    public class DataInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<BrightLearnContext>
    {
        protected override void Seed(BrightLearnContext context)
        {
            List<User> users = new List<User>()
            {
                new User(){FirstName="Admin", MiddleName="", LastName="Admin", Email="admin@brightlearn.nl", LoginID=1, Type="admin"}
            };
            
            string _salt = "FN!MK)XD@Z";

            List<Login> logins = new List<Login>()
            {
                new Login(){ID = 1, Password="PyMx73kf+U2Iqe11MOnjWg==", Salt=_salt, UserName="Admin"}
            };

            users.ForEach(u => context.User.Add(u));
            logins.ForEach(l => context.Login.Add(l));

            context.SaveChanges();
        }
    }
}