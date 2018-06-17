using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LN2ME;
using LN2ME.Controllers;

namespace LN2ME.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
