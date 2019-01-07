using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Models;

namespace BrightLearn.Tests.HandlerTests
{
    [TestClass]
    public class UserHandlerTests
    {
        //User tests

        UserHandler handler = new UserHandler(true);
        Models.User CorrectUser = new Models.User()
        {
            FirstName = "Bert",
            MiddleName = "van de",
            LastName = "Ven",
            ID = 2,
            Email = "test@mail.nl",
            UserName = "Username",
            AccountType = "user"
        };

        Models.User updatedUser = new Models.User()
        {
            FirstName = "Johan",
            MiddleName = "",
            LastName = "Boeren",
            ID = 2,
            Email = "Johan@mail.nl",
            UserName = "Johan123",
            AccountType = "admin"
        };
        
        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_GetUser_ID_WrongID()
        {
            Models.User testUser = handler.GetUser(1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_GetUser_LoginID_WrongUsername()
        {
            Models.User testUser = handler.GetUser("Wrong", "UuLduQbrS1pvqFom6dPP7A==");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_GetUser_LoginID_WrongPassword()
        {
            Models.User testUser = handler.GetUser("Username", "Wrong");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There was no username given!")]
        public void test_GetUser_LoginID_NoUserName()
        {
            Models.User testUser = handler.GetUser("", "UuLduQbrS1pvqFom6dPP7A==");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There was no password given!")]
        public void test_GetUser_LoginID_NoPassword()
        {
            Models.User testUser = handler.GetUser("Username", "");
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception), "You must fill in the firstname and lastname!")]
        public void test_GetUser_FullName_NoFirstName()
        {
            Models.User testUser = handler.GetUser("", "van de", "Ven");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "You must fill in the firstname and lastname!")]
        public void test_GetUser_FullName_NoLastName()
        {
            Models.User testUser = handler.GetUser("Bert", "van de", "");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_GetUser_FullName_WrongFirstName()
        {
            Models.User testUser = handler.GetUser("Johan", "van de", "Ven");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_GetUser_FullName_WrongLastName()
        {
            Models.User testUser = handler.GetUser("Bert", "van de", "Boer");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_GetUser_email_NoEmail()
        {
            Models.User testUser = handler.GetUser("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email must be filled in!")]
        public void test_GetUser_email_WrongMail()
        {
            Models.User testUser = handler.GetUser("test@NoMAIL.nl");
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_obj_NoFirstName()
        {
            User testUser = CorrectUser;
            testUser.FirstName = "";
            handler.UpdateUser(testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_obj_NoLastName()
        {
            User testUser = CorrectUser;
            testUser.LastName = "";
            handler.UpdateUser(testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_obj_NoEmail()
        {
            User testUser = CorrectUser;
            testUser.Email = "";
            handler.UpdateUser(testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_obj_NoAccType()
        {
            User testUser = CorrectUser;
            testUser.AccountType = "";
            handler.UpdateUser(testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_obj_NoUserName()
        {
            User testUser = CorrectUser;
            testUser.UserName = "";
            handler.UpdateUser(testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_Val_NoFirstName()
        {
            handler.UpdateUser("", "", "Boeren", "Johan@mail.nl", "admin", "Johan123", 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_Val_NoLastName()
        {
            handler.UpdateUser("Johan", "", "", "Johan@mail.nl", "admin", "Johan123", 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_Val_NoEmail()
        {
            handler.UpdateUser("Johan", "", "Boeren", "", "admin", "Johan123", 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_Val_NoAccType()
        {
            handler.UpdateUser("Johan", "", "Boeren", "Johan@mail.nl", "", "Johan123", 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "All the values must be filled in!")]
        public void test_UpdateUser_Val_NoUserName()
        {
            handler.UpdateUser("Johan", "", "Boeren", "Johan@mail.nl", "admin", "", 2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_UpdateUser_obj_NoRecord()
        {
            User testUser = CorrectUser;
            testUser.ID = 0;
            handler.UpdateUser(testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "There is no record of that user!")]
        public void test_UpdateUser_Val_NoRecord()
        {
            handler.UpdateUser("Johan", "", "Boeren", "Johan@mail.nl", "admin", "", 0);
        }

    }
}
