using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankManagement.Application;
using BankManagement.WebAPI.Responses.v1;
using Microsoft.AspNetCore.Http;

namespace BankManagement.WebAPI.Controllers.v1;

[Route("api/v1/[controller]")]
// [ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
[ApiController]
public class CustomersController : ControllerBase
{
  private readonly ILogger<CustomersController> logger;
  private readonly IMediator mediator;

  public CustomersController(ILogger<CustomersController> logger, IMediator mediator)
  {
    this.logger = logger;
    this.mediator = mediator;
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Response<ExistentCustomerDTO?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<ExistentCustomerDTO?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<ExistentCustomerDTO?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnGetAsync([FromRoute] int id)
  {
    var request = new GetCustomerByIdRequest { CustomerId = id };

    var result = await mediator.Send(request);

    var response = new Response<ExistentCustomerDTO> { Data = result };

    return Ok(response);
  }

  [HttpPost]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPostAsync([FromBody] NewCustomerDTO customerDTO)
  {
    var request = new CreateCustomerRequestCommand { CustomerInfo = customerDTO };

    var result = await mediator.Send(request);

    var response = new Response<int> { Data = result };

    return Ok(response);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPutAsync([FromRoute] int id, [FromBody] NewCustomerDTO customerDTO)
  {
    var command = new UpdateCustomerRequestCommand { CustomerId = id, CustomerInfo = customerDTO };

    _ = await mediator.Send(command);

    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnDeleteAsync([FromRoute] int id)
  {
    var command = new DeleteCustomerRequestCommand { CustomerId = id };

    _ = await mediator.Send(command);

    return NoContent();
  }
}
