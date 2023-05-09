using Application.Orders.Commands;
using Application.Orders.Dtos;
using Application.Orders.Queries;
using Application.Orders.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var query = new GetOrdersQuery();
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
                var query = new GetOrderByIdQuery(id);
                var order = await _mediator.Send(query);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
           
                _logger.LogError(ex, "Failed to get order by ID");
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
                var validator = new CreateOrderDtoValidator();
                var result = await validator.ValidateAsync(createOrderDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var command = new CreateOrderCommand { Order = createOrderDto };
                var orderId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create order");
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
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update order");
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
                await _mediator.Send(command);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete order");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}

