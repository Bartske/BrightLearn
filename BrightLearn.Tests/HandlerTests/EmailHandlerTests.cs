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
    public class EmailHandlerTests
    {
        EmailHandler handler = new EmailHandler();

        [TestMethod]
        [ExpectedException(typeof(Exception), "Reciever mail cannot be empty!")]
        public void test_SendMail_Empty_Reciever()
        {
            handler.SendEmail("", "d", "dad");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Subject cannot be empty!")]
        public void test_SendMail_Empty_Subject()
        {
            handler.SendEmail("dwda", "", "dad");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "The message cannot be empty!")]
        public void test_SendMail_Empty_Message()
        {
            handler.SendEmail("adwwdwad", "d", "");
        }
    }
}
