using Api.Controllers;
using Application.Customers.Commands;
using Application.Orders.Commands;
using Application.Orders.Dtos;
using Application.Orders.Queries;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject.ControllerUnitTest
{
    [TestClass]
    public class OrderControllerTests
    {
        private OrderController _orderController;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<OrderController>> _loggerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize mocks
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<OrderController>>();

            // Create instance of OrderController
            _orderController = new OrderController(_mediatorMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetOrders_ReturnsOkResult()
        {
            // Arrange
            var query = new GetOrdersQuery();
            var orders = new List<OrderDto>
            {
                // Add sample customer data
            };


            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrdersQuery>(), default)).ReturnsAsync(orders);


            // Act
            var result = await _orderController.GetOrders();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public async Task GetOrders_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrdersQuery>(), default)).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _orderController.GetOrders();
            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }





        [TestMethod]
        public async Task GetOrderById_ExistingOrderId_ReturnsOrderDto()
        {
            // Arrange
            int orderId = 1;
            var order = new Order();
            var orderDto = new OrderDto();
            var query = new GetOrderByIdQuery(orderId);
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), default)).ReturnsAsync(orderDto);
            // Act
            var result = await _orderController.GetOrderById(orderId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetOrderById_NonExistingOrderId_ReturnsNotFound()
        {
            // Arrange
            int orderId = 1;
            OrderDto orderDto = null; // Set customer to null to simulate not found
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), default)).ReturnsAsync(orderDto);
            // Act
            var result = await _orderController.GetOrderById(orderId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetOrderById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            int orderId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), default)).ThrowsAsync(new Exception("Test error"));
            var loggerMock = new Mock<ILogger<CustomerController>>();

            // Act
            var result = await _orderController.GetOrderById(orderId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Test error", objectResult.Value);
        }



        [TestMethod]
        public async Task UpdateOrder_ValidData_ReturnsNoContent()
        {
            // Arrange
            var orderId = 1;
            var updateOrderDto = new UpdateOrderDto
            {
                CustomerId = 1,
                Date = DateTime.Now,
                Price = 99.99M,
                OrderProducts = new List<UpdateOrderProductDto>
    {
        new UpdateOrderProductDto
        {
            Id = 1,
            OrderId = 1,
            ProductId = 1,
            Price = 10.99M,
            MembershipName = "Sample Membership",
            Quantity = 3
        },
        new UpdateOrderProductDto
        {
            Id = 2,
            OrderId = 1,
            ProductId = 2,
            Price = 20.99M,
            MembershipName = "Another Membership",
            Quantity = 2
        }
    }
            };

            var command = new UpdateOrderCommand
            {
                Id = orderId,
                Order = updateOrderDto
            };

            // Set up FluentValidation to return a valid result
            var validator = new Mock<IValidator<UpdateOrderDto>>();
            validator.Setup(v => v.ValidateAsync(updateOrderDto, default)).ReturnsAsync(new ValidationResult());
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), default)).Returns(Task.FromResult(1));
            // Act
            var result = await _orderController.UpdateOrder(orderId, updateOrderDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }


        [TestMethod]
        public async Task UpdateOrder_InvalidData_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var orderId = 1;
            var updateOrderDto = new UpdateOrderDto
            {
                // Set properties
            };
            var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error1"),
            new ValidationFailure("Property2", "Error2")
        };
            var validationResult = new ValidationResult(validationErrors);
            var command = new UpdateOrderCommand
            {
                Id = orderId,
                Order = updateOrderDto
            };

            // Set up FluentValidation to return an invalid result
            var validator = new Mock<IValidator<UpdateOrderDto>>();
            validator.Setup(v => v.ValidateAsync(updateOrderDto, default)).ReturnsAsync(validationResult);
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(IValidator<UpdateOrderDto>))).Returns(validator.Object);
            _orderController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider.Object
                }
            };

            // Act
            var result = await _orderController.UpdateOrder(orderId, updateOrderDto);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult.Value);
        }

        [TestMethod]
        public async Task CreateOrder_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = 1,
                Date = DateTime.Now,
                Price = 100.50m,
                OrderProducts = new List<CreateOrderProductDto>
    {
        new CreateOrderProductDto
        {
            ProductId = 101,
            Quantity = 2,
            OrderId = 1,
            Price = 50.25m,

        },
        new CreateOrderProductDto
        {
            ProductId = 102,
            Quantity = 3,
            OrderId = 2,
             Price = 50.25m,
        }
    }
            };


            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default(CancellationToken)))
                .ReturnsAsync(1); // Mock the mediator to return an order ID of 1

            // Act
            var result = await _orderController.CreateOrder(createOrderDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdAtActionResult = (CreatedAtActionResult)result;
            Assert.AreEqual(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
            Assert.AreEqual(nameof(OrderController.GetOrderById), createdAtActionResult.ActionName);
            Assert.AreEqual(1, createdAtActionResult.RouteValues["id"]);
        }

        [TestMethod]
        public async Task CreateOrder_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange

            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                // Set properties for valid request data
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                // Create validation errors for invalid request data
            });

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default(CancellationToken)))
                .ThrowsAsync(new ValidationException(validationResult.Errors));

            // Act
            var result = await _orderController.CreateOrder(createOrderDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            // Add further assertions for error response body if applicable
        }

        [TestMethod]
        public async Task CreateOrder_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = 1,
                Date = DateTime.Now,
                Price = 100.50m,
                OrderProducts = new List<CreateOrderProductDto>
    {
        new CreateOrderProductDto
        {
            ProductId = 101,
            Quantity = 2,
            OrderId = 1,
            Price = 50.25m,

        },
        new CreateOrderProductDto
        {
            ProductId = 102,
            Quantity = 3,
            OrderId = 2,
             Price = 50.25m,
        }
    }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default(CancellationToken)))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _orderController.CreateOrder(createOrderDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        }


        [TestMethod]
        public async Task DeleteOrder_ValidId_ReturnsNoContent()
        {
            int id = 1;

            // Set up the mock to return a completed task when Send is called with a DeleteCustomerCommand
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteOrderCommand>(), default)).Returns(Task.FromResult(true));

            // Act
            var result = await _orderController.DeleteOrder(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteOrder_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            int orderId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteOrderCommand>(), default)).ThrowsAsync(new Exception("Test error"));
            // Act
            var result = await _orderController.DeleteOrder(orderId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Test error", objectResult.Value);
        }
    }

}

