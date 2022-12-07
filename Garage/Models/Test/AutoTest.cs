using Garage.Controllers;
using Garage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garage.Test
{
    [TestClass]
    class AutoTest
    {
        [TestMethod]
        public void TestIndex()
        {
            IAuto db = new AutoFake();
            var controlller = new AutoController(db);
            var result = controlller.Index() as ViewResult;
            Assert.IsNotNull(result);


        }
        [TestMethod]
        public void TestCreate()
        {
            IAuto db = new AutoFake();
            var controlller = new AutoController(db);
            var result = controlller.Create() as ViewResult;
            Assert.IsNotNull(result);

            var auto = new Auto { PK_Auto = 99, Marke = "Jaguar", Modell = "E" };
            var test = controlller.Create() as RedirectToActionResult;
            Assert.IsNotNull(test);
            Assert.IsTrue(test.ActionName == "Index");
        }


        [TestMethod]
        public void TestDelete()
        {




        }
    }
}
