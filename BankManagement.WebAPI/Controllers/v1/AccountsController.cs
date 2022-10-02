using System.Threading.Tasks;
using BankManagement.Application;
using BankManagement.WebAPI.Responses.v1;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankManagement.WebAPI.Controllers.v1;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[ApiController]
public class AccountsController : ControllerBase
{
  private readonly ILogger<AccountsController> logger;
  private readonly IMediator mediator;

  public AccountsController(ILogger<AccountsController> logger, IMediator mediator)
  {
    this.logger = logger;
    this.mediator = mediator;
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Response<ExistentAccountDTO?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<ExistentAccountDTO?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<ExistentAccountDTO?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnGetAsync([FromRoute] int id, [FromQuery] bool getCustomer = false)
  {
    var request = new GetAccountByIdRequest { AccountId = id, RetrieveCustomerInfo = getCustomer };

    var result = await mediator.Send(request);

    var response = new Response<ExistentAccountDTO> { Data = result };

    return Ok(response);
  }

  [HttpPost]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPostAsync([FromBody] NewAccountDTO account)
  {
    var request = new CreateAccountRequestCommand { AccountInfo = account };

    var result = await mediator.Send(request);

    var response = new Response<int> { Data = result };

    return Ok(response);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPutAsync([FromRoute] int id, [FromBody] NewAccountDTO account)
  {
    var command = new UpdateAccountRequestCommand { AccountId = id, AccountInfo = account };

    _ = await mediator.Send(command);

    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnDeleteAsync([FromRoute] int id)
  {
    var command = new DeleteAccountRequestCommand { AccountId = id };

    _ = await mediator.Send(command);

    return NoContent();
  }
}
