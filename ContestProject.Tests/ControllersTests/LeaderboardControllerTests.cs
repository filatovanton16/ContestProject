using System;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ContestProject.Controllers;
using Xunit;
using System.Text;
using System.Linq;

namespace ContestProject.Tests.ControllerTests
{
    public class LeaderboardControllerTests
    {
        [Fact]
        public void GetTest()
        {
            // Arrange
            var mockData = new Mock<IDataService>();
            mockData.Setup(repo => repo.GetTop3UserTasks()).Returns(GetUserTaskGroups());
            LeaderboardController controller = new LeaderboardController(mockData.Object);

            // Act
            IEnumerable<UserTaskGroup> result = controller.Get();

            // Assert
            Assert.Equal(result.Count(), GetUserTaskGroups().Count());
            if (result.Count() != 0)
            {
                for (int i = 0; i < result.Count(); i++)
                {
                    Assert.Equal<UserTaskGroup>(GetUserTaskGroups().ElementAt(i), result.ElementAt(i));
                }
            }
        }

        public IEnumerable<UserTaskGroup> GetUserTaskGroups()
        {
            return new List<UserTaskGroup>()
            {
                new UserTaskGroup ("testName", 1,  new List<string>(){ "test1", "test2"}),
                new UserTaskGroup ("testName2", 2,  new List<string>(){ "test3", "test4"}),
            };
        }
    }
}
