
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            // Arrange
            var employeeId = "fake_employee_id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddCompensation_Returns_Compensation()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            var compensation = new CompensationRequest
            {
                EmployeeID = employeeId,
                //Employee = new Employee() { EmployeeId = employeeId },
                EffectiveDate = System.DateTimeOffset.UtcNow,
                Salary = 100000m
            };

            // Execute
            var requestContent = new JsonSerialization().ToJson(compensation);

            //Create compensation and save to persistent storage
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
                    new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var postRepsonse = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, postRepsonse.StatusCode);

            //Verify that compensation has been persisted and can be retrieved
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var getResponse = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);

            var getCompensation = getResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.EmployeeID, getCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Salary, getCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, getCompensation.EffectiveDate);
        }

    }
}
