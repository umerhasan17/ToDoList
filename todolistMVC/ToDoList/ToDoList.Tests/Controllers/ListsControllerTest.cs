using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList;
using ToDoList.Controllers;

namespace ToDoList.Tests.Controllers
{
    [TestClass]
    public class ListsControllerTest
    {
        /*[TestMethod]
        public void Index()
        {
            // Arrange
            ListsController controller = new ListsController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }*/

        [TestMethod]
        public void Create()
        {
            // Arrange
            ListsController controller = new ListsController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
