using Microsoft.AspNetCore.Mvc;
using ContestProject.Controllers;
using Xunit;
using ContestProject.Services;
using Moq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace ContestProject.Tests.ServicesTests
{
    public class JDoodleServiceTests
    {
        [Fact]
        public async void TryCompilateTest()
        {
            // Arrange
            HttpClient _httpClient = new HttpClient();
            JDoodlePOSTService service = new JDoodlePOSTService(_httpClient);
            string code = "public static int MyMethod(int input) {\nreturn 2 * input;\n}";
            int input = 1;
            string expectedStatusCode = "200";
            int expectedOutput = 2;

            // Act
            dynamic output = await service.TryCompilateAsync(code, input);

            // Assert
            Assert.Equal(expectedStatusCode, output.statusCode.ToString());
            Assert.Equal(expectedOutput, Convert.ToInt32(output.output));
        }

    }
}