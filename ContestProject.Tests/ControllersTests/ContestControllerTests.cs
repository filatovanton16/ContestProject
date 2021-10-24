using Microsoft.AspNetCore.Mvc;
using ContestProject.Controllers;
using Xunit;
using ContestProject.Services;
using Moq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ContestProject.Tests
{
    public class ContestControllerTests
    {
        [Fact]
        public void GetTest()
        {
            // Arrange
            var mockData = new Mock<IDataService>();
            var mockDoodle = new Mock<IJDoodleService>();
            mockData.Setup(repo => repo.GetAllTasks()).Returns(GetAllTasks());
            ContestController controller = new ContestController(mockData.Object, mockDoodle.Object);

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.Equal<IEnumerable<string>>(GetAllTasks(), result);
        }

        [Fact]
        public void GetTaskTest()
        {
            var mockData = new Mock<IDataService>();
            var mockDoodle = new Mock<IJDoodleService>();
            mockData.Setup(repo => repo.GetContestTask("")).Returns(GetContestTask());
            ContestController controller = new ContestController(mockData.Object, mockDoodle.Object);

            // Act
            JsonResult result = controller.Get("");

            // Assert
            Assert.Equal("test", result.Value);
        }

        public ContestTask GetContestTask()
        {
            return new ContestTask() { Description = "test", OutputParameter = 0 };
        }

        public List<string> GetAllTasks()
        {
            return new List<string>()
            {
                "test1",
                "test2",
                "test3"
            };
        }

    }
}