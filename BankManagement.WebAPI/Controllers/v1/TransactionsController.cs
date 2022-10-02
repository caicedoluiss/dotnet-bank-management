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
public class TransactionsController : ControllerBase
{
  private readonly ILogger<TransactionsController> logger;
  private readonly IMediator mediator;

  public TransactionsController(ILogger<TransactionsController> logger, IMediator mediator)
  {
    this.logger = logger;
    this.mediator = mediator;
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Response<ExistentTransactionDTO?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<ExistentTransactionDTO?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<ExistentTransactionDTO?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnGetAsync([FromRoute] int id, [FromQuery] bool getAccount = false, [FromQuery] bool getCustomer = false)
  {
    var request = new GetTransactionByIdRequest { TransactionId = id, RetrieveAccountInfo = getAccount, RetrieveCustomerInfo = getCustomer };

    var result = await mediator.Send(request);

    var response = new Response<ExistentTransactionDTO> { Data = result };

    return Ok(response);
  }

  [HttpPost()]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPostAsync([FromBody] NewTransactionDTO transaction)
  {
    var request = new CreateTransactionRequestCommand { TransactionInfo = transaction };

    var result = await mediator.Send(request);

    var response = new Response<int> { Data = result };

    return Ok(response);
  }

  [HttpPost("add")]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(Response<int?>), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPostAsync([FromBody] UpdatingTransactionDTO transaction)
  {
    var request = new AddTransactionRequestCommand { TransactionInfo = transaction };

    var result = await mediator.Send(request);

    var response = new Response<int> { Data = result };

    return Ok(response);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnPutAsync([FromRoute] int id, [FromBody] UpdatingTransactionDTO transaction)
  {
    var command = new UpdateTransactionRequestCommand { TransactionId = id, TransactionInfo = transaction };

    _ = await mediator.Send(command);

    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> OnDeleteAsync([FromRoute] int id)
  {
    var command = new DeleteTransactionRequestCommand { TransactionId = id };

    _ = await mediator.Send(command);

    return NoContent();
  }
}
