using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Controllers;
using Application.Commands;
using Application.DTOs;
using Application.Queries.CustomerQuery;
using AutoMapper;
using Castle.Core.Resource;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using ILogger = NLog.ILogger;

namespace TestProject.ControllerUnitTest
{

    [TestClass]
    public class CustomerControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger> _loggerMock;

        private CustomerController _customerController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger>();
            _customerController = new CustomerController(_mediatorMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetCustomers_ValidData_ReturnsOkResult()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                // Set up customer data for testing
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomersQuery>(), default)).ReturnsAsync(customers);

            // Act
            var result = await _customerController.GetCustomers();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(customers, okResult.Value);
        }

        [TestMethod]
        public async Task GetCustomers_ExceptionThrown_LogsErrorAndReturnsStatusCode500()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomersQuery>(), default)).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _customerController.GetCustomers();
            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCustomerById_Returns_OkResult_With_CustomerDto()
        {
            // Arrange
            int customerId = 1;
            var customer = new Customer();
            var customerDto = new CustomerDto();
            var query = new GetCustomerByIdQuery(customerId);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default)).ReturnsAsync(customerDto);
            _mapperMock.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);
            // Act
            var result = await _customerController.GetCustomerById(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetCustomerById_Returns_NotFoundResult_When_CustomerNotFound()
        {
            // Arrange
            int customerId = 1;
            CustomerDto customerDto = null; // Set customer to null to simulate not found
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default)).ReturnsAsync(customerDto);
            // Act
            var result = await _customerController.GetCustomerById(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetCustomerById_Returns_InternalServerError_When_ExceptionThrown()
        {
            // Arrange
            int customerId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default)).ThrowsAsync(new Exception("Test error"));
            var loggerMock = new Mock<ILogger<CustomerController>>();

            // Act
            var result = await _customerController.GetCustomerById(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Test error", objectResult.Value);
        }

        [TestMethod]
        public async Task UpdateCustomer_Returns_NoContentResult_When_ValidRequest()
        {
            // Arrange
            int customerId = 1;
            var updateCustomerDto = new UpdateCustomerDto { Id = 1, Name = "Customer1", Email = "customer1@example.com", IsActive = true, Phone = "1234567890" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), default)).Returns(Task.FromResult(1));
            // Act
            var result = await _customerController.UpdateCustomer(customerId, updateCustomerDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var noContentResult = (NoContentResult)result;
            Assert.AreEqual(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }


        [TestMethod]
        public async Task UpdateCustomer_Returns_BadRequestResult_When_InvalidRequest()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto
            {
                // Set up the properties of createCustomerDto with invalid data for testing
            };

            var validationFailures = new FluentValidation.Results.ValidationResult();
            validationFailures.Errors.Add(new FluentValidation.Results.ValidationFailure("PropertyName", "Error Message"));

            var validatorMock = new Mock<IValidator<UpdateCustomerDto>>();
            validatorMock.Setup(v => v.ValidateAsync(updateCustomerDto, default)).ReturnsAsync(validationFailures);

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default)).Returns(Task.FromResult(1));

            // Act
            // Act
            var result = await _customerController.UpdateCustomer(1, updateCustomerDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCustomer_Returns_InternalServerError_When_ExceptionThrown()
        {
            // Arrange
            int customerId = 1;
            var updateCustomerDto = new UpdateCustomerDto { Id = 1, Name = "Customer1", Email = "customer1@example.com", IsActive = true, Phone = "1234567890" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), default)).ThrowsAsync(new Exception("Some error occurred"));

            // Act
            // Act
            var result = await _customerController.UpdateCustomer(customerId, updateCustomerDto);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);


        }


        [TestMethod]
        public async Task CreateCustomer_Returns_NoContentResult_When_ValidRequest()
        {
            // Arrange
        
            var createCustomerDto = new CreateCustomerDto {  Name = "Customer1", Email = "customer1@example.com", IsActive = true, Phone = "1234567890" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default)).Returns(Task.FromResult(1));
            // Act
            var result = await _customerController.CreateCustomer(createCustomerDto);

            // Assert
            Assert.IsNotNull(result);
            var objectResult = (OkResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);
        }


        [TestMethod]
        public async Task CreateCustomer_Returns_BadRequestResult_When_InvalidRequest()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto
            {
                // Set up the properties of createCustomerDto with invalid data for testing
            };

            var validationFailures = new FluentValidation.Results.ValidationResult();
            validationFailures.Errors.Add(new FluentValidation.Results.ValidationFailure("PropertyName", "Error Message"));

            var validatorMock = new Mock<IValidator<UpdateCustomerDto>>();
            validatorMock.Setup(v => v.ValidateAsync(updateCustomerDto, default)).ReturnsAsync(validationFailures);

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default)).Returns(Task.FromResult(1));

            // Act
            // Act
            var result = await _customerController.UpdateCustomer(1, updateCustomerDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateCustomer_Returns_InternalServerError_When_ExceptionThrown()
        {
            // Arrange
            
            var createCustomerDto = new CreateCustomerDto {  Name = "Customer1", Email = "customer1@example.com", IsActive = true, Phone = "1234567890" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default)).ThrowsAsync(new Exception("Some error occurred"));

            // Act
            // Act
            var result = await _customerController.CreateCustomer(createCustomerDto);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);


        }
        [TestMethod]
        public async Task DeleteCustomer_Returns_OkResult_When_ValidRequest()
        {
            int id = 1;

            // Set up the mock to return a completed task when Send is called with a DeleteCustomerCommand
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCustomerCommand>(),default)).Returns(Task.FromResult(true));

            // Act
            var result = await _customerController.DeleteCustomer(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteCustomer_Returns_InternalServerError_When_ExceptionThrown()
        {
            // Arrange
            int customerId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCustomerCommand>(), default)).ThrowsAsync(new Exception("Test error"));
            // Act
            var result = await _customerController.DeleteCustomer(customerId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Test error", objectResult.Value);
        }

        
    }
}
