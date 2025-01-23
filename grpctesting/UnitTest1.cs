using Grpc.Core;
using GrpcService1;
using GrpcService1.Data;
using GrpcService1.Services;
using Microsoft.Data.SqlClient;
using Moq;
using NUnit.Framework.Internal;

namespace grpctesting
{
    [TestFixture]
    public class Test
    {
        private Mock<ISqlService> _sqlServiceMock;
        private CustomerDataService _customerService;
        

        [SetUp]
        public void Setup()
        {
            _sqlServiceMock = new Mock<ISqlService>();
            _customerService = new CustomerDataService(_sqlServiceMock.Object);
        }

        [Test]
        public async Task GetCustomers_ReturnsCustomerList_WhenDataExists()
        {
            // Arrange
            var mockDataReader = new Mock<IDataReader>();
            var sequence = new MockSequence();

            mockDataReader.InSequence(sequence).Setup(r => r.ReadAsync()).ReturnsAsync(true);
            mockDataReader.Setup(r => r.GetString("First_Name")).Returns("John");
            mockDataReader.Setup(r => r.GetString("Last_Name")).Returns("Doe");
            mockDataReader.Setup(r => r.GetString("Date_of_Birth")).Returns("2000-01-01");
            mockDataReader.Setup(r => r.GetString("Id")).Returns("123");

            mockDataReader.InSequence(sequence).Setup(r => r.ReadAsync()).ReturnsAsync(false);

            _sqlServiceMock.Setup(s => s.ExecuteTheReader("get", null)).ReturnsAsync(mockDataReader.Object);

            // Act
            var result = await _customerService.GetCustomers(new Empty(), It.IsAny<ServerCallContext>());

            // Assert
            Assert.IsFalse(result.Isfailed);
            Assert.AreEqual(1, result.Custometrs.Count);
            Assert.AreEqual("John", result.Custometrs[0].FirstName);
            Assert.AreEqual("Doe", result.Custometrs[0].LastName);
        }

        [Test]
        public async Task GetCustomers_ReturnsEmptyCustomerList_WhenDataDoesNotExists()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();

            mockReader.SetupSequence(r => r.ReadAsync())
                      .ReturnsAsync(false); // No rows to read

            var mockSqlService = new Mock<ISqlService>();
            mockSqlService.Setup(s => s.ExecuteTheReader("get", null))
                          .ReturnsAsync(mockReader.Object); // Return the mock reader

            var service = new CustomerDataService(mockSqlService.Object);

            // Act
            var result = await service.GetCustomers(new Empty(), null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Custometrs.Any()); // Ensure the customer list is empty
            Assert.IsFalse(result.Isfailed); // Ensure no failure flag is set
            Assert.AreEqual(0, result.Custometrs.Count());
        }


        [Test]
        public async Task GetCustomers_ReturnsError_WhenDatabaseConnectionFails()
        {
            // Arrange
            var mockSqlService = new Mock<ISqlService>();
            mockSqlService.Setup(s => s.ExecuteTheReader("get", null))
                          .ThrowsAsync(new Exception("Database connection failed"));

            var service = new CustomerDataService(mockSqlService.Object);

            // Act
            var result = await service.GetCustomers(new Empty(), null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Isfailed); // Ensure failure flag is set
            Assert.AreEqual("Database connection failed", result.Errortxt); // Check error message
            Assert.IsEmpty(result.Custometrs); // Ensure no customers are returned
        }


        [Test]
        public async Task GetCustomers_ReturnsPartialCustomerList_WhenSomeRowsHaveInvalidData()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();
            mockReader.SetupSequence(r => r.ReadAsync())
                      .ReturnsAsync(true) // Row 1
                      .ReturnsAsync(true) // Row 2 (invalid)
                      .ReturnsAsync(false); // End of data

            // First row is valid
            mockReader.Setup(r => r.GetString("First_Name")).Returns("John");
            mockReader.Setup(r => r.GetString("Last_Name")).Returns("Doe");
            mockReader.Setup(r => r.GetString("Date_of_Birth")).Returns("11/10/2002");
            mockReader.Setup(r => r.GetString("Id")).Returns("1");

            // Second row is invalid (throws exception for some fields)
            mockReader.Setup(r => r.GetString("First_Name"))
                      .Throws(new Exception("Invalid field data"));

            var mockSqlService = new Mock<ISqlService>();
            mockSqlService.Setup(s => s.ExecuteTheReader("get", null))
                          .ReturnsAsync(mockReader.Object);

            var service = new CustomerDataService(mockSqlService.Object);

            // Act
            var result = await service.GetCustomers(new Empty(), null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Isfailed); // Ensure failure flag is set
            Assert.AreEqual("Invalid field data", result.Errortxt); // Check error message
            Assert.AreEqual(1, result.Custometrs.Count); // Ensure only valid rows are processed
        }





        [Test]
        public async Task GetCustomers_ReturnsCorrectlyMappedData_WhenDatabaseContainsDifferentDataTypes()
        {
            // Arrange
            var mockReader = new Mock<IDataReader>();
            mockReader.SetupSequence(r => r.ReadAsync())
                      .ReturnsAsync(true)
                      .ReturnsAsync(false); // Single row

            mockReader.Setup(r => r.GetString("First_Name")).Returns("Jane");
            mockReader.Setup(r => r.GetString("Last_Name")).Returns("Smith");
            mockReader.Setup(r => r.GetString("Date_of_Birth")).Returns("1990-05-10");
            mockReader.Setup(r => r.GetString("Id")).Returns("123");

            var mockSqlService = new Mock<ISqlService>();
            mockSqlService.Setup(s => s.ExecuteTheReader("get", null))
                          .ReturnsAsync(mockReader.Object);

            var service = new CustomerDataService(mockSqlService.Object);

            // Act
            var result = await service.GetCustomers(new Empty(), null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Isfailed); // Ensure no failure flag is set
            Assert.AreEqual(1, result.Custometrs.Count); // Ensure correct number of customers
            Assert.AreEqual("Jane", result.Custometrs[0].FirstName);
            Assert.AreEqual("Smith", result.Custometrs[0].LastName);
            Assert.AreEqual("1990-05-10", result.Custometrs[0].Dateofbirth);
            Assert.AreEqual("123", result.Custometrs[0].Id);
        }




    }

}