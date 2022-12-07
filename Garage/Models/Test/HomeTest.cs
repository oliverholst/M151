using Garage.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Test
{
    [TestClass]
    public class HomeTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var controller = new HomeController(null);
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
