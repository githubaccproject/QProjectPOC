using Application.Commands;
using Application.DTOs;
using Application.Queries.CustomerQuery;
using Application.Queries.OrderQuery;
using Application.Validators;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, IMapper mapper, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var query = new GetCustomersQuery();
                var customers = await _mediator.Send(query);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get customers");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                // Use Mediator to send a GetOrderByIdQuery
                var query = new GetOrderByIdQuery(id);
                var order = await _mediator.Send(query);

                // If order is not found, return NotFound
                if (order == null)
                {
                    return NotFound();
                }

                // Map Order to OrderDto
                var orderDto = _mapper.Map<OrderDto>(order);

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                // Log error using ILogger
                _logger.LogError(ex, "Failed to get order by ID");

                // Handle exception and return appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                // Validate request data using FluentValidation
                var validator = new CreateOrderDtoValidator();
                var result = await validator.ValidateAsync(createOrderDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var command = new CreateOrderCommand { Order = createOrderDto };

                // Use Mediator to send a CreateOrderCommand
                var orderId = await _mediator.Send(command);

                // Return the created order ID in the response
                return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
            }
            catch (Exception ex)
            {
                // Log error using ILogger
                _logger.LogError(ex, "Failed to create order");

                // Handle exception and return appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                // Validate request data using FluentValidation
                var validator = new UpdateOrderDtoValidator();
                var result = await validator.ValidateAsync(updateOrderDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var command = new UpdateOrderCommand
                {
                    Id = id,
                    Order = updateOrderDto
                };

                // Use Mediator to send an UpdateOrderCommand
                await _mediator.Send(command);

                // Return a NoContent response
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update order");

                // Handle exception and return appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var command = new DeleteOrderCommand { Id = id };

                // Use Mediator to send a DeleteOrderCommand
                await _mediator.Send(command);

                // Return a NoContent response
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log error using ILogger
                _logger.LogError(ex, "Failed to delete order");

                // Handle exception and return appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

