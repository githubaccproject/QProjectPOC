using Microsoft.AspNetCore.Mvc;
using MediatR;
using NLog;
using Microsoft.AspNetCore.Authorization;
using Application.Customers.Queries;
using Application.Customers.Dtos;
using Application.Customers.Validators;
using Application.Customers.Commands;

namespace Api.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var query = new GetCustomersQuery();
                var customers = await _mediator.Send(query);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get customers");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var query = new GetCustomerByIdQuery(id);
                var customer = await _mediator.Send(query);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get customer by ID");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            try
            {
                var validator = new CreateCustomerDtoValidator();
                var result = await validator.ValidateAsync(createCustomerDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                var command = new CreateCustomerCommand
                {
                    Customer = createCustomerDto
                };
                await _mediator.Send(command);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create customer");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                var validator = new UpdateCustomerDtoValidator();
                var result = await validator.ValidateAsync(updateCustomerDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                var command = new UpdateCustomerCommand
                {
                    Id = id,
                    Customer = updateCustomerDto
                };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete customer");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var command = new DeleteCustomerCommand { Id = id };
                await _mediator.Send(command);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

       
    }
}
